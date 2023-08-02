using System.Collections;
using System.Collections.Specialized;

namespace ExampleFramework.Tooling.Maui.Controls.TreeView;

/// <summary>
/// This control is based on the original code here: https://github.com/enisn/TreeView.Maui
/// </summary>
public class TreeView : ContentView
{
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(TreeView), null, propertyChanging: (b, o, n) => ((TreeView)b).OnItemsSourceSetting((IEnumerable)o, (IEnumerable)n), propertyChanged: (b, o, v) => ((TreeView)b).OnItemsSourceSet());
    public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(TreeView), new DataTemplate(typeof(DefaultTreeViewNodeView)), propertyChanged: (b, o, n) => ((TreeView)b).OnItemTemplateChanged());
    public static readonly BindableProperty ArrowThemeProperty = BindableProperty.Create(nameof(ArrowTheme), typeof(NodeArrowTheme), typeof(TreeView), defaultValue: NodeArrowTheme.Default, propertyChanged: (bo, ov, nv) => ((TreeView)bo).OnArrowThemeChanged());

    private StackLayout _root = new StackLayout { Spacing = 0 };

    public TreeView()
    {
        this.Content = _root;
    }

    public IEnumerable ItemsSource { get => (IEnumerable)this.GetValue(ItemsSourceProperty); set => this.SetValue(ItemsSourceProperty, value); }
    public DataTemplate ItemTemplate { get => (DataTemplate)this.GetValue(ItemTemplateProperty); set => this.SetValue(ItemTemplateProperty, value); }
    public NodeArrowTheme ArrowTheme { get => (NodeArrowTheme)this.GetValue(ArrowThemeProperty); set => this.SetValue(ArrowThemeProperty, value); }

    protected virtual void OnItemsSourceSetting(IEnumerable oldValue, IEnumerable newValue)
    {
        if (oldValue is INotifyCollectionChanged oldItemsSource)
        {
            oldItemsSource.CollectionChanged -= this.OnItemsSourceChanged;
        }

        if (newValue is INotifyCollectionChanged newItemsSource)
        {
            newItemsSource.CollectionChanged += this.OnItemsSourceChanged;
        }
    }

    protected virtual void OnItemsSourceSet()
    {
        this.Render();
    }

    private protected virtual void OnItemsSourceChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                {
                    foreach (var item in e.NewItems!)
                    {
                        _root.Children.Insert(e.NewStartingIndex, new TreeViewNodeView((IHasChildrenTreeViewNode)item, this.ItemTemplate, this.ArrowTheme));
                    }
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                {
                    foreach (var item in e.OldItems!)
                    {
                        _root.Children.Remove(_root.Children.FirstOrDefault(x => ((View)x).BindingContext == item));
                    }
                }
                break;
            default:
                this.Render();
                break;
        }
    }

    protected virtual void OnItemTemplateChanged()
    {
        // TODO: Some optimizations
        // Eventually
        this.Render();
    }

    void Render()
    {
        _root.Children.Clear();

        if (this.ItemsSource == null)
        {
            return;
        }

        foreach (var item in this.ItemsSource)
        {
            if (item is IHasChildrenTreeViewNode node)
            {
                _root.Children.Add(new TreeViewNodeView(node, this.ItemTemplate, this.ArrowTheme));
            }
        }
    }

    protected virtual void OnArrowThemeChanged()
    {
        foreach (TreeViewNodeView treeViewNodeView in _root.Children.Where(x => x is TreeViewNodeView))
        {
            treeViewNodeView.UpdateArrowTheme(this.ArrowTheme);
        }
    }
}

public class TreeViewNodeView : ContentView
{
    protected ImageButton extendButton;
    protected StackLayout slChildrens;
    protected IHasChildrenTreeViewNode Node { get; }
    protected DataTemplate ItemTemplate { get; }
    protected NodeArrowTheme ArrowTheme { get; }

    public TreeViewNodeView(IHasChildrenTreeViewNode node, DataTemplate itemTemplate, NodeArrowTheme theme)
    {
        var sl = new StackLayout { Spacing = 0 };
        this.BindingContext = this.Node = node;
        this.ItemTemplate = itemTemplate;
        this.ArrowTheme = theme;
        this.Content = sl;

        slChildrens = new StackLayout { IsVisible = node.IsExtended, Margin = new Thickness(10, 0, 0, 0), Spacing = 0 };

        extendButton = new ImageButton
        {
            Source = this.GetArrowSource(theme),
            VerticalOptions = LayoutOptions.Center,
            BackgroundColor = Colors.Transparent,
            Opacity = node.IsLeaf ? 0 : 1, // Using opacity instead isvisible to keep alignment
            Rotation = node.IsExtended ? 0 : -90,
            HeightRequest = 30,
            WidthRequest = 30,
            CornerRadius = 15
        };

        extendButton.Triggers.Add(new DataTrigger(typeof(ImageButton))
        {
            Binding = new Binding(nameof(this.Node.IsLeaf)),
            Value = true,
            Setters = { new Setter { Property = ImageButton.OpacityProperty, Value = 0 } }
        });

        extendButton.Triggers.Add(new DataTrigger(typeof(ImageButton))
        {
            Binding = new Binding(nameof(this.Node.IsLeaf)),
            Value = false,
            Setters = { new Setter { Property = ImageButton.OpacityProperty, Value = 1 } }
        });

        extendButton.Triggers.Add(new DataTrigger(typeof(ImageButton))
        {
            Binding = new Binding(nameof(this.Node.IsExtended)),
            Value = true,
            EnterActions =
            {
                new GenericTriggerAction<ImageButton>((sender) =>
                {
                    sender.RotateTo(0);
                })
            },
            ExitActions =
            {
                new GenericTriggerAction<ImageButton>((sender) =>
                {
                    sender.RotateTo(-90);
                })
            }
        });

        extendButton.Clicked += (s, e) =>
        {
            node.IsExtended = !node.IsExtended;
            slChildrens.IsVisible = node.IsExtended;

            if (node.IsExtended)
            {
                extendButton.RotateTo(0);

                if (node is ILazyLoadTreeViewNode lazyNode && lazyNode.GetChildren != null && !lazyNode.Children.Any())
                {
                    var lazyChildren = lazyNode.GetChildren(lazyNode);
                    foreach (IHasChildrenTreeViewNode child in lazyChildren)
                    {
                        lazyNode.Children.Add(child);
                    }

                    if (!lazyNode.Children.Any())
                    {
                        extendButton.Opacity = 0;
                        lazyNode.IsLeaf = true;
                    }
                }
            }
            else
            {
                extendButton.RotateTo(-90);
            }
        };

        var content = this.ItemTemplate.CreateContent() as View;

        sl.Children.Add(new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Children =
            {
                extendButton,
                content
            }
        });

        foreach (IHasChildrenTreeViewNode child in node.Children)
        {
            slChildrens.Children.Add(new TreeViewNodeView(child, this.ItemTemplate, theme));
        }

        sl.Children.Add(slChildrens);

        if (this.Node.Children is INotifyCollectionChanged ovservableCollection)
        {
            ovservableCollection.CollectionChanged += this.Children_CollectionChanged;
        }
    }

    private void Children_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (var item in e.NewItems!)
            {
                slChildrens.Children.Insert(e.NewStartingIndex, new TreeViewNodeView((IHasChildrenTreeViewNode)item, this.ItemTemplate, this.ArrowTheme));
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            foreach (var item in e.OldItems!)
            {
                slChildrens.Children.Remove(slChildrens.Children.FirstOrDefault(x => ((View)x).BindingContext == item));
            }
        }
    }

    public void UpdateArrowTheme(NodeArrowTheme theme)
    {
        extendButton.Source = this.GetArrowSource(theme);

        if (slChildrens.Any())
        {
            foreach (IView? child in slChildrens.Children)
            {
                if (child is TreeViewNodeView treeViewNodeView)
                {
                    treeViewNodeView.UpdateArrowTheme(theme);
                }
            }
        }
    }

    protected virtual ImageSource GetArrowSource(NodeArrowTheme theme)
    {
        if (theme == NodeArrowTheme.Default)
        {
            return this.GetImageSource(Application.Current!.RequestedTheme == AppTheme.Dark ? "down_light.png" : "down_dark.png");
        }
        else
        {
            return theme == NodeArrowTheme.Light ? this.GetImageSource("down_light.png") : this.GetImageSource("down_dark.png");
        }
    }

    protected ImageSource GetImageSource(string fileName)
    {
        return
            ImageSource.FromResource("ExampleFramework.Tooling.Maui.Controls.TreeView.Resources." + fileName, this.GetType().Assembly);
    }
}

public enum NodeArrowTheme
{
    Default,
    Light,
    Dark
}

using System.Collections;
using System.Collections.Specialized;
using Microsoft.Maui.Controls.Shapes;

namespace ExampleFramework.Tooling.Maui.Controls.TreeView;

/// <summary>
/// This control is based on the original code here: https://github.com/enisn/TreeView.Maui
/// </summary>
public class TreeView : ContentView
{
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(TreeView), null, propertyChanging: (b, o, n) => ((TreeView)b).OnItemsSourceSetting((IEnumerable)o, (IEnumerable)n), propertyChanged: (b, o, v) => ((TreeView)b).OnItemsSourceSet());
    public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(TreeView), new DataTemplate(typeof(DefaultTreeViewNodeView)), propertyChanged: (b, o, n) => ((TreeView)b).OnItemTemplateChanged());
    public static readonly BindableProperty ArrowThemeProperty = BindableProperty.Create(nameof(ArrowTheme), typeof(NodeArrowTheme), typeof(TreeView), defaultValue: NodeArrowTheme.Default, propertyChanged: (bo, ov, nv) => ((TreeView)bo).OnArrowThemeChanged());
    public static readonly BindableProperty SelectedItemColorProperty = BindableProperty.Create(nameof(SelectedItemColor), typeof(Color), typeof(TreeView), defaultValue: Color.FromArgb("#512BD4"), propertyChanged: (bo, ov, nv) => ((TreeView)bo).OnPropertyChanged(nameof(SelectedItemColor)));
    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(TreeViewNode), typeof(TreeView), defaultValue: null, propertyChanged: (bo, ov, nv) => ((TreeView)bo).OnSelectedItemChanged(ov, nv));

    private StackLayout _root = new StackLayout { Spacing = 0 };

    public static readonly uint ExpandRotateAnimationLength = 100;   // 100ms rotate animation

    public TreeView()
    {
        Content = _root;
    }

    public IEnumerable ItemsSource { get => (IEnumerable)this.GetValue(ItemsSourceProperty); set => this.SetValue(ItemsSourceProperty, value); }
    public DataTemplate ItemTemplate { get => (DataTemplate)this.GetValue(ItemTemplateProperty); set => this.SetValue(ItemTemplateProperty, value); }
    public NodeArrowTheme ArrowTheme { get => (NodeArrowTheme)this.GetValue(ArrowThemeProperty); set => this.SetValue(ArrowThemeProperty, value); }
    public Color SelectedItemColor { get => (Color)this.GetValue(SelectedItemColorProperty); set => this.SetValue(SelectedItemColorProperty, value); }
    public TreeViewNode? SelectedItem { get => (TreeViewNode)this.GetValue(SelectedItemProperty); set => this.SetValue(SelectedItemProperty, value); }

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
                        _root.Children.Insert(e.NewStartingIndex, new TreeViewNodeView(this, (IHasChildrenTreeViewNode)item, 0));
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
                _root.Children.Add(new TreeViewNodeView(this, node, 0));
            }
        }
    }

    protected virtual void OnArrowThemeChanged()
    {
        foreach (TreeViewNodeView treeViewNodeView in _root.Children.Where(x => x is TreeViewNodeView))
        {
            treeViewNodeView.UpdateArrowTheme();
        }
    }

    protected virtual void OnSelectedItemChanged(object? ov, object? nv)
    {
        if (ov is TreeViewNode oldSelectedTreeViewNode)
        {
            oldSelectedTreeViewNode.IsSelected = false;
        }

        if (nv is TreeViewNode newSelectedTreeViewNode)
        {
            newSelectedTreeViewNode.IsSelected = true;
        }
    }
}

public class TreeViewNodeView : ContentView
{
    private ImageButton _expandButton;
    private StackLayout _slChildrens;

    protected TreeView TreeView { get; }
    protected IHasChildrenTreeViewNode Node { get; }
    protected int Depth { get; }

    public TreeViewNodeView(TreeView treeView, IHasChildrenTreeViewNode node, int depth)
    {
        var sl = new StackLayout { Spacing = 0 };

        this.TreeView = treeView;
        this.BindingContext = this.Node = node;
        this.Depth = depth;
        this.Content = sl;

        _slChildrens = new StackLayout { IsVisible = node.IsExpanded, Margin = new Thickness(0, 0, 0, 0), Spacing = 0 };

        _expandButton = new ImageButton
        {
            Source = this.GetArrowSource(),
            VerticalOptions = LayoutOptions.Center,
            BackgroundColor = Colors.Transparent,
            Opacity = node.IsLeaf ? 0 : 1, // Using opacity instead isvisible to keep alignment
            Rotation = node.IsExpanded ? 0 : -90,
            HeightRequest = 24,
            WidthRequest = 24,
            Padding = -10,
            Margin = -10,
        };

        _expandButton.Triggers.Add(new DataTrigger(typeof(ImageButton))
        {
            Binding = new Binding(nameof(this.Node.IsLeaf)),
            Value = true,
            Setters = { new Setter { Property = ImageButton.OpacityProperty, Value = 0 } }
        });

        _expandButton.Triggers.Add(new DataTrigger(typeof(ImageButton))
        {
            Binding = new Binding(nameof(this.Node.IsLeaf)),
            Value = false,
            Setters = { new Setter { Property = ImageButton.OpacityProperty, Value = 1 } }
        });

        _expandButton.Triggers.Add(new DataTrigger(typeof(ImageButton))
        {
            Binding = new Binding(nameof(this.Node.IsExpanded)),
            Value = true,
            EnterActions =
            {
                new GenericTriggerAction<ImageButton>((sender) =>
                {
                    sender.RotateTo(0, TreeView.ExpandRotateAnimationLength);
                })
            },
            ExitActions =
            {
                new GenericTriggerAction<ImageButton>((sender) =>
                {
                    sender.RotateTo(-90, TreeView.ExpandRotateAnimationLength);
                })
            }
        });

        _expandButton.Clicked += (s, e) =>
        {
            node.IsExpanded = !node.IsExpanded;
            _slChildrens.IsVisible = node.IsExpanded;

            if (node.IsExpanded)
            {
                _expandButton.RotateTo(0, TreeView.ExpandRotateAnimationLength);

                if (node is ILazyLoadTreeViewNode lazyNode && lazyNode.GetChildren != null && !lazyNode.Children.Any())
                {
                    var lazyChildren = lazyNode.GetChildren(lazyNode);
                    foreach (IHasChildrenTreeViewNode child in lazyChildren)
                    {
                        lazyNode.Children.Add(child);
                    }

                    if (!lazyNode.Children.Any())
                    {
                        _expandButton.Opacity = 0;
                        lazyNode.IsLeaf = true;
                    }
                }
            }
            else
            {
                _expandButton.RotateTo(-90, TreeView.ExpandRotateAnimationLength);
            }
        };

        var content = (View)treeView.ItemTemplate.CreateContent();

        var nodeLine = new Border
        {
            BackgroundColor = Colors.Transparent,
            Padding = new Thickness(10 * depth, 0, 0, 0),
            Margin = new Thickness(5),
            StrokeThickness = 0,
            StrokeShape = new RoundRectangle { CornerRadius = 8 },

            Content = new StackLayout
            {
                Spacing = 0,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    _expandButton,
                    content
                }
            }
        };

        this.AddSelectionTriggers(nodeLine);

        sl.Children.Add(nodeLine);

        foreach (IHasChildrenTreeViewNode child in node.Children)
        {
            _slChildrens.Children.Add(new TreeViewNodeView(treeView, child, this.Depth + 1));
        }

        sl.Children.Add(_slChildrens);

        if (this.Node.Children is INotifyCollectionChanged ovservableCollection)
        {
            ovservableCollection.CollectionChanged += this.Children_CollectionChanged;
        }
    }

    private void AddSelectionTriggers(Border nodeLine)
    {
        Color selectionColor = this.TreeView.SelectedItemColor;

        nodeLine.Triggers.Add(new DataTrigger(typeof(Border))
        {
            Binding = new Binding(nameof(TreeViewNode.IsSelected)),
            Value = true,
            Setters =
            {
                new Setter
                {
                    Property = Border.BackgroundColorProperty,
                    Value = selectionColor,
                }
            }
        });

        nodeLine.Triggers.Add(new DataTrigger(typeof(Border))
        {
            Binding = new Binding(nameof(TreeViewNode.IsSelected)),
            Value = false,
            Setters =
            {
                new Setter
                {
                    Property = Border.BackgroundColorProperty,
                    Value = Colors.Transparent
                }
            }
        });
    }

    private void Children_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (var item in e.NewItems!)
            {
                _slChildrens.Children.Insert(e.NewStartingIndex, new TreeViewNodeView(this.TreeView, (IHasChildrenTreeViewNode)item, this.Depth));
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            foreach (var item in e.OldItems!)
            {
                _slChildrens.Children.Remove(_slChildrens.Children.FirstOrDefault(x => ((View)x).BindingContext == item));
            }
        }
    }

    public void UpdateArrowTheme()
    {
        _expandButton.Source = this.GetArrowSource();

        if (_slChildrens.Any())
        {
            foreach (IView? child in _slChildrens.Children)
            {
                if (child is TreeViewNodeView treeViewNodeView)
                {
                    treeViewNodeView.UpdateArrowTheme();
                }
            }
        }
    }

    protected virtual ImageSource GetArrowSource()
    {
        NodeArrowTheme theme = this.TreeView.ArrowTheme;

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

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TreeView.Maui.Core;

namespace ExampleBookTest
{
	public class MainPageViewModel : BindableObject, INotifyPropertyChanged
    {
        private String node; 
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<TreeViewNode> Nodes { get; set; } = new();
        public String SelectedNodeContent {
            get
            {
                return node;
            }
            set
            {
                if (node != value)
                {
                    node = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedNodeContent"));
                }

            }
        }

        public MainPageViewModel()
        {
            SelectedNodeContent = "default";

            Nodes.Add(new ("A")
            {
                Children =
            {
                new TreeViewNode("A.1"),
                new TreeViewNode("A.2"),
            }
            });
            Nodes.Add(new TreeViewNode("B")
            {
                Children =
            {
                new TreeViewNode("B.1")
                {
                    Children =
                    {
                        new TreeViewNode("B.1.a"),
                        new TreeViewNode("B.1.b"),
                        new TreeViewNode("B.1.c"),
                        new TreeViewNode("B.1.d"),

                    }
                },
                new TreeViewNode("B.2"),
            }
            });
            Nodes.Add(new TreeViewNode("C"));
            Nodes.Add(new TreeViewNode("D"));
        }


    }
}


using Microsoft.VisualStudio.Settings;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace VsSettingsStoreVisualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SettingsStore settings;

        public MainWindow()
        {
            InitializeComponent();

            var devenvPath = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv.exe";
            settings = ExternalSettingsManager
                .CreateForApplication(devenvPath)
                .GetReadOnlySettingsStore(SettingsScope.Configuration);

            var root = new FolderTreeViewItem() {
                Text = "",
                FullPath = ""
            };

            root.Selected += Item_Selected;
            tree.Items.Add(root);

            PrintCollections(root);
        }

        private void Item_Selected(object sender, RoutedEventArgs e)
        {
            if (sender != tree.SelectedItem)
                return;

            var item = sender as FolderTreeViewItem;
            var properties = settings.GetPropertyNames(item.FullPath);

            list.ItemsSource = properties
                .Select(p => new ListItem
                {
                    Name = p,
                    Value = GetValue(p, item)
                });
        }

        private string GetValue(string propertyName, FolderTreeViewItem item)
        {
            var type = settings.GetPropertyType(item.FullPath, propertyName);

            switch (type)
            {
                case SettingsType.Invalid:
                    return "<INVALID>";
                case SettingsType.Int32:
                    return settings.GetInt32(item.FullPath, propertyName).ToString();
                case SettingsType.Int64:
                    return settings.GetInt64(item.FullPath, propertyName).ToString();
                    break;
                case SettingsType.String:
                    return settings.GetString(item.FullPath, propertyName);
                    break;
                case SettingsType.Binary:
                    return settings.GetString(item.FullPath, propertyName);
                default:
                    throw new ArgumentException();
            }
            
        }

        private void PrintCollections(FolderTreeViewItem parentNode)
        {
            var collections = settings.GetSubCollectionNames(parentNode.FullPath);

            foreach (var collectionName in collections)
            {
                var fullCollectionName = GetFullName(parentNode.FullPath, collectionName);

                var item =new FolderTreeViewItem()
                    {
                        Text = collectionName,
                        FullPath = fullCollectionName
                    };

                item.Selected += Item_Selected;
                parentNode.Items.Add(item);
                
                PrintCollections(item);
            }
        }
        private static string GetFullName(string parentName, string name)
        {
            if (String.IsNullOrWhiteSpace(parentName))
                return name;

            return String.Join(@"\", parentName, name);
        }

    }
}

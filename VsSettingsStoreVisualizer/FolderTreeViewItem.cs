using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace VsSettingsStoreVisualizer
{
    class FolderTreeViewItem : TreeViewItem
    {
        Image _image = null;
        TextBlock _textBlock = null;

        public string Text
        {
            get { return _textBlock.Text; }
            set { _textBlock.Text = value; }
        }

        public string FullPath { get; set; }

        public FolderTreeViewItem() {
            CreateTreeViewItemTemplate();
        }

        private void CreateTreeViewItemTemplate()
        {
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;
            _image = new Image();
            _image.Source = new BitmapImage(new Uri("pack://application:,,/folder.gif"));
            _image.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            _image.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            _image.Width = 16;
            _image.Height = 16;
            _image.Margin = new System.Windows.Thickness(2);
            stack.Children.Add(_image);
            _textBlock = new TextBlock();
            _textBlock.Margin = new System.Windows.Thickness(2);
            _textBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            stack.Children.Add(_textBlock);
            Header = stack;
        }
    }
}

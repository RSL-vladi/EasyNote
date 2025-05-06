using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EasyNote.Properties;


namespace EasyNote
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ICON();
        }
        private void ICON()
        {
            Uri iconUri = new("pack://application:,,,/Resources/EasyNoteLogo.ico", UriKind.Absolute);
            var bitmap = new BitmapImage(iconUri);

            this.Icon = bitmap;
        }
        private int _categoryCounter = 1;

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            var newCategory = new TextBlock
            {
                Text = $"Kategorie {_categoryCounter++}",
                FontSize = 14,
                Foreground = Brushes.White,
                Margin = new Thickness(5),
                Cursor = Cursors.Hand
            };

            CategoryPanel.Children.Add(newCategory);
        }
        private int noteCounter = 1;

        private void NewNoteButton_Click(object sender, RoutedEventArgs e)
        {
            var contentBox = new TextBox
            {
                AcceptsReturn = true,
                TextWrapping = TextWrapping.Wrap,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Foreground = Brushes.White,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Margin = new Thickness(0),
                Padding = new Thickness(10),
                FontSize = 16,
                FontFamily = new FontFamily("Arial"),
                FontStyle = FontStyles.Normal,
                FontWeight = FontWeights.Normal,
                Text = "Neue Notiz"
            };

            var textBoxBorder = new Border
            {
                CornerRadius = new CornerRadius(10, 10, 10, 0),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#383838")),
                Child = contentBox,
                Margin = new Thickness(0)
            };

            var scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = textBoxBorder
            };

            var tab = new TabItem
            {
                Content = scrollViewer
            };

            var headerGrid = new Grid
            {
                Width = 180,
                Height = 30
            };
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var headerText = new TextBlock
            {
                Text = $"Neue Notiz {noteCounter}",
                Foreground = Brushes.White,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                TextTrimming = TextTrimming.CharacterEllipsis,
                Margin = new Thickness(10, 0, 0, 0)
            };

            var closeBtn = new Button
            {
                Width = 20,
                Height = 20,
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Padding = new Thickness(0),
                Margin = new Thickness(0),
                Focusable = false,
                Cursor = Cursors.Hand,
                Template = new ControlTemplate(typeof(Button))
                {
                    VisualTree = new FrameworkElementFactory(typeof(Image), "img")
                }
            };

            var image = new BitmapImage(new Uri("pack://application:,,,/Resources/Cross.png"));
            closeBtn.ApplyTemplate();
            var presenter = (Image)closeBtn.Template.FindName("img", closeBtn);
            if (presenter != null)
            {
                presenter.Source = image;
                presenter.Stretch = Stretch.Uniform;
                presenter.Width = 20;
                presenter.Height = 20;
            }

            closeBtn.Click += (s, ev) =>
            {
                var result = MessageBox.Show("Notiz speichern?", "Speichern", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Cancel) return;
                if (result == MessageBoxResult.Yes)
                    SaveNoteContent(contentBox.Text);

                NoteTabControl.Items.Remove(tab);
                SaveButton.Visibility = Visibility.Collapsed;
                DeleteButton.Visibility = Visibility.Collapsed;
            };

            contentBox.TextChanged += (s, ev) =>
            {
                var firstLine = contentBox.Text.Split('\n').FirstOrDefault()?.Trim();
                if (!string.IsNullOrWhiteSpace(firstLine))
                    headerText.Text = firstLine;
            };

            Grid.SetColumn(headerText, 0);
            Grid.SetColumn(closeBtn, 1);
            headerGrid.Children.Add(headerText);
            headerGrid.Children.Add(closeBtn);

            var headerBorder = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#383838")),
                BorderThickness = new Thickness(2.5),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#383838")),
                CornerRadius = new CornerRadius(10, 10, 10, 0),
                Child = headerGrid
            };

            tab.Header = headerBorder;

            NoteTabControl.Items.Add(tab);
            NoteTabControl.SelectedItem = tab;
            noteCounter++;

            SaveButton.Visibility = Visibility.Visible;
            DeleteButton.Visibility = Visibility.Visible;
        }

        private void SaveNoteButton_Click(object sender, RoutedEventArgs e)
        {
            if (NoteTabControl.SelectedItem is TabItem tab &&
                tab.Content is ScrollViewer scroll &&
                scroll.Content is Border border &&
                border.Child is TextBox content)
            {
                SaveNoteContent(content.Text);
            }
        }

        private void DeleteNoteButton_Click(object sender, RoutedEventArgs e)
        {
            if (NoteTabControl.SelectedItem is TabItem tab)
            {
                var result = MessageBox.Show("Notiz wirklich löschen?", "Löschen", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    NoteTabControl.Items.Remove(tab);
                    SaveButton.Visibility = Visibility.Collapsed;
                    DeleteButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        private static void SaveNoteContent(string text)
        {
            string fileName = $"Note_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            File.WriteAllText(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName), text);
            MessageBox.Show("Notiz wurde gespeichert.");
        }
        private void NoteTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NoteTabControl.SelectedItem is TabItem)
            {
                SaveButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Visible;
            }
            else
            {
                SaveButton.Visibility = Visibility.Collapsed;
                DeleteButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}
        
    


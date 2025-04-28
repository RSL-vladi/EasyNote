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
            Uri iconUri = new Uri("pack://application:,,,/Resources/EasyNoteLogo.ico", UriKind.Absolute);
            var bitmap = new BitmapImage(iconUri);

            this.Icon = bitmap;
        }
    }
}
using System.Windows;
using System.Windows.Input;

namespace SimpleTimer.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // HINT::Missing import. ADDITIONALLY type ambiguity: type name (ViewModel) equals namespace (ViewModel).
            // HINT::Add using SimpleTimer.ViewModel inside namespace or FQN type to emphasize type name: new ViewModel.ViewModel() 
            // or rename namespace/type: use plural for namespace: Models, ViewModels. I recommend renaming the namespace 
            // (I did it for you)
            // this.DataContext = new ViewModel();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}

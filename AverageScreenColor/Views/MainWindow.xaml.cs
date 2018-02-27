using System.Windows;
using System.Windows.Controls;
using AverageScreenColor.Utility;
using AverageScreenColor.ViewModels;

namespace AverageScreenColor.Views
{
    /// <inheritdoc cref="Window" />
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel mwvm && sender is RadioButton radioButton)
            {
                if (ReferenceEquals(radioButton, _radioButtonActive))
                    mwvm.CaptureMode = CaptureMode.ActiveScreen;
                else if (ReferenceEquals(radioButton, _radioButtonAll))
                    mwvm.CaptureMode = CaptureMode.AllScreens;
                else if (ReferenceEquals(radioButton, _radioButtonSpecific))
                    mwvm.CaptureMode = CaptureMode.SpecificScreen;
            }
        }
    }
}

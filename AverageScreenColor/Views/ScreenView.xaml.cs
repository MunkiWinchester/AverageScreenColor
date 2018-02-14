using System.Windows;
using System.Windows.Controls;
using AverageScreenColor.Utility;

namespace AverageScreenColor.Views
{
    /// <summary>
    /// Interaction logic for ScreenView.xaml
    /// </summary>
    public partial class ScreenView : UserControl
    {
        public static readonly DependencyProperty LabelContentProperty = DependencyProperty.Register(
            nameof(ScreenDisplayItem), typeof(ScreenDisplayItem), typeof(ScreenView), new PropertyMetadata(default(ScreenDisplayItem)));

        public ScreenDisplayItem ScreenDisplayItem
        {
            get => (ScreenDisplayItem) GetValue(LabelContentProperty);
            set => SetValue(LabelContentProperty, value);
        }

        public ScreenView()
        {
            InitializeComponent();
        }
    }
}

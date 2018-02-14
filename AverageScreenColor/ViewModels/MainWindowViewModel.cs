using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AverageScreenColor.Utility;
using WpfUtility.Services;

namespace AverageScreenColor.ViewModels
{
    public class MainWindowViewModel  : ObservableObject
    {
        private readonly Interaction _interaction;
        private SolidColorBrush _dominantBrush = new SolidColorBrush(Color.FromRgb(255,0,255));
        private BitmapImage _bitmapImage = new BitmapImage();
        private bool _captureAllScreens = true;
        private ObservableCollection<ScreenDisplayItem> _screenDisplayItems;

        public SolidColorBrush DominantBrush
        {
            get => _dominantBrush;
            set
            {
                _dominantBrush = value;
                OnPropertyChanged();
            }
        }

        public BitmapImage BitmapImage
        {
            get => _bitmapImage;
            set
            {
                _bitmapImage = value;
                OnPropertyChanged();
            }
        }

        public bool CaptureAllScreens
        {
            get => _captureAllScreens;
            set
            {
                _captureAllScreens = value;
                _interaction.AllScreens = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ScreenDisplayItem> ScreenDisplayItems
        {
            get => _screenDisplayItems;
            set => SetField(ref _screenDisplayItems, value);
        }

        public ICommand LoadAmbientColorCommand => new DelegateCommand(LoadAmbientColor);

        public MainWindowViewModel()
        {
            var img = new BitmapImage(new Uri($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Utility/favicon.png"));
            BitmapImage = img;
            _interaction = new Interaction {AllScreens = _captureAllScreens};
            ScreenDisplayItems = new ObservableCollection<ScreenDisplayItem>(_interaction.LoadScreens());
        }

        private void LoadAmbientColor()
        {
            var color = _interaction.LoadAverageColor();
            DominantBrush = new SolidColorBrush()
            {
                Color = Color.FromArgb(color.Color.A, color.Color.R, color.Color.G, color.Color.B)
            };
            BitmapImage = _interaction.LoadSceen();
        }
    }
}
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AverageScreenColor.Utility;
using WpfUtility.Services;
using CaptureMode = AverageScreenColor.Utility.CaptureMode;

namespace AverageScreenColor.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly Interaction _interaction;
        private SolidColorBrush _dominantBrush = new SolidColorBrush(Color.FromRgb(255, 0, 255));
        private BitmapImage _bitmapImage = new BitmapImage();
        private Timer _timer;
        private ObservableCollection<ScreenDisplayItem> _screenDisplayItems;
        private CaptureMode _captureMode = Utility.CaptureMode.AllScreens;
        private bool _automaticallyRefresh
            ;

        public CaptureMode CaptureMode
        {
            get => _captureMode;
            set
            {
                _captureMode = value;
                OnPropertyChanged();
            }
        }

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

        public bool AutomaticallyRefresh
        {
            get => _automaticallyRefresh;
            set
            {
                _automaticallyRefresh = value;
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
            _interaction = new Interaction {AllScreens = true};
            ScreenDisplayItems = new ObservableCollection<ScreenDisplayItem>(_interaction.LoadScreens());

            _timer = new Timer();
            _timer.Tick += Refresh;
            _timer.Interval = (int) TimeSpan.FromSeconds(1).TotalMilliseconds;
            _timer.Start();
        }

        private void Refresh(object sender, EventArgs e)
        {
            if(_automaticallyRefresh)
                LoadAmbientColor();
        }

        private void LoadAmbientColor()
        {
            var specific = ScreenDisplayItems.FirstOrDefault(s => s.IsChecked);
            var color = _interaction.LoadAverageColor(CaptureMode, specific);
            DominantBrush = new SolidColorBrush()
            {
                Color = Color.FromArgb(color.Color.A, color.Color.R, color.Color.G, color.Color.B)
            };
            BitmapImage = _interaction.LoadScreen(CaptureMode, specific);
        }
    }
}
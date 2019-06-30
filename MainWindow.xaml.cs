using System.Windows;

namespace _4Dtest_2
{
    public partial class MainWindow : Window
    {
        public enum ImageType  
        {
            GrayScale,
            RGB
        }
        private ImageType imageType;

        private RGBFilter rgbFilter;
        private GrayScaleFilter grayScaleFilter;
        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            grayScaleFilter = new GrayScaleFilter();
            rgbFilter = new RGBFilter();
        }

        private void FirstButton_Click(object sender, RoutedEventArgs e)
        {
            firstImage.Source = rgbFilter.SampleImage;
            imageType = ImageType.RGB;
        }

        private void SecondButton_Click(object sender, RoutedEventArgs e)
        {
            grayScaleFilter.GenerateRandomImage();
            firstImage.Source = grayScaleFilter.ColorImage(grayScaleFilter.SampleArray);
            imageType = ImageType.GrayScale;
        }

        private void ThirdButton_Click(object sender, RoutedEventArgs e)
        {
            if (imageType == ImageType.RGB)
            {
                secondImage.Source = rgbFilter.ColorImage(rgbFilter.ApplyFirstFilter());
            }
            else
            {
                secondImage.Source = grayScaleFilter.ColorImage(grayScaleFilter.ApplyFirstFilter());
            }

        }

        private void ForthButton_Click(object sender, RoutedEventArgs e)
        {
            if (imageType == ImageType.RGB)
            {
                secondImage.Source = rgbFilter.ColorImage(rgbFilter.ApplySecondFilter());
            }
            else
            {
                secondImage.Source = grayScaleFilter.ColorImage(grayScaleFilter.ApplySecondFilter());
            }
        }
    }
}

///Interface which define the function that i will be defining in my both filtering class

using System.Windows.Media.Imaging;

namespace _4Dtest_2
{
    public interface IFilter
    {
        byte[] ApplyFirstFilter();
        byte[] ApplySecondFilter();
        void FillBorders(byte[] result);
        BitmapSource ColorImage(byte[] image);
    }
}

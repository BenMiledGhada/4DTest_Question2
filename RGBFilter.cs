using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace _4Dtest_2
{
    public class RGBFilter:IFilter
    {
        public BitmapSource SampleImage;

        private int height;

        private int width;

        private int stride;

        private int size;

        byte[] pixelArray;

        private enum Color { red, green, blue }

        public RGBFilter()
        {
            GettingImageProperties();
        }

        private void GettingImageProperties()
        {
            string sampleImagePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "Lena.jpg");

            SampleImage = new BitmapImage(new Uri(sampleImagePath));

            height = SampleImage.PixelHeight;

            width = SampleImage.PixelWidth;

            stride = width * 4;

            size = height * stride;

            pixelArray = new byte[size];

            SampleImage.CopyPixels(pixelArray, stride, 0);
        }

        public byte[] ApplyFirstFilter()
        {
            byte[] result = new byte[size];

            for (int j = 1; j < height - 1; j++)
            {
                for (int i = 1; i < width - 1; i++)
                {
                    int position = GetPosition(i, j);

                    int leftPixel = GetPosition(i - 1, j);
                    int rightPixel = GetPosition(i + 1, j);
                    int upPixel = GetPosition(i, j + 1);
                    int bottomPixel = GetPosition(i, j - 1);

                    byte average = GetBrightnessAverage(pixelArray, leftPixel, rightPixel, upPixel, bottomPixel);
                    int difference = (int)(GetPixelBrightness(pixelArray, position) - average);

                    if ((difference < 10) || (difference > 10))
                    {
              
                        result[position] = AverageColor(pixelArray, leftPixel, rightPixel, upPixel, bottomPixel, Color.red);
                        result[position + 1] = AverageColor(pixelArray, leftPixel, rightPixel, upPixel, bottomPixel, Color.green);
                        result[position + 2] = AverageColor(pixelArray, leftPixel, rightPixel, upPixel, bottomPixel, Color.blue);
                    }
                    else
                    {
                        result[position] = pixelArray[position];
                        result[position + 1] = pixelArray[position + 1];
                        result[position + 2] = pixelArray[position + 2];
                    }
                }
            }
            FillBorders(result);
            return result;
        }

        public byte[] ApplySecondFilter()
        {
            byte[] result = new byte[size];

            for (int j = 1; j < height - 1; j++)
            {
                for (int i = 1; i < width - 1; i++)
                {
                    int position = GetPosition(i, j);
                    int leftPixel = GetPosition(i - 1, j);
                    int rightPixel = GetPosition(i + 1, j);
                    int upPixel = GetPosition(i, j + 1);
                    int bottomPixel = GetPosition(i, j - 1);
                    int leftUpPixel = GetPosition(i - 1, j + 1);
                    int rightUpPixel = GetPosition(i + 1, j + 1);
                    int leftDownPixel = GetPosition(i - 1, j - 1);
                    int rightDownPixel = GetPosition(i + 1, j - 1);
                    byte average = GetBrightnessAverage(pixelArray, position, leftPixel, rightPixel, upPixel, bottomPixel, leftUpPixel, rightUpPixel, leftDownPixel, rightDownPixel);
                    int difference = (int)(GetPixelBrightness(pixelArray, position) - average);

                    if ((difference < 10) || (difference > 10))
                    {
                        result[position] = AverageColor(pixelArray, position, leftPixel, rightPixel, upPixel, bottomPixel, leftUpPixel, rightUpPixel, leftDownPixel, rightDownPixel, Color.red);
                        result[position + 1] = AverageColor(pixelArray, position, leftPixel, rightPixel, upPixel, bottomPixel, leftUpPixel, rightUpPixel, leftDownPixel, rightDownPixel, Color.green);
                        result[position + 2] = AverageColor(pixelArray, position, leftPixel, rightPixel, upPixel, bottomPixel, leftUpPixel, rightUpPixel, leftDownPixel, rightDownPixel, Color.blue);
                    }
                    else
                    {
                        result[position] = pixelArray[position];
                        result[position + 1] = pixelArray[position + 1];
                        result[position + 2] = pixelArray[position + 2];
                    }
                }
            }

            FillBorders(result);
            return result;
        }

        /// <summary>
        /// Get the brightness value from RGB Color 
        /// Reference of the formula : https://www.w3.org/Graphics/Color/sRGB
        /// </summary>
        /// <returns></returns>
        private byte GetPixelBrightness(byte[] array, int position)
        {
            return (byte)(0.2126 * array[position] + 0.7152 * array[position + 1] + 0.0722 * array[position + 2]);
        }

        /// <summary>
        /// Averaging the brightness values from an array ( after sorting it following the question 2-2) over the number of positions entered as a parameter
        /// the function has 2 definitions depending on the number of inputs (either used in the first filter or the 2nd filter)
        /// </summary>
        private byte GetBrightnessAverage(byte[] array, int p1, int p2, int p3, int p4)
        {
            byte b1 = GetPixelBrightness(array, p1);
            byte b2 = GetPixelBrightness(array, p2);
            byte b3 = GetPixelBrightness(array, p3);
            byte b4 = GetPixelBrightness(array, p4);

            return (byte)((b1 + b2 + b3 + b4) / 4);
        }

        private byte GetBrightnessAverage(byte[] array, int p1, int p2, int p3, int p4, int p5, int p6, int p7, int p8, int p9)
        {
            byte b1 = GetPixelBrightness(array, p1);
            byte b2 = GetPixelBrightness(array, p2);
            byte b3 = GetPixelBrightness(array, p3);
            byte b4 = GetPixelBrightness(array, p4);
            byte b5 = GetPixelBrightness(array, p5);
            byte b6 = GetPixelBrightness(array, p6);
            byte b7 = GetPixelBrightness(array, p7);
            byte b8 = GetPixelBrightness(array, p8);
            byte b9 = GetPixelBrightness(array, p9);

            byte[] brightnessArray = { b1, b2, b3, b4, b5, b6, b7, b8, b9 };

            Array.Sort(brightnessArray);
            byte sum = 0;
            for (int i = 2; i < brightnessArray.Length - 2; i++)
            {
                sum += brightnessArray[i];
            }
            return (byte)(sum/ 5);
        }

        /// <summary>
        /// Averaging the value of each color from a byte array(RGB)
        /// the function has 2 definitions depending on the number of inputs(position)
        /// one for the first method, the second for the second filtering method 
        /// </summary>
        private byte AverageColor(byte[] array, int p1, int p2, int p3, int p4, Color Color)
        {
            int colorDeterminant;
            if (Color == Color.red)
            {
                colorDeterminant = 0;
            }
            else if (Color == Color.green)
            {
                colorDeterminant = 1;
            }
            else
            {
                colorDeterminant = 2;
            }

            return (byte)((array[p1 + colorDeterminant] + array[p2 + colorDeterminant] + array[p3 + colorDeterminant] + array[p4 + colorDeterminant]) / 4);
        }
        private byte AverageColor(byte[] array, int p1, int p2, int p3, int p4, int p5, int p6, int p7, int p8, int p9, Color Color)
        {
            {
                int colorDeterminant;

                if (Color == Color.red)
                {
                    colorDeterminant = 0;
                }
                else if (Color == Color.green)
                {
                    colorDeterminant = 1;
                }
                else
                {
                    colorDeterminant = 2;
                }
                byte average = (byte)((array[p1 + colorDeterminant] + array[p2 + colorDeterminant] + array[p3 + colorDeterminant] + array[p4 + colorDeterminant] +
                    array[p5 + colorDeterminant] + array[p6 + colorDeterminant] + array[p7 + colorDeterminant] + array[p8 + colorDeterminant] + array[p9 + colorDeterminant]) / 9);

                return average;
            }
        }
        private int GetPosition(int i, int j)
        {
            return (j * stride + 4 * i);
        }
        /// <summary>
        /// filling the border of the array fom the original image 
        /// </summary>
        public void FillBorders(byte[] result)
        {
            for (int i = 0; i < width; i++)
            {
                int position = GetPosition(i, 0);
                result[position] = pixelArray[position];
                result[position + 1] = pixelArray[position + 1];
                result[position + 2] = pixelArray[position + 2];

                int position2 = GetPosition(i, height - 1);
                result[position2] = pixelArray[position2];
                result[position2 + 1] = pixelArray[position2 + 1];
                result[position2 + 2] = pixelArray[position2 + 2];
            }

            for (int j = 0; j < height; j++)
            {
                int position = GetPosition(0, j);
                result[position] = pixelArray[position];
                result[position + 1] = pixelArray[position + 1];
                result[position + 2] = pixelArray[position + 2];

                int position2 = GetPosition(width - 1, j);
                result[position2] = pixelArray[position2];
                result[position2 + 1] = pixelArray[position2 + 1];
                result[position2 + 2] = pixelArray[position2 + 2];
            }
        }
        public BitmapSource ColorImage(byte[] image)
        {
            BitmapSource firstImage = BitmapSource.Create(width, height, 96, 96, System.Windows.Media.PixelFormats.Bgr32, null, image, stride);

            return firstImage;
        }
    }
}

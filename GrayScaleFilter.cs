using System;
using System.Windows.Media.Imaging;

namespace _4Dtest_2
{

    public class GrayScaleFilter : IFilter
    {
        private readonly int width;
        private readonly int height;
        public byte[] SampleArray;

        public GrayScaleFilter()
        {
            width = 640;
            height = 480;
        }

        public void GenerateRandomImage()
        {
            byte[] image = new byte[width * height];

            Random random = new Random();

            random.NextBytes(image);

            SampleArray = image;
        }

        public byte[] ApplyFirstFilter()
        {
            byte[] result = new byte[width * height];
            for (int j = 1; j < height - 1; j++)
            {
                for (int i = 1; i < width - 1; i++)
                {
                    int position = (width * j) + i;
                    int leftPixel = (width * j) + (i - 1);
                    int rightPixel = (width * j) + (i + 1);
                    int upPixel = (width * (j + 1)) + i;
                    int bottomPixel = (width * (j - 1)) + i;

                    byte sum = (byte)(SampleArray[rightPixel]
                                      + SampleArray[upPixel]
                                      + SampleArray[bottomPixel]
                                      + SampleArray[leftPixel]);

                    byte average = (byte)(sum / 4);

                    int difference = (int)(SampleArray[position] - average);
                    if ((difference < 10) || (difference > 10))
                    {
                        result[position] = average;
                    }
                    else
                    {
                        result[position] = SampleArray[position];
                    }
                }
            }
            FillBorders(result);
            return result;
        }

        public byte[] ApplySecondFilter()
        {
            byte[] result = new byte[width * height];
            for (int j = 1; j < height - 1; j++)
            {
                for (int i = 1; i < width - 1; i++)
                {
                    int position = (width * j) + i;
                    int leftPixel = (width * j) + (i - 1);
                    int rightPixel = (width * j) + (i + 1);
                    int upPixel = (width * (j + 1)) + i;
                    int bottomPixel = (width * (j - 1)) + i;
                    int leftUpPixel = (width * (j + 1)) + (i - 1);
                    int rightUpPixel = (width * (j + 1)) + (i + 1);
                    int leftDownPixel = (width * (j - 1)) + (i - 1);
                    int rightDownPixel = (width * (j - 1)) + (i + 1);

                    byte[] array = { SampleArray[position],SampleArray[leftDownPixel],SampleArray[leftPixel]
                    ,SampleArray[upPixel],SampleArray[leftUpPixel],SampleArray[rightUpPixel],SampleArray[rightPixel]
                    ,SampleArray[rightDownPixel],SampleArray[bottomPixel]};

                    byte average = SortAndAverage(array);

                    int difference = (int)(SampleArray[position] - average);

                    if ((difference < 10) || (difference > 10))
                    {
                        result[position] = average;
                    }
                    else
                    {
                        result[position] = SampleArray[position];
                    }

                }
            }
            FillBorders(result);
            return result;
        }
        public void FillBorders(byte[] result)
        {
            for (int i = 0; i < width; i++)
            {
                int position = i;
                result[position] = SampleArray[position];
                int position2 = i + width * (height - 1);
                result[position2] = SampleArray[position2];
            }
            for (int j=0; j<height;j++)
            {
                int position = j * width;
                result[position] = SampleArray[position];
                int position2 = j * width + width - 1;
                result[position2] = SampleArray[position2];

            }
        }
        private byte SortAndAverage(byte[] array)
        {
            Array.Sort(array);

            byte sum = 0;
            for (int i = 2; i < array.Length - 2; i++)
            {
                sum += array[i];
            }

            return (byte)(sum / 5);
        }
        public BitmapSource ColorImage(byte[] image)
        {
            BitmapSource firstImage = BitmapSource.Create(width, height, 96, 96, System.Windows.Media.PixelFormats.Gray8, null, image, width);

            return firstImage;
        }
    }
}

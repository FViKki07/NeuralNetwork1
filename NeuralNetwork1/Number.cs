using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
//using static System.Net.Mime.MediaTypeNames;

namespace NeuralNetwork1
{

    /// <summary>
    /// Цифра
    /// </summary>
    public enum NumType : byte { Zero = 0, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Undef };

    public class Number
    {
        /// <summary>
        /// Бинарное представление образа
        /// </summary>
        public bool[,] img = new bool[300, 300];

        /// <summary>
        /// Текущее изображение
        /// </summary>
        public Bitmap currentBitMap;

        //  private int margin = 50;
        private Random rand = new Random();

        /// <summary>
        /// Текущая сгенерированная цифра
        /// </summary>
        public NumType currentNum = NumType.Undef;

        /// <summary>
        /// Количество классов генерируемых фигур (4 - максимум)
        /// </summary>
        public int NumCount { get; set; } = 10;

        /// <summary>
        /// Диапазон смещения центра фигуры (по умолчанию +/- 20 пикселов от центра)
        /// </summary>
        public int FigureCenterGitter { get; set; } = 50;

        /// <summary>
        /// Диапазон разброса размера фигур
        /// </summary>
        public int FigureSizeGitter { get; set; } = 50;

        /// <summary>
        /// Диапазон разброса размера фигур
        /// </summary>
        public int FigureSize { get; set; } = 100;

        /// <summary>
        /// Очистка образа
        /// </summary>
        public void ClearImage()
        {
            for (int i = 0; i < 300; ++i)
                for (int j = 0; j < 300; ++j)
                    img[i, j] = false;
        }

        public Sample GenerateFigure()
        {
            generate_figure();
            double[] input = new double[600];
            for (int i = 0; i < 600; i++)
                input[i] = 0;

            NumType type = currentNum;

            for (int i = 0; i < 300; i++)
                for (int j = 0; j < 300; j++)
                    if (img[i, j])
                    {
                        input[i] += 1;
                        input[300 + j] += 1;
                    }
            return new Sample(input, NumCount, type);
        }

        public bool GetNum(NumType type = NumType.Undef, int num = -1)
        {
            currentNum = type;
            int randomNumber;
            if (num == -1)
                randomNumber = rand.Next(30, 180);
            else randomNumber = num;

            string fileName = $"Images\\{(int)type}_{randomNumber}.jpg";
            using (Stream bmpStream = System.IO.File.Open(fileName, System.IO.FileMode.Open))
            {
                Image image = Image.FromStream(bmpStream);
                currentBitMap = new Bitmap(image);
            }

            for (int x = 0; x < currentBitMap.Width; x++)
            {
                for (int y = 0; y < currentBitMap.Height; y++)
                {
                    System.Drawing.Color pixelColor = currentBitMap.GetPixel(x, y);
                    if(pixelColor.R == 0 && pixelColor.G == 0 && pixelColor.B == 0)
                        img[x, y] = true;
                    else
                        img[x, y] = false;
                }
            }
            return true;
        }

        public void generate_figure(NumType type = NumType.Undef)
        {

            if (type == NumType.Undef || (int)type >= NumCount)
                type = (NumType)rand.Next(NumCount);
            ClearImage();
            switch (type)
            {
                case NumType.Zero: GetNum(NumType.Zero); break;
                case NumType.One: GetNum(NumType.One); break;
                case NumType.Two: GetNum(NumType.Two); break;
                case NumType.Three: GetNum(NumType.Three); break;
                case NumType.Four: GetNum(NumType.Four); break;
                case NumType.Five: GetNum(NumType.Five); break;
                case NumType.Six: GetNum(NumType.Six); break;
                case NumType.Seven: GetNum(NumType.Seven); break;
                case NumType.Eight: GetNum(NumType.Eight); break;
                case NumType.Nine: GetNum(NumType.Nine); break;
                default:
                    type = NumType.Undef;
                    throw new Exception("WTF?!!! Не могу я создать такую фигуру!");
            }
        }

        public void get_random_figure()
        {
            ClearImage();
            generate_figure();

        }

        public Bitmap GenBitmap()
        {
            return currentBitMap;
        }

        public void CreateDataBase()
        {
            string fileName = $"Images\\AllImages.txt";
            using (StreamWriter writer = new StreamWriter(fileName, false))
            {
                double[] input = new double[600];
                for (int k = 0; k < 600; k++)
                    input[k] = 0;
                for (int i = 0; i < 10; i++)
                {
                    for(int j = 1; j < 181; j++)
                    {
                        GetNum((NumType)i,j);

                        NumType type = currentNum;

                        for (int k = 0; k < 300; k++)
                            for (int l = 0; l < 300; l++)
                                if (img[k, l])
                                {
                                    input[k] += 1;
                                    input[300 + l] += 1;
                                }

                        string str =i+";"+j+';'+ string.Join(",", input);
                        writer.WriteLine(str);
                    }
                }
                
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class ColorGenerator
    {
        const string colorPath = @"C:\temp\cardata\";
        const int squareSize = 3;
        int fileSeq = 0;

        List<string> strColorList = new List<string>();

        public ColorGenerator()
        {
            //strColorList.Add("A-4A569DDC2424");
            //strColorList.Add("B-FFEDBCED4264");
            //strColorList.Add("C-F3A183EC6F66");
            //strColorList.Add("D-5f2c8249a09d");
            //strColorList.Add("E-480048C04848");
            //strColorList.Add("F-414D0B727A17");
            //strColorList.Add("G-FC354C0ABFBC");
            //strColorList.Add("H-4B6CB7182848");
            //strColorList.Add("I-E9D362333333");
            //strColorList.Add("J-FFC500C21500");
            //strColorList.Add("K-E4E4D9215F00");
            //strColorList.Add("L-A8CABA5D4157");
            strColorList.Add("A-FF0000FFFFFF");
            strColorList.Add("B-00FF00FFFFFF");
            strColorList.Add("C-0000FFFFFFFF");
            strColorList.Add("D-FFFF00FFFFFF");
            strColorList.Add("E-FF00FFFFFFFF");
            strColorList.Add("F-00FFFFFFFFFF");

            strColorList.Add("A-4A569DFFFFFF");
            strColorList.Add("B-ED4264FFFFFF");
            strColorList.Add("C-EC6F66FFFFFF");
            strColorList.Add("D-49a09dFFFFFF");
            strColorList.Add("E-480048FFFFFF");
            strColorList.Add("F-414D0BFFFFFF");
            strColorList.Add("G-0ABFBCFFFFFF");
            strColorList.Add("H-182848FFFFFF");
            strColorList.Add("I-333333FFFFFF");
            strColorList.Add("J-C21500FFFFFF");
            strColorList.Add("K-215F00FFFFFF");
            strColorList.Add("L-5D4157FFFFFF");
            strColorList.Add("L-5D0234FFFFFF");
            strColorList.Add("L-123456FFFFFF");
            fileSeq = 0;
        }
        public void resetFileSeq() {
            fileSeq = 0;
        }
        public void generateCarPixel(int totalSeq, int daySeq, string carName)
        {
            if (!System.IO.Directory.Exists(colorPath))
            {
                System.IO.Directory.CreateDirectory(colorPath);
            }
            Bitmap bmp = new Bitmap(squareSize, squareSize);
            if (fileSeq == 0)
                bmp = new Bitmap(10, 10);
            int noofpic = 1000;
            string strColor = strColorList[daySeq];
            string batchName = strColor.Split('-')[0];

            Color cr = Color.FromArgb(getGradient(strColor.Split('-')[1].Substring(0, 6), strColor.Split('-')[1].Substring(6, 6), fileSeq, noofpic));
            fileSeq++;
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                graphics.FillRectangle(new SolidBrush(cr), 0, 0, bmp.Width, bmp.Height);
                graphics.Flush();
                graphics.Dispose();
            }

            if (!System.IO.Directory.Exists(string.Format(@"{0}{1}\color\", colorPath, carName)))
            {
                System.IO.Directory.CreateDirectory(string.Format(@"{0}{1}\color\", colorPath, carName));
            }
            bmp.Save(string.Format(@"{0}{1}\color\{2}", colorPath, carName, totalSeq) + ".png", System.Drawing.Imaging.ImageFormat.Png);
        }

        public void generateOnePixel()
        {
            if (!System.IO.Directory.Exists(colorPath))
            {
                System.IO.Directory.CreateDirectory(colorPath);
            }
            Bitmap bmp = new Bitmap(squareSize, squareSize);
            int noofpic = 1000;
            for (int i = 0; i < strColorList.Count; i++)
            {
                string batchName = strColorList[i].Split('-')[0];
                for (int k = 0; k < noofpic; k++)
                {
                    Color cr = Color.FromArgb(getGradient(strColorList[i].Split('-')[1].Substring(0,6), strColorList[i].Split('-')[1].Substring(6, 6), k, noofpic));
                    using (Graphics graphics = Graphics.FromImage(bmp))
                    {
                        graphics.FillRectangle(new SolidBrush(cr), 0, 0, bmp.Width, bmp.Height);
                        graphics.Flush();
                        graphics.Dispose();
                    }
                    bmp.Save(string.Format(@"{0}{1}{2}", colorPath, batchName, k) + ".png", System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }

        public Color GenerateColor(int countInaDay, int seqInaDay, int daySeq)
        {
            string strColor = strColorList[daySeq % 6];
            Color cr = Color.FromArgb(getGradient(strColor.Split('-')[1].Substring(0, 6), strColor.Split('-')[1].Substring(6, 6), seqInaDay, countInaDay));
            return cr;
        }


        private int getGreenToRedGradientByValue(int currentValue, int maxValue)
        {
            int a = 255;
            int r = ((255 * currentValue) / maxValue);
            int g = (255 * (maxValue - currentValue)) / maxValue;
            int b = 0;
            return ((a & 0x0ff) << 24) | ((r & 0x0ff) << 16) | ((g & 0x0ff) << 8) | (b & 0x0ff);
        }


        private int getGradient(string strStart, string strEnd, int currentValue, int maxValue)
        {
            int a = 255;
            int rS = Convert.ToInt32(strStart.Substring(0, 2), 16);
            int rE = Convert.ToInt32(strEnd.Substring(0, 2), 16);
            int gS = Convert.ToInt32(strStart.Substring(2, 2), 16);
            int gE = Convert.ToInt32(strEnd.Substring(2, 2), 16);
            int bS = Convert.ToInt32(strStart.Substring(4, 2), 16);
            int bE = Convert.ToInt32(strEnd.Substring(4, 2), 16);

            float rDelta = (float)(rE - rS) / (float)maxValue;
            float gDelta = (float)(gE - gS) / (float)maxValue;
            float bDelta = (float)(bE - bS) / (float)maxValue;
            int r = rS + (int)(rDelta * currentValue);
            int g = gS + (int)(gDelta * currentValue);
            int b = bS + (int)(bDelta * currentValue);
            return ((a & 0x0ff) << 24) | ((r & 0x0ff) << 16) | ((g & 0x0ff) << 8) | (b & 0x0ff);
        }

    }
}

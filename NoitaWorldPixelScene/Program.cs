using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Drawing;
using System.Drawing.Imaging;

namespace NoitaWorldPixelScene
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument xDoc = new XmlDocument();
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\world_pixel_scenes.xml";

            Console.WriteLine("Testing if xml file exists...");
            if (!File.Exists(path))
            {
                Console.WriteLine("There is no world_pixel_scenes.xml inside this folder!");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Complete\n");

            xDoc.Load(path);


            int highestXPos = 5;
            int lowestXPos = -5;

            int highestYPos = 5;
            int lowestYPos = -5;
            Console.WriteLine("Calculating highest and lowest x/y values of xml...");
            foreach (XmlNode node in xDoc.SelectNodes("PixelScenes/mBufferedPixelScenes/PixelScene"))
            {
                int xPos = int.Parse(node.Attributes["pos_x"].Value);
                int yPos = int.Parse(node.Attributes["pos_y"].Value);

                if (xPos > highestXPos)
                {
                    highestXPos = xPos;
                }
                else if (xPos < lowestXPos)
                {
                    lowestXPos = xPos;
                }

                if (yPos > highestYPos)
                {
                    highestYPos = yPos;
                }
                else if (yPos < lowestYPos)
                {
                    lowestYPos = yPos;
                }
            }
            Console.WriteLine("Complete\n");


            int width = highestXPos + Math.Abs(lowestXPos);
            int height = highestYPos + Math.Abs(lowestYPos);

            // change 20 for a different rectangle width
            int rectangleWidth = 20;
            // change 20 for a different rectangle height
            int rectangleHeight = 20;

            width /= 10;
            width += rectangleWidth + 1;
            height /= 10;
            height += rectangleHeight + 1;

            Bitmap bitmap = new Bitmap(width, height);

            Console.WriteLine("Giving image background color...");
            Graphics gfx = Graphics.FromImage(bitmap);
            // change values inside FromArgb() for a different background color
            SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255));
            {
                gfx.FillRectangle(brush, 0, 0, width, height);
            }
            Console.WriteLine("Complete\n");



            Random rnd = new Random();
            // change 0 for a different font and 7 for different font size
            Font font = new Font(FontFamily.Families[0], 7);
            // change values inside FromArgb() for different font color
            brush = new SolidBrush(Color.FromArgb(0, 0, 0));


            int lowestColorValue = 20;
            int highestColorValue = 240;

            int rGrowth = 10;
            int gGrowth = 20;
            int bGrowth = 30;

            int r = lowestColorValue;
            int g = lowestColorValue;
            int b = lowestColorValue;
            Console.WriteLine("Filling rest of image with data from xml...");
            foreach (XmlNode node in xDoc.SelectNodes("PixelScenes/mBufferedPixelScenes/PixelScene"))
            {
                int xPos = int.Parse(node.Attributes["pos_x"].Value) + Math.Abs(lowestXPos);
                int yPos = int.Parse(node.Attributes["pos_y"].Value) + Math.Abs(lowestYPos);
                string text = node.Attributes["material_filename"].Value;

                text = text.Substring(16, text.Length - 16 - 4);
                if (text.Contains("spliced") == true)
                {
                    text = text.Substring(8, text.Length - 8);
                }

                xPos /= 10;
                yPos /= 10;

                r += rGrowth;
                if (r > highestColorValue)
                    r = lowestColorValue;

                g += gGrowth;
                if (g > highestColorValue)
                    g = lowestColorValue;

                b += bGrowth;
                if (b > highestColorValue)
                    b = lowestColorValue;
                Color color = Color.FromArgb(r, g, b);

                for (int x = xPos; x < xPos + rectangleWidth; x++)
                {
                    for (int y = yPos; y < yPos + rectangleHeight; y++)
                    {
                        bitmap.SetPixel(x, y, color);
                    }
                }

                gfx = Graphics.FromImage(bitmap);
                // change the first 0 to move the text horizontally
                // change the second 0 to move the text vertically
                // remove the rnd.Next(-5, 5) to disable randomness
                // change the -5s and 5s to change the range of the randomness
                gfx.DrawString(text, font, brush, xPos + 0 + rnd.Next(-10, 10), yPos + rectangleHeight + 0 + rnd.Next(-10, 10));
                gfx.Save();
            }
            Console.WriteLine("Complete\n");

            Console.WriteLine("Making image into a file...");
            Image imgFromBmp = (Image)bitmap;
            imgFromBmp.Save("bitmap.png", ImageFormat.Png);
            Console.WriteLine("Complete\n");

            Console.WriteLine("Program done!\nPlease press a key to exit...");


            Console.ReadKey();
        }
    }
}

using BigGustave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertC64Font
{
    public class ConvertMultiColorSpriteToPng
    {
        Pixel[] colors = new Pixel[4];
        const int bytesPerSprite = 8;           //21*3  
        const int nrSpritesPerRow = 16;         //6
        const int nrWidthPixelsPerSprite = 16;  //16*3 : 24 width * 2 pixels for each point
        const int nrHeightPixelsPerSprite = 16; //21*2 : 21 height * 2 pixels for each point

        public ConvertMultiColorSpriteToPng(byte[] font, string outputFilename) {

            int height = 256*32;
            var builder = PngBuilder.Create(nrWidthPixelsPerSprite * nrSpritesPerRow, height, false);  //24 * 2 * 6
            colors[0] = new Pixel(0, 0, 0);
            colors[1] = new Pixel(255, 0, 0);
            colors[2] = new Pixel(0, 255, 0);
            colors[3] = new Pixel(255, 255, 255);

            using var memory = new MemoryStream();

            int counter = 0;
            int bytesToHandle = 65536;
            for (int i = 0; i < bytesToHandle; i += bytesPerSprite)
            {
                byte[] data = new byte[bytesPerSprite];
                Array.Copy(font, i, data, 0, bytesPerSprite);
                SetSprite(builder, data, counter++);
            }

            builder.Save(memory);
            File.WriteAllBytes(outputFilename, memory.ToArray());
        }

        void SetSprite(PngBuilder builder, byte[] data, int position)
        {
            int x = 0, y = 0;
            int row = position / nrSpritesPerRow;
            int col = position % nrSpritesPerRow;
            x = col * nrWidthPixelsPerSprite;
            y = row * nrHeightPixelsPerSprite;
            for (int i = 0; i < 21; i++)
            {
                SetSpriteRow(builder, new byte[3] { data[i * 3], data[(i * 3) + 1], data[(i * 3) + 2] }, x, y + (i * 2));
                SetSpriteRow(builder, new byte[3] { data[i * 3], data[(i * 3) + 1], data[(i * 3) + 2] }, x, y + ((i * 2) + 1));
            }
        }

        void SetSpriteRow(PngBuilder builder, byte[] data, int x, int y)
        {
            int counter = 0;
            for (int j = 0; j < 3; j++)
            {
                for (int i = 3; i >= 0; i--)
                {
                    int firstBit = IsBitSet((i * 2) + 1, data[j]) ? 1 : 0;
                    int secondBit = IsBitSet(i * 2, data[j]) ? 1 : 0;

                    int color = (firstBit << 1) + secondBit;
                    builder.SetPixel(colors[color], x + (counter * 4), y);
                    builder.SetPixel(colors[color], x + (counter * 4) + 1, y);
                    builder.SetPixel(colors[color], x + (counter * 4) + 2, y);
                    builder.SetPixel(colors[color], x + (counter * 4) + 3, y);
                    counter++;
                }
            }
        }

        bool IsBitSet(int nPos, byte b)
        {
            return (((byte)0x1 << (byte)nPos) & b) != 0;
        }

    }
}

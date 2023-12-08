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
        const int bytesPerSpriteRow = 1;
        const int bytesPerSprite = 8;           //21*3  
        const int nrSpritesPerRow = 16;         //6
        const int nrWidthPixelsPerSprite = 16;  //16*3 : 24 width * 2 pixels for each point
        const int nrHeightPixelsPerSprite = 16; //21*2 : 21 height * 2 pixels for each point

        public ConvertMultiColorSpriteToPng(byte[] font, string outputFilename) {

            int height = 256*32;
            var builder = PngBuilder.Create(nrWidthPixelsPerSprite * nrSpritesPerRow, height, false);
            colors[0] = new Pixel(221, 136, 85);         // color 8: orange
            colors[3] = new Pixel(0, 0, 0);         // color 0: black
            colors[2] = new Pixel(255, 255, 255);   // color 1: white
            colors[1] = new Pixel(255, 119, 119);   // color 10: light red

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

            for (int i = 0; i < (nrHeightPixelsPerSprite/2); i++)
            {
                byte[] rowData = new byte[bytesPerSpriteRow];
                Array.Copy(data, i* bytesPerSpriteRow, rowData, 0, bytesPerSpriteRow);

                SetSpriteRow(builder, rowData, x, y + (i * 2));
                SetSpriteRow(builder, rowData, x, y + ((i * 2) + 1));
            }
        }

        void SetSpriteRow(PngBuilder builder, byte[] data, int x, int y)
        {
            int counter = 0;
            for (int j = 0; j < bytesPerSpriteRow; j++)
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

using BigGustave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertC64Font
{
    public class ConvertMultiColorSpriteToPng
    {
        Pixel textColor;
        Pixel backColor;

        public ConvertMultiColorSpriteToPng(byte[] font, string outputFilename, bool totalMemory = false) {

            int height = totalMemory ? 256*32 : 16*3*6;
            var builder = PngBuilder.Create(256, height, false);
            textColor = new Pixel(255, 255, 255);
            backColor = new Pixel(0, 0, 0);

            using var memory = new MemoryStream();

            int counter = 0;
            int bytesToHandle = totalMemory ? 65536 : 2048;
            for (int i = 0; i < bytesToHandle; i += 21*3)
            {
                byte[] data = new byte[21 * 3];
                Array.Copy(font, i, data, 0, 21 * 3);
                SetSprite(builder, data, counter++);
            }

            builder.Save(memory);
            File.WriteAllBytes(outputFilename, memory.ToArray());
        }

        void SetSprite(PngBuilder builder, byte[] data, int position)
        {
            int x = 0, y = 0;
            int row = position / 21*3;
            int col = position % 6;
            x = col * 16;
            y = row * 16;
            for (int i = 0; i < 8; i++)
            {
                SetCharRow(builder, data[i], x, y + (i * 2));
                SetCharRow(builder, data[i], x, y + ((i * 2) + 1));
            }
        }

        void SetCharRow(PngBuilder builder, byte data, int x, int y)
        {
            int counter = 0;
            for (int i = 7; i >= 0; i--)
            {
                if (IsBitSet(i, data))
                {
                    builder.SetPixel(textColor, x + (counter * 2), y);
                    builder.SetPixel(textColor, x + (counter * 2) + 1, y);
                }
                else
                {
                    builder.SetPixel(backColor, x + (counter * 2), y);
                    builder.SetPixel(backColor, x + (counter * 2) + 1, y);
                }

                counter++;
            }
        }

        bool IsBitSet(int nPos, byte b)
        {
            return (((byte)0x1 << (byte)nPos) & b) != 0;
        }

    }
}

using BigGustave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertC64Font
{
    public class ConverterFont8ToPng
    {
        Pixel textColor;
        Pixel backColor;

        public ConverterFont8ToPng(byte[] font, string outputFilename, bool totalMemoryFont = false) {

            int height = totalMemoryFont ? 256*32 : 256;
            var builder = PngBuilder.Create(256, height, false);
            textColor = new Pixel(255, 255, 255);
            backColor = new Pixel(0, 0, 0);

            using var memory = new MemoryStream();

            int counter = 0;
            int bytesToHandle = totalMemoryFont ? 2048 * 32 : 2048;
            for (int i = 0; i < bytesToHandle; i += 8)
            {
                byte[] data = new byte[8] { (byte)font[i], (byte)font[i + 1], (byte)font[i + 2], (byte)font[i + 3], (byte)font[i + 4], (byte)font[i + 5], (byte)font[i + 6], (byte)font[i + 7] };
                SetCharacter(builder, data, counter++);
            }

            builder.Save(memory);
            File.WriteAllBytes(outputFilename, memory.ToArray());
        }

        void SetCharacter(PngBuilder builder, byte[] data, int position)
        {
            int x = 0, y = 0;
            int row = position / 16;
            int col = position % 16;
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

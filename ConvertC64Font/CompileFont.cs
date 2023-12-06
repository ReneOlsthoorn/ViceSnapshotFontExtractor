using BigGustave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertC64Font
{
    // Compile a font from different sources. It basically copies PNG pixels. This class is not much used.

    public class CompileFont
    {
        public PngBuilder builder;

        public void CopyCharacter(Png sourceImage, int sourceX, int sourceY, int destX, int destY, int amount)
        {
            for (int k = 0; k < amount; k++)
            {
                for (int x = 0; x < 16; x++)
                {
                    for (int y = 0; y < 16; y++)
                    {
                        builder.SetPixel(sourceImage.GetPixel((sourceX * 16) + x, (sourceY * 16) + y), (destX * 16) + x, (destY * 16) + y);
                    }
                }
                sourceX++;
                destX++;
            }
        }


        public CompileFont()
        {
            const int imageWidth = 256;
            const int imageHeight = 256;

            using var atariStream = File.OpenRead("c:\\dotnet\\ConvertC64Font\\charset16x16_atari.png");
            Png atariFont = Png.Open(atariStream);

            using var cbm128Stream = File.OpenRead("c:\\dotnet\\ConvertC64Font\\charset16x16_cbm128_390059.png");
            Png cbm128Font = Png.Open(cbm128Stream);

            builder = PngBuilder.Create(imageWidth, imageHeight, false);
            var textColor = new Pixel(255, 255, 255);
            var backColor = new Pixel(0, 0, 0);

            using var memory = new MemoryStream();

            CopyCharacter(cbm128Font, 0, 2, 0, 2, 16);  // !"#$%&'()*+,-./
            CopyCharacter(cbm128Font, 0, 3, 0, 3, 16);  // 01234....?
            CopyCharacter(atariFont, 0, 4, 0, 4, 16);
            CopyCharacter(atariFont, 0, 5, 0, 5, 16);
            CopyCharacter(atariFont, 0, 6, 0, 6, 1);
            CopyCharacter(cbm128Font, 1, 48, 1, 6, 15);
            CopyCharacter(cbm128Font, 0, 49, 0, 7, 11);
            CopyCharacter(atariFont, 11, 7, 11, 7, 4);

            CopyCharacter(cbm128Font, 6, 6, 0, 8, 1);
            CopyCharacter(cbm128Font, 3, 5, 1, 8, 1);
            CopyCharacter(cbm128Font, 13, 4, 2, 8, 2);
            CopyCharacter(cbm128Font, 1, 5, 4, 8, 1);
            CopyCharacter(cbm128Font, 7, 5, 5, 8, 1);
            CopyCharacter(cbm128Font, 14, 5, 6, 8, 2);
            CopyCharacter(cbm128Font, 9, 6, 8, 8, 1);
            CopyCharacter(cbm128Font, 15, 13, 9, 8, 1);
            CopyCharacter(cbm128Font, 9, 14, 10, 8, 1);
            CopyCharacter(cbm128Font, 11, 5, 11, 8, 1);
            CopyCharacter(cbm128Font, 15, 5 + 48, 12, 8, 1);
            CopyCharacter(cbm128Font, 10, 7 + 48, 13, 8, 1);
            CopyCharacter(cbm128Font, 9, 6 + 48, 14, 8, 1);
            CopyCharacter(cbm128Font, 15, 7, 15, 8, 1);
            CopyCharacter(cbm128Font, 0, 10, 15, 1, 1);

            CopyCharacter(atariFont, 12, 3, 12, 3, 1);  // <
            CopyCharacter(atariFont, 14, 3, 14, 3, 1);  // >

            CopyCharacter(atariFont, 2, 8, 0, 9, 1);   // é
            CopyCharacter(atariFont, 7, 8, 1, 9, 1);  // cedile
            CopyCharacter(atariFont, 4, 8, 2, 9, 1);  // :a
            CopyCharacter(atariFont, 11, 8, 3, 9, 1);  // :i
            CopyCharacter(atariFont, 9, 9, 4, 9, 2);  // :o :u

            CopyCharacter(atariFont, 0, 12, 6, 9, 1);  // ij
            CopyCharacter(atariFont, 1, 12, 7, 9, 1);  // IJ

            //NOT NEEDED ANYMORE: The CBM128 characterset already has a nice l.
            //CopyCharacter(atariFont, 12, 6, 12, 6, 1);  // atari l

            builder.Save(memory);
            File.WriteAllBytes("c:\\dotnet\\ConvertC64Font\\charset16x16.png", memory.ToArray());
        }
    }
}

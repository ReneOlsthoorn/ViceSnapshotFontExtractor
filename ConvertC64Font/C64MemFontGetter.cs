using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViceSnapshotLoader;

namespace ConvertC64Font
{
    public class C64MemFontGetter
    {
        public byte[] fontBytes = new byte[2048];

        public C64MemFontGetter(C64DataSilo silo) {
            var mem = silo.c64Mem?.ram;
            var vic2_d000 = silo.vic2_d000_d040;
            var cia2_dd00 = silo.cia2_dd00_dd10;

            byte gfxPointers = vic2_d000[0x18]; // $d018
            byte bankSelection = cia2_dd00[0];  // $dd00
            bankSelection = (byte)(bankSelection & 0x03);
            int fontLocationInBank = ((gfxPointers >> 1) & 0x07) * 2048;

            int fontLocation = fontLocationInBank;
            if (bankSelection == 3)
                fontLocation += 0;
            if (bankSelection == 2)
                fontLocation += 0x4000;
            if (bankSelection == 1)
                fontLocation += 0x8000;
            if (bankSelection == 0)
                fontLocation += 0xc000;

            Array.Copy(mem, fontLocation, fontBytes, 0, 2048);

            //byte[] watGeheugen = new byte[2048];
            //Array.Copy(mem, 0xdd00, watGeheugen, 0, 2048);
            //Array.Copy(mem, 0xd000, fontBytes, 0, 2048);

            //int fontLocation = ((gfxPointers >> 1) & 0x07) * 2048;
            //Array.Copy(mem, fontLocation, fontBytes, 0, 2048);
        }
    }
}

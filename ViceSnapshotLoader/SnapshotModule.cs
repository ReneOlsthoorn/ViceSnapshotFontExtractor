using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViceSnapshotLoader
{
    public class SnapshotModule
    {
        public string modulename;
        public byte vmajor;
        public byte vminor;
        public Int32 modulesize;
        public byte[] content;

        public void ReadFrom(BinaryReader reader)
        {
            modulename = ReadZeroTerminatedString(reader.ReadBytes(16));
            vmajor = reader.ReadByte();
            vminor = reader.ReadByte();
            modulesize = reader.ReadInt32();
            content = reader.ReadBytes(modulesize-22);
        }

        public string ReadZeroTerminatedString(byte[] buffer)
        {
            int counter = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] != 0)
                    counter++;
                else
                    break;
            }
            return System.Text.ASCIIEncoding.ASCII.GetString(buffer, 0, counter);
        }


    }
}

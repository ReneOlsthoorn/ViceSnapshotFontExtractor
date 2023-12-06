using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViceSnapshotLoader
{
    public class SnapshotHeader
    {
        public byte[] magic;
        public byte vmajor;
        public byte vminor;
        public string machinename;
        public byte[] version_magic;
        public byte version_major;
        public byte version_minor;
        public byte version_micro;
        public byte version_zero;
        public Int32 svnversion;

        public void ReadFrom(BinaryReader reader)
        {
            magic = reader.ReadBytes(19);
            vmajor = reader.ReadByte();
            vminor = reader.ReadByte();
            machinename = ReadZeroTerminatedString(reader.ReadBytes(16));
            version_magic = reader.ReadBytes(13);
            version_major = reader.ReadByte();
            version_minor = reader.ReadByte();
            version_micro = reader.ReadByte();
            version_zero = reader.ReadByte();
            svnversion = reader.ReadInt32();
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

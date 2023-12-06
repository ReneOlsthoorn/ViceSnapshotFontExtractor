
namespace ViceSnapshotLoader
{
    public class C64DataSilo
    {
        public C64Mem? c64Mem = null;
        public byte[] cia2_dd00_dd10 = new byte[0x10];
        public byte[] vic2_d000_d040 = new byte[0x40];

        public void LoadSnapshot(string vsfFilename)
        {
            using FileStream input = new FileStream(vsfFilename, FileMode.Open, FileAccess.Read);
            using BinaryReader reader = new BinaryReader(input);

            SnapshotHeader header = new SnapshotHeader();
            header.ReadFrom(reader);

            while ((input.Position+1) < input.Length)
            {
                SnapshotModule module = new SnapshotModule();
                module.ReadFrom(reader);
                Console.WriteLine($"Module: {module.modulename}   Size: {module.modulesize}");
                if (module.modulename == "C64MEM")
                {
                    C64Mem mem = new C64Mem();
                    mem.cpudata = module.content[0];
                    mem.cpudir = module.content[1];
                    mem.exrom = module.content[2];
                    mem.game = module.content[3];
                    mem.ram = new byte[65536];
                    Array.Copy(module.content, 4, mem.ram, 0, 65536);                    
                    c64Mem = mem;
                }
                if (module.modulename == "CIA2")
                {
                    Array.Copy(module.content, 0, cia2_dd00_dd10, 0, 0x10);
                }
                if (module.modulename == "VIC-II")
                {
                    Array.Copy(module.content, 1, vic2_d000_d040, 0, 0x40);
                }
            }

            if (c64Mem != null)
            {
                //File.WriteAllBytes("c:\\dotnet\\ViceSnapshotLoader\\snapshot_ram.bin", c64Mem.ram);
                //File.WriteAllBytes("c:\\dotnet\\ViceSnapshotLoader\\snapshot_cia2.bin", cia2_dd00_dd10);
                //File.WriteAllBytes("c:\\dotnet\\ViceSnapshotLoader\\snapshot_vic2.bin", vic2_d000_d040);
            }

        }
    }
}

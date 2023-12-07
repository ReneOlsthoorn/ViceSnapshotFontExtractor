using BigGustave;
using ConvertC64Font;
using ViceSnapshotLoader;

string inputFilename = "..\\..\\..\\..\\vice-snapshot.vsf";
string outputFilename = "..\\..\\..\\..\\font_.png";
string totalMemoryFontFilename = "..\\..\\..\\..\\totalMemory.png";
string totalMemorySpriteFilename = "..\\..\\..\\..\\ViceSnapshotFontExtractor\\totalMemorySprite.png";

C64DataSilo c64Silo = new ViceSnapshotLoader.C64DataSilo();
c64Silo.LoadSnapshot(inputFilename);

var fontGetter = new C64MemFontGetter(c64Silo);
var font = fontGetter.fontBytes;
//font = File.ReadAllBytes("c:\\dotnet\\ConvertC64Font\\chargen.rom");
new ConverterFont8ToPng(font, outputFilename);
new ConverterFont8ToPng(c64Silo.c64Mem.ram, totalMemoryFontFilename, totalMemoryFont: true);
new ConvertMultiColorSpriteToPng(c64Silo.c64Mem.ram, totalMemorySpriteFilename, totalMemory: true);

Console.WriteLine($"PNG {outputFilename} written to disk.");

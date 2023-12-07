using BigGustave;
using ConvertC64Font;
using ViceSnapshotLoader;

string inputFilename = "c:\\dotnet\\ViceSnapshotFontExtractor\\vice-snapshot.vsf";
string outputFilename = "c:\\dotnet\\ViceSnapshotFontExtractor\\font_.png";
string totalMemoryFontFilename = "c:\\dotnet\\ViceSnapshotFontExtractor\\totalMemory.png";
string totalMemorySpriteFilename = "c:\\dotnet\\ViceSnapshotFontExtractor\\totalMemorySprite.png";

C64DataSilo c64Silo = new ViceSnapshotLoader.C64DataSilo();
c64Silo.LoadSnapshot(inputFilename);

var fontGetter = new C64MemFontGetter(c64Silo);
var font = fontGetter.fontBytes;
//font = File.ReadAllBytes("c:\\dotnet\\ConvertC64Font\\chargen.rom");
new ConverterFont8ToPng(font, outputFilename);
new ConverterFont8ToPng(c64Silo.c64Mem.ram, totalMemoryFontFilename, totalMemoryFont: true);
new ConvertMultiColorSpriteToPng(c64Silo.c64Mem.ram, totalMemorySpriteFilename, totalMemory: true);

Console.WriteLine($"PNG {outputFilename} written to disk.");

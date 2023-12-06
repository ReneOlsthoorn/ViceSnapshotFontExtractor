using BigGustave;
using ConvertC64Font;
using ViceSnapshotLoader;

string inputFilename = "c:\\dotnet\\ViceSnapshotFontExtractor\\snapshot.vsf";
string outputFilename = "c:\\dotnet\\ViceSnapshotFontExtractor\\snapshot_font.png";

C64DataSilo c64Silo = new ViceSnapshotLoader.C64DataSilo();
c64Silo.LoadSnapshot(inputFilename);

var fontGetter = new C64MemFontGetter(c64Silo);
var font = fontGetter.fontBytes;
//font = File.ReadAllBytes("c:\\dotnet\\ConvertC64Font\\chargen.rom");
new ConverterFont8ToPng(font, outputFilename);

Console.WriteLine($"PNG {outputFilename} written to disk.");

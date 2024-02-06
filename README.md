# ViceSnapshotFontExtractor
Commodore 64 VICE 3.7 Snapshot Font Extractor C64

This dotnet C# program can read a VICE C64 emulator snapshot file and look for the used font, and save the font to PNG in 256x256 format. (filename is: font_.png).
A 256x8192 picture containing the entire 64k memory is also created. If the font is anywhere in memory, you will find it. (filename is: totalMemory.png).

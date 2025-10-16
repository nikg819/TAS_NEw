using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using TAS_Test.Config;


namespace TAS_Test.services;

public class PdfService
{
    
    public void HtmlToPdf(string htmlContent, string fileName)
    {
        var config = ConfigService.LoadConfig();
        string princePath = config.Pdf.PrincePath;
        string outputPath = config.Pdf.outputPath + fileName;
        
        // 1. Temporäre HTML-Datei speichern
        string tempHtmlPath = Path.GetTempFileName() + ".html";
        File.WriteAllText(tempHtmlPath, htmlContent);

        // 2. Prince aufrufen
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = princePath, // prince liegt in dem bin/debug/net9.0 ordner in tas_test
                Arguments = $"\"{tempHtmlPath}\" -o \"{outputPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            Console.WriteLine($"Prince error: {error}");
            throw new Exception("Prince PDF generation failed");
        }

        Console.WriteLine($"✅ PDF erfolgreich erstellt: {outputPath}");
        
        //Pdf wird direkt geöffnet:
        
        OpenPdf(outputPath); 
    }

    public void OpenPdf(string pdfPath)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Process.Start(new ProcessStartInfo 
            {
                FileName = pdfPath, 
                UseShellExecute = true 
            });
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", pdfPath);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", pdfPath);
        }
        else
        {
            Console.WriteLine("Unbekanntes OS");
        }
    }
}

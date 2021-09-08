using DinkToPdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace dotnet_core_console
{
    class Program
    {
        static void Main(string[] args)
        {
            dynamic data = new
            {
                User = new
                {
                    FirstName = "John",
                    LastName = "Reiner"
                },
                Activity = new
                {
                    Scans = 27,
                    NewDesignGaps = 83,
                    SolvedDesignGaps = 16
                }
            };

            Console.WriteLine($"Current directory: {Directory.GetCurrentDirectory()}");
            WritePdf(data);
            Console.ReadKey();
        }

        private static string GetHtmlTemplate()
        {
            return System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "assets", "demo.html"));
        }

        private static void WritePdf(dynamic data)
        {
            var converter = new SynchronizedConverter(new PdfTools());

            string html = GetHtmlTemplate();
            html = html
                .Replace("{{User.FirstName}}", data.User.FirstName)
                .Replace("{{Activity.Scans}}", data.Activity.Scans.ToString())
                .Replace("{{Activity.NewDesignGaps}}", data.Activity.NewDesignGaps.ToString())
                .Replace("{{Activity.SolvedDesignGaps}}", data.Activity.SolvedDesignGaps.ToString());

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
                Out = Path.Combine(Directory.GetCurrentDirectory(), "demo.pdf")
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = html,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "This is the Heaader" },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Right = "Page [page] of [toPage]" },
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            
            converter.Convert(pdf);
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using CommandLine;
using Microsoft.SqlServer.XEvent.XELite;

namespace XelConsole
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                  .WithParsed(o =>
                  {
                      Exec(o);
                  });
        }

        private static void Exec(Options o)
        {
            Console.WriteLine($"XEL Console");

            if (!File.Exists(o.InputFileName))
            {
                Console.WriteLine($"ERROR: Input file {o.InputFileName} not exists");
                return;
            }

            Console.WriteLine($"Parsing {o.InputFileName} ...");

            var outDir = Path.GetDirectoryName(o.OutputFileName);
            if(!Directory.Exists(outDir))
            {
                Console.WriteLine($"ERROR: Output directory {outDir} not exists");
                return;
            }

            using (var outputStream = File.CreateText(o.OutputFileName))
            {
                WriteFileStart(o, outputStream);

                var xelStream = new XEFileEventStreamer(o.InputFileName);

                xelStream.ReadEventStream(
                     xevent =>
                     {
                         switch (o.Format)
                         {
                             case ExportFormat.txt:
                                 ExportTxt(xevent, outputStream);
                                 break;
                             case ExportFormat.html:
                                 ExportHtml(xevent, outputStream);
                                 break;
                             case ExportFormat.json:
                             default:
                                 ExportJson(xevent, outputStream);
                                 break;
                         }

                         return Task.CompletedTask;
                     },
                     CancellationToken.None).Wait();

                WriteFileEnd(o, outputStream);
            }

            Console.WriteLine($"Exported as {o.Format} to {o.OutputFileName}");
        }

        private static void WriteFileStart(Options o, StreamWriter outputStream)
        {
            switch (o.Format)
            {
                case ExportFormat.txt:
                    break;
                case ExportFormat.html:
                    outputStream.Write("<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"utf-8\"></head><body>");
                    break;
                case ExportFormat.json:
                default:
                    outputStream.Write('[');
                    break;
            }
        }

        private static void WriteFileEnd(Options o, StreamWriter outputStream)
        {
            switch (o.Format)
            {
                case ExportFormat.txt:
                    break;
                case ExportFormat.html:
                    outputStream.Write("</body></html");
                    break;
                case ExportFormat.json:
                default:
                    outputStream.Write("{}]");
                    break;
            }
        }

        private static void ExportJson(IXEvent xevent, StreamWriter outputStream)
        {
            var buffer = JsonSerializer.Serialize(xevent.Fields);
            outputStream.Write(buffer + ",");
        }

        private static void ExportTxt(IXEvent xevent, StreamWriter outputStream)
        {
            outputStream.WriteLine($"---{xevent.Timestamp.ToString()}---");

            foreach (var item in xevent.Fields)
            {
                var text = $"{item.Key}: {item.Value}";
                outputStream.WriteLine(text);
            }

            outputStream.WriteLine("-------------------------------");
            outputStream.WriteLine("");
        }

        private static void ExportHtml(IXEvent xevent, StreamWriter outputStream)
        {
            outputStream.WriteLine($"<div><h3>{xevent.Timestamp.ToString()}</h3>");

            foreach (var item in xevent.Fields)
            {
                var text = $"<span><strong>{item.Key}:</strong> {item.Value}</span>";
                outputStream.WriteLine(text);
                outputStream.WriteLine("<br/>");
            }

            outputStream.WriteLine("</div><br/>");
        }
    }
}

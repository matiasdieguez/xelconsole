using CommandLine;

namespace XelConsole
{
    partial class Program
    {
        public class Options
        {
            [Option('i', "input", Required = true, HelpText = "Input file name. Must be a .XEL file.")]
            public string InputFileName { get; set; }

            [Option('o', "output", Required = true, HelpText = "Output file name.")]
            public string OutputFileName { get; set; }

            [Option('f', "format", Required = false, HelpText = "Output format: json|txt|html", Default = ExportFormat.json)]
            public ExportFormat Format { get; set; }

        }

    }
}

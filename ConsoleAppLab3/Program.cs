using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using ClassLibraryCPPLab3;

namespace CPP_Lab3
{
    public class BrainLand
    {
        [Option(ShortName = "i")]
        public string InputFile { get; }

        [Option(ShortName = "o")]
        public string OutputFile { get; }



        public static void Main(string[] args)
            => CommandLineApplication.Execute<BrainLand>(args);

        private void OnExecute()
        {
            ClassLibraryCPPLab3.BrainLandLib.Execute(InputFile, OutputFile);
        }
    }
}
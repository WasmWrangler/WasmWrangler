﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace WasmWrangler.BindingGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFileName = args[0];

            var root = DTSToJson.Convert(Path.GetFullPath(inputFileName));

            var context = new Context()
            {
                InputFileName = inputFileName,
            };

            SyntaxTreeParser.Parse(context, root);

            var document = context.Interfaces.SingleOrDefault(x => x.Name == "Document");
        }

       
    }
}

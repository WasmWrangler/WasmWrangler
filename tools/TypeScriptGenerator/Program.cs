using System.IO;
using System.Linq;

namespace TypeScriptGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFileName = args[0];

            var root = TSToJson.Convert(Path.GetFullPath(inputFileName));

            var context = new Context()
            {
                InputFileName = inputFileName,
            };

            SyntaxTreeParser.Parse(context, root);

            var document = context.Interfaces.SingleOrDefault(x => x.Name == "Document");
        }

       
    }
}

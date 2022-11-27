using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using System.IO;
using System.Text;
using System;

namespace SourceGenerator
{
    [Generator]
    public class MathGenerator : ISourceGenerator
    {
        private const string libraryCode = @"
    using System.Linq;
    using System;
    using System.Collections.Generic;
    namespace Maths {
     public static class FormulaHelpers {
            public static IEnumerable<double> ConvertToDouble(IEnumerable<int> col)
            {
                foreach (var s in col)
                    yield return (double) s;
            }
            public static double MySum(int start, int end, Func<double, double> f) =>
                Enumerable.Sum<double>(ConvertToDouble(Enumerable.Range(start, end - start)), f);
        }
    }
    ";

        public void Execute(GeneratorExecutionContext context)
        {
            foreach (AdditionalText file in context.AdditionalFiles)
            {
                if (Path.GetExtension(file.Path).Equals(".math", StringComparison.OrdinalIgnoreCase))
                {
                    // Load formulas from .math files
                    var mathText = file.GetText();
                    var mathString = "";

                    if (mathText != null)
                    {
                        mathString = mathText.ToString();
                    }
                    else
                    {
                        throw new Exception($"Cannot load file {file.Path}");
                    }

                    // Get name of generated namespace from file name
                    string fileName = Path.GetFileNameWithoutExtension(file.Path);

                    // Parse and gen the formulas functions
                    //var tokens = Lexer.Tokenize(mathString);
                    //var code = Parser.Parse(tokens);

                    //var codeFileName = $@"{fileName}.g.cs";

                    //context.AddSource(codeFileName, SourceText.From(code, Encoding.UTF8));
                }
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForPostInitialization((pi) => pi.AddSource("__MathLibrary__.g.cs", libraryCode));
        }
    }
}


using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectF
{
    class Program
    {

        static void Main(string[] args) {
            string inputCode = System.IO.File.ReadAllText(@"input_code.f");
            AntlrInputStream inputStream = new AntlrInputStream(inputCode);
            ProjectFLexer fLexer = new ProjectFLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(fLexer);
            ProjectFParser fParser = new ProjectFParser(commonTokenStream);
            var programContext = fParser.program();
            VisitorGenerator visitor = new VisitorGenerator();
            string[] output_c_code = { visitor.Visit(programContext)};
            System.IO.File.WriteAllLines(@"output.c", output_c_code);
           Console.ReadLine();
            var gcc = Process.Start("gcc.exe", "-g output.c -o a.exe");
            gcc.WaitForExit();
           Process.Start("a.exe");

        }
    }
}

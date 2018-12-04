using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectF
{
    class Program
    {
        static void Main(string[] args){
            string inputCode = System.IO.File.ReadAllText(@"input_code.txt");
            AntlrInputStream inputStream = new AntlrInputStream(inputCode);
            ProjectFLexer fLexer = new ProjectFLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(fLexer);
            ProjectFParser fParser = new ProjectFParser(commonTokenStream);
            var programContext = fParser.program();
            VisitorGenerator visitor = new VisitorGenerator();
            output_c_code = visitor.Visit(programContext);
            System.IO.File.WriteAllLines(@"output.c", output_c_code);
            Process.Start("gcc.exe", "output.c -o a.exe");
        }
    }
}

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
            AntlrInputStream inputStream = new AntlrInputStream("a: integer is 777");
            ProjectFLexer fLexer = new ProjectFLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(fLexer);
            ProjectFParser fParser = new ProjectFParser(commonTokenStream);

            var programContext = fParser.program();
            VisitorGenerator visitor = new VisitorGenerator();
            Console.WriteLine(visitor.Visit(programContext));
            Console.ReadLine();
        }
    }
}

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
            AntlrInputStream inputStream = new AntlrInputStream("f1: func() is func(p:integer): integer => p+1;" +
                "b: integer is -10;"+
                "a: integer is f1(b)");
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

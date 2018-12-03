using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace ProjectF
{
    class VisitorGenerator: ProjectFBaseVisitor<string>
    {
        public override string VisitProgram([NotNull] ProjectFParser.ProgramContext context)
        {
            string children = VisitChildren(context);
            return "main(){\r\n" + children + "}";
        }
    }
}

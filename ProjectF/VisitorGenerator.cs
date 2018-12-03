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

        enum FType
        {
            String,
            Integer,
            Complex,
            Real,
            Rational,
            Function,
            Tuple,
            Map,
            List
        }

        private Dictionary<string, FType> _symbolTable = new Dictionary<string, FType>();

        public override string VisitProgram([NotNull] ProjectFParser.ProgramContext context)
        {
            string children = "";
            foreach(var decl in context.declaration())
            {
                children += VisitDeclaration(decl);
            }
            return "main(){\r\n" + children + "}";
        }

        public override string VisitDeclaration([NotNull] ProjectFParser.DeclarationContext context)
        {
            var name = context.variable().GetText();
            var type = context.type().GetText();
            var ftype = FType.Integer;
            switch (type)
            {
                case "integer":
                    type = "int";
                    break;
                case "string":
                    type = "char[]";
                    ftype = FType.String;
                    break;
                case "real":
                    type = "float";
                    ftype = FType.Real;
                    break;
            }
            _symbolTable.Add(name, ftype);
            var expression = VisitExpression(context.expression());
            return type + " " + name + " = " + expression + ";\r\n";
        }

        public override string VisitExpression([NotNull] ProjectFParser.ExpressionContext context)
        {
            var result = VisitRelation(context.relation()[0]);
            if(context.logicalOp() != null)
            {
                var op = "";
                switch (context.logicalOp().GetText())
                {
                    case "and": op = "&";
                        break;
                    case "or": op = "|";
                        break;
                    case "xor":
                        op = "^";
                        break;
                }
                result += op + VisitRelation(context.relation()[1]);
            }
            return result;
        }

        public override string VisitRelation([NotNull] ProjectFParser.RelationContext context)
        {
            var result = VisitFactor(context.factor()[0]);
            if (context.relationOp() != null)
            {
                var op = context.relationOp().GetText();
                result += op + VisitFactor(context.factor()[1]);
            }
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

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
            List,
            Bool
        }

        private Dictionary<string, FType> _symbolTable = new Dictionary<string, FType>();
        private Dictionary<string, string> _listTable = new Dictionary<string, string>();
        private List<string> functions = new List<string>();
        private Dictionary<string, string> functionCast = new Dictionary<string, string>();
        private Dictionary<string, string> varFunction = new Dictionary<string, string>();
        private Stack<string> initializers = new Stack<string>();
        private Stack<string> listVarNames = new Stack<string>();
        private int varCount = 0;

        public override string VisitProgram([NotNull] ProjectFParser.ProgramContext context)
        {
            string children = "";
            /*foreach(var decl in context.declaration())
            {
                children += VisitDeclaration(decl);
            }*/
            foreach (var decl in context.declaration())
            {
                children += VisitDeclaration(decl);
            }
            var addFunctions = "";
            foreach (var func in functions)
            {
                addFunctions += func + "\r\n";
            }
            return addFunctions + "main(){\r\n" + children + "}";
        }

        public override string VisitChildren([NotNull] IRuleNode node)
        {
            var result = "";
            for(int i = 0; i < node.ChildCount; i++)
            {
                result += Visit(node.GetChild(i));
            }
            return result;
        }

        protected override bool ShouldVisitNextChild([NotNull] IRuleNode node, string currentResult)
        {
            return true;
        }

        public override string VisitDeclaration([NotNull] ProjectFParser.DeclarationContext context)
        {
            var name = context.variable().GetText();
            var type = context.type().GetText();
            var ftype = FType.Bool;
            switch (type)
            {
                case "integer":
                    type = "int";
                    ftype = FType.Integer;
                    break;
                case "string":
                    type = "char";
                    name += "[]";
                    ftype = FType.String;
                    break;
                case "real":
                    type = "float";
                    ftype = FType.Real;
                    break;
            }
            if (type.Contains("func"))
            {
                type = "void*";
                ftype = FType.Function;
            }
            if (type[0] == '(' && type[type.Length-1] == ')')
            {
                _listTable.Add(name, GetCType(type.Substring(1, type.Length - 2)));
                listVarNames.Push(name);
                type = "struct Node*";
                ftype = FType.List;
            }
            _symbolTable.Add(name, ftype);
            var expression = VisitExpression(context.expression());
            varFunction.Add(name, expression.Substring(1));
            var result = type + " " + name + " = " + expression + ";\r\n";
            if(ftype == FType.List)
            {
                result += initializers.Pop().Replace("head", name);
            }
            return result;
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

        public override string VisitFactor([NotNull] ProjectFParser.FactorContext context)
        {
            var result = VisitTerm(context.term()[0]);
            for(int i = 0; i < context.factorOp().Length; i++)
            {
                var op = context.factorOp()[i].GetText();
                result += op + VisitTerm(context.term()[i+1]);
            }
            return result;
        }

        public override string VisitTerm([NotNull] ProjectFParser.TermContext context)
        {
            var result = VisitUnary(context.unary()[0]);
            for (int i = 0; i < context.termOp().Length; i++)
            {
                var op = context.termOp()[i].GetText();
                result += op + VisitUnary(context.unary()[i + 1]);
            }
            return result;
        }

        public override string VisitUnary([NotNull] ProjectFParser.UnaryContext context)
        {
            var result = "";
            if (context.factorOp() != null)
            {
                result += context.factorOp().GetText();
            }
            result += VisitSecondary(context.secondary());
            return result;
        }



        public override string VisitSecondary([NotNull] ProjectFParser.SecondaryContext context)
        {
            var result = "";
            var id = context.primary()?.elementary()?.GetText();
            if (id != null && _symbolTable.Keys.Contains(id))
            {
                switch (_symbolTable[id])
                {
                    case FType.List:
                        result = "*(("+_listTable[id]+"*)l_get(" + id + "," + VisitExpression(context.tail()[0].expression()) + "))";
                        break;
                    //Tuples and Maps
                    case FType.Function:
                        var fname = varFunction[id];
                        result = "(" + functionCast[fname] + id + ")(" + VisitExpressions(context.tail()[0].expressions()) +")";
                        break;
                    //func call
                }
            }
            if (result == "")
            {
                result = VisitPrimary(context.primary());
                for (int i = 0; i < context.tail().Length; i++)
                {
                    result += VisitTail(context.tail()[i]);
                }
            }
            return result;
            //VisitChildren()
        }

        public override string VisitExpressions([NotNull] ProjectFParser.ExpressionsContext context)
        {
            var expressions = "";
            foreach(var expr in context.expression())
            {
                expressions += VisitExpression(expr) + ",";
            }
            return expressions.Substring(0, expressions.Length - 1);
        }

        public override string VisitPrimary([NotNull] ProjectFParser.PrimaryContext context)
        {
            if(context.elementary() != null)
            {
                return context.elementary().GetText().Replace("false", "0").Replace("true", "1");
            }
            if (context.function() != null)
            {
                return VisitFunction(context.function());
            }
            if (context.list() != null)
            {
                return VisitList(context.list());
            }
            if (context.map() != null)
            {
                return VisitMap(context.map());
            }
            if (context.expression() != null)
            {
                return VisitExpression(context.expression());
            }
            return VisitTuple(context.tuple());
        }

        public override string VisitFunction([NotNull] ProjectFParser.FunctionContext context)
        {
            var parameters = "";
            if (context.parameters() != null) {
                parameters = VisitParameters(context.parameters());
            }
            var name = "_func" + (functions.Count + 1).ToString();
            var function = GetCType(context.type().GetText()) + " " + name + " " + "(" + parameters + ")" + VisitBody(context.body());
            functions.Add(function);
            functionCast.Add(name, "(" + GetCType(context.type().GetText()) + "(*)" + "(" + GetParameterTypes(context.parameters()) + ")" + ")");
            return "&" + name;
        }

        public string GetCType(string ftype)
        {
            switch (ftype)
            {
                case "integer":
                    ftype = "int";
                    break;
                case "string":
                    ftype = "char*";
                    break;
                case "real":
                    ftype = "float";
                    break;
            }
            return ftype;
        }

        public string GetParameterTypes(ProjectFParser.ParametersContext context)
        {
            var types = "";
            foreach(var type in context?.type())
            {
                types += GetCType(type.GetText()) + ",";
            }
            types = types.Substring(0, types.Length - 1);
            return types;
        }

        public override string VisitParameters([NotNull] ProjectFParser.ParametersContext context)
        {
            var parameters = "";
            for (int i = 0; i < context?.type().Length; i++)
            {
                parameters += GetCType(context?.type()[i].GetText()) + " " + context?.variable()[i].GetText() + ",";
            }
            parameters = parameters.Substring(0, parameters.Length - 1);
            return parameters;
        }

        public override string VisitBody([NotNull] ProjectFParser.BodyContext context)
        {
            if(context.statements() != null)
            {
                return "{\r\n" + VisitStatements(context.statements()) + "}\r\n";
            }
            return "{\r\n return " + VisitExpression(context.expression()) + "}\r\n";
        }

        public override string VisitStatements([NotNull] ProjectFParser.StatementsContext context)
        {
            return VisitChildren(context);
        }

        public override string VisitStatement([NotNull] ProjectFParser.StatementContext context)
        {
            if (context.assignmentOrCall() != null)
            {
                return VisitAssignmentOrCall(context.assignmentOrCall());
            }
            if (context.conditional() != null)
            {
                return VisitConditional(context.conditional());
            }
            if (context.loop() != null)
            {
                return VisitLoop(context.loop());
            }
            if (context.GetText().Contains("return"))
            {
                if(context.expression() != null)
                {
                    return "return " + VisitExpression(context.expression())+";";
                }
                return "return;";
            }
            if (context.declaration() != null)
            {
                return VisitDeclaration(context.declaration());
            }
            return "";
        }

        public override string VisitList([NotNull] ProjectFParser.ListContext context)
        {
            string listGenerator = "";
            var currentListName = listVarNames.Pop();
            var currentListType = _listTable[currentListName];
            foreach (var expr in context.expressions()?.expression())
            {
                var varName = "_ptr" + (varCount++).ToString();
                listGenerator += "void *" + varName + " = malloc(sizeof(" + currentListType + "));\r\n";
                listGenerator += "*(("+ currentListType +" *)" + varName + " = " + VisitExpression(expr) + ";\r\n";
                listGenerator += "l_put(head, " + varName + ");\r\n";
            }
            initializers.Push(listGenerator);
            return "l_createEmptyList()";
        }

        public override string VisitAssignmentOrCall([NotNull] ProjectFParser.AssignmentOrCallContext context)
        {
            var result = VisitSecondary(context.secondary());
            if(context.expression() != null)
            {
                result += " = " + VisitExpression(context.expression());
            }
            return result;
        }

        public override string VisitConditional([NotNull] ProjectFParser.ConditionalContext context)
        {
            return base.VisitConditional(context);
        }
    }
}

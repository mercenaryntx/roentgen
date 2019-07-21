using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.ScOut;
using Neurotoxin.ScOut.Analysis;
using Neurotoxin.ScOut.Visitors;

namespace RoslynDemo
{
    public class SqlCommandExecutionPostProcessor : PostProcessor
    {
        private readonly FindVariableVisitor _findVariableVisitor = new FindVariableVisitor();
        private readonly FindLiteralVisitor _findLiteralVisitor = new FindLiteralVisitor();
        private readonly string[] _methodCalls =
        {
            "System.Data.SqlClient.SqlCommand.ExecuteReader",
            "System.Data.SqlClient.SqlCommand.ExecuteScalar",
            "System.Data.SqlClient.SqlCommand.ExecuteNonQuery"
        };

        public SqlCommandExecutionPostProcessor(AnalysisWorkspace workspace, ExcludingRules excludingRules) : base(workspace, excludingRules)
        {
        }

        public override void Process()
        {
            var invocations = Workspace.Links.OfType<InternalCall>().GroupBy(c => c.Callee).ToDictionary(g => g.Key.Declaration, g => g.Select(c => c.Invocation).ToList());

            foreach (var call in Workspace.Links.OfType<ExternalCall>().Where(c => _methodCalls.Any(mc => c.CalleeSymbol.ToString().StartsWith(mc))).ToArray())
            {
                var variableIdentifer = ((IdentifierNameSyntax)((MemberAccessExpressionSyntax)call.Invocation.Expression).Expression).Identifier;
                var cmdVariable = _findVariableVisitor.FindVariable(call.Invocation, variableIdentifer);

                switch (cmdVariable)
                {
                    case ObjectCreationExpressionSyntax objectCreation:
                        if (!objectCreation.ArgumentList.Arguments.Any())
                        {
                            //TODO: support parameterless commands
                            break;
                        }
                        var firstArg = objectCreation.ArgumentList.Arguments.First().Expression;
                        var foundLiterals = _findLiteralVisitor.FindLiteral(firstArg, invocations);
                        if (foundLiterals != null)
                        {
                            foreach (var literal in foundLiterals)
                            {
                                Workspace.Register(new SqlCommandCall(call.Caller, literal));
                            }
                        }
                        break;
                    default:
                        //TODO: if it's not a freshly created SqlCommand
                        //i.e. ParenthesizedExpressionSyntax ParenthesizedExpression ((global::System.Data.SqlClient.SqlCommand)(this.CommandCollection[0]))
                        break;
                }
            }
        }
    }
}
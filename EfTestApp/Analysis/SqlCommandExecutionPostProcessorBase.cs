using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.Roentgen;
using Neurotoxin.Roentgen.Analysis;
using Neurotoxin.Roentgen.Extensions;
using Neurotoxin.Roentgen.PostProcessors;
using Neurotoxin.Roentgen.Visitors;

namespace EfTestApp.Analysis
{
    public class SqlCommandExecutionPostProcessorBase : PostProcessorBase
    {
        private readonly ILogger<SqlCommandExecutionPostProcessorBase> _logger;
        private readonly FindVariableVisitor _findVariableVisitor;
        private readonly FindLiteralVisitor _findLiteralVisitor;
        private readonly string[] _methodCalls =
        {
            "System.Data.SqlClient.SqlCommand.ExecuteReader",
            "System.Data.SqlClient.SqlCommand.ExecuteScalar",
            "System.Data.SqlClient.SqlCommand.ExecuteNonQuery"
        };

        public SqlCommandExecutionPostProcessorBase(AnalysisWorkspace workspace, 
                                                    ExcludingRules excludingRules, 
                                                    FindVariableVisitor findVariableVisitor, 
                                                    FindLiteralVisitor findLiteralVisitor, 
                                                    ILogger<SqlCommandExecutionPostProcessorBase> logger)
            : base(workspace, excludingRules)
        {
            _findVariableVisitor = findVariableVisitor;
            _findLiteralVisitor = findLiteralVisitor;
            _logger = logger;
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
                            _logger.Warning($"The analysis of parameterless SqlCommand declarations are not yet supported. ({cmdVariable.GetPosition()})");
                            break;
                        }
                        var firstArg = objectCreation.ArgumentList.Arguments.First().Expression;
                        var foundLiterals = _findLiteralVisitor.FindLiteral(firstArg, invocations);
                        foundLiterals?.ForEach(literal => Workspace.Register(new SqlCommandCall(call.Caller, literal)));
                        break;
                    default:
                        //TODO: if it's not a freshly created SqlCommand
                        //i.e. ParenthesizedExpressionSyntax ParenthesizedExpression ((global::System.Data.SqlClient.SqlCommand)(this.CommandCollection[0]))
                        _logger.Warning($"The analysis of not freshly created SqlCommands are not yet supported. ({cmdVariable.GetPosition()})");
                        break;
                }
            }
        }
    }
}
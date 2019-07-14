using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.ScOut.Extensions;
using Neurotoxin.ScOut.Models;
using Neurotoxin.ScOut.Patterns;

namespace RoslynDemo.Patterns
{
    //TODO: 99% of this could go to a base class or something
    public class SqlCommandExecutionScanner : IDependencyScanner
    {
        public string[] MethodCall { get; } =
        {
            "System.Data.SqlClient.SqlCommand.ExecuteReader",
            "System.Data.SqlClient.SqlCommand.ExecuteScalar",
            "System.Data.SqlClient.SqlCommand.ExecuteNonQuery"
        };

        public string Scan(MethodCall call)
        {
            var result = new List<string>();
            Console.WriteLine(call.Caller.ToString());

            var variableIdentifer = ((IdentifierNameSyntax)((MemberAccessExpressionSyntax)call.Invocation.Expression).Expression).Identifier;
            var cmdVariable = VariableSearch(call.Invocation, variableIdentifer);

            switch (cmdVariable)
            {
                case ObjectCreationExpressionSyntax objectCreation:
                    if (objectCreation.ArgumentList.Arguments.First().Expression is IdentifierNameSyntax firstArg)
                    {
                        var x = FindLiteral(cmdVariable, firstArg.Identifier, call.Caller);
                        if (x != null) result.AddRange(x);
                    }
                    else
                    {
                        Debugger.Break();
                    }
                    break;
                default:
                    //TODO: if it's not a freshly created SqlCommand
                    //i.e. ParenthesizedExpressionSyntax ParenthesizedExpression ((global::System.Data.SqlClient.SqlCommand)(this.CommandCollection[0]))
                    break;
            }
            return string.Join(string.Empty, result);
        }

        private IEnumerable<string> FindLiteral(SyntaxNode node, SyntaxToken identifier, Method context)
        {
            var queryVariable = VariableSearch(node, identifier);
            return queryVariable == null ? new string[0] : GetLiteral(queryVariable, context);
        }

        private IEnumerable<string> GetLiteral(SyntaxNode node, Method context)
        {
            switch (node)
            {
                case LiteralExpressionSyntax literalExpression:
                    return new[] { literalExpression.Token.ValueText };
                case BinaryExpressionSyntax binaryExpression:
                    var left = GetLiteral(binaryExpression.Left, context);
                    var right = GetLiteral(binaryExpression.Right, context);
                    return left.Concat(right);
                case MemberAccessExpressionSyntax memberAccess:
                    //TODO: support consts
                    break;
                case IdentifierNameSyntax identifier:
                    return FindLiteral(node, identifier.Identifier, context);
                case ParameterSyntax parameter:
                    var parameterIndex = context.Declaration.ParameterList.Parameters.IndexOf(parameter);
                    foreach (var caller in context.Callers)
                    {
                        foreach (var internalCall in caller.InternalCalls.Where(ic => ic.Callee == context))
                        {
                            var arg = internalCall.Invocation.ArgumentList.Arguments[parameterIndex].Expression;
                            switch (arg)
                            {
                                case IdentifierNameSyntax invocationArg:
                                    var invocationParam = FindLiteral(internalCall.Invocation, invocationArg.Identifier, caller);
                                    if (invocationParam != null) return invocationParam;
                                    Debugger.Break();
                                    break;
                                default:
                                    return GetLiteral(arg, context);
                            }
                        }
                    }
                    break;
                case InvocationExpressionSyntax invocationExpression:
                    //HACK: So far we were pretty generic, but this is too tricky so we just implement exact calls where we know what to do
                    if (((MemberAccessExpressionSyntax)invocationExpression.Expression).Name.ToString() == "Join<string>")
                    {
                        var secondArg = (IdentifierNameSyntax)invocationExpression.ArgumentList.Arguments[1].Expression;
                        return FindLiteral(node, secondArg.Identifier, context);
                    }
                    Debugger.Break();
                    break;
                case CastExpressionSyntax cast:
                    return GetLiteral(cast.Expression, context);
                default:
                    Debugger.Break();
                    break;
            }
            return new string[0];
        }

        private SyntaxNode VariableSearch(SyntaxNode node, SyntaxToken identifier)
        {
            switch (node)
            {
                case AssignmentExpressionSyntax assignment:
                {
                    var left = assignment.Left as IdentifierNameSyntax;
                    if (left?.Identifier.ValueText == identifier.ValueText) return assignment.Right;
                    break;
                }
                case MethodDeclarationSyntax methodDeclaration:
                    var parameter = methodDeclaration.ParameterList.Parameters.SingleOrDefault(p => p.Identifier.ValueText == identifier.ValueText);
                    if (parameter != null) return parameter;
                    break;
                default:
                    var variableDeclaration = node.FindNodes<VariableDeclaratorSyntax>().SingleOrDefault(d => d.Identifier.ValueText == identifier.ValueText);
                    if (variableDeclaration != null) return variableDeclaration.Initializer.Value;

                    //TODO: First isn't good enough - we have to know where we came from
                    var assignments = node.FindNodes<AssignmentExpressionSyntax>();
                    foreach (var assignment in assignments)
                    {
                        var left = assignment.Left as IdentifierNameSyntax;
                        if (left?.Identifier.ValueText == identifier.ValueText) return assignment.Right;
                    }
                    break;
            }

            return node.Parent != null ? VariableSearch(node.Parent, identifier) : null;
        }
    }
}
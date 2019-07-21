using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.ScOut.Extensions;
using Neurotoxin.ScOut.Models;
using Neurotoxin.ScOut.Patterns;
using Neurotoxin.ScOut.Visitors;

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

        public string Scan(MethodCall call, Dictionary<MethodDeclarationSyntax, List<InvocationExpressionSyntax>> invocations)
        {
            var result = new List<string>();
            //Console.WriteLine(call.Caller.ToString());

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
                    if (foundLiterals != null) result.AddRange(foundLiterals);
                    Console.WriteLine($"{call.Caller} - {result.Count}");
                    //switch (firstArg)
                    //{
                    //    case IdentifierNameSyntax identifier:
                    //        var x = FindLiteral(cmdVariable, identifier.Identifier, call.Caller);
                    //        if (x != null) result.AddRange(x);
                    //        break;
                    //    case LiteralExpressionSyntax literalExpression:
                    //        result.Add(literalExpression.Token.ValueText);
                    //        break;
                    //    default:
                    //        Debugger.Break();
                    //        break;
                    //}
                    break;
                default:
                    //TODO: if it's not a freshly created SqlCommand
                    //i.e. ParenthesizedExpressionSyntax ParenthesizedExpression ((global::System.Data.SqlClient.SqlCommand)(this.CommandCollection[0]))
                    break;
            }
            return string.Join(Environment.NewLine, result);
        }

        //private IEnumerable<string> FindLiteral(SyntaxNode node, SyntaxToken identifier, Method context)
        //{
        //    var queryVariable = _findVariableVisitor.FindVariable(node, identifier);
        //    return queryVariable == null ? new string[0] : GetLiteral(queryVariable, context);
        //}

        //private IEnumerable<string> GetLiteral(SyntaxNode node, Method context)
        //{
        //    switch (node)
        //    {
        //        case LiteralExpressionSyntax literalExpression:
        //            return new[] { literalExpression.Token.ValueText };
        //        case BinaryExpressionSyntax binaryExpression:
        //            var left = GetLiteral(binaryExpression.Left, context);
        //            var right = GetLiteral(binaryExpression.Right, context);
        //            return left.Concat(right);
        //        case MemberAccessExpressionSyntax memberAccess:
        //            //TODO: support consts
        //            break;
        //        case IdentifierNameSyntax identifier:
        //            return FindLiteral(node, identifier.Identifier, context);
        //        case ParameterSyntax parameter:
        //            //TODO: doesn't support `params` arg
        //            var parameterIndex = context.Declaration.ParameterList.Parameters.IndexOf(parameter);
        //            if (parameterIndex == -1) return new string[0];

        //            foreach (var caller in context.Callers)
        //            {
        //                foreach (var internalCall in caller.InternalCalls.Where(ic => ic.Callee == context))
        //                {
        //                    var arg = internalCall.Invocation.ArgumentList.Arguments[parameterIndex].Expression;
        //                    switch (arg)
        //                    {
        //                        case IdentifierNameSyntax invocationArg:
        //                            var invocationParam = FindLiteral(internalCall.Invocation, invocationArg.Identifier, caller);
        //                            if (invocationParam != null) return invocationParam;
        //                            Debugger.Break();
        //                            break;
        //                        default:
        //                            return GetLiteral(arg, context);
        //                    }
        //                }
        //            }
        //            break;
        //        case InvocationExpressionSyntax invocationExpression:
        //            //HACK: So far we were pretty generic, but this is too tricky so we just implement exact calls where we know what to do
        //            if (((MemberAccessExpressionSyntax)invocationExpression.Expression).Name.ToString() == "Join<string>")
        //            {
        //                var secondArg = (IdentifierNameSyntax)invocationExpression.ArgumentList.Arguments[1].Expression;
        //                return FindLiteral(node, secondArg.Identifier, context);
        //            }
        //            //TODO: log
        //            Console.WriteLine("Can't parse InvocationExpression: " + invocationExpression);
        //            return new string[0];
        //        case CastExpressionSyntax cast:
        //            return GetLiteral(cast.Expression, context);
        //        default:
        //            Debugger.Break();
        //            break;
        //    }
        //    return new string[0];
        //}
    }
}
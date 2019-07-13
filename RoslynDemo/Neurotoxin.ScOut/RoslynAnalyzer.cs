using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Neurotoxin.ScOut.Mappers;
using Neurotoxin.ScOut.Models;
using Solution = Neurotoxin.ScOut.Models.Solution;

namespace Neurotoxin.ScOut
{
    public class RoslynAnalyzer
    {
        private readonly IMapper<Microsoft.CodeAnalysis.Solution, Solution> _solutionMapper;

        private Solution _solution;
        private Dictionary<string, Member> _memberCollection;

        public RoslynAnalyzer(IMapper<Microsoft.CodeAnalysis.Solution, Solution> solutionMapper)
        {
            _solutionMapper = solutionMapper;
        }

        public RoslynAnalyzer LoadSolution(string path)
        {
            var workspace = MSBuildWorkspace.Create();
            var sln = workspace.OpenSolutionAsync(path).GetAwaiter().GetResult();
            //TODO: logger
            foreach (var log in workspace.Diagnostics.Where(d => d.Kind == WorkspaceDiagnosticKind.Failure))
            {
                Console.WriteLine(log);
            }
            _solution = _solutionMapper.Map(sln);
            return this;
        }

        public Solution Analyze()
        {
            _memberCollection = _solution.Classes.SelectMany(c => c.Value.Properties.Values.Cast<Member>().Concat(c.Value.Methods.SelectMany(m => m.Value))).ToDictionary(m => m.FullName, m => m);

            foreach (var method in _solution.Classes.SelectMany(c => c.Value.Methods.SelectMany(m => m.Value)))
            {
                PopulateMethodDependencies(method);
            }

            foreach (var property in _solution.Classes.SelectMany(c => c.Value.Properties.Values))
            {
                if (property.Getter != null) PopulateMethodDependencies(property.Getter);
                if (property.Setter != null) PopulateMethodDependencies(property.Setter);
            }
            return _solution;
        }

        public void PopulateMethodDependencies(Method method)
        {
            foreach (var call in method.Calls)
            {
                call.Target = TryToIdentifyMethodCall(method, call.Ref);
            }
        }

        private Method TryToIdentifyMethodCall(Method source, InvocationExpressionSyntax syntax)
        {
            var argumentTypes = syntax.ArgumentList.Arguments.Select(arg => GetExpressionType(source, arg.Expression)).ToArray();
            if (argumentTypes.Any(arg => arg == null))
            {
                //TODO: log 
                return null;
            }

            return TryToFindEntity(source.ParentClass, syntax.Expression.ToString(), key => FindMethod(key, argumentTypes)) as Method;

            //Method method;
            //if ((method = FindMethod(@ref, argumentTypes)) != null) return method;
            //if ((method = FindMethod($"{source.ParentClass.Namespace}.{@ref}", argumentTypes)) != null) return method;

            //var withAlias = usings.SingleOrDefault(u => u.Alias != null && @ref.StartsWith(u.Alias + "."));
            //if (withAlias != null)
            //{
            //    var aliasRef = new Regex($@"^{withAlias.Alias}\.").Replace(@ref, withAlias.Namespace);
            //    if ((method = FindMethod(aliasRef, argumentTypes)) != null) return method;
            //}

            //return usings.Where(u => u.Alias == null)
            //             .Any(u => (method = FindMethod($"{u.Namespace}.{@ref}", argumentTypes)) != null)
            //    ? method
            //    : null;
        }

        private T TryToFindEntity<T>(Class src, string initialKey, Func<string, T> selector) where T : class
        {
            T instance;
            if ((instance = selector(initialKey)) != null) return instance;
            if ((instance = selector($"{src.Namespace}.{initialKey}")) != null) return instance;

            var withAlias = src.Usings.SingleOrDefault(u => u.Alias != null && initialKey.StartsWith(u.Alias + "."));
            if (withAlias != null)
            {
                var aliasRef = new Regex($@"^{withAlias.Alias}\.").Replace(initialKey, withAlias.Namespace);
                if ((instance = selector(aliasRef)) != null) return instance;
            }

            return src.Usings.Where(u => u.Alias == null)
                .Any(u => (instance = selector($"{u.Namespace}.{initialKey}")) != null)
                ? instance
                : null;
        }

        private Method FindMethod(string @ref, string[] args)
        {
            var key = $"{@ref}({string.Join(", ", args)})";
            if (_memberCollection.ContainsKey(key)) return (Method)_memberCollection[key];

            if (args.Length == 1 && args[0].Contains("<"))
            {
                var r = new Regex(@"\<.*?\>");
                var t = "<T>"; //temp solution
                key = $"{@ref}{t}({r.Replace(args[0], t)})";
                if (_memberCollection.ContainsKey(key)) return (Method)_memberCollection[key];
            }
            return null;
        }

        private string GetExpressionType(Method source, ExpressionSyntax expression)
        {
            var kind = expression.Kind();
            switch (kind)
            {
                case SyntaxKind.StringLiteralExpression:
                    return "string";
                case SyntaxKind.TrueLiteralExpression:
                case SyntaxKind.FalseLiteralExpression:
                    return "bool";
                case SyntaxKind.CharacterLiteralExpression:
                    return "char";
                case SyntaxKind.NumericLiteralExpression:
                    return "int";
                case SyntaxKind.IdentifierName:
                {
                    return TryToFindDeclaration(expression.ToString(), expression)?.ToString();
                }
                case SyntaxKind.SimpleMemberAccessExpression:
                {
                    var memberAccess = (MemberAccessExpressionSyntax) expression;
                    var type = TryToFindDeclaration(memberAccess.Expression.ToString(), expression)?.ToString();
                    if (type == null) Debugger.Break();
                    var targetClass = TryToFindEntity(source.ParentClass, type, key => _solution.Classes.ContainsKey(key) ? _solution.Classes[key] : null);
                    return targetClass.Properties[memberAccess.Name.Identifier.ValueText].Type;
                }
                case SyntaxKind.ElementAccessExpression:
                    Debugger.Break();
                    throw new Exception();
                case SyntaxKind.InvocationExpression:
                    var method = TryToIdentifyMethodCall(source, (InvocationExpressionSyntax) expression);
                    return method?.Type;
                case SyntaxKind.AnonymousObjectCreationExpression:
                    return null;
                //case SyntaxKind.AddExpression:
                //    return TryToFindArgumentType(((BinaryExpressionSyntax)expression).Left, arg);
                default:
                    throw new NotSupportedException("Unknown expression kind: " + kind);
            }
        }

        private static TypeSyntax TryToFindDeclaration(string arg, SyntaxNode syntax)
        {
            var variableDeclaration = syntax.ChildNodes().OfType<VariableDeclarationSyntax>();
            var localDeclarationStatement = syntax.ChildNodes().OfType<LocalDeclarationStatementSyntax>().SelectMany(s => s.ChildNodes().OfType<VariableDeclarationSyntax>());
            var declaration = variableDeclaration.Concat(localDeclarationStatement).SingleOrDefault(v => v.Variables.Any(s => s.Identifier.ValueText == arg));
            if (declaration != null) return declaration.Type;

            switch (syntax.Kind())
            {
                case SyntaxKind.MethodDeclaration:
                    var methodDeclaration = (MethodDeclarationSyntax)syntax;
                    var inputParameter = methodDeclaration.ParameterList.Parameters.SingleOrDefault(p => p.Identifier.ValueText == arg);
                    if (inputParameter != null) return inputParameter.Type;
                    break;
            }

            return syntax.Parent != null ? TryToFindDeclaration(arg, syntax.Parent) : null;
        }
    }
}
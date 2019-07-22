using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Neurotoxin.Roentgen.Visitors
{
    public abstract class VisitorBase<TResult> where TResult : class
    {
        private readonly Dictionary<Type, MethodInfo> _visitOverloads;
        private HashSet<SyntaxNode> _visitedNodes = new HashSet<SyntaxNode>();

        protected VisitorBase()
        {
            var syntaxNodeBase = typeof(SyntaxNode);
            _visitOverloads = GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                       .Where(m => m.Name == nameof(Visit))
                                       .Select(m => new { Method = m, m.GetParameters().FirstOrDefault()?.ParameterType })
                                       .Where(a => syntaxNodeBase.IsAssignableFrom(a.ParameterType))
                                       .ToDictionary(m => m.ParameterType, m => m.Method);
        }

        public void Reset()
        {
            _visitedNodes = new HashSet<SyntaxNode>();
        }

        public virtual TResult Visit(SyntaxNode node)
        {
            if (node == null || _visitedNodes.Contains(node)) return null;
            _visitedNodes.Add(node);
            //Console.WriteLine(node.GetType());

            return VisitTyped(node) ?? ContinueWith(node);
        }

        protected TResult VisitTyped(SyntaxNode node)
        {
            var nodeType = node.GetType();
            return _visitOverloads.ContainsKey(nodeType)
                ? _visitOverloads[nodeType].Invoke(this, new object[] {node}) as TResult
                : null;
        }

        protected virtual TResult ContinueWith(SyntaxNode node)
        {
            return node.ChildNodes().Select(Visit).FirstOrDefault(x => x != null);
        }
    }
}
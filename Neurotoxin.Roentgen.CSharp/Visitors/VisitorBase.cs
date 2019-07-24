using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace Neurotoxin.Roentgen.CSharp.Visitors
{
    public abstract class VisitorBase<TResult> where TResult : class
    {
        private readonly Dictionary<Type, MethodInfo> _visitOverloads;
        private HashSet<SyntaxNode> _visitedNodes = new HashSet<SyntaxNode>();
        protected readonly ILogger Logger;

        protected VisitorBase(ILogger logger)
        {
            Logger = logger;
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
            return VisitTyped(node) ?? ContinueWith(node);
        }

        protected TResult VisitTyped(SyntaxNode node)
        {
            var nodeType = node.GetType();
            if (!_visitOverloads.ContainsKey(nodeType)) return null;

            var method = _visitOverloads[nodeType];
            if (method.ReturnType == typeof(TResult)) return method.Invoke(this, new object[] { node }) as TResult;

            Logger.Warning("Invalid visit node with return type: " + method.ReturnType);
            return null;
        }

        protected virtual TResult ContinueWith(SyntaxNode node)
        {
            return node.ChildNodes().Select(Visit).FirstOrDefault(x => x != null);
        }
    }
}
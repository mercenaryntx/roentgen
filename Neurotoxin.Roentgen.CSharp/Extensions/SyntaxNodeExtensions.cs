using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Neurotoxin.Roentgen.CSharp.Extensions
{
    public static class SyntaxNodeExtensions
    {
        private const string Item = "├──";
        private const string LastItem = "└──";
        private const string Line = "|  ";
        private const string Empty = "   ";

        public static T FindNode<T>(this SyntaxNode node)
        {
            return node.DescendantNodes().OfType<T>().SingleOrDefault();
        }

        public static T FindNode<T>(this SyntaxNode node, Func<T, bool> predicate)
        {
            return node.DescendantNodes().OfType<T>().SingleOrDefault(predicate);
        }

        public static IEnumerable<T> FindNodes<T>(this SyntaxNode node)
        {
            return node.DescendantNodes().OfType<T>();
        }

        public static IEnumerable<T> FindNodes<T>(this SyntaxNode node, Func<T, bool> predicate)
        {
            return node.DescendantNodes().OfType<T>().Where(predicate);
        }

        public static string GetPosition(this SyntaxNode node)
        {
            var span = node.SyntaxTree.GetLineSpan(node.Span);
            var lineNumber = span.StartLinePosition.Line + 1;
            return $"{node.SyntaxTree.FilePath}:{lineNumber}";
        }

        public static string Print(this SyntaxNode node)
        {
            var sb = new StringBuilder();
            PrintInternal(node, null, sb);
            return sb.ToString();
        }

        private static void PrintInternal(this SyntaxNode node, string prefix, StringBuilder sb)
        {
            sb.AppendLine($"{prefix}{node.GetType().Name} {node.Kind()} {node}");
            var p = prefix != null ? prefix.Substring(0, prefix.Length - 3) + (prefix.EndsWith(LastItem) ? Empty : Line) : string.Empty;
            var childNodes = node.ChildNodes();
            if (childNodes == null) return;

            var n = childNodes.Count();
            using (var e = childNodes.GetEnumerator())
            {
                for (var i = 0; i < n; i++)
                {
                    e.MoveNext();
                    PrintInternal(e.Current, p + (i == n - 1 ? LastItem : Item), sb);
                }
            }
        }
    }
}
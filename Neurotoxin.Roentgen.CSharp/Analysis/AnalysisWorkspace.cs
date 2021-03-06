﻿using System;
using System.Collections.Generic;
using Neurotoxin.Roentgen.CSharp.Extensions;
using Neurotoxin.Roentgen.CSharp.Models;

namespace Neurotoxin.Roentgen.CSharp.Analysis
{
    public class AnalysisWorkspace
    {
        public List<LogMessage> Diagnostics { get; } = new List<LogMessage>();
        public Dictionary<string, Solution> Solutions { get; } = new Dictionary<string, Solution>();
        public Dictionary<string, Project> Projects { get; } = new Dictionary<string, Project>();
        public Dictionary<string, SourceFile> SourceFiles { get; } = new Dictionary<string, SourceFile>();
        public Dictionary<string, IList<Class>> Classes { get; } = new Dictionary<string, IList<Class>>();
        public Dictionary<string, Method> Methods { get; } = new Dictionary<string, Method>();
        public Dictionary<string, IList<Class>> Interfaces { get; } = new Dictionary<string, IList<Class>>();

        public HashSet<LinkBase> Links { get; } = new HashSet<LinkBase>();

        public void Register(ICodePart child, ICodePart parent = null)
        {
            if (parent != null) Register(new ChildLink(parent, child));
            switch (child)
            {
                case Solution solution:
                    Solutions.TryAdd(solution.FullName, solution);
                    solution.Projects.ForEach(p => Register(p, solution));
                    break;
                case Project project:
                    Projects.TryAdd(project.FullName, project);
                    project.SourceFiles.Values.ForEach(c => Register(c, project));
                    break;
                case SourceFile sourceFile:
                    SourceFiles.TryAdd(sourceFile.FullName, sourceFile);
                    sourceFile.Classes.Values.ForEach(c => Register(c, parent));
                    sourceFile.Classes.Values.ForEach(c => Register(c, sourceFile));
                    break;
                case Class cls:
                    Classes.TryAdd(cls.FullName, cls);
                    cls.Children.ForEach(c => Register(c, cls));
                    cls.Implements.ForEach(interfaceName => Interfaces.TryAdd(interfaceName, cls));
                    break;
                case Method method:
                    Methods.TryAdd(method.FullName, method);
                    break;
                case Property property:
                    //TODO
                    break;
                default:
                    throw new NotSupportedException("Unknown code part: " + child.GetType());
            }
        }

        public void Register(LinkBase link)
        {
            Links.Add(link);
        }
    }
}
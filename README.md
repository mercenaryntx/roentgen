# Roentgen

Roentgen is a small, simple and extendible Roslyn-based static code analyzer tool for .NET Framework.<br>*(.NET Standard version is currently WIP)*

## Usage

### First steps

Create a `RoslynAnalyzer`, add one or more solutions to it with `AddSolution` or `AddSolutions` and call `Analyze` to get immediate result:
```csharp
var result = new RoslynAnalyzer().AddSolution(path).Analyze();
```
Then you can find the parsed solutions hierarchy in the result's `Solutions` property.<br>
Or if you want to iterate through all parent-child relations found in the source then please use the `Links` property.

But wait, there's more!

Roentgen's true power is in its extensibility where it can be taught to find more interesting links between the code parts.

### MethodInvocationsFinder

```csharp
var result = new RoslynAnalyzer().AddSolution(path).RegisterPostProcessor<MethodInvocationsFinder>().Analyze();
```
`MethodInvocationsFinder` is a built-in post-processor that can collect all method invocations in the code.<br>
It can collect the following link types:

| Link type | Description |
| ----------- | ----------- |
| `InternalCall`| Something calls a method within the code base |
| `ExternalCall`| Something calls an external method outside of the code base (i.e. .NET Framework dependencies) |
| `UnknownCall`| Something calls an unknown method, typically of a Nuget package where the method symbol cannot be found |

### Custom post-processor

The most typical use-case of Roentgen is when you want to collect specific external calls like SQL queries and command calls.

Just simply create your own post-processor by deriving from the `PostProcessorBase` class. It comes with one important constructor parameter, the `AnalysisWorkspace` where you can find all interim analysis data.<br>
Using this you can easily teach your post-processor to collect certain method invocations.

### Visitors

However, method calls are often not enough, we need to analyze their arguments as well.

#### FindLiteralVisitor

The `FindLiteralVisitor` can find **ALL** literals in the code the given method was called with, even if this value comes from a chain of method invocations.<br>
For example, the creator of the original code wrapped a SQL command call with a method and he used that in the code everywhere to pass a `string query` to that method. Sometimes with a string literal argument directly, sometimes with a variable or an argument of another method, and sometimes with a string concatenation to build up a certain SQL query. This visitor collects them all! Of course, it won't be able to figure out runtime values, because it's a static code analyzer, but still it's a very powerful thing to find all SQL calls within a huge code base.

#### FindVariableVisitor

The `FindLiteralVisitor` heavily relies on the `FindVariableVisitor` to jump from one part of the code to another, and you can use this too if it helps your discovery. Typically when the method you found was invoked on a variable and you are more interested in the object creation than the actual method call. Like when someone calls an `ExecuteReader` on an `SqlCommand` typed variable declared somewhere above the method invocation.

```csharp
public class SqlCommandExecutionFinder : PostProcessorBase
{
  private readonly FindVariableVisitor _findVariableVisitor;
  private readonly FindLiteralVisitor _findLiteralVisitor;

  public SqlCommandExecutionFinder(AnalysisWorkspace workspace, FindVariableVisitor findVariableVisitor, FindLiteralVisitor findLiteralVisitor) : base(workspace)
  {
     _findVariableVisitor = findVariableVisitor;
     _findLiteralVisitor = findLiteralVisitor;
  }

  public override void Process()
  {
    var invocations = Workspace.Links.InternalCalls();
    foreach (var call in Workspace.Links.ExternalCalls("System.Data.SqlClient.SqlCommand.ExecuteReader"))
    {
      if (_findVariableVisitor.FindReferenceVariable(call) as ObjectCreationExpressionSyntax objectCreation)
      {
        var literals = _findLiteralVisitor.FindLiteral(objectCreation.GetArgument(0), invocations);
        foundLiterals?.ForEach(literal => Workspace.Register(new SqlCommandCall(call.Caller, literal)));
      }
    }
  }
}
```

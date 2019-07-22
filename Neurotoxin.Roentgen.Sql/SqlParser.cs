using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Neurotoxin.Roentgen.Sql.Extensions;

namespace Neurotoxin.Roentgen.Sql
{
    //TODO: refactor
    public class SqlParser : TSqlFragmentVisitor
    {
        public IList<ParseError> Errors { get; private set; }
        public List<SqlParserResult> Result { get; private set; }

        private static TSqlScript ParsedScript;

        public override void Visit(SelectStatement selectStatement)
        {
            var result = ParseSelect(selectStatement.QueryExpression);
            StoreResult(result);
        }

        public override void Visit(UpdateStatement updateStatement)
        {
            SqlParserResult result = null;
            var us = updateStatement.UpdateSpecification;
            foreach (var set in us.SetClauses)
            {
                var assignmentSetClause = set as AssignmentSetClause;
                if (assignmentSetClause == null)
                {
                    Debugger.Break();
                    continue;
                }

                if (assignmentSetClause.Column == null) continue;

                if (result == null) result = CreateNewResult(TSqlTokenType.Update);
                result.Fields.Add(new SqlColumnDefinition(assignmentSetClause.Column.MultiPartIdentifier.Identifiers));
            }

            if (result == null) return;
            if (us.FromClause != null)
            {
                GetNamedTableReferences(us.FromClause, result);
            }
            else
            {
                ExtractNamedTableReference(updateStatement.UpdateSpecification.Target.ToTableReferenceModel(), result);
            }
            StoreResult(result);
        }

        public override void Visit(InsertStatement insertStatement)
        {
            var tableRef = insertStatement.InsertSpecification.Target.ToTableReferenceModel();
            if (tableRef != null)
            {
                var result = CreateNewResult(TSqlTokenType.Insert);
                ExtractNamedTableReference(tableRef, result);
                StoreResult(result);
            }

            if (insertStatement.InsertSpecification.InsertSource is SelectInsertSource selectInsertSource)
            {
                var result = ParseSelect(selectInsertSource.Select);
                StoreResult(result);
            }
        }

        public override void Visit(DeleteStatement deleteStatement)
        {
            var tableRef = deleteStatement.DeleteSpecification.Target.ToTableReferenceModel();
            if (tableRef == null) return;
            var result = CreateNewResult(TSqlTokenType.Delete);
            ExtractNamedTableReference(tableRef, result);
            StoreResult(result);
        }

        public override void Visit(ExecuteStatement executeStatement)
        {
            if (!(executeStatement.ExecuteSpecification.ExecutableEntity is ExecutableProcedureReference executableProcedureReference) 
               || executableProcedureReference.ProcedureReference.ProcedureReference == null) return;

            var name = executableProcedureReference.ProcedureReference.ProcedureReference.Name;
            var sb = new StringBuilder();
            if (name.DatabaseIdentifier != null)
            {
                sb.Append(name.DatabaseIdentifier.Value);
                sb.Append(".");
            }
            if (name.SchemaIdentifier != null)
            {
                sb.Append(name.SchemaIdentifier.Value);
                sb.Append(".");
            }
            sb.Append(name.BaseIdentifier.Value);
            var result = CreateNewResult(TSqlTokenType.Exec);
            result.Target = sb.ToString();
            StoreResult(result);
        }

        public override void Visit(CommonTableExpression commonTableExpression)
        {
            var result = ParseSelect(commonTableExpression.QueryExpression);
            StoreResult(result);
        }

        public override void Visit(SetVariableStatement setVariableStatement)
        {
            var result = ParseScalarExpression(setVariableStatement.Expression);
            StoreResult(result);
        }

        public override void Visit(IfStatement ifStatement)
        {
            var result = ParseIf(ifStatement.Predicate);
            StoreResult(result);
        }

        public static SqlParser Parse(string sqlText)
        {
            var visitor = new SqlParser();
            var parser = new TSql120Parser(true);
            using (var txtReader = new StringReader(sqlText))
            {
                ParsedScript = (TSqlScript)parser.Parse(txtReader, out var errors);
                visitor.Errors = errors;
                if (errors.Any()) return visitor;
                ParsedScript.Accept(visitor);
            }

            if (visitor.Result == null) return visitor;

            foreach (var result in visitor.Result)
            {
                var defaultTable = result.Tables.FirstOrDefault();
                if (defaultTable == null) continue;
                foreach (var field in result.Fields)
                {
                    var table = result.Tables.FirstOrDefault(t => t == field.Prefix || t.EndsWith("." + field.Prefix));
                    if (table != null)
                    {
                        field.Table = table;
                    }
                    else if (field.Alias == null)
                    {
                        field.Table = defaultTable;
                    }
                    else
                    {
                        var a = field.Alias.ToLower();
                        if (result.Aliases.ContainsKey(a))
                        {
                            field.Table = result.Aliases[a];
                        }
                        else
                        {
                            //Debugger.Break();
                        }
                    }
                }
            }
            return visitor;
        }

        private SqlParserResult ParseSelect(QueryExpression expression, SqlParserResult result = null)
        {
            switch (expression)
            {
                case QuerySpecification querySpecification:
                    if (querySpecification.FromClause != null)
                    {
                        foreach (var element in querySpecification.SelectElements)
                        {
                            switch (element)
                            {
                                case SelectScalarExpression scalarExpression:
                                    result = ParseScalarExpression(scalarExpression.Expression, result);
                                    continue;
                                case SelectStarExpression starExpression:
                                    if (result == null) result = CreateNewResult(TSqlTokenType.Select);
                                    result.Fields.Add(new SqlColumnDefinition(new List<Identifier> { new Identifier { Value = "*" } }));
                                    break;
                            }
                        }

                        if (result != null) GetNamedTableReferences(querySpecification.FromClause, result);
                    }
                    return result;
                case BinaryQueryExpression binaryQueryExpression:
                    result = ParseSelect(binaryQueryExpression.FirstQueryExpression, result);
                    result = ParseSelect(binaryQueryExpression.SecondQueryExpression, result);
                    return result;
                case QueryParenthesisExpression queryParenthesisExpression:
                    result = ParseSelect(queryParenthesisExpression.QueryExpression, result);
                    return result;
            }

            Debugger.Break();
            return result;
        }

        private SqlParserResult ParseIf(BooleanExpression expression, SqlParserResult result = null)
        {
            switch (expression)
            {
                case BooleanBinaryExpression booleanBinaryExpression:
                    result = ParseIf(booleanBinaryExpression.FirstExpression, result);
                    result = ParseIf(booleanBinaryExpression.SecondExpression, result);
                    return result;
                case BooleanComparisonExpression booleanComparisonExpression:
                    result = ParseScalarExpression(booleanComparisonExpression.FirstExpression, result);
                    result = ParseScalarExpression(booleanComparisonExpression.SecondExpression, result);
                    return result;
                case BooleanIsNullExpression booleanIsNullExpression:
                    result = ParseScalarExpression(booleanIsNullExpression.Expression, result);
                    return result;
                case BooleanParenthesisExpression booleanParenthesisExpression:
                    result = ParseIf(booleanParenthesisExpression.Expression, result);
                    return result;
                case ExistsPredicate existsPredicate:
                    result = ParseScalarExpression(existsPredicate.Subquery, result);
                    return result;
                case BooleanNotExpression booleanNotExpression:
                    result = ParseIf(booleanNotExpression.Expression, result);
                    return result;
                case InPredicate inPredicate:
                    result = ParseScalarExpression(inPredicate.Expression, result);
                    result = ParseScalarExpression(inPredicate.Subquery, result);
                    return result;
                case LikePredicate likePredicate:
                    result = ParseScalarExpression(likePredicate.FirstExpression, result);
                    result = ParseScalarExpression(likePredicate.SecondExpression, result);
                    return result;
            }
            return result;
        }

        private SqlParserResult ParseScalarExpression(ScalarExpression expression, SqlParserResult result = null)
        {
            switch (expression)
            {
                case null:
                    return result;
                case ColumnReferenceExpression columnReferenceExpression:
                    if (result == null) result = CreateNewResult(TSqlTokenType.Select);
                    if (columnReferenceExpression.MultiPartIdentifier != null)
                    {
                        result.Fields.Add(new SqlColumnDefinition(columnReferenceExpression.MultiPartIdentifier.Identifiers));
                    }
                    return result;
                case FunctionCall functionCall:
                    if (functionCall.CallTarget != null)
                    {
                        if (functionCall.CallTarget is MultiPartIdentifierCallTarget multipartIdentifierCallTarget)
                        {
                            var r = CreateNewResult(TSqlTokenType.Function);
                            var sb = new StringBuilder();
                            foreach (var identifier in multipartIdentifierCallTarget.MultiPartIdentifier.Identifiers)
                            {
                                if (sb.Length > 0) sb.Append(".");
                                sb.Append(identifier.Value);
                            }
                            if (sb.Length > 0) sb.Append(".");
                            sb.Append(functionCall.FunctionName.Value);
                            r.Target = sb.ToString();
                            StoreResult(r);
                        }
                        else if (functionCall.CallTarget is ExpressionCallTarget)
                        {
                            var expressionCallTarget = (ExpressionCallTarget)functionCall.CallTarget;
                            result = ParseScalarExpression(expressionCallTarget.Expression, result);
                        }
                        else
                        {
                            Debugger.Break();
                        }
                    }

                    foreach (var parameter in functionCall.Parameters)
                    {
                        result = ParseScalarExpression(parameter, result);
                    }
                    return result;
                case LeftFunctionCall leftFunctionCall:
                    foreach (var parameter in leftFunctionCall.Parameters)
                    {
                        result = ParseScalarExpression(parameter, result);
                    }
                    return result;
                case RightFunctionCall rightFunctionCall:
                    foreach (var parameter in rightFunctionCall.Parameters)
                    {
                        result = ParseScalarExpression(parameter, result);
                    }
                    return result;
                case ConvertCall convertCall:
                    result = ParseScalarExpression(convertCall.Parameter, result);
                    return result;
                case BinaryExpression binaryExpression:
                    result = ParseScalarExpression(binaryExpression.FirstExpression, result);
                    result = ParseScalarExpression(binaryExpression.SecondExpression, result);
                    return result;
                case CastCall castCall:
                    result = ParseScalarExpression(castCall.Parameter, result);
                    return result;
                case ParenthesisExpression parenthesisExpression:
                    result = ParseScalarExpression(parenthesisExpression.Expression, result);
                    return result;
                case UnaryExpression unaryExpression:
                    result = ParseScalarExpression(unaryExpression.Expression, result);
                    return result;
                case SearchedCaseExpression searchCaseExpression:
                    foreach (var when in searchCaseExpression.WhenClauses)
                    {
                        result = ParseScalarExpression(when.ThenExpression, result);
                    }
                    return result;
                case ScalarSubquery scalarSubQuery:
                    result = ParseSelect(scalarSubQuery.QueryExpression, result);
                    return result;
                case NullIfExpression nullIfExpression:
                    result = ParseScalarExpression(nullIfExpression.FirstExpression, result);
                    result = ParseScalarExpression(nullIfExpression.SecondExpression, result);
                    return result;
                case CoalesceExpression coalesceExpression:
                    foreach (var e in coalesceExpression.Expressions)
                    {
                        result = ParseScalarExpression(e, result);
                    }
                    return result;
                case SimpleCaseExpression simpleCaseExpression:
                    result = ParseScalarExpression(simpleCaseExpression.InputExpression, result);
                    foreach (var when in simpleCaseExpression.WhenClauses)
                    {
                        result = ParseScalarExpression(when.ThenExpression, result);
                    }
                    return result;
                case Literal _:
                case GlobalVariableExpression _:
                case IdentityFunctionCall _:
                    return result;
            }

            Debug.WriteLine(expression.GetType() + " ignored");
            return result;
        }

        private string GetFQParameterType(ProcedureParameter procedureParameter)
        {
            if (procedureParameter.DataType?.Name?.DatabaseIdentifier?.Value != null)
                return $"{procedureParameter.DataType.Name.DatabaseIdentifier.Value}.{procedureParameter.DataType.Name.SchemaIdentifier.Value}.{procedureParameter.DataType.Name.BaseIdentifier.Value}";
            if (procedureParameter.DataType?.Name?.SchemaIdentifier?.Value != null)
                return $"{procedureParameter.DataType.Name.SchemaIdentifier.Value}.{procedureParameter.DataType.Name.BaseIdentifier.Value}";
            if (procedureParameter.DataType?.Name?.BaseIdentifier?.Value != null)
                return $"{procedureParameter.DataType.Name.BaseIdentifier.Value}";
            throw new NotSupportedException("");
        }

        private SqlParserResult CreateNewResult(TSqlTokenType type)
        {
            var r = new SqlParserResult { Type = type };
            return r;
        }

        private void StoreResult(SqlParserResult result)
        {
            if (result == null) return;
            if (Result == null) Result = new List<SqlParserResult>();
            Result.Add(result);
        }

        private static void ExtractNamedTableReference(TableReferenceModel tableReference, SqlParserResult result)
        {
            if (tableReference == null) return;

            var baseObjectName = tableReference.SchemaObject.AsObjectName();
            result.Tables.Add(baseObjectName);

            var alias = tableReference.Alias;
            if (alias == null) return;
            result.Aliases[alias.Value.ToLower()] = baseObjectName;
        }

        private void GetNamedTableReferences(FromClause from, SqlParserResult result)
        {
            foreach (var reference in from.TableReferences)
            {
                CollectNamedTableReferences(reference, result);
            }
        }

        private void CollectNamedTableReferences(TableReference reference, SqlParserResult result)
        {
            var referenceModel = reference.ToTableReferenceModel();
            if (referenceModel != null)
            {
                ExtractNamedTableReference(referenceModel, result);
                return;
            }

            switch (reference)
            {
                case JoinTableReference joinTableReference:
                    CollectNamedTableReferences(joinTableReference.FirstTableReference, result);
                    CollectNamedTableReferences(joinTableReference.SecondTableReference, result);
                    return;
                case JoinParenthesisTableReference joinParenthesisTableReference:
                    CollectNamedTableReferences(joinParenthesisTableReference.Join, result);
                    return;
                case QueryDerivedTable queryDerivedTable:
                    ParseSelect(queryDerivedTable.QueryExpression, result);
                    result.Aliases[queryDerivedTable.Alias.Value.ToLower()] = null;
                    return;
                case PivotedTableReference pivotedTableReference:
                    CollectNamedTableReferences(pivotedTableReference.TableReference, result);
                    return;
                case UnpivotedTableReference unpivotedTableReference:
                    CollectNamedTableReferences(unpivotedTableReference.TableReference, result);
                    return;
                case VariableTableReference variableTableReference:
                    var statements = ParsedScript?.Batches.Single().Statements;
                    if (statements != null)
                    {
                        foreach (var p in statements.OfType<ProcedureStatementBodyBase>().SelectMany(s => s.Parameters).Where(p => p.VariableName.Value == variableTableReference.Variable.Name))
                        {
                            if (result == null) result = CreateNewResult(TSqlTokenType.Select);
                            result.Tables.Add(GetFQParameterType(p));
                        }
                    }
                    break;
            }
        }
    }
}
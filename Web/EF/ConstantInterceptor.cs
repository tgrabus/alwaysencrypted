using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;

namespace Web.EF
{
    public class ConstantInterceptor : DbCommandInterceptor, IDbCommandTreeInterceptor
    {
        private Dictionary<string, object> parameterValueMap;

        public ConstantInterceptor()
        {
            parameterValueMap = new Dictionary<string, object>();
        }

        public void TreeCreated(DbCommandTreeInterceptionContext interceptionContext)
        {
            if (interceptionContext.OriginalResult.DataSpace == DataSpace.CSpace)
            {
                parameterValueMap.Clear();
                if (interceptionContext.OriginalResult.CommandTreeKind ==
                    DbCommandTreeKind.Query)
                {
                    var queryCommandTree =
                        (DbQueryCommandTree)interceptionContext.OriginalResult;
                    var originalParameterCount = queryCommandTree.Parameters.Count();

                    var visitor = new
                        ConstantExpressionReplacingVisitor(originalParameterCount);

                    var newExpression = queryCommandTree.Query.Accept(visitor);
                    parameterValueMap = visitor.ParameterValueMap;

                    if (visitor.ParameterValueMap.Count > 0)
                    {
                        interceptionContext.Result = new DbQueryCommandTree(
                            queryCommandTree.MetadataWorkspace,
                            queryCommandTree.DataSpace,
                            newExpression);
                    }
                }
            }
        }

        public override void ReaderExecuting(
            DbCommand command,
            DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            ProcessParameters(command);
            base.ReaderExecuting(command, interceptionContext);
        }

        public override void ScalarExecuting(
            DbCommand command,
            DbCommandInterceptionContext<object> interceptionContext)
        {
            ProcessParameters(command);
            base.ScalarExecuting(command, interceptionContext);
        }

        public override void NonQueryExecuting(
            DbCommand command,
            DbCommandInterceptionContext<int> interceptionContext)
        {
            ProcessParameters(command);
            base.NonQueryExecuting(command, interceptionContext);
        }

        private void ProcessParameters(DbCommand command)
        {
            if (parameterValueMap.Count > 0)
            {
                foreach (DbParameter prm in command.Parameters)
                {
                    if (parameterValueMap.TryGetValue(
                        prm.ParameterName,
                        out var parameterValue))
                    {
                        prm.Value = parameterValue;
                    }
                }
            }

            parameterValueMap.Clear();
        }
    }

    public class ConstantExpressionReplacingVisitor : DefaultExpressionVisitor
    {
        private int originalParameterCount;

        public Dictionary<string, object> ParameterValueMap { get; private set; }

        public ConstantExpressionReplacingVisitor(int originalParameterCount)
        {
            this.originalParameterCount = originalParameterCount;
            ParameterValueMap = new Dictionary<string, object>();
        }

        public override DbExpression Visit(DbConstantExpression expression)
        {
            var count = originalParameterCount + ParameterValueMap.Count;
            var result = DbExpressionBuilder.Parameter(
                expression.ResultType,
                "p__linq__" + count);
            ParameterValueMap.Add(result.ParameterName, expression.Value);

            return result;
        }
    }
}
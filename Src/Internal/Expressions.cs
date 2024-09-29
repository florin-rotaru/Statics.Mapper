using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Statics.Mapper.Internal
{
    internal static class Expressions
    {
        public static string GetName(Expression expression, bool includePath = true)
        {
            Stack<string> nameSegments = new();
            Expression? evaluate = expression;

            while (evaluate != null)
            {
                switch (evaluate)
                {
                    case ParameterExpression:
                        evaluate = null;
                        break;
                    case MemberExpression memberExpression:
                        nameSegments.Push(memberExpression.Member.Name);
                        evaluate = includePath ? memberExpression.Expression : null;
                        break;
                    case UnaryExpression unaryExpression:
                        evaluate = unaryExpression.Operand;
                        break;
                    case LambdaExpression lambdaExpression:
                        evaluate = lambdaExpression.Body;
                        break;
                    case ConstantExpression constant:
                        nameSegments.Push(constant.Value?.ToString() ?? string.Empty);
                        evaluate = null;
                        break;
                    default:
                        throw new ArgumentException("Unsupported expression type.", nameof(expression));
                }
            }

            return nameSegments.Count != 0 ? string.Join(".", [.. nameSegments]) : string.Empty;
        }

        public static List<string> GetNames(Expression expression, bool includePath = true)
        {
            List<string> names = [];
            Queue<Expression> queue = new([expression]);
            Stack<string> memberName = new();

            while (queue.Count != 0)
            {
                Expression expr = queue.Dequeue();
                if (expr is MemberExpression memberExpression)
                {
                    names.Add(memberExpression.Member.Name);
                }
                else if (expr is NewExpression newExpression)
                {
                    names.AddRange(
                        newExpression.Arguments.Select((s, i) =>
                        {
                            if (s.NodeType == ExpressionType.Constant)
                                return newExpression.Members[i].Name;
                            else
                                return GetName(s, includePath);
                        }));
                }
                else if (expr is UnaryExpression && expr.NodeType == ExpressionType.Convert || expr.NodeType == ExpressionType.ConvertChecked)
                {
                    queue.Enqueue(((UnaryExpression)expr).Operand);
                }
                else if (expr is LambdaExpression lambdaExpression)
                {
                    queue.Enqueue(lambdaExpression.Body);
                }
            }

            return names;
        }

    }
}

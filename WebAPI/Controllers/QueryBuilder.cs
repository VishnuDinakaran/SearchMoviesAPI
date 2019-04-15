using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using WebAPI.Entities;

namespace WebAPI.Controllers
{
    public static class QueryBuilder
    {
        public static Func<T, bool> GetCompiledFunction<T>(Query query)
            where T : class
        {
            Func<T, bool> returnCompiledFunction = null;
            Expression<Func<T, bool>> dynamic_Expression;
            dynamic_Expression = GetExpression<T>(query);
            returnCompiledFunction = dynamic_Expression.Compile();
            return returnCompiledFunction;
        }

        private static Expression<Func<T, bool>> GetExpression<T>(Query query) where T : class
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "param");
            return Expression.Lambda<Func<T, bool>>(GetFullExpressionFromQuery(query, parameterExpression), parameterExpression);
        }

        private static Expression GetFullExpressionFromQuery(Query query, ParameterExpression parameterExpression)
        {
            Expression rootExpression = BuildDynamicExpression(query, parameterExpression);
            if (query.Queries != null && query.Queries.Any())
            {
                IOrderedEnumerable<Query> queries = query.Queries.OrderBy(x => x.Id);
                foreach (Query item in queries)
                {
                    if (!string.IsNullOrWhiteSpace(item.Junction))
                    {
                        switch (item.Junction.ToUpper())
                        {
                            case "AND":
                                {
                                    rootExpression = Expression.AndAlso(rootExpression, GetFullExpressionFromQuery(item, parameterExpression));
                                }
                                break;
                            case "OR":
                                {
                                    rootExpression = Expression.OrElse(rootExpression, GetFullExpressionFromQuery(item, parameterExpression));
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        throw new NullReferenceException($"Junction on Query object cannot be null or empty");
                    }
                }
            }

            return rootExpression;
        }

        private static Expression BuildDynamicExpression(Query query, ParameterExpression parameterExpression)
        {
            var propInfo = parameterExpression.Type.GetProperty(query.PropertyName);

            if (propInfo == null)
            {
                throw new ArgumentException( $"propertyName: '{query.PropertyName}' on search query is not valid.");
            }

            string operatorStrUpper = query.Operator.ToUpper();
            object queryValue = null;
            string valueDataTypeUpper = propInfo.PropertyType.Name.ToUpper();
            Type propDataType = propInfo.PropertyType;

            if (valueDataTypeUpper == "TIMESPAN")
            {
                queryValue = TimeSpan.Parse(query.Value);
            }
            else
            {
                queryValue = Convert.ChangeType(query.Value, propDataType);
            }

            Expression dynamic_Exression = Expression.Empty();
            switch (operatorStrUpper)
            {
                case "==":
                case "EQUALS":
                    {
                        dynamic_Exression = Expression.Equal(Expression.Property(parameterExpression, propInfo), Expression.Constant(queryValue));
                    }
                    break;
                case "!=":
                case "NOTEQUALS":
                    {
                        dynamic_Exression = Expression.NotEqual(Expression.Property(parameterExpression, propInfo), Expression.Constant(queryValue));
                    }
                    break;
                case "<":
                case "LESSTHAN":
                    {
                        dynamic_Exression = Expression.LessThan(Expression.Property(parameterExpression, propInfo), Expression.Constant(queryValue));
                    }
                    break;
                case "<=":
                case "LESSTHANOREQUAL":
                    {
                        dynamic_Exression = Expression.LessThanOrEqual(Expression.Property(parameterExpression, propInfo), Expression.Constant(queryValue));
                    }
                    break;
                case ">":
                case "GREATERTHAN":
                    {
                        dynamic_Exression = Expression.GreaterThan(Expression.Property(parameterExpression, propInfo), Expression.Constant(queryValue));
                    }
                    break;
                case ">=":
                case "GREATERTHANOREQUAL":
                    {
                        dynamic_Exression = Expression.GreaterThanOrEqual(Expression.Property(parameterExpression, propInfo), Expression.Constant(queryValue));
                    }
                    break;
                case "STRINGCONTAINS":
                    {
                        dynamic_Exression = GetStringContainsExpression(parameterExpression, query.Value, propInfo);
                    }
                    break;
                default:
                    break;
            }

            return dynamic_Exression;
        }

        private static Expression GetStringContainsExpression(ParameterExpression parameterExpression, string value, PropertyInfo propInfo)
        {
            MethodInfo methodInfo = typeof(string).GetRuntimeMethod("Contains", new Type[] { typeof(string), typeof(StringComparison) });
            ConstantExpression valueConstantExpression = Expression.Constant(value);
            ConstantExpression strCompOrdIgnConstExp = Expression.Constant(StringComparison.OrdinalIgnoreCase);
            MemberExpression memberExpression = Expression.Property(parameterExpression, propInfo);            
            return Expression.Call(memberExpression, methodInfo, valueConstantExpression, strCompOrdIgnConstExp);
        }
    }
}

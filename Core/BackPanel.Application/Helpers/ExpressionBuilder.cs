using System;
using System.Linq.Expressions;
using System.Reflection;
using BackPanel.Domain.Enums;

namespace BackPanel.Application.Helpers;
public static class ExpressionBuilder
{
    public static Expression<Func<T, bool>> BuildComparisonExpression<T>(string propertyName, ComparisonOperator op, string value)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(param, propertyName);
        var parsedValue = ParseValue(value, property.Type);
        var constant = Expression.Constant(parsedValue);
        var comparison = BuildComparison(property, constant, op);

        return Expression.Lambda<Func<T, bool>>(comparison, param);
    }

    private static object ParseValue(string value, Type targetType)
    {
        return Convert.ChangeType(value, targetType);
        throw new ArgumentException($"Unsupported data type: {targetType.Name}");
    }

    private static Expression BuildComparison(Expression left, Expression right, ComparisonOperator op)
    {
        MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
        return op switch
        {
            ComparisonOperator.Equal => Expression.Equal(left, right),
            ComparisonOperator.NotEqual => Expression.NotEqual(left, right),
            ComparisonOperator.LessThan => Expression.LessThan(left, right),
            ComparisonOperator.LessThanOrEqual => Expression.LessThanOrEqual(left, right),
            ComparisonOperator.GreaterThan => Expression.GreaterThan(left, right),
            ComparisonOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(left, right),
            ComparisonOperator.Contains => Expression.Call(left, containsMethod, right),
            _ => throw new ArgumentException("Invalid comparison operator."),
        };
    }
}
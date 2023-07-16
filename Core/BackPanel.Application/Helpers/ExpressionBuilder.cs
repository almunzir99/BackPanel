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
        if (!IsSupportedComparison(left.Type, op) || !IsSupportedComparison(right.Type, op))
        {
            return Expression.Equal(left, right);
        }
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
    private static bool IsSupportedComparison(Type type, ComparisonOperator op)
    {
        if (type == typeof(string))
        {
            return op == ComparisonOperator.Equal || op == ComparisonOperator.NotEqual || op == ComparisonOperator.Contains || op == ComparisonOperator.StartsWith || op == ComparisonOperator.EndsWith;
        }
        else if (type == typeof(int) || type == typeof(double) || type == typeof(float) || type == typeof(decimal) || type == typeof(DateTime))
        {
            return op == ComparisonOperator.Equal || op == ComparisonOperator.NotEqual ||
                   op == ComparisonOperator.LessThan || op == ComparisonOperator.LessThanOrEqual ||
                   op == ComparisonOperator.GreaterThan || op == ComparisonOperator.GreaterThanOrEqual;
        }
        else if (type == typeof(bool))
        {
            return op == ComparisonOperator.Equal || op == ComparisonOperator.NotEqual;
        }

        return false;
    }
}
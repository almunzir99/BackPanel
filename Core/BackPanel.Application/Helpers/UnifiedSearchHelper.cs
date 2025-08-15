using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BackPanel.Application.Helpers
{
    public static class UnifiedSearchHelper
    {


        // Alternative version with configurable options
        public static Expression<Func<T, bool>>? GetUnifiedSearchExpression<T>(
            string? search,
            bool caseSensitive = false,
            bool includeNullValues = false)
        {
            if (string.IsNullOrWhiteSpace(search))
                return (c => true);

            var parameter = Expression.Parameter(typeof(T), "e");
            var searchTerm = caseSensitive ? search : search.ToLowerInvariant();
            var searchTermExpression = Expression.Constant(searchTerm, typeof(string));

            var searchableProperties = typeof(T).GetProperties()
                .Where(IsSearchableProperty)
                .ToList();

            if (!searchableProperties.Any())
                return null;

            var predicates = searchableProperties.Select(propertyInfo =>
            {
                return CreatePropertySearchExpressionWithOptions(
                    parameter,
                    propertyInfo,
                    searchTermExpression,
                    caseSensitive,
                    includeNullValues);
            }).ToList();

            var orExpression = predicates
                .Aggregate(Expression.OrElse);

            return Expression.Lambda<Func<T, bool>>(orExpression, parameter);
        }

        private static Expression CreatePropertySearchExpressionWithOptions(
            ParameterExpression parameter,
            PropertyInfo propertyInfo,
            ConstantExpression searchTermExpression,
            bool caseSensitive,
            bool includeNullValues)
        {
            Expression propertyAccess = Expression.Property(parameter, propertyInfo);
            var propertyType = propertyInfo.PropertyType;
            var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            Expression? nullCheck = null;
            if (propertyType != underlyingType) // Nullable type
            {
                nullCheck = Expression.Equal(propertyAccess, Expression.Constant(null, propertyType));

                if (includeNullValues)
                {
                    // If including null values in search, return true for null properties
                    // This means null properties will match any search term
                    return nullCheck;
                }
            }

            Expression stringExpression;

            if (underlyingType == typeof(string))
            {
                stringExpression = Expression.Condition(
                    Expression.Equal(propertyAccess, Expression.Constant(null, typeof(string))),
                    Expression.Constant(string.Empty, typeof(string)),
                    propertyAccess);
            }
            else
            {
                if (propertyType != underlyingType) // Nullable
                {
                    var hasValueProperty = Expression.Property(propertyAccess, "HasValue");
                    var valueProperty = Expression.Property(propertyAccess, "Value");
                    var toStringCall = Expression.Call(valueProperty, "ToString", Type.EmptyTypes);

                    stringExpression = Expression.Condition(
                        hasValueProperty,
                        toStringCall,
                        Expression.Constant(string.Empty, typeof(string)));
                }
                else
                {
                    stringExpression = Expression.Call(propertyAccess, "ToString", Type.EmptyTypes);
                }
            }

            // Apply case sensitivity
            if (!caseSensitive)
            {
                var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant", Type.EmptyTypes);
                stringExpression = Expression.Call(stringExpression, toLowerMethod!);
            }

            // Create Contains call
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var containsCall = Expression.Call(stringExpression, containsMethod!, searchTermExpression);

            // Handle null check for nullable types
            if (nullCheck != null && !includeNullValues)
            {
                return Expression.AndAlso(Expression.Not(nullCheck), containsCall);
            }

            return containsCall;
        }
        private static bool IsSearchableProperty(PropertyInfo property)
        {
            // Check if property is readable
            if (!property.CanRead)
                return false;

            var propertyType = property.PropertyType;

            // Handle nullable types
            var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            // Include string, numeric types, DateTime, bool, and enums
            return underlyingType == typeof(string) ||
                   underlyingType.IsPrimitive ||
                   underlyingType == typeof(DateTime) ||
                   underlyingType == typeof(decimal) ||
                   underlyingType == typeof(Guid) ||
                   underlyingType.IsEnum;
        }
    }
}
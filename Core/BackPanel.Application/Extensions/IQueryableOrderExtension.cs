using System.Linq.Expressions;

namespace BackPanel.Application.Extensions;
public static class IQueryableOrderExtension
{
    public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> source, string propertyName, bool descendingOrder = false)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(param, propertyName);
        var selector = Expression.Lambda(property, param);

        string methodName = descendingOrder ? "OrderByDescending" : "OrderBy";
        var methodCall = Expression.Call(
            typeof(Queryable),
            methodName,
            new Type[] { typeof(T), property.Type },
            source.Expression,
            Expression.Quote(selector)
        );
        return source.Provider.CreateQuery<T>(methodCall);
    }
}
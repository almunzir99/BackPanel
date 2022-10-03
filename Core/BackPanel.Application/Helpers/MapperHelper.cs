using System.Collections;

namespace BackPanel.Application.Helpers;

public class MapperHelper
{
    public TDest Map<TSource, TDest>(TSource source, TDest dest, IList<Func<TSource, bool>>? conditions = null,
        string[]? propsToExclude = null)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (dest == null)
            throw new ArgumentNullException(nameof(dest));
        propsToExclude = propsToExclude ?? Array.Empty<string>();
        var sourceProps = source.GetType().GetProperties();
        var destProps = dest.GetType().GetProperties();

        foreach (var prop in destProps)
        {
            var propName = prop.Name;
            var propValue = prop.GetValue(dest);
            foreach (var sourceProp in sourceProps)
            {
                var sourcePropName = sourceProp.Name;
                var sourcePropValue = sourceProp.GetValue(source);
                if (propName == sourcePropName
                    && prop.PropertyType == sourceProp.PropertyType
                    && sourcePropValue != default && propsToExclude.Contains(propName) == false)
                {
                    if (conditions != null)
                        foreach (var condition in conditions)
                        {
                            var result = condition.Invoke(source);
                            if (result == false)
                                continue;
                        }

                    if (prop.PropertyType.IsPrimitive
                        || prop.PropertyType == typeof(Decimal)
                        || prop.PropertyType == typeof(String) || prop.PropertyType == typeof(DateTime))
                    {
                        if (sourcePropValue == propValue)
                            continue;
                        if(sourcePropValue is int && (sourcePropValue as int?) == 0)
                            continue;
                        prop.SetValue(dest, sourcePropValue);
                    }
                    else if (typeof(IEnumerable).IsAssignableFrom(sourceProp.PropertyType))
                    {
                        if (propValue is IEnumerable listPropValue)
                            foreach (var value in listPropValue)
                            {
                                var valueIdProp = value.GetType().GetProperties().SingleOrDefault(c => c.Name == "Id");
                                if (valueIdProp == null)
                                    continue;
                                var valueIdPropValue = valueIdProp.GetValue(value);
                                if (valueIdPropValue == null)
                                    continue;
                                if (sourcePropValue is IEnumerable listSourcePropValue)
                                    foreach (var sourceValue in listSourcePropValue)
                                    {
                                        var sourceValueIdProp = sourceValue.GetType().GetProperties()
                                            .SingleOrDefault(c => c.Name == "Id");
                                        if (sourceValueIdProp == null)
                                            continue;
                                        var sourceValueIdPropValue = sourceValueIdProp.GetValue(value);
                                        if (sourceValueIdPropValue == null)
                                            continue;

                                        if (sourceValueIdPropValue == valueIdPropValue)
                                        {
                                            var mapMethodInfo = this.GetType().GetMethod("Map");
                                            if (mapMethodInfo != null)
                                                mapMethodInfo.MakeGenericMethod(sourceProp.PropertyType,
                                                        prop.PropertyType)
                                                    .Invoke(this, new[] { sourceValue, value, null, null });
                                        }
                                    }
                            }
                    }
                    else
                    {
                        var mapMethodInfo = this.GetType().GetMethod("Map");
                        if (mapMethodInfo != null)
                            mapMethodInfo.MakeGenericMethod(sourceProp.PropertyType, prop.PropertyType)
                                .Invoke(this, new[] { sourcePropValue, propValue, null, propsToExclude });
                    }
                }
            }
        }

        return dest;
    }
}
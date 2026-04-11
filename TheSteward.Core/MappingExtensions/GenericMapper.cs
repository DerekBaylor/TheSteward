// TheSteward.Core/MappingExtensions/GenericMapper.cs

using System.Reflection;
using System.Runtime.CompilerServices;

namespace TheSteward.Core.MappingExtensions;

/// <summary>
/// A lightweight reflection-based property mapper.
/// Copies values between two objects where property names and types match exactly.
/// Use this for flat, scalar-only mappings. For computed fields or navigation
/// flattening, write an explicit extension method instead.
/// </summary>
public static class GenericMapper
{
    // Cache property pairs per type combination to avoid repeated reflection on hot paths.
    // Populated once per unique (TSource, TDestination) pair for the lifetime of the app.
    private static readonly Dictionary<(Type, Type), (PropertyInfo Source, PropertyInfo Dest)[]> _cache = new();
    private static readonly Lock _lock = new();

    /// <summary>
    /// Copies all matching scalar properties from <paramref name="source"/> to
    /// <paramref name="destination"/>. Properties are matched by name and exact type.
    /// Read-only destination properties are skipped silently.
    /// </summary>
    public static void MapProperties<TSource, TDestination>(TSource source, TDestination destination)
    {
        if (source is null || destination is null) return;

        var pairs = GetCachedPairs(typeof(TSource), typeof(TDestination));

        foreach (var (srcProp, destProp) in pairs)
            destProp.SetValue(destination, srcProp.GetValue(source));
    }

    /// <summary>
    /// Creates a new <typeparamref name="TDestination"/> instance and copies all matching
    /// scalar properties from <paramref name="source"/> into it.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="RuntimeHelpers.GetUninitializedObject"/> internally so that types
    /// with <c>required</c> members are supported without needing a <c>new()</c> constraint.
    /// All required properties must be covered by matching source properties, or you must
    /// set them manually after calling this method.
    /// </remarks>
    public static TDestination Map<TSource, TDestination>(TSource source)
    {
        // GetUninitializedObject bypasses constructor and required-member checks,
        // letting reflection populate every property including required ones.
        var destination = (TDestination)RuntimeHelpers.GetUninitializedObject(typeof(TDestination));
        MapProperties(source, destination);
        return destination;
    }

    /// <summary>
    /// Projects a sequence of <typeparamref name="TSource"/> into a list of
    /// <typeparamref name="TDestination"/> using <see cref="Map{TSource,TDestination}"/>.
    /// </summary>
    public static List<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> source)
        => source.Select(Map<TSource, TDestination>).ToList();

    /// <summary>
    /// Projects a sequence using a caller-supplied mapping delegate.
    /// Use this when the type needs an explicit ToDto() override.
    /// </summary>
    public static List<TDestination> MapList<TSource, TDestination>(
        IEnumerable<TSource> source,
        Func<TSource, TDestination> mapper)
        => source.Select(mapper).ToList();


    #region Private Methods
    /// <summary>
    /// Retrieves an array of property pairs that have matching names and types between the specified source and
    /// destination types. Only writable properties on the destination type are considered.
    /// </summary>
    /// <remarks>Results are cached for each unique pair of source and destination types to improve
    /// performance on subsequent calls.</remarks>
    /// <param name="sourceType">The type from which properties are matched. Only public instance properties are considered.</param>
    /// <param name="destType">The type to which properties are matched. Only public instance properties that are writable are considered.</param>
    /// <returns>An array of tuples, each containing a source property and a corresponding destination property with the same
    /// name and type. The array is empty if no matching pairs are found.</returns>
    private static (PropertyInfo, PropertyInfo)[] GetCachedPairs(Type sourceType, Type destType)
    {
        var key = (sourceType, destType);

        lock (_lock)
        {
            if (_cache.TryGetValue(key, out var cached))
                return cached;

            var srcProps = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var destProps = destType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                    .Where(p => p.CanWrite)
                                    .ToDictionary(p => (p.Name, p.PropertyType));

            var pairs = srcProps
                .Where(sp => destProps.ContainsKey((sp.Name, sp.PropertyType)))
                .Select(sp => (sp, destProps[(sp.Name, sp.PropertyType)]))
                .ToArray();

            _cache[key] = pairs;
            return pairs;
        }
    }
    #endregion Private Methods
}



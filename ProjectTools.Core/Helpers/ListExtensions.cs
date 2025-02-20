namespace ProjectTools.Core.Helpers;

/// <summary>
///     Various extensions for lists.
/// </summary>
public static class ListExtensions
{
    /// <summary>
    ///     Combines two lists into one.
    /// </summary>
    /// <param name="list1">The first list</param>
    /// <param name="list2">The second list</param>
    /// <typeparam name="T">The type for the lists</typeparam>
    /// <returns>The combined list</returns>
    public static List<T> CombineLists<T>(this List<T> list1, List<T> list2)
    {
        var combinedList = new List<T>(list1);
        combinedList.AddRange(list2);
        return combinedList;
    }

    /// <summary>
    ///     Compares two lists to see if they contain the same elemants.
    /// </summary>
    /// <param name="list1">The first list.</param>
    /// <param name="list2">The second list.</param>
    /// <typeparam name="T">The type of the lists.</typeparam>
    /// <returns>True if equal, False otherwise.</returns>
    public static bool CompareLists<T>(this List<T> list1, List<T> list2)
    {
        if (list1.Count != list2.Count)
        {
            return false;
        }

        var oldList = new List<T>(list1);
        oldList.Sort();
        var newList = new List<T>(list2);
        newList.Sort();

        return oldList.SequenceEqual(newList);
    }

    /// <summary>
    ///     Checks whether the value is contained in the list.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="value">The value.</param>
    /// <param name="caseSensitive">Whether the check is case-insensitive or not. Only applicable for strings.</param>
    /// <returns>True if the value is contained, False otherwise.</returns>
    public static bool IsContained<T>(this List<T> list, T value, bool caseSensitive)
    {
        if (list == null || list.Count == 0)
        {
            return false;
        }

        if (value is string)
        {
            var actualList = caseSensitive
                ? list.Select(x => x.ToString()).ToList()
                : list.Select(x => x.ToString().ToLower()).ToList();
            var actualValue = caseSensitive ? value.ToString() : value.ToString().ToLower();

            return actualList.Contains(actualValue);
        }

        return list.Contains(value);
    }
}

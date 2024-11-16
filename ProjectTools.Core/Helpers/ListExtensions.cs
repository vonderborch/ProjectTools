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
}

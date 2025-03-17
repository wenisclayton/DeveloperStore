namespace Ambev.DeveloperEvaluation.Common.Extensions;

/// <summary>
/// Provides extension methods for the string class.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Determines whether the specified string is null or an empty string.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns>
    ///   <c>true</c> if the specified string is null or empty; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Determines whether the specified string is null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns>
    ///   <c>true</c> if the specified string is null, empty, or only white-space; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNullOrWhiteSpace(this string value)
    {
        return string.IsNullOrWhiteSpace(value);
    }
}
using Avalonia.Media;

namespace FractalsDesktopMultiOS;

/// <summary>
/// Defines a color palette contract that maps normalized fractal escape values to RGB colors.
/// </summary>
public interface IFractalColorPalette
{
    /// <summary>
    /// Gets the human-readable display name of the color palette.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Resolves a color for a normalized value in the range [0.0, 1.0].
    /// </summary>
    /// <param name="normalizedValue">The normalized escape or height coordinate.</param>
    /// <returns>The resolved <see cref="Color"/> for the given value.</returns>
    Color GetColor(double normalizedValue);
}

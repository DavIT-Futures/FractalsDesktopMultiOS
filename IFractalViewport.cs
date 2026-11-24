namespace FractalsDesktopMultiOS;

/// <summary>
/// Defines a named complex-plane viewport window used for escape-time fractal rendering presets.
/// </summary>
public interface IFractalViewport
{
    /// <summary>
    /// Gets the horizontal real-axis center coordinate of the viewport.
    /// </summary>
    double CenterX { get; }

    /// <summary>
    /// Gets the vertical imaginary-axis center coordinate of the viewport.
    /// </summary>
    double CenterY { get; }

    /// <summary>
    /// Gets the horizontal span of the viewport in complex-plane units.
    /// </summary>
    double Width { get; }

    /// <summary>
    /// Gets the human-readable display name of the viewport preset.
    /// </summary>
    string Name { get; }
}

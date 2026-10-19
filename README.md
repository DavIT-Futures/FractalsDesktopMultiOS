# FractalsDesktopMultiOS

A modern, high-performance, cross-platform desktop application built with **C# (.NET 10 / Avalonia UI)** for generating and exploring fractals — including the **Sierpinski Triangle** and the **Mandelbrot Set** — with full parameter customization, rich color palette selection, and multiple generation algorithms.

Powered by **Avalonia UI** and the Skia rendering engine, it runs natively on:
- 🪟 **Windows** (x64 / ARM64)
- 🐧 **Linux** (X11 / Wayland, Ubuntu, Fedora, Debian, Arch, etc.)
- 🍏 **macOS** (Apple Silicon M1/M2/M3/M4 & Intel)

---

## Supported Fractals & Algorithms

### 1. Mandelbrot Set
- **Parallel Multi-Threaded Engine:** Computes millions of pixels simultaneously using `Parallel.For`.
- **Modes:**
  - **Smooth Continuous Shading:** Normalized iteration count algorithm avoiding harsh color stepping.
  - **Classic Escape-Time Bands:** Discrete color cycling.
- **View Presets:**
  - `Full View (Classic)`
  - `Seahorse Valley`
  - `Elephant Valley`
  - `Mini Mandelbrot`
  - `Triple Spiral`
- **Configurable Parameters:**
  - Max iterations (50 to 2,000).
  - Color cycle frequency multiplier.

### 2. Sierpinski Triangle
- **Algorithms:**
  - **Recursion (Filled Triangles):** Geometric subdivision of triangles.
  - **Recursion (Wireframe):** Outline / border visualization of triangle subdivisions.
  - **Chaos Game (Random Points):** Stochastic fractal generation via random vertex midpoint iterations.
- **Configurable Parameters:**
  - Recursion depth (0 to 9) or point count (up to 1,000,000 points).
  - Line thickness and point radius (1 to 6 px).

---

## Visual Themes & Exporting

- **Dynamic Palettes:** Rainbow / Height Gradient, Neon Cyan, Gold / Amber, Matrix Green, Purple Violet, Fire.
- **Backgrounds:** Smoky Black (Dark Theme), Pure White (Light Theme), Midnight Navy.
- **Full Screen / Maximized Window:** Opens maximized automatically on launch.
- **Cross-Platform Image Export:** Save generated fractals to PNG using the native platform dialog.

---

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/) (or newer)

---

## Getting Started

Run with dotnet:

```bash
dotnet run
```

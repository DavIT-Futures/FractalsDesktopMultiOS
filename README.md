# FractalsDesktopMultiOS

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Avalonia UI](https://img.shields.io/badge/Avalonia-12.1-8C4FFF?logo=avalonia&logoColor=white)](https://avaloniaui.net/)
[![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20Linux%20%7C%20macOS-blue)](https://github.com/DavIT-Futures/FractalsDesktopMultiOS)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

A modern, high-performance, cross-platform desktop application built with **C# (.NET 10 / Avalonia UI)** for generating and exploring iconic fractals with full parameter customization, rich color palette selection, view presets, and parallel multi-threaded rendering.

Powered by **Avalonia UI** and the Skia graphics engine, it runs natively on:
- 🪟 **Windows** (x64 / ARM64)
- 🐧 **Linux** (X11 / Wayland, Ubuntu, Fedora, Debian, Arch, etc.)
- 🍏 **macOS** (Apple Silicon M1/M2/M3/M4 & Intel)

---

## Supported Fractals & Algorithms

### 1. Mandelbrot Set
- **Description:** The most famous mathematical fractal, generated via the iteration $z_{n+1} = z_n^2 + c$ with $z_0 = 0$.
- **Engine:** Parallelized scanline rendering across all CPU cores (`Parallel.For`).
- **Shading Modes:** Smooth Continuous Shading (normalized iteration count) & Classic Escape-Time Bands.
- **View Presets:** Full View, Seahorse Valley, Elephant Valley, Mini Mandelbrot, Triple Spiral.

### 2. Julia Set
- **Description:** The intimate companion to Mandelbrot. While Mandelbrot iterates from $z_0 = 0$ with varying $c$, the Julia Set holds $c$ constant while varying $z_0$ across the complex plane.
- **Engine:** Multi-threaded parallel escape-time algorithm.
- **View Presets:**
  - `Electro Swirl (-0.7 + 0.27i)`
  - `Classic Dendrite (-0.4 + 0.6i)`
  - `Douady's Rabbit (-0.123 + 0.745i)`
  - `San Marco Dragon (-0.75 + 0.0i)`
  - `Siegel Disk (-0.39 - 0.58i)`

### 3. Barnsley Fern
- **Description:** Discovered by mathematician Michael Barnsley in 1988 using Iterated Function Systems (IFS). Recreates the biological self-similarity of natural fern leaves (*Asplenium adiantum-nigrum*) using four affine stochastic transformations.
- **Engine:** Monte Carlo chaos game drawing with configurable point count (up to 2,000,000 points) and dot radius.

### 4. Burning Ship
- **Description:** Created by Michael Michelitsch and Otto E. Rössler in 1992. It modifies the Mandelbrot equation by taking the absolute value of both real and imaginary components before squaring: $z_{n+1} = (|Re(z_n)| + i|Im(z_n)|)^2 + c$, breaking symmetry and creating fire, mast, and armada formations.
- **View Presets:** Full Ship View, Burning Ship Core, Ship Masts & Flame Jets.

### 5. Sierpinski Triangle
- **Algorithms:**
  - **Recursion (Filled Triangles):** Geometric subdivision of triangles.
  - **Recursion (Wireframe):** Outline / border visualization of triangle subdivisions.
  - **Chaos Game (Random Points):** Stochastic fractal generation via random vertex midpoint iterations.
- **Configurable Parameters:** Recursion depth (0 to 9) or point count (up to 1,000,000 points), line thickness / point radius.

---

## Visual Themes & Exporting

- **Dynamic Palettes:** Rainbow / Height Gradient, Neon Cyan, Gold / Amber, Matrix Green, Purple Violet, Fire.
- **Backgrounds:** Smoky Black (Dark Theme), Pure White (Light Theme), Midnight Navy.
- **Maximized Full Screen:** Automatically launches in maximized window mode.
- **Cross-Platform Image Export:** Save generated fractals to PNG using the native platform file dialog.

---

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/) (or newer)

---

## Getting Started

Run with dotnet:

```bash
dotnet run
```

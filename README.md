# FractalsDesktopMultiOS

A modern, cross-platform desktop application built with **C# (.NET 10 / Avalonia UI)** for generating and rendering fractals (including the **Sierpinski Triangle**) with full parameter customization, color palette selection, and multiple generation algorithms.

Powered by **Avalonia UI** and the Skia rendering engine, it runs natively on:
- 🪟 **Windows** (x64 / ARM64)
- 🐧 **Linux** (X11 / Wayland, Ubuntu, Fedora, Debian, Arch, etc.)
- 🍏 **macOS** (Apple Silicon M1/M2/M3/M4 & Intel)

---

## Features

- **Cross-Platform:** Native GPU-accelerated rendering powered by Skia.
- **Generation Algorithms:**
  - **Recursion (Filled Triangles):** Geometric subdivision of triangles.
  - **Recursion (Wireframe):** Outline / border visualization of triangle subdivisions.
  - **Chaos Game (Random Points):** Stochastic fractal generation via random vertex midpoint iterations.
- **Configurable Parameters:**
  - Adjustable recursion depth (0–9) or point count (up to 1,000,000 points).
  - Configurable line thickness and point radius (1–6 px).
  - Dynamic color palettes (Rainbow / Height Gradient, Neon Cyan, Gold, Matrix Green, Purple, Fire).
  - Background themes (Smoky Black Dark Theme, Pure White Light Theme, Midnight Navy).
- **High Performance:** Background computation on thread pool (`Task.Run`) and direct unmanaged pixel buffer manipulation (`WriteableBitmap`).
- **Exporting:** Cross-platform native image saving to PNG files via Avalonia's `StorageProvider`.

---

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/) (or newer)

---

## Getting Started

Clone the repository and run:

```bash
dotnet run
```

Or target the project file directly:

```bash
dotnet run --project FractalsDesktopMultiOS.csproj
```

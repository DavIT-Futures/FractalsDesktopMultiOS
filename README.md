# FractalsDesktopMultiOS

Wieloplatformowa aplikacja desktopowa w języku **C# (.NET 10 / Avalonia UI)** umożliwiająca generowanie i wizualizację fraktali (w tym fraktala **Trójkąt Sierpińskiego**) z możliwością pełnej parametryzacji, wyboru palety barw i algorytmu.

Dzięki wykorzystaniu **Avalonia UI** aplikacja działa i renderuje się natywnie na:
- 🪟 **Windows** (x64 / ARM64)
- 🐧 **Linux** (X11 / Wayland, Ubuntu, Fedora, Debian itp.)
- 🍏 **macOS** (Apple Silicon M1/M2/M3/M4 & Intel)

---

## Funkcjonalności

- **Wieloplatformowość:** Natywna wydajność graficzna oparta o silnik renderowania Skia.
- **Algorytmy generowania:**
  - **Rekurencja (Wypełnione Trójkąty):** Geometryczny podział powierzchni trójkąta.
  - **Rekurencja (Kontury):** Wizualizacja siatki i krawędzi trójkątów.
  - **Gra Chaosu (Chaos Game):** Algorytm stochastyczny generujący fraktal z losowych skoków punktu.
- **Parametry:**
  - Regulacja poziomu rekurencji (0–9) lub liczby punktów (do 1 000 000).
  - Regulacja grubości linii i rozmiaru punktów (1–6 px).
  - Dynamiczne palety kolorów (gradient tęczy, neonowy cyjan, złoty, matrix, purpura, ogień).
  - Wybór tła (ciemny motyw, jasny, midnight navy).
- **Wydajność:** Obliczenia asynchroniczne (`Task.Run`) i bezpośredni zapis do bufora pikseli (`WriteableBitmap`).
- **Eksport:** Natywny, wieloplatformowy zapis wygenerowanego fraktala do pliku graficznego PNG (`StorageProvider`).

---

## Wymagania

- [.NET 10.0 SDK](https://dotnet.microsoft.com/) (lub nowszy)

---

## Uruchomienie

### Windows:
```powershell
dotnet run --project FractalsDesktopMultiOS.csproj
```

### Linux:
```bash
dotnet run --project FractalsDesktopMultiOS.csproj
```

### macOS:
```bash
dotnet run --project FractalsDesktopMultiOS.csproj
```

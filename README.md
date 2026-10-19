# FractalsUIDesktopApp

Desktopowa aplikacja w języku C# (WPF, .NET 10) umożliwiająca generowanie i wizualizację fraktali (w tym fraktala **Trójkąt Sierpińskiego**) z możliwością parametryzacji, dostosowywania palet barwnych i wyboru algorytmu.

## Funkcjonalności

- **Algorytmy generowania:**
  - **Rekurencja (Wypełnione Trójkąty):** Klasyczny geometryczny podział trójkąta na mniejsze podobszary.
  - **Rekurencja (Kontury):** Wizualizacja krawędzi i konturów trójkątów.
  - **Gra Chaosu (Chaos Game):** Algorytm probabilistyczny generujący fraktal z losowych skoków punktu.
- **Parametry:**
  - Regulacja głębokości rekurencji (0–9) lub liczby punktów (do 1 000 000).
  - Regulacja grubości linii i rozmiaru punktów (1–6 px).
  - Palety kolorów (gradient tęczy, neonowy cyjan, złoty, matrix, purpura, ogień).
  - Wybór tła (ciemne, jasne, midnight navy).
- **Wydajność:** Obliczenia asynchroniczne (`Task.Run`) i renderowanie przy użyciu `WriteableBitmap`.
- **Eksport:** Możliwość zapisu wygenerowanego fraktala do pliku graficznego PNG.

## Wymagania

- .NET 10.0 SDK (lub nowszy z obsługą WPF)
- System operacyjny Windows

## Uruchomienie

```powershell
dotnet run --project FractalsUIDesktopApp.csproj
```
lub
```powershell
dotnet run
```

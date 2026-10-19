using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace sierpinski
{
    public partial class MainWindow : Window
    {
        private WriteableBitmap? _bitmap;
        private bool _isUpdatingUi = false;
        private bool _isGenerating = false;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateParamBounds();
            // Inicjalne automatyczne wygenerowanie
            _ = GenerateTriangleAsync();
        }

        private void CmbMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsInitialized) return;
            UpdateParamBounds();
        }

        private void UpdateParamBounds()
        {
            _isUpdatingUi = true;
            int methodIndex = cmbMethod.SelectedIndex;
            if (methodIndex == 0 || methodIndex == 1)
            {
                // Rekurencja
                lblParamName.Text = "Głębokość rekurencji (0 - 9):";
                sliderParam.Minimum = 0;
                sliderParam.Maximum = 9;
                sliderParam.TickFrequency = 1;
                sliderParam.IsSnapToTickEnabled = true;

                if (sliderParam.Value > 9) sliderParam.Value = 6;
                txtParamValue.Text = ((int)sliderParam.Value).ToString();
            }
            else
            {
                // Gra Chaosu
                lblParamName.Text = "Liczba punktów:";
                sliderParam.Minimum = 5000;
                sliderParam.Maximum = 1000000;
                sliderParam.TickFrequency = 5000;
                sliderParam.IsSnapToTickEnabled = false;

                if (sliderParam.Value < 5000) sliderParam.Value = 150000;
                txtParamValue.Text = string.Format("{0:N0}", (int)sliderParam.Value);
            }
            _isUpdatingUi = false;
        }

        private void SliderParam_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isUpdatingUi || txtParamValue == null) return;
            int methodIndex = cmbMethod.SelectedIndex;
            if (methodIndex == 0 || methodIndex == 1)
            {
                txtParamValue.Text = ((int)e.NewValue).ToString();
            }
            else
            {
                txtParamValue.Text = string.Format("{0:N0}", (int)e.NewValue);
            }
        }

        private void TxtParamValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdatingUi || sliderParam == null) return;
            string cleanText = txtParamValue.Text.Replace(" ", "").Replace(",", "").Replace(".", "");
            if (int.TryParse(cleanText, out int val))
            {
                if (val >= sliderParam.Minimum && val <= sliderParam.Maximum)
                {
                    _isUpdatingUi = true;
                    sliderParam.Value = val;
                    _isUpdatingUi = false;
                }
            }
        }

        private void SliderThickness_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (txtThicknessValue != null)
            {
                txtThicknessValue.Text = $"{(int)e.NewValue} px";
            }
        }

        private void CmbPalette_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void CmbBackground_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void CanvasContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Możliwość automatycznego ponownego przerysowania po zmianie rozmiaru
        }

        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            await GenerateTriangleAsync();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            if (_bitmap != null)
            {
                int width = _bitmap.PixelWidth;
                int height = _bitmap.PixelHeight;
                byte[] bgPixels = new byte[width * height * 4];
                Color bgColor = GetBackgroundColor();

                for (int i = 0; i < bgPixels.Length; i += 4)
                {
                    bgPixels[i] = bgColor.B;
                    bgPixels[i + 1] = bgColor.G;
                    bgPixels[i + 2] = bgColor.R;
                    bgPixels[i + 3] = 255;
                }

                _bitmap.WritePixels(new Int32Rect(0, 0, width, height), bgPixels, width * 4, 0);
                txtStatus.Text = "Wyczyszczono obszar roboczy.";
                txtStats.Text = "Wymiary: - | Czas: -";
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_bitmap == null)
            {
                MessageBox.Show("Brak wygenerowanego obrazu do zapisania!", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Pliki PNG (*.png)|*.png|Wszystkie pliki (*.*)|*.*",
                FileName = "Trojkat_Sierpinskiego.png",
                Title = "Zapisz obraz Trójkąta Sierpińskiego"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(_bitmap));
                    using FileStream stream = new FileStream(dialog.FileName, FileMode.Create);
                    encoder.Save(stream);
                    MessageBox.Show("Obraz został pomyślnie zapisany!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd podczas zapisu pliku: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task GenerateTriangleAsync()
        {
            if (_isGenerating) return;

            try
            {
                _isGenerating = true;
                btnStart.IsEnabled = false;
                progressBar.Visibility = Visibility.Visible;
                progressBar.IsIndeterminate = true;
                txtStatus.Text = "Trwa generowanie trójkąta...";

                int width = (int)canvasContainer.ActualWidth;
                int height = (int)canvasContainer.ActualHeight;

                if (width < 200) width = 1200;
                if (height < 200) height = 900;

                int methodIndex = cmbMethod.SelectedIndex;
                int paramValue = (int)sliderParam.Value;
                int thickness = (int)sliderThickness.Value;
                int paletteIndex = cmbPalette.SelectedIndex;
                Color bgColor = GetBackgroundColor();

                Stopwatch sw = Stopwatch.StartNew();

                // Wykonanie wyliczeń w tle
                var (pixelBuffer, elementCount) = await Task.Run(() =>
                {
                    byte[] pixels = new byte[width * height * 4];

                    // Wypełnienie tła
                    for (int i = 0; i < pixels.Length; i += 4)
                    {
                        pixels[i] = bgColor.B;
                        pixels[i + 1] = bgColor.G;
                        pixels[i + 2] = bgColor.R;
                        pixels[i + 3] = 255;
                    }

                    // Obliczenie wierzchołków trójkąta głównego z marginesem
                    double margin = 30;
                    double triangleHeight = height - margin * 2;
                    double triangleWidth = triangleHeight * 2.0 / Math.Sqrt(3);

                    if (triangleWidth > width - margin * 2)
                    {
                        triangleWidth = width - margin * 2;
                        triangleHeight = triangleWidth * Math.Sqrt(3) / 2.0;
                    }

                    PointD p1 = new PointD(width / 2.0, (height - triangleHeight) / 2.0); // Wierzchołek górny
                    PointD p2 = new PointD(p1.X - triangleWidth / 2.0, p1.Y + triangleHeight); // Lewy dolny
                    PointD p3 = new PointD(p1.X + triangleWidth / 2.0, p1.Y + triangleHeight); // Prawy dolny

                    int count = 0;

                    if (methodIndex == 0)
                    {
                        // Rekurencja (Wypełnione trójkąty)
                        count = DrawFilledSierpinski(pixels, width, height, p1, p2, p3, paramValue, paletteIndex);
                    }
                    else if (methodIndex == 1)
                    {
                        // Rekurencja (Kontury)
                        count = DrawWireframeSierpinski(pixels, width, height, p1, p2, p3, paramValue, thickness, paletteIndex);
                    }
                    else
                    {
                        // Gra Chaosu (Chaos Game)
                        count = DrawChaosGame(pixels, width, height, p1, p2, p3, paramValue, thickness, paletteIndex);
                    }

                    return (pixels, count);
                });

                sw.Stop();

                // Aktualizacja bitmapy na wątku UI
                _bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
                _bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixelBuffer, width * 4, 0);
                imgCanvas.Source = _bitmap;

                txtStatus.Text = "Generowanie zakończone pomyślnie!";
                txtStats.Text = $"Wymiary: {width}x{height} px | Elementy: {elementCount:N0} | Czas: {sw.ElapsedMilliseconds} ms";
            }
            catch (Exception ex)
            {
                txtStatus.Text = $"Błąd podczas generowania: {ex.Message}";
            }
            finally
            {
                _isGenerating = false;
                btnStart.IsEnabled = true;
                progressBar.Visibility = Visibility.Hidden;
            }
        }

        private Color GetBackgroundColor()
        {
            return cmbBackground.SelectedIndex switch
            {
                1 => Colors.White,
                2 => Color.FromRgb(13, 17, 23), // Midnight Navy
                _ => Color.FromRgb(9, 9, 10)     // Ciemne tło
            };
        }

        // --- Algorytm Rekurencyjny: Wypełnione Trójkąty ---
        private int DrawFilledSierpinski(byte[] pixels, int width, int height, PointD p1, PointD p2, PointD p3, int depth, int paletteIndex)
        {
            int triangleCount = 0;

            void Recurse(PointD a, PointD b, PointD c, int currentDepth)
            {
                if (currentDepth == 0)
                {
                    Color color = GetColorForPoint(a, height, paletteIndex);
                    FillTriangle(pixels, width, height, a, b, c, color);
                    triangleCount++;
                }
                else
                {
                    PointD ab = MidPoint(a, b);
                    PointD bc = MidPoint(b, c);
                    PointD ca = MidPoint(c, a);

                    Recurse(a, ab, ca, currentDepth - 1);
                    Recurse(ab, b, bc, currentDepth - 1);
                    Recurse(ca, bc, c, currentDepth - 1);
                }
            }

            Recurse(p1, p2, p3, depth);
            return triangleCount;
        }

        // --- Algorytm Rekurencyjny: Kontury ---
        private int DrawWireframeSierpinski(byte[] pixels, int width, int height, PointD p1, PointD p2, PointD p3, int depth, int thickness, int paletteIndex)
        {
            int lineCount = 0;

            void Recurse(PointD a, PointD b, PointD c, int currentDepth)
            {
                if (currentDepth == 0)
                {
                    Color color = GetColorForPoint(a, height, paletteIndex);
                    DrawLine(pixels, width, height, a, b, color, thickness);
                    DrawLine(pixels, width, height, b, c, color, thickness);
                    DrawLine(pixels, width, height, c, a, color, thickness);
                    lineCount += 3;
                }
                else
                {
                    PointD ab = MidPoint(a, b);
                    PointD bc = MidPoint(b, c);
                    PointD ca = MidPoint(c, a);

                    Recurse(a, ab, ca, currentDepth - 1);
                    Recurse(ab, b, bc, currentDepth - 1);
                    Recurse(ca, bc, c, currentDepth - 1);
                }
            }

            Recurse(p1, p2, p3, depth);
            return lineCount;
        }

        // --- Algorytm: Gra Chaosu (Chaos Game) ---
        private int DrawChaosGame(byte[] pixels, int width, int height, PointD v1, PointD v2, PointD v3, int pointCount, int thickness, int paletteIndex)
        {
            Random rand = new Random();
            PointD current = v1;
            PointD[] vertices = { v1, v2, v3 };

            // Odrzucenie pierwszych 20 punktów rozbiegowych
            for (int i = 0; i < 20; i++)
            {
                PointD target = vertices[rand.Next(3)];
                current = MidPoint(current, target);
            }

            for (int i = 0; i < pointCount; i++)
            {
                PointD target = vertices[rand.Next(3)];
                current = MidPoint(current, target);

                Color color = GetColorForPoint(current, height, paletteIndex);
                DrawDot(pixels, width, height, (int)current.X, (int)current.Y, color, thickness);
            }

            return pointCount;
        }

        // --- Pomocnicze funkcje matematyczne i rysujące ---

        private static PointD MidPoint(PointD p1, PointD p2)
        {
            return new PointD((p1.X + p2.X) / 2.0, (p1.Y + p2.Y) / 2.0);
        }

        private static Color GetColorForPoint(PointD pt, double totalHeight, int paletteIndex)
        {
            double normY = Math.Clamp(pt.Y / totalHeight, 0.0, 1.0);

            return paletteIndex switch
            {
                0 => HsvToRgb(normY * 300.0, 0.85, 1.0), // Tęcza
                1 => Color.FromRgb(0, 212, 255),          // Neonowy Cyjan
                2 => Color.FromRgb(255, 190, 10),         // Złoty
                3 => Color.FromRgb(0, 255, 128),          // Zielony Matrix
                4 => Color.FromRgb(190, 70, 255),         // Purpurowy Fiolet
                5 => HsvToRgb(normY * 60.0, 0.95, 1.0),   // Ogień
                _ => Color.FromRgb(0, 212, 255)
            };
        }

        private static Color HsvToRgb(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value *= 255;
            byte v = (byte)Math.Clamp(value, 0, 255);
            byte p = (byte)Math.Clamp(value * (1 - saturation), 0, 255);
            byte q = (byte)Math.Clamp(value * (1 - f * saturation), 0, 255);
            byte t = (byte)Math.Clamp(value * (1 - (1 - f) * saturation), 0, 255);

            return hi switch
            {
                0 => Color.FromRgb(v, t, p),
                1 => Color.FromRgb(q, v, p),
                2 => Color.FromRgb(p, v, t),
                3 => Color.FromRgb(p, q, v),
                4 => Color.FromRgb(t, p, v),
                _ => Color.FromRgb(v, p, q),
            };
        }

        private static void SetPixel(byte[] pixels, int width, int height, int x, int y, Color color)
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return;
            int index = (y * width + x) * 4;
            pixels[index] = color.B;
            pixels[index + 1] = color.G;
            pixels[index + 2] = color.R;
            pixels[index + 3] = 255;
        }

        private static void DrawDot(byte[] pixels, int width, int height, int cx, int cy, Color color, int radius)
        {
            if (radius <= 1)
            {
                SetPixel(pixels, width, height, cx, cy, color);
                return;
            }

            int r2 = radius * radius;
            for (int dy = -radius; dy <= radius; dy++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    if (dx * dx + dy * dy <= r2)
                    {
                        SetPixel(pixels, width, height, cx + dx, cy + dy, color);
                    }
                }
            }
        }

        private static void DrawLine(byte[] pixels, int width, int height, PointD p1, PointD p2, Color color, int thickness)
        {
            int x0 = (int)Math.Round(p1.X);
            int y0 = (int)Math.Round(p1.Y);
            int x1 = (int)Math.Round(p2.X);
            int y1 = (int)Math.Round(p2.Y);

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                DrawDot(pixels, width, height, x0, y0, color, thickness / 2 + 1);

                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        private static void FillTriangle(byte[] pixels, int width, int height, PointD p1, PointD p2, PointD p3, Color color)
        {
            // Sortowanie wierzchołków po współrzędnej Y (y0 <= y1 <= y2)
            PointD[] pts = { p1, p2, p3 };
            Array.Sort(pts, (a, b) => a.Y.CompareTo(b.Y));

            PointD pMin = pts[0];
            PointD pMid = pts[1];
            PointD pMax = pts[2];

            int totalHeight = (int)Math.Ceiling(pMax.Y - pMin.Y);
            if (totalHeight == 0) return;

            for (int i = 0; i < totalHeight; i++)
            {
                bool secondHalf = i > pMid.Y - pMin.Y || pMid.Y == pMin.Y;
                int segmentHeight = secondHalf ? (int)Math.Ceiling(pMax.Y - pMid.Y) : (int)Math.Ceiling(pMid.Y - pMin.Y);
                if (segmentHeight == 0) continue;

                double alpha = (double)i / totalHeight;
                double beta = (double)(i - (secondHalf ? pMid.Y - pMin.Y : 0)) / segmentHeight;

                int xA = (int)(pMin.X + (pMax.X - pMin.X) * alpha);
                int xB = secondHalf ? (int)(pMid.X + (pMax.X - pMid.X) * beta) : (int)(pMin.X + (pMid.X - pMin.X) * beta);

                if (xA > xB) (xA, xB) = (xB, xA);

                int currentY = (int)(pMin.Y + i);
                for (int x = xA; x <= xB; x++)
                {
                    SetPixel(pixels, width, height, x, currentY, color);
                }
            }
        }
    }

    public readonly struct PointD
    {
        public double X { get; }
        public double Y { get; }

        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
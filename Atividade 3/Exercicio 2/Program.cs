using SkiaSharp;

namespace Projeto {
    class Program {
        static void Main(string[] args) {
      
            using (SKBitmap bitmapEntrada = SKBitmap.Decode("C:\\Users\\mateus.freitas\\Downloads\\Atv3Cog\\Atividade 3\\Exercicio 2\\Exercicio2.png"),
                bitmapSaida = new SKBitmap(new SKImageInfo(bitmapEntrada.Width, bitmapEntrada.Height, SKColorType.Bgra8888))) {

               
                Console.WriteLine(bitmapEntrada.ColorType);
                Console.WriteLine(bitmapSaida.ColorType);

               
                unsafe {
                    byte* entrada = (byte*)bitmapEntrada.GetPixels();
                    byte* saida = (byte*)bitmapSaida.GetPixels();
                    int pixelsTotais = bitmapEntrada.Width * bitmapEntrada.Height;

                    for (int e = 0, s = 0; s < pixelsTotais; e += 4, s++) {
                      
                        byte H, S, V;
                        byte r = entrada[e + 2];
                        byte g = entrada[e + 1];
                        byte b = entrada[e];
                        HSV(r, g, b, out H, out S, out V);

                        // Verifica a luminosidade (V) e aplica uma máscara na imagem de saída
                        if (V >= 85) {
                            // Mantém a cor original se a luminosidade for suficientemente alta
                            saida[e + 3] = entrada[e + 3];
                            saida[e + 2] = entrada[e + 2];
                            saida[e + 1] = entrada[e + 1];
                            saida[e] = entrada[e];
                        } else {
                            // Torna o pixel preto se a luminosidade for baixa
                            saida[e + 3] = 255;
                            saida[e + 2] = 0;
                            saida[e + 1] = 0;
                            saida[e] = 0;
                        }
                    }
                }

          
                using (FileStream stream = new FileStream("C:\\Users\\mateus.freitas\\Downloads\\Atv3Cog\\Atividade 3\\Exercicio 2\\Exercicio2_Saida.png", FileMode.OpenOrCreate, FileAccess.Write)) {
                    bitmapSaida.Encode(stream, SKEncodedImageFormat.Png, 100);
                }
            }
        }

       
        static void HSV(byte r, byte g, byte b, out byte h, out byte s, out byte v) {
            double red = (double)r / 255.0;
            double green = (double)g / 255.0;
            double blue = (double)b / 255.0;
            double maximo = Math.Max(Math.Max(red, green), blue);
            double minimo = Math.Min(Math.Min(red, green), blue);
            double H = 0;
            double S = ((maximo == 0) ? 0 : ((maximo - minimo) / maximo));
            double V = maximo;

            if (maximo != minimo) {
                if (maximo == red) {
                    if (green >= blue)
                        H = (60.0 * ((green - blue) / (maximo - minimo))) / 360.0;
                    else
                        H = ((60.0 * ((green - blue) / (maximo - minimo))) + 360.0) / 360.0;
                } else if (maximo == green) {
                    H = ((60.0 * ((blue - red) / (maximo - minimo))) + 120.0) / 360.0;
                } else {
                    H = ((60.0 * ((red - green) / (maximo - minimo))) + 240.0) / 360.0;
                }
            }

            h = (byte)(255.0 * H);
            s = (byte)(255.0 * S);
            v = (byte)(255.0 * V);
        }
    }
}

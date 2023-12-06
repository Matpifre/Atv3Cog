using SkiaSharp;

namespace Projeto {
	class Program {
		static void Main(string[] args) {

			int imagens;

			Console.WriteLine("Quantas imagens você deseja analisar?");
            imagens = Convert.ToInt32(Console.ReadLine());
			Dictionary<string, int> objetos = new Dictionary<string, int>(imagens);
			for (int i = 0 ; i < imagens; i++){
				using (SKBitmap bitmapEntrada = SKBitmap.Decode("C:\\Users\\mateus.freitas\\Downloads\\Atv3Cog\\Atividade 3\\Exercicio 1\\Exercicio1_"+i+".png"),
				bitmapSaidaAritmetica = new SKBitmap(new SKImageInfo(bitmapEntrada.Width, bitmapEntrada.Height, SKColorType.Gray8))) {

					unsafe {
						byte* entrada = (byte*)bitmapEntrada.GetPixels();
						byte* saida = (byte*)bitmapSaidaAritmetica.GetPixels();
						int largura = bitmapEntrada.Width;
						int altura = bitmapEntrada.Height;
						bool considerar8vizinhos = true;
						List<Forma> formas = new List<Forma>();

						int pixelsTotais = bitmapEntrada.Width * bitmapEntrada.Height;

						for (int e = 0, s = 0; s < pixelsTotais; e += 4, s++) {
							if(entrada[e] == 255 && entrada[e+1] == 255 && entrada[e+2]== 255){
								saida[s] = 255;
							}else{
								saida[s] = 0;
							}
						}

						formas = Forma.DetectarFormas(saida, largura, altura, considerar8vizinhos);
						objetos.Add("Exercicio1_"+i+".png", formas.Count);
					}
					using (FileStream stream = new FileStream("C:\\Users\\mateus.freitas\\Downloads\\Atv3Cog\\Atividade 3\\Exercicio 1\\Exercicio1_"+i+"_saida"+".png", FileMode.OpenOrCreate, FileAccess.Write)) {
						bitmapSaidaAritmetica.Encode(stream, SKEncodedImageFormat.Png, 100);
					}

				}
			}

			var objetosordenados = objetos.OrderByDescending(kv => kv.Value).ToDictionary(kv => kv.Key, kv => kv.Value);
			for (int i = 0; i < objetosordenados.Count; i++){
    			Console.WriteLine(objetosordenados.ElementAt(i).Key + " - "+objetosordenados.ElementAt(i).Value);
			}
		}
	}
}
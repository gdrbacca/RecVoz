using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.Speech.Recognition;
using System.Threading;

namespace RecVoz
{
    class Program
    {
        private static SpeechRecognitionEngine engine;
        static void Main(string[] args)
        {


            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
            CultureInfo pt = new CultureInfo("pt-BR", false);

            try { 
                engine = new SpeechRecognitionEngine(pt);
               
                engine.SetInputToDefaultAudioDevice();

                Choices numeros = new Choices();

                for (int i = 0; i <= 100; ++i)
                    numeros.Add(i.ToString());

                GrammarBuilder gbNumeros = new GrammarBuilder();
                gbNumeros.Append(numeros);
                gbNumeros.Append(new Choices("mais", "menos", "vezes", "divide"));
                gbNumeros.Append(numeros);

                Grammar gNumeros = new Grammar(gbNumeros);

                engine.LoadGrammar(gNumeros);
                engine.RecognizeAsync(RecognizeMode.Multiple);

                engine.SpeechRecognized += rec;

                Console.WriteLine("Pronto");
                Console.ReadKey();

            }
            catch (Exception e)
            {
                Console.WriteLine("deu ruim \n{0}", e.Message);
                Console.ReadKey();
            }

        }

        private static void rec(object s, SpeechRecognizedEventArgs e)
        {
            string fala = e.Result.Text;

            string[] partes = fala.Split(' ');

            double a = double.Parse(partes[0]);
            double b = double.Parse(partes[2]);
            double c = 0.0;

            switch (partes[1])
            {
                case "mais":
                    c = a + b;
                    break;
                case "menos":
                    c = a - b;
                    break;
                case "vezes":
                    c = a * b;
                    break;
                case "divide":
                    c = a / b;
                    break;
            }

            Console.WriteLine("{0} {1} {2} = {3}", partes[0], partes[1], partes[2], c);
        }
    }
}

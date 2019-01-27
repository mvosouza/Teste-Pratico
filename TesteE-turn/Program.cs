using System;
using System.Collections.Generic;
using TesteE_turn.Entidades;

namespace TesteE_turn
{
    class Program
    {
        public static List<Arco> _arcos = new List<Arco>
        {
            new Arco("A","B", 5),
            new Arco("B","C", 4),
            new Arco("C","D", 8),
            new Arco("D","C", 8),
            new Arco("D","E", 6),
            new Arco("A","D", 5),
            new Arco("C","E", 2),
            new Arco("E","B", 3),
            new Arco("A","E", 7)
        };

        static void Main(string[] args)
        {
            try
            {
                var g = new Grafo(_arcos);

                //Q1
                System.Console.WriteLine($"1: {g.CalcularDistanciaRota(new List<string> { "A", "B", "C" })}");
                //Q2
                System.Console.WriteLine($"2: {g.CalcularDistanciaRota(new List<string> { "A", "D" })}");
                //Q3
                System.Console.WriteLine($"3: {g.CalcularDistanciaRota(new List<string> { "A", "D", "C" })}");
                //Q4
                System.Console.WriteLine($"4: {g.CalcularDistanciaRota(new List<string> { "A", "E", "B", "C", "D" })}");
                //Q5
                System.Console.WriteLine($"5: {g.CalcularDistanciaRota(new List<string> { "A", "E", "D" })}");
                //Q6
                System.Console.WriteLine($"6: {g.ContarRotasMaxParadas("C", "C", 4)}");
                //Q7
                System.Console.WriteLine($"7: {g.ContarRotasMaxParadas("A", "C", 4)}");
                //Q8
                System.Console.WriteLine($"8: {g.CalcularMenorCaminho("A", "C")}");
                //Q9
                System.Console.WriteLine($"9: {g.CalcularMenorCaminho("B", "B")}");
                //Q10
                System.Console.WriteLine($"10: {g.ContarRotasDistanciaMaxima("C", "C", 29)}");

                System.Console.WriteLine("\nAperte a tecla Enter para sair.");
                System.Console.Read();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Ocorreu na execução do programa! \nMessage: {ex.Message} \nStrackTrace:{ex.StackTrace}");
            }
        }
    }
}

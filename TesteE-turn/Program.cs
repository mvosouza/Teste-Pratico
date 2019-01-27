using System.Collections.Generic;
using TesteE_turn.Entidades;

namespace TesteE_turn
{
    class Program
    {
        public static List<Aresta> _arestas = new List<Aresta>
        {
            new Aresta("A","B", 5),
            new Aresta("B","C", 4),
            new Aresta("C","D", 8),
            new Aresta("D","C", 8),
            new Aresta("D","E", 6),
            new Aresta("A","D", 5),
            new Aresta("C","E", 2),
            new Aresta("E","B", 3),
            new Aresta("A","E", 7)
        };

        static void Main(string[] args)
        {
            var g = new Grafo(_arestas);

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
            System.Console.WriteLine($"6: {g.ContadorDeRotas("C", "C", 4)}");
            //Q7
            System.Console.WriteLine($"7: {g.ContadorDeRotas("A", "C", 4)}");
            //Q8
            System.Console.WriteLine($"8: {g.CalcularMenorCaminho("A", "C")}");
            //Q9
            System.Console.WriteLine($"9: {g.CalcularMenorCaminho("B", "B")}");
            //Q10
            System.Console.WriteLine($"10: {g.ContadorDeRotasDistanciaMaxima("C", "C", 29)}");

            System.Console.WriteLine("\nAperte a tecla Enter para sair.");
            System.Console.Read();
        }
    }
}

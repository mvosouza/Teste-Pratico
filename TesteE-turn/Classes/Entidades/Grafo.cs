using System;
using System.Collections.Generic;
using System.Linq;
using TesteE_turn.Entidades.Exceções;

namespace TesteE_turn.Entidades
{
    public class Grafo
    {
        private Dictionary<string, Dictionary<string, Aresta>> _grafo;
        private List<Aresta> _arestas;
        private Dictionary<string, Dictionary<string, int>> _chacheDts;
        private Dictionary<string, Dictionary<string, string>> _chacheRots;


        public Grafo(List<Aresta> arestas)
        {
            _arestas = arestas;
            _chacheDts = new Dictionary<string, Dictionary<string, int>>();
            _chacheRots = new Dictionary<string, Dictionary<string, string>>();
            Criar(arestas);
        }

        #region MÉTODOS
        private void Criar(List<Aresta> arestas)
        {
            _grafo = new Dictionary<string, Dictionary<string, Aresta>>();

            foreach (var aresta in arestas)
            {
                Dictionary<string, Aresta> aux;
                _grafo.TryGetValue(aresta.Origem, out aux);
                if (aux == null)
                    _grafo.Add(aresta.Origem, new Dictionary<string, Aresta>());
                _grafo[aresta.Origem][aresta.Destino] = aresta;
            }
        }

        /// <summary>
        /// Calcula a distância de uma determinada rota.
        /// </summary>
        /// <param name="cidades">Listas das cidades.</param>
        /// <returns>O valor da distância referente a rota desejada.</returns>
        /// <exception cref="RotaInexistenteException">Essa exceção é gerada quando não existe a rota informada.</exception>
        private int CalcularDistanciaRota(List<string> cidades)
        {
            int result = 0;

            for (int i = 0; i < cidades.Count - 1; i++)
            {
                Aresta aresta;
                _grafo[cidades[i]].TryGetValue(cidades[i + 1], out aresta);

                if (aresta == null)
                    throw new RotaInexistenteException("Rota não existente");
                else
                    result += aresta.Distancia;
            }

            return result;
        }

        public string CalcularDistanciaRota(List<string> cidades, bool imprimirRota = false)
        {
            string resultado = string.Empty;
            if (imprimirRota)
                resultado = $"Rota {String.Join("-", cidades)}: ";
            try
            {
                resultado = $"{resultado}{CalcularDistanciaRota(cidades)}";
            }
            catch (RotaInexistenteException ex)
            {
                resultado = $"{resultado}{ex.Message}";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultado;
        }

        public int ContadorDeRotas(string origem, string destino, int qtdMaximaArestasPercorridas)
        {
            if (qtdMaximaArestasPercorridas == 0 && origem == destino)
                return 1;
            if (qtdMaximaArestasPercorridas == 1 && _grafo[origem].Any(e => e.Key.Equals(destino)))
                return 1;
            if (qtdMaximaArestasPercorridas <= 0)
                return 0;

            int contador = 0;

            foreach (var elem in _grafo[origem].Keys)
            {
                contador += ContadorDeRotas(elem, destino, qtdMaximaArestasPercorridas - 1);
            }

            return contador;
        }

        public int ContadorDeRotasDistanciaMaxima(string origem, string destino, int distanciaMax, string rota = "", bool printRota = false)
        {
            if (distanciaMax == 0 && origem.Equals(destino))
            {
                if (printRota)
                    Console.WriteLine(rota + destino);
                return 1;
            }
            if (distanciaMax < 0)
                return 0;

            int contador = 0;

            if (distanciaMax > 0 && origem.Equals(destino) && !string.IsNullOrEmpty(rota))
            {
                if (printRota)
                    Console.WriteLine(rota + destino);
                contador++;
            }

            foreach (var elem in _grafo[origem].Keys)
            {
                contador += ContadorDeRotasDistanciaMaxima(elem, destino, distanciaMax - _grafo[origem][elem].Distancia, rota + origem, printRota);
            }

            return contador;
        }

        public string CalcularMenorCaminho(string nodoInicial, string nodoFinal)
        {
            string resultado = string.Empty;

            try
            {
                //Ciclo
                if (nodoFinal.Equals(nodoInicial))
                {
                    resultado = CalcularMenorCaminhoEmUmCiclo(nodoInicial);
                }
                else
                {
                    resultado = CalcularMenorCaminhoEntrePontosDistintos(nodoInicial, nodoFinal).ToString();
                }
            }
            catch (RotaInexistenteException ex)
            {
                resultado = ex.Message;
            }
            catch (VerticeInacessivelException ex)
            {
                resultado = ex.Message;
            }
            catch (Exception ex)
            {
                resultado = $"Ocorreu uma exceção na função CalcularMenorCaminho \nMessage: {ex.Message} \nStrackTrace:{ex.StackTrace}";
            }

            return resultado;
        }

        private int CalcularMenorCaminhoEntrePontosDistintos(string nodoInicial, string nodoFinal)
        {
            Dictionary<string, int> dt;
            Dictionary<string, string> rot;
            _chacheDts.TryGetValue(nodoInicial, out dt);
            _chacheRots.TryGetValue(nodoInicial, out rot);

            if (dt == null && rot == null)
            {
                dt = CriarListaDeNodosExistentes<int>(0);
                rot = CriarListaDeNodosExistentes<string>(string.Empty);

                dt[nodoInicial] = 0;
                rot[nodoInicial] = nodoInicial;

                foreach (var nodo in _grafo.Keys.Where(e => !e.Equals(nodoInicial)))
                    dt[nodo] = int.MaxValue;

                Queue<string> fila = new Queue<string>();
                fila.Enqueue(nodoInicial);

                while (fila.Count != 0)
                {
                    var i = fila.Dequeue();

                    foreach (var j in _grafo[i].Keys)
                    {
                        var distancia = dt[j] < (dt[i] + _grafo[i][j].Distancia) ? dt[j] : (dt[i] + _grafo[i][j].Distancia);
                        if (distancia < dt[j])
                        {
                            dt[j] = distancia;
                            rot[j] = i;
                            fila.Enqueue(j);
                        }
                    }
                }

                //ImprimirDictionary(dt, "DT");
                //ImprimirDictionary(rot, "ROT");
                if (string.IsNullOrEmpty(rot[nodoFinal]))
                    throw new RotaInexistenteException($"Rota '{nodoInicial}' para '{nodoFinal}' não existe.");

                _chacheDts.Add(nodoInicial, dt);
                _chacheRots.Add(nodoInicial, rot);
            }

            return dt[nodoFinal];
        }

        private string CalcularMenorCaminhoEmUmCiclo(string nodoInicial)
        {
            string resultado;
            List<string> ascendentes = BuscarAscendentesAcessiveis(nodoInicial);

            if (ascendentes.Count == 0)
                throw new VerticeInacessivelException($"O vertice '{nodoInicial}' nao contem vertices ascendentes ou ascendentes acessiveis por ele.");
            else
            {
                var distanciasEntreAscendentes = new Dictionary<string, int>();

                foreach (var ascendente in ascendentes)
                {
                    distanciasEntreAscendentes.Add(ascendente, CalcularMenorCaminhoEntrePontosDistintos(nodoInicial, ascendente) + CalcularMenorCaminhoEntrePontosDistintos(ascendente, nodoInicial));
                }

                resultado = (distanciasEntreAscendentes.Min(e => e.Value)).ToString();
            }

            return resultado;
        }

        #endregion

        #region MÉTODOS AUXILIARES
        private Dictionary<string, bool> CriarListaDeNodosVisitados()
        {
            Dictionary<string, bool> visitados = CriarListaDeNodosExistentes<bool>(false);
            return visitados;
        }

        private Dictionary<string, T> CriarListaDeNodosExistentes<T>(T tipoNulo)
        {
            Dictionary<string, T> nodos = new Dictionary<string, T>();

            foreach (var vertice in _grafo.Keys)
            {
                nodos.Add(vertice, tipoNulo);
            }

            return nodos;
        }

        private static void ImprimirDictionary<T>(Dictionary<string, T> elementos, string descricao)
        {
            Console.WriteLine($"{descricao}:");
            foreach (var elem in elementos)
            {
                Console.Write($"{elem.Key}:{elem.Value} \t");
            }
            Console.WriteLine();
        }

        public List<string> BuscaEmProfundidade(string nodoInicail)
        {
            Dictionary<string, bool> visitados = CriarListaDeNodosVisitados();

            return DFS(nodoInicail, visitados);
        }

        private List<string> DFS(string nodoInicail, Dictionary<string, bool> visitados)
        {
            var resultado = new List<string>();
            visitados[nodoInicail] = true;
            resultado.Add(nodoInicail);

            foreach (var nodo in _grafo[nodoInicail].Keys)
            {
                if (!visitados[nodo])
                {
                    resultado.AddRange(DFS(nodo, visitados));
                }
            }
            return resultado;
        }

        private List<string> BuscarAscendentesAcessiveis(string nodoInicial)
        {
            var ascendentes = new List<string>();

            foreach (var i in _grafo.Keys)
                foreach (var j in _grafo[i].Keys)
                    if (j.Equals(nodoInicial))
                        ascendentes.Add(i);

            var verticesAcessiveisPeloInicial = BuscaEmProfundidade(nodoInicial);
            ascendentes.RemoveAll(a => !(verticesAcessiveisPeloInicial.Contains(a)));

            return ascendentes;
        }
        #endregion
    }
}

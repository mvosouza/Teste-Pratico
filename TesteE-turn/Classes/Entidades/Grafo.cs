using System;
using System.Collections.Generic;
using System.Linq;
using TesteE_turn.Classes.Excecoes;

namespace TesteE_turn.Entidades
{
    public class Grafo
    {

        private Dictionary<string, Dictionary<string, Arco>> _grafo;
        private List<Arco> _arcos;
        private Dictionary<string, Dictionary<string, int>> _chacheDts;
        private Dictionary<string, Dictionary<string, string>> _chacheRots;
        private HashSet<string> _cidades;

        public Grafo(List<Arco> arcos)
        {
            _arcos = arcos;
            _chacheDts = new Dictionary<string, Dictionary<string, int>>();
            _chacheRots = new Dictionary<string, Dictionary<string, string>>();
            _cidades = new HashSet<string>();
            Criar(arcos);
        }

        #region MÉTODOS
        private void Criar(List<Arco> arcos)
        {
            _grafo = new Dictionary<string, Dictionary<string, Arco>>();

            foreach (var arco in arcos)
            {
                Dictionary<string, Arco> aux;
                _grafo.TryGetValue(arco.Origem, out aux);
                if (aux == null)
                    _grafo.Add(arco.Origem, new Dictionary<string, Arco>());
                _grafo[arco.Origem][arco.Destino] = arco;

                _cidades.Add(arco.Origem);
                _cidades.Add(arco.Destino);
            }
        }

        private int CalcularDistanciaRota(List<string> cidades)
        {
            int result = 0;

            for (int i = 0; i < cidades.Count - 1; i++)
            {
                Dictionary<string, Arco> aux;
                _grafo.TryGetValue(cidades[i], out aux);

                if (aux == null)
                    throw new CidadeInexistenteException($"A cidade '{cidades[i]}' informada não existe, ou não cotem rota para outra cidade.");
                else
                {
                    Arco arco;
                    aux.TryGetValue(cidades[i + 1], out arco);

                    if (arco == null)
                        throw new RotaInexistenteException();
                    else
                        result += arco.Distancia;
                }
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
            catch (ObjetoInexistenteException ex)
            {
                resultado = $"{resultado}{ex.Message}";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultado;
        }

        public string ContarRotasValorExatoDeParadas(string origem, string destino, int qtdMaxParadas, bool printRota = false)
        {
            var resultado = string.Empty;

            try
            {
                resultado = ContadorDeRotasMaxParadas(origem, destino, qtdMaxParadas, printRota, true).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultado;
        }

        public string ContarRotasAteValorMaxParadas(string origem, string destino, int qtdMaxParadas, bool printRota = false)
        {
            var resultado = string.Empty;

            try
            {
                resultado = ContadorDeRotasMaxParadas(origem, destino, qtdMaxParadas, printRota).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultado;
        }

        public int ContadorDeRotasMaxParadas(string origem, string destino, int qtdMaxParadas, bool printRota, bool valorExatoDeParadas = false, string rota = "")
        {
            Dictionary<string, Arco> descendentesOrigem;
            _grafo.TryGetValue(origem, out descendentesOrigem);

            if (descendentesOrigem == null)
                return 0;

            if (qtdMaxParadas == 0 && origem.Equals(destino))
            {
                if (printRota)
                    Console.WriteLine(rota + destino);
                return 1;
            }
            if (qtdMaxParadas <= 0)
                return 0;

            int contador = 0;

            foreach (var elem in descendentesOrigem.Keys)
            {
                contador += ContadorDeRotasMaxParadas(elem, destino, qtdMaxParadas - 1, printRota, valorExatoDeParadas, rota + origem);
            }

            if (!valorExatoDeParadas && qtdMaxParadas > 0 && origem.Equals(destino) && !string.IsNullOrEmpty(rota))
            {
                if (printRota)
                    Console.WriteLine(rota + destino);
                contador++;
            }

            return contador;
        }

        public string ContarRotasDistanciaMaxima(string origem, string destino, int distanciaMax, bool printRota = false)
        {
            string resultado = string.Empty;

            try
            {
                resultado = ContadorDeRotasDistanciaMaxima(origem, destino, distanciaMax, printRota).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultado;
        }

        public int ContadorDeRotasDistanciaMaxima(string origem, string destino, int distanciaMax, bool printRota, string rota = "")
        {
            Dictionary<string, Arco> descendentesOrigem;
            _grafo.TryGetValue(origem, out descendentesOrigem);

            if (descendentesOrigem == null)
                return 0;

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

            foreach (var elem in descendentesOrigem.Keys)
            {
                contador += ContadorDeRotasDistanciaMaxima(elem, destino, distanciaMax - descendentesOrigem[elem].Distancia, printRota, rota + origem);
            }

            return contador;
        }

        public string CalcularMenorCaminho(string nodoInicial, string nodoFinal)
        {
            string resultado = string.Empty;

            try
            {
                if (!_cidades.Contains(nodoInicial))
                    throw new CidadeInexistenteException(nodoInicial, true);
                if (!_cidades.Contains(nodoFinal))
                    throw new CidadeInexistenteException(nodoFinal, true);

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
            catch (ObjetoInexistenteException ex)
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
                    Dictionary<string, Arco> descendentesI;
                    _grafo.TryGetValue(i, out descendentesI);

                    foreach (var j in descendentesI.Keys)
                    {
                        var distancia = dt[j] < (dt[i] + descendentesI[j].Distancia) ? dt[j] : (dt[i] + descendentesI[j].Distancia);
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

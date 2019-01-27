namespace TesteE_turn.Entidades
{
    public class Aresta
    {
        private string _origem;
        private string _destino;
        private int _distancia;
        private bool _jaVisitada;

        public Aresta(string origem, string destino, int distancia)
        {
            _origem = origem;
            _distancia = distancia;
            _destino = destino;
            _jaVisitada = false;
        }

        public string Origem
        {
            get { return _origem; }
            private set { _origem = value; }
        }

        public string Destino
        {
            get { return _destino; }
            private set { _destino = value; }
        }

        public int Distancia
        {
            get { return _distancia; }
            private set { _distancia = value; }
        }

        public bool jaVisitada
        {
            get { return _jaVisitada; }
            set { _jaVisitada = value; }
        }
    }
}

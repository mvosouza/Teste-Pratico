using System;

namespace TesteE_turn.Classes.Excecoes
{
    public class RotaInexistenteException : ObjetoInexistenteException
    {
        public RotaInexistenteException() : base("Rota não existente")
        {
           
        }

        public RotaInexistenteException(string message) : base(message)
        {

        }

        public RotaInexistenteException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}

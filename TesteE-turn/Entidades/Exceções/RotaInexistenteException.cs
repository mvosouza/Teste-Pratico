using System;

namespace TesteE_turn.Entidades.Exceções
{
    public class RotaInexistenteException : Exception
    {
        public RotaInexistenteException() : base()
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

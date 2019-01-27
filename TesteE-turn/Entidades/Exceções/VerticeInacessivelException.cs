using System;

namespace TesteE_turn.Entidades.Exceções
{
    public class VerticeInacessivelException : Exception
    {
        public VerticeInacessivelException() : base()
        {

        }

        public VerticeInacessivelException(string message) : base(message)
        {

        }

        public VerticeInacessivelException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}

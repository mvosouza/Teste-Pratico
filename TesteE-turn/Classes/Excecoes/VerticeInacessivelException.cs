using System;

namespace TesteE_turn.Classes.Excecoes
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

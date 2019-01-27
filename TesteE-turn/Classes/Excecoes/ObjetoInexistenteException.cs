using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteE_turn.Classes.Excecoes
{
    public class ObjetoInexistenteException : Exception
    {
        public ObjetoInexistenteException() : base()
        {

        }

        public ObjetoInexistenteException(string message) : base(message)
        {

        }

        public ObjetoInexistenteException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}

using System;

namespace TesteE_turn.Classes.Excecoes
{
    public class CidadeInexistenteException : ObjetoInexistenteException
    {
        private const string TEXTO_CIDADE_INEXISTENTE_DEFAULT = "A cidade<Cidade> informada não existe.";

        public CidadeInexistenteException() : base()
        {

        }

        public CidadeInexistenteException(string message) : base(message)
        {

        }

        public CidadeInexistenteException(string cidade, bool usarTxtPadrao) : base(TEXTO_CIDADE_INEXISTENTE_DEFAULT.Replace("<Cidade>", $" '{cidade}'"))
        {

        }

        public CidadeInexistenteException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}

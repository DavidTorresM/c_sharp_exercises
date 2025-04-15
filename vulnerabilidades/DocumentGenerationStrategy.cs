// archivo: Program.cs



using Vulnerabilidades;

using System.IO;

namespace Document
{
    internal interface DocumentGenerationStrategy
    {
        public string GenerateDocument(Vulnerabilidad content);
    }
    internal class DocumentGenerationMarkdownStrategy : DocumentGenerationStrategy
    {
        public string GenerateDocument(Vulnerabilidad content)
        {
            return $@"
                # {content.titulo}

                **Criticidad:** {content.criticidad}

                ## Reproducción
                {content.reproduccion}

                ## Sugerencia de Mitigación
                {content.sugerenciaMiticacion}

                ## Referencias
                {content.referencias}
                ";
        }
    }
    internal class DocumentGenerationLatexStrategy : DocumentGenerationStrategy
    {
        public string GenerateDocument(Vulnerabilidad content)
        {

            return $@"
                \documentclass{{article}}
                \usepackage[utf8]{{inputenc}}

                \title{{{content.titulo}}}
                \begin{{document}}

                \maketitle

                \section*{{Criticidad}}
                \textbf{{{content.criticidad}}}

                \section*{{Reproducción}}
                {content.reproduccion}

                \section*{{Sugerencia de Mitigación}}
                {content.sugerenciaMiticacion}

                \section*{{Referencias}}
                {content.referencias}

                \end{{document}}
                ";
        }
    }

}
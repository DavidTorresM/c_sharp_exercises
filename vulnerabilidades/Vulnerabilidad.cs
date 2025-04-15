using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vulnerabilidades
{
    internal class Vulnerabilidad
    {
        public string titulo { get; set; }
        public Criticidad criticidad { get; set; }
        public string reproduccion { get; set; }
        public string sugerenciaMiticacion { get; set; }
        public string referencias { get; set; }

        public override string ToString()
        {
            return $"Título: {titulo}\n" +
               $"Criticidad: {criticidad}\n" +
               $"Reproducción: {reproduccion}\n" +
               $"Sugerencia de Mitigación: {sugerenciaMiticacion}\n" +
               $"Referencias: {referencias}";

        }

    }

    enum Criticidad
    {
        Info,
        Baja,
        Media,
        Alta,
        Critica
    }
}

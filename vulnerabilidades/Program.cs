// archivo: Program.cs
using System;
using Document;
using Serilog;
using Vulnerabilidades;
class Program
{
    static void Main()
    {
        // Configuración del logger
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        Log.Information("Inicio de la aplicación");

        try
        {
            DocumentGenerationStrategy documentGeneration = new DocumentGenerationMarkdownStrategy();
            string nombreDocumento = "documento.tex";
            Vulnerabilidad v = new Vulnerabilidad();
            v.titulo = "Vulnerabilidad de ejemplo";
            v.criticidad = Criticidad.Alta;
            v.reproduccion = "Pasos de reproduccion";
            v.sugerenciaMiticacion = "Sugerencias mitigacion";
            v.referencias = "Referencias de la vulnerabilidad";

            string formato = documentGeneration.GenerateDocument(v);

            File.WriteAllText(nombreDocumento, formato);

            Log.Information("Documento generado exitosamente");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ocurrió un error durante la ejecución");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}

using System;
using System.Threading;

class ProgramaConHilos
{
    static void Main()
    {
        // Crear los hilos con parámetros
        Thread hilo1 = new Thread(MetodoHiloConParametro);
        Thread hilo2 = new Thread(MetodoHiloConParametro);

        // Iniciar los hilos con parámetros
        hilo1.Start("Hilo 1");
        hilo2.Start("Hilo 2");

        // Esperar a que terminen
        hilo1.Join();
        hilo2.Join();

        Console.WriteLine("Los hilos han terminado.");
    }

    static void MetodoHiloConParametro(object parametro)
    {
        string nombreHilo = (string)parametro;
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine($"{nombreHilo} - Iteración {i}");
            Thread.Sleep(500); // Simula trabajo
        }
    }
}

using System;
using System.Diagnostics;


void EjecutarProceso(string comando, string argumentos)
{
    ProcessStartInfo psi = new ProcessStartInfo
    {
        FileName = comando,
        Arguments = argumentos,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = false
    };

    using (Process proc = Process.Start(psi))
    {
        string salidaEstandar = proc.StandardOutput.ReadToEnd();
        string errorEstandar = proc.StandardError.ReadToEnd();

        Console.WriteLine("Salida estándar:");
        Console.WriteLine(salidaEstandar);

        if (!string.IsNullOrEmpty(errorEstandar))
        {
            Console.WriteLine("Error estándar:");
            Console.WriteLine(errorEstandar);
        }
    }
}

Console.WriteLine("Ejercicio para aprender a ejecutar procesos en C#");


EjecutarProceso("cmd.exe", "/c dir");

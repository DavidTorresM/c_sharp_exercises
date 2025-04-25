using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandAndControl
{
    internal class Server
    {
        public string ip = "0.0.0.0";
        public int port = 1234;

        public Server(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        public void Start()
        {
            Console.WriteLine($"Server started at {ip}:{port}");
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAd = IPAddress.Parse(ip);
            socket.Bind(new IPEndPoint(ipAd, port));
            while (true)
            {
                try
                {
                    socket.Listen(int.MaxValue);
                    Console.WriteLine("Waiting for a connection...");
                    Socket client = socket.Accept();
                    Task.Run(() => HandleClient(client)); // Maneja el cliente en un hilo separado
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

            }
            

        }
        private void HandleClient(Socket client)
        {
            byte[] buffer = new byte[1024];
            string data;
            try
            {
                Console.WriteLine("Client connected");

                // Espera un mensaje entrante del cliente
                int bytesRead = client.Receive(buffer);
                data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received from client: {data}");

                // 🐇 Conectarse a RabbitMQ
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                
                
                
                string exchangeName = "c2"; // o el que estés usando
                channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);

                // Crear cola temporal y enlazar
                var queueName = channel.QueueDeclare().QueueName;
                Console.WriteLine($" [*] Waiting for messages in {queueName}. To exit press CTRL+C");
                channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: data);






                Console.WriteLine("Waiting for RabbitMQ message...");

                // Consumir el mensaje en tiempo real
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" [x] From RabbitMQ: {message}");

                    // Enviar al cliente TCP
                    client.Send(Encoding.UTF8.GetBytes(message));
                    // Esperar un nuevo mensaje del cliente TCP
                    /*
                    byte[] clientBuffer = new byte[1024];
                    int clientBytesRead = client.Receive(clientBuffer);
                    string clientMessage = Encoding.UTF8.GetString(clientBuffer, 0, clientBytesRead);
                    Console.WriteLine($"Received new message from client: {clientMessage}");
                    */
                };

                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                // Mantener el cliente conectado mientras el consumidor está escuchando
                Console.WriteLine("Press [enter] to exit.");
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with client: {ex.Message}");
            }
            finally
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
        }

    }
}

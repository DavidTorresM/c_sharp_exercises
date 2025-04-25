using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using CommandAndControl;


//Clase maestra
Task tarea = Task.Run(() => {
    Server server = new Server("0.0.0.0", 1234);
    server.Start();
});


tarea.Wait(); // Espera a que termine la tarea (opcional)
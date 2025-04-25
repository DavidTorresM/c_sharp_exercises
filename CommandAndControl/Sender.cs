using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandAndControl
{
    internal interface Sender
    {
        void Send(string message);
        void Send(string message, string recipient);
        void Send(string message, string recipient, string sender);
        void Send(string message, string recipient, string sender, DateTime timestamp);
        void Send(string message, string recipient, string sender, DateTime timestamp, Dictionary<string, string> metadata);


    }
}

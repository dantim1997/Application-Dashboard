using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Application_Dashboard
{
    public class Connection
    {
        public Connection()
        {
            // TODO change to pc ip adress in network --- port is not that important?
            IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, 1234); // Address  
            TcpListener listener = new TcpListener(ep); // Instantiate the object  
            listener.Start(); // Start listening to the client

            while (true)
            {
                const int bytesize = 1024 * 1024;
                byte[] buffer = new byte[bytesize];

                var sender = listener.AcceptTcpClient();
                sender.GetStream().Read(buffer, 0, bytesize);
                string message = cleanMessage(buffer);

                foreach (string line in message.Split("\r\n"))
                {
                    // StartRun is the command where the application name is given.
                    if (line.Contains("StartRun"))
                    {
                        var runProgram = line.Split(':');
                        StartApplication.RunApplication(runProgram[1]);
                    }
                }
                sender.Close();
            }
        }

        //from byte array to readable string
        private static string cleanMessage(byte[] bytes)
        {
            string message = System.Text.Encoding.ASCII.GetString(bytes);

            string messageToPrint = null;
            foreach (var nullChar in message)
            {
                if (nullChar != '\0')
                {
                    messageToPrint += nullChar;
                }
            }
            return messageToPrint;
        }
    }
}

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
            IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, 1234); // Address  
            TcpListener listener = new TcpListener(ep); // Instantiate the object  
            listener.Start(); // Start listening...  
            while (true)
            {
                const int bytesize = 1024 * 1024;

                string message = null;
                byte[] buffer = new byte[bytesize];

                var sender = listener.AcceptTcpClient();
                sender.GetStream().Read(buffer, 0, bytesize);

                // Read the message and perform different actions  
                message = cleanMessage(buffer);


                foreach (string line in message.Split("\r\n"))
                {
                    // do something
                    if (line.Contains("StartRun"))
                    {
                        var runProgram = line.Split(':');
                        RunApplication(runProgram[1]);
                    }
                }
                sender.Close();
            }
        }

        private void RunApplication(string app)
        {
            Console.WriteLine("Hello World!");
            var apps = ReadExePaths.ReadAllPaths();
            var usePath = apps.Where(a => a.Key == app).Select(b => b.Value).FirstOrDefault();
            System.Diagnostics.Process.Start(usePath);
        }

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

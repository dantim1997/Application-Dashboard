using Application_Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application_Dashboard
{
    public class Connection
    {
        public static HttpListener listener;
        public static string url = "http://localhost:8000/";
        public static int pageViews = 0;
        public static int requestCount = 0;
        public static string pageData = "Hello";

        public Connection()
        {
            // TODO change to pc ip adress in network --- port is not that important?
            //IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, 1234); // Address  
            //TcpListener listener = new TcpListener(ep); // Instantiate the object  
            //listener.Start(); // Start listening to the client

            // Create a Http server and start listening for incoming connections
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine("Listening for connections on {0}", url);

            // Handle requests
            Task listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().GetResult();

            // Close the listener
            listener.Close();
        }

        public static async Task HandleIncomingConnections()
        {
            bool runServer = true;

            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (runServer)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                // Print out some info about the request
                Console.WriteLine("Request #: {0}", ++requestCount);
                Console.WriteLine(req.Url.ToString());
                Console.WriteLine(req.HttpMethod);
                Console.WriteLine(req.UserHostName);
                Console.WriteLine(req.UserAgent);
                Console.WriteLine();

                var message = "";
                if (!ctx.Request.HasEntityBody)
                {
                }

                using (System.IO.Stream body = ctx.Request.InputStream) // here we have data
                {
                    using (var reader = new System.IO.StreamReader(body, ctx.Request.ContentEncoding))
                    {
                        message = reader.ReadToEnd();
                    }
                }
                foreach (string line in message.Split("\r\n"))
                {
                    // StartRun is the command where the application name is given.
                    if (line.Contains("StartRun"))
                    {
                        var runProgram = line.Split(':');
                        StartApplication.RunApplication(runProgram[1]);
                    }
                    if (line.Contains("GetAllApplications"))
                    {
                        List<Apps> items = new();
                        var apps = ReadExePaths.ReadAllPaths();
                        foreach (var item in apps)
                        {
                            var icon = ReadExePaths.GetIcons(item);
                            items.Add(new Apps() { Key = item.Key, Value = icon });
                        }
                        pageData = JsonSerializer.Serialize(items);
                    }
                }

                byte[] data = Encoding.UTF8.GetBytes(pageData);
                resp.ContentType = "text/html";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;

                // Write out to the response stream (asynchronously), then close it
                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close();
            }
        }
    }
}

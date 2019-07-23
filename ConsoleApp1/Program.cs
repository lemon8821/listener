using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ListenerServer();
            SimpleListenerExample(new [] {"http://localhost:2222/"});
        }

        private static void ListenerServer()
        {
            var path = @"D:/dev/test.txt";
            HttpListener listener = null;
            try
            {
                listener = new HttpListener();
                listener.Prefixes.Add("http://localhost:2222/");
                listener.Start();
                while (true)
                {
                    Console.WriteLine("waiting...");
                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    StreamReader reader = new StreamReader(request.InputStream);
                    string x = reader.ReadToEnd();
                    Console.WriteLine(x);
                    File.AppendAllText(path, request.HttpMethod);
                    File.AppendAllText(path, x + "\n");
                    string msg = "hello";
                    context.Response.ContentLength64 = Encoding.UTF8.GetByteCount(msg);
                    context.Response.StatusCode = 200;
                    using (Stream stream = context.Response.OutputStream)
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.Write(msg);
                        }
                    }

                    Console.WriteLine("sent");
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        // This example requires the System and System.Net namespaces.
        public static void SimpleListenerExample(string[] prefixes)
        {
            string path = @"D:\dev\test.txt";

            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // URI prefixes are required,
            // for example "http://contoso.com:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.
            foreach (string s in prefixes)
            {
                //listener.Prefixes.Add(s);
            }
            listener.Prefixes.Add("http://localhost:2222/");
            listener.Start();
            Console.WriteLine("Listening...");
            // Note: The GetContext method blocks while waiting for a request. 
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            StreamReader reader = new StreamReader(request.InputStream);
            string x = reader.ReadToEnd();
            Console.WriteLine(x);
            File.AppendAllText(path,request.HttpMethod);
            File.AppendAllText(path,x + "\n");
            
            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            // Construct a response.
            string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // You must close the output stream.
            output.Close();
            listener.Stop();
            Console.ReadKey();
        }
    }
}

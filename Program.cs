using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Server
{
    class Program
    {
        static TcpListener listener;
        public static async Task Main(string[] args)
        {
            TcpClient client;
            listener = new TcpListener(IPAddress.Any, 12345);
            listener.Start();

            try
            {
                while (true)
                {
                    client = await listener.AcceptTcpClientAsync();
                    ConnectionHandle(client);
                }
            } catch { }
        }
        public static async Task ConnectionHandle(TcpClient client)
        {
            bool end = false;
            string msg, amsg;
            StreamReader rdr;
            StreamWriter wtr;
            NetworkStream s;

            s = client.GetStream();
            rdr = new StreamReader(s);
            wtr = new StreamWriter(s);

            while (!end) 
            {
                msg = rdr.ReadLine();
                if (msg != "" && msg != "__stop__")
                {
                    //msg = rdr.ReadLine();
                    wtr.WriteLine($"Got: {msg}");
                    amsg = msg.Replace("LAMK", "LAB").Replace("Lahden ammattikorkeakoulu", "LAB-ammattikorkeakoulu")
                            .Replace("Saimia", "LAB").Replace("Saimaan ammattikorkeakoulu", "LAB-ammattikorkeakoulu");
                    wtr.WriteLine($"Adapted: {amsg}");
                    wtr.Flush();
                }
                if (msg == "")
                {
                    end = true;
                    wtr.Write($"Got: {msg}");
                    wtr.Flush();
                    client.Close();
                }
                if (msg == "__stop__")
                {
                    end = true;
                    wtr.Write("Server stopped");
                    wtr.Flush();
                    client.Close();
                    listener.Stop();
                }
                //client.Close();
            }
        }
    }
}

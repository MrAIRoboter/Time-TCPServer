using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TimeServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TimeServer server = new TimeServer(IPAddress.Any, 19900);
            server.Start();
        }
    }

    public class TimeServer : IDisposable
    {
        public IPAddress IP { get; private set; }
        public ushort Port { get; private set; }
        public bool IsAlive { get; private set; }

        public TimeServer(IPAddress ip, ushort port)
        {
            IP = ip;
            Port = port;
        }

        public void Start()
        {
            IsAlive = true;
            TcpListener listener = new TcpListener(IP, Port);
            listener.Start();

            while (IsAlive == true)
            {
                TcpClient client = listener.AcceptTcpClient();

                Task.Run(() => HandleClient(client));
            }
        }

        public void Stop() => IsAlive = false;

        public void Dispose() => Stop();

        private void HandleClient(TcpClient client)
        {
            try
            {
                using (NetworkStream networkStream = client.GetStream())
                {
                    long time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    byte[] encodedTime = BitConverter.GetBytes(time);

                    networkStream.Write(encodedTime, 0, encodedTime.Length);
                }
            }
            catch { }
        }
    }
}

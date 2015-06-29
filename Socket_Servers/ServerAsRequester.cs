using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketServers
{
    class ServerAsRequester
    {

        private const string server = "127.0.0.1";
        private const int servPort = 4008;

        // ===================================
        public static void Main(string[] args)
        {
            //Console.WriteLine("the number of args: " + args.Length.ToString());

            //for (int i = 0; i < args.Length; i++)
            //{
            //    Console.WriteLine("arg[" + i + "]: " + args[i]);
            //}
            //Console.Read();

            //if (args.Length < 2 || args.Length > 3)
            //{
            //    throw new ArgumentException("Parameters: <Server> <echo string> [Port]");
            //}

            //String server = args[0];

            //int servPort = (args.Length == 3) ? Int32.Parse(args[2]) : 7;


            ASCIIEncoding encoder = new ASCIIEncoding();
            

            byte[] byteBuffer = Encoding.ASCII.GetBytes(Console.ReadLine());

            TcpClient echoClient = null;
            NetworkStream netStream = null;


            try
            {
                echoClient = new TcpClient(server, servPort);

                Console.WriteLine("Connected to server end point: <{0}>:<{1}> \nSending echo string...", server, servPort);

                netStream = echoClient.GetStream();
                netStream.Write(byteBuffer, 0, byteBuffer.Length);

                int totalByteRcvd = 0;
                int bytesRcvd = 0;
                while (totalByteRcvd < byteBuffer.Length)
                {
                    bytesRcvd = netStream.Read(byteBuffer, totalByteRcvd, byteBuffer.Length - totalByteRcvd);
                    if (bytesRcvd == 0)
                    {
                        Console.WriteLine("Connection[sendin stream] closed prematurely.");
                        break;
                    }
                    totalByteRcvd += bytesRcvd;
                }
                Console.WriteLine("{0} echoes: {1}", server, Encoding.ASCII.GetString(byteBuffer, 0, totalByteRcvd));
                Console.Read();

            }
            catch (SocketException e)
            {

                Console.WriteLine(e.Message);
            }
            catch (IOException e) 
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (netStream != null)
                {
                    netStream.Close();
                }
                if (echoClient != null)
                {
                    echoClient.Close();
                }
            }
        }
    }
}

using ApplicationProtocolAndUtilities.Utilities;
using ApplicationProtocolAndUtilities.MessageDEncoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ApplicationProtocolAndUtilities;

namespace SocketServers
{
    
    class QuoteServer
    {
        private int BUFSIZE = 65536;     // Size of recieve buffer
        private int servPort = 4050;
        private string specificNetworkInterface = "127.0.0.1";

        public QuoteServer()
        {
        }

        public void Start()
        {
            
            StatePrinter.PrintProcedureState("Setting up server listener...");
            

            TcpListener listener = null;

            try
            {

                listener = new TcpListener(IPAddress.Any, servPort);
                listener.Start();
                Console.WriteLine("Start server at socket: <{0}>:<{1}>", IPAddress.Any, servPort);
                StatePrinter.PrintProcedureState("wait for client connection...");
                //Console.Write("wait for client connection...");
                
            }
            catch (SocketException se)
            {
                Console.WriteLine("\n---------");
                Console.WriteLine(System.DateTime.Now.ToString() + ":   " + "Set up listener error!");
                Console.WriteLine("Error: " + se.ErrorCode +
                                    "\nMessage: " + se.Message);
                Environment.Exit(se.ErrorCode);
            }

            TcpClient client = null;
            NetworkStream netStream = null;

            try
            {

                for (; ; )
                {

                    // set up connection
                    client = listener.AcceptTcpClient();
                    StatePrinter.PrintProcedureState("one client connection established...");
                    // get bidirectional communication channel
                    netStream = client.GetStream();


                    /** read request from client
                     * 1. prepare decoder
                     * 2. do the decode
                     */
                    ItemQuoteBinDecoder decoder = new ItemQuoteBinDecoder();
                    ItemQuoteProtocolFormat itemQuote = decoder.Decode(netStream);

                    Console.WriteLine("Rcvd bin-encoded Quote: ");
                    Console.WriteLine(itemQuote);


                    /** action based on the request
                     *      1. construct the response
                     *      2. encode it
                     *      3. send
                     */
                    ItemQuoteBinEncoder encoder = new ItemQuoteBinEncoder();
                    itemQuote.unitPrice += 10.9;
                    byte[] messageInBytes = encoder.Encode(itemQuote);
                    netStream.Write(messageInBytes, 0, messageInBytes.Length);

                    // prepare protocol data


                    netStream.Close();
                    client.Close();
                }

            }
            catch (Exception)
            {


            }
            finally
            {
                if (netStream != null)
                    netStream.Close();
                if (client != null)
                    client.Close();
            }

        }
    }

    class QuoteServerTester
    {

        // ===================================
        public static void Main(string[] args)
        {
            QuoteServer server = new QuoteServer();
            server.Start();
        }
	
    }
}

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
using SocketServers.MessageDEncoders;
using SocketServers.ProtocolFormats;

namespace SocketServers
{
    
    public class RemoteTVLServer
    {
        private int BUFSIZE = 65536;     // Size of recieve buffer
        private int servPort = 4050;
        private string specificNetworkInterface = "127.0.0.1";

        public RemoteTVLServer()
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

                for (; ;)
                {

                    // set up connection
                    StatePrinter.PrintProcedureState("wait for client connection...");
                    client = listener.AcceptTcpClient();
                    StatePrinter.PrintProcedureState("one-time client endpoint established...");
                    // get bidirectional communication channel
                    netStream = client.GetStream();


                    /** read request from client
                     * 1. prepare decoder
                     * 2. do the decode
                     */
                    StatePrinter.PrintProcedureState("Reading one message from the network stream....");
                    TVLForPassengerRecordDecoder decoder = new TVLForPassengerRecordDecoder();
                    TVLForPassengerRecord decodedTVL = decoder.Decode(netStream);

                    Console.WriteLine("Rcvd bin-encoded Quote: ");
                    Console.WriteLine(decodedTVL);


                    /** action based on the request
                     *      0. Decide actions
                     *      1. construct the response
                     *      2. encode it
                     *      3. send
                     */
                    TVLForPassengerRecordEncoder encoder = new TVLForPassengerRecordEncoder();

                    if (decodedTVL.Type == TVLForPassengerRecordConst.FULLREQUEST)
                    { }
                    List<PassengerRecord> pasRds = new List<PassengerRecord>();
                    pasRds.Add(new PassengerRecord("cc00010001", "112233445566",    0.1f,   0.5f,   "kk20130906001"));
                    pasRds.Add(new PassengerRecord("cc00010002", "null",            0f,     9f,     "kk20130906001"));
                    pasRds.Add(new PassengerRecord("cc00010003", "110233445566",    2f,     5f,     "kk20130906001"));
                    pasRds.Add(new PassengerRecord("cc00010004", "102233445566",    3f,     4.5f,   "kk20130906001"));
                    pasRds.Add(new PassengerRecord("cc00010005", "null",            1f,     5f,     "kk20130906001"));
                    pasRds.Add(new PassengerRecord("cc00010006", "112033445566",    1.5f,   6f,     "kk20130906001"));
                    pasRds.Add(new PassengerRecord("cc00010007", "612233445566",    0f,     7f,     "kk20130906001"));
                    pasRds.Add(new PassengerRecord("cc00010008", "912233445566",    2f,     9f,     "kk20130906001"));
                    pasRds.Add(new PassengerRecord("cc00010009", "122233445566",    6f,     8.7f,   "kk20130906001"));
                    pasRds.Add(new PassengerRecord("cc0001000A", "132233445566",    4f,     7f,     "kk20130906001"));
                    

                    TVLForPassengerRecord respone = new TVLForPassengerRecord(TVLForPassengerRecordConst.RESPONSE, pasRds.Count, pasRds);

                    StatePrinter.PrintProcedureState("Start sending response....");
                    byte[] messageInBytes = encoder.Encode(respone);
                    netStream.Write(messageInBytes, 0, messageInBytes.Length);

                    // prepare protocol data
                    StatePrinter.PrintProcedureState("Finish sending response....");
                    StatePrinter.PrintProcedureState("Close one-time service client ...\n>>>>>>>>>>>>>>>>>>");

                    //Close one-time service client...
                    netStream.Close();
                    client.Close();
                }

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);

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


    public class RemoteTVLServerTester
    {

        // ===================================
        static void Main(string[] args)
        {
            RemoteTVLServer server = new RemoteTVLServer();
            server.Start();
        }
	
    }
}

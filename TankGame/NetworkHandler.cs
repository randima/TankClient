using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;


namespace TankGame
{
    class NetworkHandler
    {
         //*****Fix Variables 
        private TcpClient client; //To talk back to the client
        private IPAddress ipAd = IPAddress.Parse("127.0.0.1"); // use local m/c IP address        
        private TcpListener listener; //To listen to the clinets 
        private static NetworkHandler comm = new NetworkHandler();
        private GameManager engine = GameManager.getInstance();

        public static NetworkHandler GetInstance()
        {
            return comm;
        }


        public void ReceiveData()
        {
            Socket s = null;
            try
            {
                /* Initializes the Listener */
                this.listener = new TcpListener(ipAd, 7000);

                /* Start Listeneting at the specified port */
                this.listener.Start();
                String data = null;

               
                //Console.WriteLine("Waiting for a connection.....");
                

                while (true)
                {
                    s = listener.AcceptSocket();
                    if (s.Connected)
                    {
                        NetworkStream stream = new NetworkStream(s);
                        int i;
                        byte[] bytes = new byte[256];
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            // Translate data bytes to a ASCII string.
                            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                            
                            //Console.WriteLine("Received: {0}", data);
                            /*using (StreamWriter writer = new StreamWriter("debug.txt", true))
                            {
                                writer.WriteLine(data);
                            }*/
                            ThreadPool.QueueUserWorkItem(new WaitCallback(engine.analyze), (Object)data);
                        }                                               
                        
                        //engine.analyze(data);                       
                    }
                    
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
            finally
            {
                if (s != null)
                    if (s.Connected)
                        s.Close();

            }
        }


        public void SendData(String data)
        {

            try
            {
                this.client = new TcpClient();
                this.client.Connect("127.0.0.1", 6000);
                NetworkStream stm = client.GetStream();
                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] msg = encoder.GetBytes(data);                
                stm.Write(msg, 0, msg.Length);

            }

            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
            finally
            {
                this.client.Close();
            }
        }
    }
    }


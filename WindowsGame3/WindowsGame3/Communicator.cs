using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace WindowsGame3
{
    public class Communicator
    {
        #region "Variables"
        private NetworkStream clientStream; //Stream - outgoing
        private TcpClient client; //To talk back to the client
        private BinaryWriter writer; //To write to the clients

        private NetworkStream serverStream; //Stream - incoming        
        private TcpListener listener; //To listen to the clinets        
        public string reply = ""; //The message to be written

        public List<Player> playerList;
        public List<Cell> brickWallList;
        public List<Cell> stoneWallList;
        public List<Cell> waterList;
        public List<CoinPile> coinPileList=new List<CoinPile>();
        public List<HealthPack> healthPackList=new List<HealthPack>();
        public Player me;
        public int meNumber;
        public int Nplayers;

        public bool startG = false;
        private bool startSever = false;
        public bool finishG = false;


        private static Communicator comm = new Communicator();
        #endregion

        private Communicator()
        {

        }

        public static Communicator GetInstance()
        {
            return comm;
        }

        public void ReceiveData()
        {
            Console.WriteLine("yyyyyyyyyyyyyy yyyyy");
            bool errorOcurred = false;
            Socket connection = null; //The socket that is listened to       
            try
            {
                if (!startSever)
                {
                    //Creating listening Socket
                    this.listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7000);
                    //Starts listening

                    this.listener.Start();
                }
                //Establish connection upon client request
                //DataObject dataObj;
                while (true)
                {
                   
                    //connection is connected socket
                    connection = listener.AcceptSocket();
                   
                    if (connection.Connected)
                    {
                        Console.WriteLine("bbbbb bbbbjkhlkjhlkh");
                        //To read from socket create NetworkStream object associated with socket
                        this.serverStream = new NetworkStream(connection);

                        SocketAddress sockAdd = connection.RemoteEndPoint.Serialize();
                        string s = connection.RemoteEndPoint.ToString();
                        List<Byte> inputStr = new List<byte>();

                        int asw = 0;
                        while (asw != -1)
                        {
                            asw = this.serverStream.ReadByte();
                            inputStr.Add((Byte)asw);
                        }

                        reply = Encoding.UTF8.GetString(inputStr.ToArray());
                        //  this.divide();
                      
                        //ThreadPool.QueueUserWorkItem(new WaitCallback(this.divide));
                       
                        if (reply[0] == 'S')
                        {
                            // SendData("RIGHT#");
                            //  Thread.Sleep(1000);
                            //ThreadPool.QueueUserWorkItem(new WaitCallback(this.SendData), (object)"RIGHT#");
                            //Thread.Sleep(1000);
                            // ThreadPool.QueueUserWorkItem(new WaitCallback(this.SendData), (object)"RIGHT#");


                        }
                        this.serverStream.Close();
                       
                        string ip = s.Substring(0, s.IndexOf(":"));
                        // int port = Constant.CLIENT_PORT;
                        try
                        {
                            string ss = reply.Substring(0, reply.IndexOf(";"));
                            //  port = Convert.ToInt32(ss);
                        }
                        catch (Exception)
                        {

                            // port = Constant.CLIENT_PORT;
                        }
                        Console.WriteLine(ip + ": " + reply.Substring(0, reply.Length - 1));
                        startSever = true;
                        break;
                        // dataObj = new DataObject(reply.Substring(0, reply.Length - 1), ip, port);
                        // ThreadPool.QueueUserWorkItem(new WaitCallback(GameEngine.Resolve), (object)dataObj);


                    }
                   
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Communication (RECEIVING) Failed! \n " + e.Message);
                errorOcurred = true;
            }
            finally
            {
                if (connection != null)
                    if (connection.Connected)
                        connection.Close();
                if (errorOcurred)
                    this.ReceiveData();
            }
        }
       
       // public void divide(object statinfo)
        public void divide()
        {
            //string rep = (string)statinfo;
            if (reply[0] == 'S')
            {
                startG = true;
                string[] words = reply.Substring(0, reply.IndexOf('#')).Split(':');
                if (reply.Length < 10)
                {
                   


                }
                Nplayers = words.Length - 1;
                playerList = new List<Player>();
                for(int i=0;i<words.Length-1;i++)
                {
                    Player p;
                    string[] playerwords = words[i+1].Split(';');
                     string[] coodinate = playerwords[1].Split(',');
                    p=new Player(i,int.Parse(coodinate[0]),int.Parse(coodinate[1]),int.Parse(playerwords[2]));
                    playerList.Add(p);
                    if(i==meNumber)
                        me=p;

                }
               
               // me = new Player(int.Parse(words[0].Substring(2, words[1].Length-1)), int.Parse(coodinate[0]), int.Parse(coodinate[1]), int.Parse(words[3]));

            }
            else if (reply[0] == 'I'& reply[1]==':')
            {
                
                
                string[] words = reply.Substring(0, reply.IndexOf('#')).Split(':');
                
                meNumber = int.Parse(words[1].Substring(1));
                    
               /* me = new Player(words[1][1], 0, 0, 0);
                Player ap;
                playerList = new List<Player>();
                for (int i = 1; i < 6; i++)
                {

                    ap = new Player(i, 0, 0, 0);
                    playerList.Add(ap);



                }*/
                string[] brickwords = words[2].Split(';');
                brickWallList = new List<Cell>();
                Cell b;
                foreach (string brickword in brickwords)
                {
                    string[] coodinate = brickword.Split(',');
                    b = new Cell(int.Parse(coodinate[0]), int.Parse(coodinate[1]));
                    brickWallList.Add(b);

                }
                string[] stonewords = words[3].Split(';');
                stoneWallList = new List<Cell>();
                Cell s;
                foreach (string stoneword in stonewords)
                {
                    string[] coodinate = stoneword.Split(',');
                    s = new Cell(int.Parse(coodinate[0]), int.Parse(coodinate[1]));
                    stoneWallList.Add(s);

                }
                string[] waterwords = words[4].Split(';');
                waterList = new List<Cell>();
                Cell w;
                foreach (string waterword in waterwords)
                {
                    string[] coodinate = waterword.Split(',');
                    w = new Cell(int.Parse(coodinate[0]), int.Parse(coodinate[1]));
                    waterList.Add(w);

                }




            }
            else if (reply[0] == 'G')
            {
                string[] words = reply.Substring(0, reply.IndexOf('#')).Split(':');
                if (words[0] == "GAME_HAS_FINISHED")
                {
                    finishG = true;
                }
               // playerList = new List<Player>();
                int i = 1;
                if(!finishG)
                foreach (Player ap in playerList)
                {

                    string[] playerwords = words[i].Split(';');
                    string[] coodinate = playerwords[1].Split(',');
                    // ap = new Player(int.Parse(playerwords[0].Substring(1, playerwords[0].Length)), int.Parse(coodinate[0]), int.Parse(coodinate[1]), int.Parse(playerwords[2]));
                    ap.update(int.Parse(coodinate[0]), int.Parse(coodinate[1]), int.Parse(playerwords[2]), int.Parse(playerwords[4]), int.Parse(playerwords[5]), int.Parse(playerwords[6]));
                    
                    if (ap.number == me.number)
                        me = ap;
                    i++;


                }
                string[] brickwords = words[Nplayers+1].Split(';');
                brickWallList = new List<Cell>();
                Cell b;
                foreach (string brickword in brickwords)
                {
                    string[] coodinate = brickword.Split(',');
                    b = new Cell(int.Parse(coodinate[0]), int.Parse(coodinate[1]));
                    b.update(int.Parse(coodinate[2]));
                    brickWallList.Add(b);

                }
            }
            else if (reply[0] == 'C'&reply[1]==':')
            {
                string[] words = reply.Substring(0, reply.IndexOf('#')).Split(':');
                string[] coodinate = words[1].Split(',');
                CoinPile pk;
                pk = new CoinPile(int.Parse(coodinate[0]), int.Parse(coodinate[1]),int.Parse(words[2]),int.Parse(words[3]));
                coinPileList.Add(pk);

            }
            else if (reply[0] == 'L')
            {
                string[] words = reply.Substring(0, reply.IndexOf('#')).Split(':');
                string[] coodinate = words[1].Split(',');
                HealthPack hp;
                hp = new HealthPack(int.Parse(coodinate[0]), int.Parse(coodinate[1]), int.Parse(words[2]));
                healthPackList.Add(hp);

            }


        }
        
        public void SendData(object stateInfo)
        {
            // DataObject dataObj = (DataObject)stateInfo;
            string cmd = (string)stateInfo;
            //Opening the connection
            this.client = new TcpClient();

            try
            {


                this.client.Connect(IPAddress.Parse("127.0.0.1"), 6000);

                if (this.client.Connected)
                {
                    //To write to the socket
                    this.clientStream = client.GetStream();

                    //Create objects for writing across stream
                    this.writer = new BinaryWriter(clientStream);
                    Byte[] tempStr = Encoding.ASCII.GetBytes(cmd);

                    //writing to the port                
                    this.writer.Write(tempStr);
                    Console.WriteLine("\t Data: " + cmd + " is written to " + "localhost" + " on " + 6000);

                    this.writer.Close();
                    this.clientStream.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Communication (WRITING) to " + cmd + " on " + "localhost" + "Failed! \n " + e.Message);
            }
            finally
            {
                this.client.Close();
            }




        }
    }
}

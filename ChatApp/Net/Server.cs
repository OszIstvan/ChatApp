using ChatClient.MVVM.Model;
using ChatClient.MVVM.ViewModel;
using ChatClient.Net.IO;
using ChatServer;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;


namespace ChatClient.Net
{
    class Server
    {
        static SqlConnection connection;
        static SqlCommand command;

        TcpClient _client;
        public PacketReader PacketReader;
        public string uzenet;
        Users user = new Users();
        UserModel model = new UserModel();

        public event Action connectedEvent;
        public event Action msgReceivedEvent;
        public event Action userDisconnectEvent;

        public Server()
        {
            _client = new TcpClient();
        }

        public void ConnectToServer(string username)
        {
            if (!_client.Connected)
            {
                _client.Connect("127.0.0.1", 7891);
                PacketReader = new PacketReader(_client.GetStream());

                if (!string.IsNullOrEmpty(username))
                {
                    var connectPacket = new PacketBuilder();
                    connectPacket.WriteOpCode(0);
                    connectPacket.WriteMessage(username);
                    _client.Client.Send(connectPacket.GetPacketBytes());
                }
                ReadPackets();
                
                user.Idopont = DateTime.Now;
                user.Uid = model.UID;
                user.Username = username;
                user.Uzenet = uzenet;
                //ABKezelo.Csatlakozas(user);
            }
        }

        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var opcode = PacketReader.ReadByte();
                    switch (opcode)
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;

                        case 5:
                            msgReceivedEvent?.Invoke();
                            break;

                        case 10:
                            userDisconnectEvent?.Invoke();
                            break;

                        default:
                            Console.WriteLine("Packetreader");
                            break;
                    }
                }
            });
        }

        public void SendMessageToServer(string message)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteMessage(message);
            _client.Client.Send(messagePacket.GetPacketBytes());
            user.Idopont = DateTime.Now;
            user.Uid = model.UID;
            user.Username = user.Username;
            user.Uzenet = message;
            ABKezelo.Beszuras(user);
        }

        public void NewUser()
        {
            ChatApp.MainWindow newUser = new ChatApp.MainWindow(); 
            newUser.Show();
        }

        public void SqlRefresh()
        {
            try
            {
                command = new SqlCommand();
                connection = new SqlConnection();
                connection.Close();
                command.Dispose();
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["ABEleres"].ConnectionString;
                connection.Open();
                command.Connection = connection;
                //System.Windows.Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                throw new ABKivetel("A kapcsolódás sikertelen!", ex);
            }
        }
        public void CloseApplication()
        {
            System.Windows.Application.Current.Shutdown();
            ABKezelo.Torol(user);
        }
    }
}

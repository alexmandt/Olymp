using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Olymp.Persistence;
using Olymp.Util;
using static Olymp.Util.Log;

namespace Olymp.Communication
{
    public class NodeCommunicationServer
    {
        private readonly string _address;
        private readonly int _port;
        private readonly string _name;

        public NodeCommunicationServer(string address, int port,string name)
        {
            _address = address;
            _port = port;
            _name = name;
        }

        public void Start(Func<Message,string, (Command cmd, string unencryptedMessage)> onReceiveFunction)
        {
            TcpListener server = null;
            while (true)
            {
                try
                {
                    var localAddress = IPAddress.Parse(_address);
                    server = new TcpListener(localAddress, _port);

                    server.Start();
                    while (true)
                    {
                        var client = server.AcceptTcpClient();
                        var stream = client.GetStream();

                        var bytes = new byte[client.ReceiveBufferSize];
                        int i;
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var data = Encoding.UTF8.GetString(bytes, 0, i);

                            #region Decrypt message

                            var deserializedMessage = JsonConvert.DeserializeObject<Message>(data);
                            var iv = new byte[16];
                            var bytesIv = Encoding.UTF8.GetBytes(deserializedMessage.User);
                            for (var j = 0; j < bytesIv.Length; j++)
                            {
                                iv[j] = bytesIv[j];
                            }

                            var pwd = new byte[32];
                            var bytesPwd =
                                Encoding.UTF8.GetBytes(UserRepository.Instance.GetUser(deserializedMessage.User)
                                    .Password);

                            for (var j = 0; j < bytesPwd.Length; j++)
                            {
                                pwd[j] = bytesPwd[j];
                            }

                            string decryptedMessage;
                            try
                            {
                                decryptedMessage =
                                    RijndaelManager.DecryptStringFromBytes(deserializedMessage.Content, pwd, iv);
                            }
                            catch (Exception)
                            {
                                //ignored - Invalid encryption
                                //Drop message - could be an attack
                                bytes = new byte[256];
                                client.Close();
                                continue;
                            }

                            #endregion

                            var responseMsg = new Message();
                            switch (deserializedMessage.Command)
                            {
                                #region Request node neighbour

                                case Command.REQ:
                                    Info($"Node {decryptedMessage} connected!", _name);
                                    responseMsg.User = deserializedMessage.User;
                                    responseMsg.Command = Command.OK;
                                    responseMsg.Content = RijndaelManager.EncryptStringToBytes(_name, pwd, iv);
                                    break;

                                #endregion

                                #region Base commands

                                case Command.OK:
                                case Command.FAIL:
                                    responseMsg.User = deserializedMessage.User;
                                    responseMsg.Command = deserializedMessage.Command;
                                    responseMsg.Content = RijndaelManager.EncryptStringToBytes(deserializedMessage.Command.ToString(), pwd, iv);
                                    break;

                                #endregion

                                default:
                                    responseMsg.User = deserializedMessage.User;
                                    var (command, content) = onReceiveFunction(deserializedMessage, decryptedMessage);
                                    responseMsg.Command = command;
                                    responseMsg.Content = RijndaelManager.EncryptStringToBytes(content, pwd, iv);
                                    break;
                            }

                            var msg = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(responseMsg));
                            stream.Write(msg, 0, msg.Length);
                        }

                        client.Close();
                    }
                }
                catch (SocketException e)
                {
                    Error($"SocketException: {e.Message}");
                }
                catch (Exception e)
                {
                    Error($"Exception: {e.Message}"); 
                }
                finally
                {
                    server?.Stop();
                }
            }
        }
    }
}
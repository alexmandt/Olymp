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
            try
            {
                var localAddress = IPAddress.Parse(_address);
                server = new TcpListener(localAddress, _port);

                server.Start();
                var bytes = new byte[256];
                while (true)
                {
                    var client = server.AcceptTcpClient();
                    var stream = client.GetStream();

                    int i;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var data = Encoding.UTF8.GetString(bytes, 0, i);

                        #region Decrypt message

                        var deserializedMessage = JsonConvert.DeserializeObject<Message>(data);
                        var iv = new byte[16];
                        var bytesIv = Encoding.UTF8.GetBytes(deserializedMessage.user);
                        for (var j = 0; j < bytesIv.Length; j++)
                        {
                            iv[j] = bytesIv[j];
                        }

                        var pwd = new byte[32];
                        var bytesPwd = Encoding.UTF8.GetBytes(UserRepository.Instance.GetUser(deserializedMessage.user).Password);

                        for (var j = 0; j < bytesPwd.Length; j++)
                        {
                            pwd[j] = bytesPwd[j];
                        }

                        string decryptedMessage;
                        try
                        {
                            decryptedMessage = RijndaelManager.DecryptStringFromBytes(deserializedMessage.content, pwd, iv);
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
                        switch (deserializedMessage.command)
                        {
                            #region Request node neighbour

                            case Command.REQ:
                                Info($"Node {decryptedMessage} connected!", _name);
                                responseMsg.user = deserializedMessage.user;
                                responseMsg.command = Command.OK;
                                responseMsg.content = RijndaelManager.EncryptStringToBytes(_name, pwd, iv);
                                break;

                            #endregion
                            
                            default:
                                responseMsg.user = deserializedMessage.user;
                                var (command, content) = onReceiveFunction(deserializedMessage,decryptedMessage);
                                responseMsg.command = command;
                                responseMsg.content = RijndaelManager.EncryptStringToBytes(content, pwd, iv);
                                break;
                        }

                        var msg = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(responseMsg));
                        stream.Write(msg, 0, msg.Length);
                    }
                    bytes = new byte[256];
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Error($"SocketException: {e.Message}");
            }
            finally
            {
                server?.Stop();
            }
        }
    }
}
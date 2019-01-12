using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MessagePack;
using Olymp.Communication.Messages;
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
        private bool printStart = false;
        
        public NodeCommunicationServer(string address, int port, string name)
        {
            this._address = address;
            this._port = port;
            this._name = name;
        }

        public void Start(Func<Message, byte[], (Command cmd, IMessage unencryptedMessage)> onReceiveFunction)
        {
            TcpListener server = null;
            while (true)
            {
                try
                {
                    var localAddress = IPAddress.Parse(this._address);
                    server = new TcpListener(localAddress, this._port);

                    server.Start();

                    if (printStart)
                    {
                        Info("Server restarted!",_name);
                        printStart = false;
                    }
                    
                    while (true)
                    {
                        var client = server.AcceptTcpClient();
                        var stream = client.GetStream();

                        var bytes = new byte[client.ReceiveBufferSize];
                        while (stream.Read(bytes, 0, bytes.Length) != 0)
                        {
                            #region Decrypt message

                            var deserializedMessage = MessagePackSerializer.Deserialize<Message>(bytes);
                            var pwd = UserRepository.Instance.GetUser(deserializedMessage.User).Password;

                            byte[] decryptedMessage;
                            try
                            {
                                decryptedMessage = RijndaelManager.Decrypt(deserializedMessage.Content, pwd);
                            }
                            catch (Exception e)
                            {
                                //ignored - Invalid encryption
                                //Drop message - could be an attack
                                bytes = new byte[256];
                                client.Close();
                                // If you continue the client will try to receive more bytes, but since it is closed it'll throw an Exception
                                // Instead throw an exception that decryption failed, which will be handled by the outer try-catch block
                                // and the error will be logged
                                continue;
                            }

                            #endregion

                            var responseMsg = new Message();
                            switch (deserializedMessage.Command)
                            {
                                #region Request node neighbour

                                case Command.REQ:
                                    Info($"Node {MessagePackSerializer.Deserialize<SingleValueMessage>(decryptedMessage).Value} connected!", _name);
                                    responseMsg.User = deserializedMessage.User;
                                    responseMsg.Command = Command.OK;
                                    var b = MessagePackSerializer.Serialize(new SingleValueMessage {Value = _name});
                                    responseMsg.Content = RijndaelManager.Encrypt(b, pwd);
                                    break;

                                #endregion

                                #region Base commands

                                case Command.OK:
                                case Command.FAIL:
                                    responseMsg = deserializedMessage;
                                    break;

                                #endregion

                                default:
                                    responseMsg.User = deserializedMessage.User;
                                    (var command, object content) = onReceiveFunction(deserializedMessage, decryptedMessage);
                                    responseMsg.Command = command;
                                    responseMsg.Content = RijndaelManager.Encrypt(MessagePackSerializer.Serialize(content), pwd);
                                    break;
                            }

                            var msg = MessagePackSerializer.Serialize(responseMsg);
                            stream.Write(msg, 0, msg.Length);
                        }

                        client.Close();
                    }
                }
                catch (SocketException e)
                {
                    restart(e);
                }
                catch (Exception e)
                {
                    restart(e);
                }
                finally
                {
                    server?.Stop();
                }
            }

            void restart(Exception e)
            {
                Error($"Exception: {e.Message}", _name);
                Info("Restarting server...", _name);
                printStart = true;
            }
        }
    }
}
using System;
using System.Net;
using System.Net.Sockets;
using MessagePack;
using Olymp.Communication.Messages;
using Olymp.Exceptions;
using Olymp.Persistence;
using Olymp.Util;
using static Olymp.Util.Log;

namespace Olymp.Communication
{
    public class NodeCommunicationServer
    {
        private readonly string _address;
        private readonly string _name;
        private readonly int _port;
        private bool _printRestartMessage;

        public NodeCommunicationServer(string address, int port, string name)
        {
            _address = address;
            _port = port;
            _name = name;
        }

        public void Start(Func<Message, byte[], (Command cmd, IMessage unencryptedMessage)> onReceiveFunction)
        {
            TcpListener server = null;
            while (true)
                try
                {
                    var localAddress = IPAddress.Parse(_address);
                    server = new TcpListener(localAddress, _port);

                    server.Start();

                    if (_printRestartMessage)
                    {
                        Info("Server restarted!", _name);
                        _printRestartMessage = false;
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
                            catch (Exception)
                            {
                                // Ignored - Invalid encryption
                                // Drop message - could be an attack
                                client.Close();
                                throw new DecryptionFailedException();
                            }

                            #endregion

                            var responseMsg = new Message {User = deserializedMessage.User};
                            switch (deserializedMessage.Command)
                            {
                                #region Request node neighbour

                                case Command.REQ:
                                    Info(
                                        $"Node {MessagePackSerializer.Deserialize<SingleValueMessage>(decryptedMessage).Value} connected!",
                                        _name);
                                    responseMsg.Command = Command.OK;
                                    var newMessage = MessagePackSerializer.Serialize(new SingleValueMessage
                                        {Value = _name});
                                    responseMsg.Content = RijndaelManager.Encrypt(newMessage, pwd);
                                    break;

                                #endregion

                                #region Base commands

                                case Command.OK:
                                case Command.FAIL:
                                    responseMsg.Command = Command.OK;
                                    responseMsg.Content = 
                                        RijndaelManager.Encrypt(MessagePackSerializer.Serialize(deserializedMessage), pwd);
                                    break;

                                #endregion

                                default:
                                    (var command, object content) =
                                        onReceiveFunction(deserializedMessage, decryptedMessage);
                                    responseMsg.Command = command;
                                    responseMsg.Content =
                                        RijndaelManager.Encrypt(MessagePackSerializer.Serialize(content), pwd);
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
                    Restart(e);
                }
                catch (Exception e)
                {
                    Restart(e);
                }
                finally
                {
                    server?.Stop();
                }

            void Restart(Exception e)
            {
                Error($"Exception: {e.Message}", _name);
                Info("Restarting server...", _name);
                _printRestartMessage = true;
            }
        }
    }
}
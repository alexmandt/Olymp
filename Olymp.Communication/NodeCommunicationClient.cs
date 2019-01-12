using System;
using System.Net.Sockets;
using MessagePack;
using Olymp.Communication.Messages;
using Olymp.Util;
using static Olymp.Util.Log;

namespace Olymp.Communication
{
    public static class NodeCommunicationClient
    {
        public static (Message message, byte[] content) Send(string server, string username, string password, object message, Command command, string name)
        {
            TcpClient client = null;
            NetworkStream stream = null;
            try
            {
                var data = EncryptMessage(username, password, command, message);
                client = new TcpClient(server.Split(":")[0],int.Parse(server.Split(":")[1]));
                stream = client.GetStream();

                stream.Write(data, 0, data.Length);
                stream.Flush();
                data = new byte[256];
                stream.Read(data, 0, data.Length);

                var returnData = MessagePackSerializer.Deserialize<Message>(data);
                return (returnData,RijndaelManager.Decrypt(returnData.Content,MD5Helper.CalculateMD5Hash(password)));
            }
            catch (ArgumentNullException e)
            {
                Error($"ArgumentNullException: {e}", name);
                throw;
            }
            catch (SocketException e)
            {
                Error($"SocketException: {e}", name);
                throw;
            }
            finally
            {
                stream?.Close();
                client?.Close();
            }
        }

        private static byte[] EncryptMessage(string username, string password, Command command, object message)
        {
            password = MD5Helper.CalculateMD5Hash(password);
            var bytes = MessagePackSerializer.Serialize(message);
            return MessagePackSerializer.Serialize(new Message
            {
                User = username,
                Command = command,
                Content = RijndaelManager.Encrypt(bytes, password)
            });
        }
    }
}
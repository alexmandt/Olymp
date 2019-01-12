using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Olymp.Util;
using static Olymp.Util.Log;

namespace Olymp.Communication
{
    public static class NodeCommunicationClient
    {
        public static (Message message, string content) Send(string server, string username, string password, string message, Command command, string name)
        {
            TcpClient client = null;
            NetworkStream stream = null;

            try
            {
                var (msg, iv, pwd) = EncryptMessage(username, password, message, command);

                var data = Encoding.UTF8.GetBytes(msg);

                client = new TcpClient(server.Split(":")[0],int.Parse(server.Split(":")[1]));
                stream = client.GetStream();

                stream.Write(data, 0, data.Length);
                stream.Flush();
                data = new byte[256];
                stream.Read(data, 0, data.Length);

                var returnData = JsonConvert.DeserializeObject<Message>(Encoding.UTF8.GetString(data));

                return (returnData,RijndaelManager.DecryptStringFromBytes(returnData.Content, pwd, iv));
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

        private static (string msg, byte[] pwd, byte[] iv) EncryptMessage(string username, string password, string message, Command command)
        {
            password = MD5Helper.CalculateMD5Hash(password);

            var iv = new byte[16];
            var bytesiv = Encoding.UTF8.GetBytes(username);
            for (var i = 0; i < bytesiv.Length; i++)
            {
                iv[i] = bytesiv[i];
            }

            var pwd = new byte[32];
            var bytespwd = Encoding.UTF8.GetBytes(password);

            for (var i = 0; i < bytespwd.Length; i++)
            {
                pwd[i] = bytespwd[i];
            }

            return (JsonConvert.SerializeObject(new Message
            {
                User = username,
                Command = command,
                Content = RijndaelManager.EncryptStringToBytes(message, pwd, iv)
            }), pwd, iv);
        }
    }
}
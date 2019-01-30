using System;
using System.Text.RegularExpressions;
using Olymp.Exceptions;

namespace Olymp.Util
{
    public static class Validator
    {
        private const string IpRegex =
            @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";

        private const string PortRegex =
            @"^([0-9]{1,4}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])$";

        public static string ValidateIp(string ip)
        {
            if (ip == null)
                throw new ArgumentNullException(nameof(ip));

            var match = Regex.Match(ip, IpRegex, RegexOptions.IgnoreCase);

            if (!match.Success)
                throw new InvalidIpException(ip);

            return ip;
        }

        public static string ValidatePort(string port)
        {
            if (port == null)
                throw new ArgumentNullException(nameof(port));

            var match = Regex.Match(port, PortRegex, RegexOptions.IgnoreCase);

            if (!match.Success)
                throw new InvalidPortException(port);

            return port;
        }
    }
}
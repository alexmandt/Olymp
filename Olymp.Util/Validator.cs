using System.Text.RegularExpressions;
using Olymp.Exceptions;

namespace Olymp.Util
{
    public static class Validator
    {
        private const string IpRegex =
            @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";

        public static string ValidateIp(string ip)
        {
            var match = Regex.Match(ip, IpRegex, RegexOptions.IgnoreCase);

            if (!match.Success) throw new InvalidIpException(ip);

            return ip;
        }
    }
}
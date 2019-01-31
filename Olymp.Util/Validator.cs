using System;
using System.Text.RegularExpressions;
using Olymp.Exceptions;

namespace Olymp.Util
{
    public static class Validator
    {
        private const string Ipv4Regex =
            @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";

        private const string PortRegex =
            @"^([0-9]{1,4}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])$";

        private const string Ipv6Regex =
            @"^([0-9a-f]{0,4}:){2,7}(:|[0-9a-f]{1,4})$";

        //HostnameRegex is valid as per RFC 1123.
        private const string HostnameRegex =
            @"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$";

        //TODO: Implement referent method with UriParser
        public static bool ValidateAddress(string ip)
        {
            if (ip == null)
                throw new ArgumentNullException(nameof(ip));

            var match = Regex.Match(ip, Ipv4Regex, RegexOptions.IgnoreCase).Success ||
                        Regex.Match(ip, Ipv6Regex, RegexOptions.IgnoreCase).Success ||
                        Regex.Match(ip, HostnameRegex, RegexOptions.IgnoreCase).Success;

            if (!match) throw new InvalidIpOrHostnameException(ip);
            return true;
        }

        public static bool ValidatePort(string port)
        {
            if (port == null)
                throw new ArgumentNullException(nameof(port));

            var match = Regex.Match(port, PortRegex, RegexOptions.IgnoreCase);

            if (!match.Success)
                throw new InvalidPortException(port);

            return true;
        }
    }
}
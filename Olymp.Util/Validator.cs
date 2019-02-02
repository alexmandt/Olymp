using System;
using System.Text.RegularExpressions;
using Olymp.Exceptions;

namespace Olymp.Util
{
    public static class Validator
    {
        private static readonly Regex Ipv4Regex = new Regex(
            @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$",
            RegexOptions.Compiled);

        private static readonly Regex PortRegex = new Regex(
            @"^([0-9]{1,4}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])$",
            RegexOptions.Compiled);

        private static readonly Regex Ipv6Regex = new Regex(
            @"^([0-9a-f]{0,4}:){2,7}(:|[0-9a-f]{1,4})$", RegexOptions.Compiled);

        //HostnameRegex is valid as per RFC 1123.
        private static readonly Regex HostnameRegex = new Regex(
            @"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$",
            RegexOptions.Compiled);

        public static bool ValidateAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
                throw new ArgumentNullException(nameof(address));

            switch (Uri.CheckHostName(address))
            {
                case UriHostNameType.Dns:
                    return Ipv4Regex.IsMatch(address);
                case UriHostNameType.IPv4:
                    return Ipv4Regex.IsMatch(address);
                case UriHostNameType.IPv6:
                    return Ipv6Regex.IsMatch(address);
                default:
                    throw new InvalidIpOrHostnameException(nameof(address));
            }
        }

        public static bool ValidatePort(string port)
        {
            if (string.IsNullOrEmpty(port))
                throw new ArgumentNullException(nameof(port));

            var match = PortRegex.IsMatch(port);

            if (!match)
                throw new InvalidPortException(port);

            return true;
        }

        public static bool ValidateStringValues(params string[] values)
        {
            foreach (var value in values)
                if (string.IsNullOrEmpty(value))
                    throw new InvalidArgumentException(value);
            return true;
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace Bitstamp.Client.Websocket.Utils
{
    /// <summary>
    /// Helper class for working with pair identifications
    /// </summary>
    public static class CryptoPairsHelper
    {
        /// <summary>
        /// Clean pair from any unnecessary characters and make lowercase
        /// </summary>
        public static string Clean(string pair)
        {
            return (pair ?? string.Empty)
                .Trim()
                .ToLower()
                .Replace("/", "")
                .Replace("-", "")
                .Replace("\\", "");
        }

        /// <summary>
        /// Compare two pairs, clean them before
        /// </summary>
        public static bool AreSame(string firstPair, string secondPair)
        {
            var first = Clean(firstPair);
            var second = Clean(secondPair);
            return first.Equals(second);
        }

        public static List<string> WalletSymbols(List<string> pairs)
        {
            var pairsList = new List<string>();
            foreach (var pair in pairs)
            {
                var clean = Clean(pair);

                if (clean.Length == 6)
                {
                    var quoteSymbol = clean.Substring(0, 3);
                    var baseSymbol = clean.Substring(3, 3);

                    pairsList.Add(quoteSymbol);
                    pairsList.Add(baseSymbol);
                }

                // TODO
                // ETHUSDT
                // LINKUSD

                if (clean.Length > 6)
                {
                    // check this if quote is 3/4 chars
                    var quoteSymbol = clean.Substring(0, 3);
                    var baseSymbol = clean.Substring(3, 4);

                    pairsList.Add(quoteSymbol);
                    pairsList.Add(baseSymbol);
                }
            }

            return pairsList.Distinct().ToList();
        }
    }
}
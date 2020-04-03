using Newtonsoft.Json;

namespace Bitstamp.Client.Websocket.Responses.Books
{
    public class OrderBookDiff
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        public long? Timestamp { get; set; }

        /// <summary>
        /// Microtimestamp
        /// </summary>
        public string Microtimestamp { get; set; }

        /// <summary>
        /// Order book bid levels
        /// </summary>
        [JsonConverter(typeof(OrderBookLevelConverter), OrderBookSide.Buy)]
        public BookLevel[] Bids { get; set; }

        /// <summary>
        /// Order book ask levels
        /// </summary>
        [JsonConverter(typeof(OrderBookLevelConverter), OrderBookSide.Sell)]
        public BookLevel[] Asks { get; set; }
    }
}
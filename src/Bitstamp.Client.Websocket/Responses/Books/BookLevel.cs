namespace Bitstamp.Client.Websocket.Responses.Books
{
    /// <summary>
    /// One order book level
    /// </summary>
    public class BookLevel
    {
        public OrderBookSide Side { get; set; }

        public double Price { get; set; }

        public double Amount { get; set; }

        public long Mts { get; set; }
    }
}
namespace Bitstamp.Client.Websocket.Channels
{
    public enum Channel
    {
        /// <summary>
        ///     Ticker
        /// </summary>
        Heartbeat,

        /// <summary>
        ///     Ticker
        /// </summary>
        Ticker,

        /// <summary>
        ///     Orders
        /// </summary>
        Orders,

        /// <summary>
        ///     OrderBook
        /// </summary>
        OrderBook,

        /// <summary>
        ///     OrderBookDetail
        /// </summary>
        OrderBookDetail,

        /// <summary>
        ///     OrderBookFull
        /// </summary>
        OrderBookFull
    }
}
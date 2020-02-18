using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Responses;

namespace Bitstamp.Client.Websocket.Client
{
    /// <summary>
    /// All provided streams.
    /// You need to send subscription request in advance (via method `Send()` on BitstampWebsocketClient)
    /// </summary>
    public class BitstampClientStreams
    {
        internal readonly Subject<ErrorResponse> ErrorSubject = new Subject<ErrorResponse>();

        //internal readonly Subject<InfoResponse> InfoSubject = new Subject<InfoResponse>();
        internal readonly Subject<HeartbeatResponse> HeartbeatSubject = new Subject<HeartbeatResponse>();

        internal readonly Subject<OrderBookDetailResponse> OrderBookDetailSubject =
            new Subject<OrderBookDetailResponse>();

        internal readonly Subject<OrderBookFullResponse> OrderBookFullSubject = new Subject<OrderBookFullResponse>();

        internal readonly Subject<OrderBookResponse> OrderBookSubject = new Subject<OrderBookResponse>();

        internal readonly Subject<OrderResponse> OrdersSubject = new Subject<OrderResponse>();

        internal readonly Subject<SubscriptionSucceeded> SubscriptionSucceededSubject =
            new Subject<SubscriptionSucceeded>();

        internal readonly Subject<Ticker> TickerSubject = new Subject<Ticker>();

        // PUBLIC

        /// <summary>
        /// Server errors stream.
        /// Error messages: Most failure cases will cause an error message to be emitted.
        /// This can be helpful for implementing a client or debugging issues.
        /// </summary>
        public IObservable<ErrorResponse> ErrorStream => ErrorSubject.AsObservable();

        /// <summary>
        /// Response stream to every ping request
        /// </summary>
        public IObservable<HeartbeatResponse> HeartbeatStream => HeartbeatSubject.AsObservable();

        /// <summary>
        /// Subscription info stream, emits status after sending subscription request
        /// </summary>
        public IObservable<OrderResponse> OrdersStream => OrdersSubject.AsObservable();

        public IObservable<Ticker> TickerStream => TickerSubject.AsObservable();

        public IObservable<OrderBookResponse> OrderBookStream => OrderBookSubject.AsObservable();

        public IObservable<OrderBookDetailResponse> OrderBookDetailStream => OrderBookDetailSubject.AsObservable();

        public IObservable<OrderBookFullResponse> OrderBookFullStream => OrderBookFullSubject.AsObservable();

        public IObservable<SubscriptionSucceeded> SubscriptionSucceededStream =>
            SubscriptionSucceededSubject.AsObservable();


        // PRIVATE
    }
}
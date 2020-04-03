﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Websocket.Client.Sample.Models;
//
//    var orderBook = Book.FromJson(jsonString);

using System;
using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses.Books
{
    public class OrderBookResponse : ResponseBase
    {
        public override MessageType Event => MessageType.OrderBook;
        public OrderBook Data { get; set; }

        internal static bool TryHandle(JObject response, ISubject<OrderBookResponse> subject)
        {
            if (response != null && (bool) !response?["channel"].Value<string>().StartsWith("order_book")) return false;

            var parsed = response?.ToObject<OrderBookResponse>(BitstampJsonSerializer.Serializer);

            if (parsed != null)
            {
                var index = 2;
                var channel = response?["channel"].Value<string>();

                parsed.Symbol = response?["channel"].Value<string>().Split('_')[index];
                subject.OnNext(parsed);
            }

            return true;
        }
    }
}
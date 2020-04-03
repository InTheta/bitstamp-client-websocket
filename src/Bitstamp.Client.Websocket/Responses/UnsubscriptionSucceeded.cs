using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses
{
    public class UnsubscriptionSucceeded : ResponseBase
    {
        public override MessageType Event => MessageType.Unsubscribe;
        public Data Data { get; set; }

        internal static bool TryHandle(JObject response, ISubject<UnsubscriptionSucceeded> subject)
        {
            if (response?["event"].Value<string>() != "bts:unsubscribe") return false;

            var parsed = response.ToObject<UnsubscriptionSucceeded>(BitstampJsonSerializer.Serializer);

            parsed.Symbol = response["channel"].Value<string>().Split('_')[1];

            subject.OnNext(parsed);
            return true;
        }
    }

    public class Data
    {
    }
}
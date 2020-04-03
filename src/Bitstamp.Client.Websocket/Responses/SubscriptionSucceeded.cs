using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses
{
    public class SubscriptionSucceeded : ResponseBase
    {
        public override MessageType Event => MessageType.Subscribe;

        internal static bool TryHandle(JObject response, ISubject<SubscriptionSucceeded> subject)
        {
            if (response?["event"].Value<string>() != "bts:subscription_succeeded") return false;

            var parsed = response.ToObject<SubscriptionSucceeded>(BitstampJsonSerializer.Serializer);

            parsed.Symbol = response["channel"].Value<string>().Split('_')[1];

            subject.OnNext(parsed);
            return true;
        }
    }
}
using Bitstamp.Client.Websocket.Json;
using Newtonsoft.Json.Linq;
using System.Reactive.Subjects;

namespace Bitstamp.Client.Websocket.Responses
{
    public class SubscriptionSucceeded : ResponseBase<SubscriptionSucceeded>
    {
        internal static bool TryHandle(JObject response, ISubject<SubscriptionSucceeded> subject)
        {
            if (response?["event"].Value<string>() != "bts:subscription_succeeded") return false;

            var parsed = response.ToObject<SubscriptionSucceeded>(BitstampJsonSerializer.Serializer);

            parsed.Symbol = response["channel"].Value<string>().Split('_')[2];

            subject.OnNext(parsed);
            return true;
        }
    }
}
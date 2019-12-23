using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using Bitstamp.Client.Websocket.Channels;
using Bitstamp.Client.Websocket.Client;
using Bitstamp.Client.Websocket.Communicator;
using Bitstamp.Client.Websocket.Requests;
using Serilog;
using Serilog.Events;

namespace Bitstamp.Client.Websocket.Sample
{
    internal class Program
    {
        private static readonly ManualResetEvent ExitEvent = new ManualResetEvent(false);

        private static readonly string API_KEY = "your api key";
        private static readonly string API_SECRET = "";

        private static void Main(string[] args)
        {
            InitLogging();

            AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnProcessExit;
            AssemblyLoadContext.Default.Unloading += DefaultOnUnloading;
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;

            Console.WriteLine("|=======================|");
            Console.WriteLine("|    COINBASE CLIENT    |");
            Console.WriteLine("|=======================|");
            Console.WriteLine();

            Log.Debug("====================================");
            Log.Debug("              STARTING              ");
            Log.Debug("====================================");


            var url = BitstampValues.ApiWebsocketUrl;
            using (var communicator = new BitstampWebsocketCommunicator(url))
            {
                communicator.Name = "Bitstamp-1";
                //communicator.ReconnectTimeoutMs = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;

                using (var client = new BitstampWebsocketClient(communicator))
                {
                    SubscribeToStreams(client);

                    communicator.ReconnectionHappened.Subscribe(async type =>
                    {
                        Log.Information($"Reconnection happened, type: {type}, resubscribing..");
                        await SendSubscriptionRequests(client);
                    });

                    communicator.Start().Wait();

                    ExitEvent.WaitOne();
                }
            }

            Log.Debug("====================================");
            Log.Debug("              STOPPING              ");
            Log.Debug("====================================");
            Log.CloseAndFlush();
        }

        private static async Task SendSubscriptionRequests(BitstampWebsocketClient client)
        {
            var orderBook = new SubscribeRequest("btcusd", Channel.OrderBook);
            //var orderBook = new SubscribeRequest("btcusd", ChannelType.Book);
            var orderBookDetail = new SubscribeRequest("btcusd", Channel.OrderBookDetail);

            var orderBookFull = new SubscribeRequest("btcusd", Channel.OrderBookFull);

            var heartbeat = new SubscribeRequest("btcusd", Channel.Heartbeat);

            await client.Send(orderBook);
            await client.Send(orderBookDetail);
            await client.Send(orderBookFull);
        }

        private static void SubscribeToStreams(BitstampWebsocketClient client)
        {
            client.Streams.ErrorStream.Subscribe(x =>
                Log.Warning($"Error received, message: {x?.Data?.Message}"));

            client.Streams.SubscriptionSucceededStream.Subscribe(x =>
            {
                Log.Information($"SUCESS!");
                Log.Information($"{x?.Symbol} {x?.Data?.Channel} {x?.Data?.Data}");
            });

            client.Streams.OrderBookStream.Subscribe(x =>
            {
                Log.Information($"Order book");
                Log.Information($"{x.Symbol} {x.Data?.Channel} {x.Data?.Asks.FirstOrDefault()?.Price} {x.Data?.Asks.FirstOrDefault()?.Amount??0} {x.Data?.Asks.FirstOrDefault()?.Side}");
                Log.Information($"{x.Symbol} {x.Data?.Channel} {x.Data?.Bids.FirstOrDefault()?.Price} {x.Data?.Bids.FirstOrDefault()?.Amount ?? 0} {x.Data?.Bids.FirstOrDefault()?.Side}");
            });

            client.Streams.OrdersStream.Subscribe(x =>
            {
                //Log.Information($"{x.Data} {x.Data.Asks[0]} {x.Data.EventBids[0]}");
                //Log.Information($"{x.Symbol} {x.Data.Channel} {x.Data.Amount}");
            });

            client.Streams.OrderBookDetailStream.Subscribe(x =>
            {
                Log.Information($"Order book detail");
                Log.Information($"{x.Symbol} {x.Data?.Channel} {x.Data?.Asks.FirstOrDefault()?.Price} {x.Data?.Asks.FirstOrDefault()?.Amount ?? 0} {x.Data?.Asks.FirstOrDefault()?.Side}");
                Log.Information($"{x.Symbol} {x.Data?.Channel} {x.Data?.Bids.FirstOrDefault()?.Price} {x.Data?.Bids.FirstOrDefault()?.Amount ?? 0} {x.Data?.Bids.FirstOrDefault()?.Side}");
            });

            client.Streams.OrderBookFullStream.Subscribe(x =>
            {
                Log.Information($"Order book full");
                Log.Information($"{x.Symbol} {x.Data?.Channel} {x.Data?.Asks.FirstOrDefault()?.Price} {x.Data?.Asks.FirstOrDefault()?.Amount ?? 0} {x.Data?.Asks.FirstOrDefault()?.Side}");
                Log.Information($"{x.Symbol} {x.Data?.Channel} {x.Data?.Bids.FirstOrDefault()?.Price} {x.Data?.Bids.FirstOrDefault()?.Amount ?? 0} {x.Data?.Bids.FirstOrDefault()?.Side}");
            });

            
            client.Streams.HeartbeatStream.Subscribe(x =>
                Log.Information($"Heartbeat received, product: {x?.Channel}, seq: {x?.Event}"));

            /*
            client.Streams.TickerStream.Subscribe(x =>
                    Log.Information($"Ticker {x.ProductId}. Bid: {x.BestBid} Ask: {x.BestAsk} Last size: {x.LastSize}, Price: {x.Price}")
                );

            client.Streams.TradesStream.Subscribe(x =>
            {
                Log.Information($"Trade executed [{x.ProductId}] {x.TradeSide} price: {x.Price} size: {x.Size}");
            });

            client.Streams.OrderBookSnapshotStream.Subscribe(x =>
            {
                Log.Information($"OB snapshot [{x.ProductId}] bids: {x.Bids.Length}, asks: {x.Asks.Length}");
            });

            client.Streams.OrderBookUpdateStream.Subscribe(x =>
            {
                Log.Information($"OB updates [{x.ProductId}] changes: {x.Changes.Length}");
            });
            */

            // example of unsubscribe requests
            //Task.Run(async () =>
            //{
            //    await Task.Delay(5000);
            //    await client.Send(new BookSubscribeRequest("XBTUSD") {IsUnsubscribe = true});
            //    await Task.Delay(5000);
            //    await client.Send(new TradesSubscribeRequest() {IsUnsubscribe = true});
            //});
        }

        private static void InitLogging()
        {
            var executingDir = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            var logPath = Path.Combine(executingDir, "logs", "verbose.log");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .WriteTo.ColoredConsole(LogEventLevel.Debug)
                .CreateLogger();
        }

        private static void CurrentDomainOnProcessExit(object sender, EventArgs eventArgs)
        {
            Log.Warning("Exiting process");
            ExitEvent.Set();
        }

        private static void DefaultOnUnloading(AssemblyLoadContext assemblyLoadContext)
        {
            Log.Warning("Unloading process");
            ExitEvent.Set();
        }

        private static void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Log.Warning("Canceling process");
            e.Cancel = true;
            ExitEvent.Set();
        }
    }
}
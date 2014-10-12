using System;
using System.Globalization;
using WebSocketSharp;

namespace AV.CodeEnjoy.Core
{
    public class WebsocketClient<TGame>: IDisposable where TGame: IGame, new() 
    {

      public string ConnectionString { get; private set; }

      private WebSocket _webSocket;
      private IGameProcessor<TGame> _gameProcessor;

      public WebsocketClient(ConnectionOptions options, IGameProcessor<TGame> gameProcessor)
      {
        _gameProcessor = gameProcessor;

        ConnectionString = string.Format(@"ws://{0}:{1}/{2}/ws?user={3}", options.Host, options.Port, options.Game, options.UserName);
        _webSocket = new WebSocket(ConnectionString);

        AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) => Stop();
      }

      /// <summary>
      /// Run the client.
      /// </summary>
      /// <remarks>Only one instance of the client can be running.</remarks>
      public void Start()
      {
        // check if conncetion is already open
        if (_webSocket.ReadyState != WebSocketState.Connecting ||
            _webSocket.ReadyState != WebSocketState.Open)
        // preconfigure communication
          _webSocket.OnMessage += OnMessageReceive;
        
        // start communication
        _webSocket.Connect();
      }

      /// <summary>
      /// Stop Communication with server
      /// </summary>
      public void Stop()
      {
        if (_webSocket.ReadyState != WebSocketState.Closed)
        {
          _webSocket.Close();
          _webSocket.OnMessage -= OnMessageReceive;
        }
      }

      private void OnMessageReceive(object sender, MessageEventArgs args)
      {
        var serverMessage = args.Data.ToString(CultureInfo.InvariantCulture);
        var game = new TGame();
        game.DeserializeFrom(serverMessage);
        
        _gameProcessor.ProcessTick(game);
        
        _webSocket.Send(game.GetRecordedManipulations());
      }

      public void Dispose()
      {
        Stop();
      }
    }
}

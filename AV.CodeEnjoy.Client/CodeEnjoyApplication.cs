using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AV.CodeEnjoy.Core;
using AV.CodeEnjoy.Tetris.Core;

namespace AV.CodeEnjoy.Client
{
  /// <summary>
  /// The main entry point to the code enjoy application
  /// </summary>
  class CodeEnjoyApplication<TGame> where TGame: IGame, new()
  {
    private readonly ConnectionOptions _options;
    private WebsocketClient<TGame> _connector;

    public CodeEnjoyApplication(ConnectionOptions options)
    {
      _options = options;
    }

    /// <summary>
    /// Pass configuratio for the current code enjoy game
    /// </summary>
    public CodeEnjoyApplication<TGame> Configure(IGameProcessor<TGame> gameProcessor)
    {
      _connector = new WebsocketClient<TGame>(_options, gameProcessor);
      return this;
    }

    /// <summary>
    /// Call to start communicate with code enjoy server
    /// </summary>
    public void Run()
    {
      try
      {
        _connector.Start();

        Console.WriteLine("Please, press 'Enter' to exit...");
        Console.ReadLine();
      }
      finally 
      {        
        _connector.Stop();
      }
    }
  }
}

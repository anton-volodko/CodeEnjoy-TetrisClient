namespace AV.CodeEnjoy.Core
{
  /// <summary>
  /// Default configuration options for all games
  /// </summary>
  public class ConnectionOptions
  {
    public string Host { get; private set; }
    public int Port { get; private set; }
    public string Game { get; private set; }
    public string UserName { get; private set; }

    public ConnectionOptions(string host, int port, string game, string userName)
    {
      Host = host;
      Port = port;
      Game = game;
      UserName = userName;

    }
  }
}

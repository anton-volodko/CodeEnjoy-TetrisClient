namespace AV.CodeEnjoy.Core
{
  public interface IGameProcessor<in TGame> where TGame: IGame, new()
  {
    /// <summary>
    /// Process server tick and provide result of the processing.
    /// </summary>
    /// <returns>Answer for processed server data.</returns>
    void ProcessTick(TGame game);
  }
}

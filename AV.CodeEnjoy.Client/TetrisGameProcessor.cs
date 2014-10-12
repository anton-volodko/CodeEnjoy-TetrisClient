using AV.CodeEnjoy.Core;
using AV.CodeEnjoy.Tetris.Core;

namespace AV.CodeEnjoy.Client
{
  class TetrisGameProcessor: IGameProcessor<Game>
  {
    /// <summary>
    /// The component which help visually understand that is happening.
    /// Also it provides keyboard control over the game.
    /// Left, Right arrows: to shift figure horizontaly
    /// Down Arrow: to drop the figure 
    /// Spacebar - for the figure Rotation
    /// </summary>
    private TetrisConsoleVisualizer _consoleVisualizer = new TetrisConsoleVisualizer();

    public void ProcessTick(Game game)
    {
      _consoleVisualizer.Visualize(game);
      // game.Figure.Drop();
    }
  }
}

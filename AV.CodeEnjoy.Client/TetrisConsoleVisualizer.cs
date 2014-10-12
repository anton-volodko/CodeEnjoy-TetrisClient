using System;
using AV.CodeEnjoy.Tetris.Core;

namespace AV.CodeEnjoy.Client
{
  class TetrisConsoleVisualizer
  {
    private readonly KeysSequence _keys;

    public TetrisConsoleVisualizer(KeysSequence keys)
    {
      _keys = keys;
    }

    public void Visualize(Game game)
    {
      while (_keys.Count != 0)
      {
        var key = _keys.Dequeue();
       
        switch (key)
        {
          case ConsoleKey.LeftArrow:
            game.Figure.MoveHorizontaly(-1);
            break;
          case ConsoleKey.RightArrow:
            game.Figure.MoveHorizontaly(+1);
            break;
          case ConsoleKey.DownArrow:
            game.Figure.Drop();
            break;
          case ConsoleKey.Spacebar:
            game.Figure.Rotate(1);
            break;
        }
      }
      Console.CursorTop = 2;

      VisualizeGlass(0,  game.Glass);

      Console.WriteLine();

      Console.WriteLine("Actions: {0}                                                                            ", 
        game.Figure.GetRecordedManipulations());
            
      Console.CursorTop = 2;
      VisualizeFigure(3 /* count of symbols used for drawing glass countours*/, 
                      game.Glass, game.Figure);

    }

    private static void VisualizeGlass(int startPoint, Glass field)
    {
      Console.CursorLeft = startPoint; 
      for (int row = field.Size.Height - 1; 0 <= row; row--)
      {
        Console.Write("{0, 2}|", row);
        for (int column = 0; column < field.Size.Width; column++)
        {
          Console.Write(field[row, column] ? '*' : ' ');
        }
        Console.WriteLine("|                             ");
        
        Console.CursorLeft = startPoint;
      }
      for (int column = 0; column <= field.Size.Width + 3; column++)
      {
        Console.Write('_');
      }
    }

    private static void VisualizeFigure(int startPoint, Glass field, Figure figure)
    {
      var figureMask = figure.GetFigureMask();
      var centerXoffset = figureMask.Location.X - figure.Location.X;
      var centerYoffset = figureMask.Location.Y - figure.Location.Y;

      startPoint += figureMask.Location.X;
      Console.CursorTop += field.Size.Height - figureMask.Location.Y - 1;
      for (int row = 0; row < figureMask.Size.Height; row++)
      {
        Console.CursorLeft = startPoint - 1;
        Console.Write("{0, 1}", row);
        for (int column = 0; column < figureMask.Size.Width; column++)
        {
          var isCenterPoint = row == centerYoffset && column == centerXoffset;
          var fillSym = isCenterPoint ? '+' : '*';
          Console.Write(figureMask[row, column] ? fillSym : ' ');
        }
        if (row == 0) Console.Write("({0}, {1})", figureMask.Location.X, figureMask.Location.Y);
        Console.CursorTop++;
        Console.CursorLeft = startPoint;
      }
    }
  }
}

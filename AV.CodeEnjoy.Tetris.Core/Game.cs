using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AV.CodeEnjoy.Core;

namespace AV.CodeEnjoy.Tetris.Core
{
  public class Game: IGame
  {
    public Glass Glass { get; private set; }
    public Figure Figure { get; private set; }
    public FigureType[] NextFigures { get; private set; }

    public void DeserializeFrom(string data)
    {
      var parameters = Regex.Match(data.ToString(CultureInfo.InvariantCulture),
        @"^figure=(\w+)&x=(\d+)&y=(\d+)&glass=(.*)&next=(\w*)$");

      var figure = (FigureType) parameters.Groups[1].ToString()[0];
      var x = int.Parse(parameters.Groups[2].ToString());
      var y = int.Parse(parameters.Groups[3].ToString());
      var glass = parameters.Groups[4].ToString();
      var nextFigures = parameters.Groups[5].ToString();

      Figure = new Figure(figure, new Point(x, y));
      Glass = ParseGlass(glass);
      NextFigures = nextFigures.Select(f => (FigureType)f).ToArray();
    }

    private static Glass ParseGlass(string glassData)
    {
      var glass = new Glass(new Size(10, 20));
      var charIndex = 0;
      for (int row = 0; row < glass.Size.Height; row++)
      {
        for (int column = 0; column < glass.Size.Width; column++)
        {
          glass[row, column] = (glassData[charIndex] != ' ');
          charIndex++;
        }
      }
      return glass;
    }

    public string GetRecordedManipulations()
    {
      return Figure.GetRecordedManipulations();
    }
  }
}

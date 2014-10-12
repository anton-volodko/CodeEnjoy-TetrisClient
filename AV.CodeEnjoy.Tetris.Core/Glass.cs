using System.Drawing;

namespace AV.CodeEnjoy.Tetris.Core
{
  public class Glass
  {
    public Size Size { get; private set; }
    public bool[,] Field { get; private set; }

    public Glass(Size size)
    {
      Size = size;
      Field = new bool[Size.Height, Size.Width];
    }

    public bool this[int row, int column]
    {
      get { return Field[row, column]; }
      set { Field[row, column] = value; }
    }
  }
}

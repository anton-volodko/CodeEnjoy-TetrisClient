using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AV.CodeEnjoy.Tetris.Core
{
  public class Mask: Glass
  {
    public Point Location { get; private set; }

    public Mask(Point location, Size size)
      : base(size)
    {
      Location = location;
    }
  }
}

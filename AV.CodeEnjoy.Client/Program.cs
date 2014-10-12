using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AV.CodeEnjoy.Core;
using AV.CodeEnjoy.Tetris.Core;

namespace AV.CodeEnjoy.Client
{
  class Program
  {
    static void Main(string[] args)
    {
      // You should modify the TetrisGameProcessor class to add application specific logic.
      var tetrisProcessor = new TetrisGameProcessor();
      new CodeEnjoyApplication<Game>(
        new ConnectionOptions("localhost", 8080, "tetris-contest", "anton")).
        Configure(tetrisProcessor).
        Run();
    }
  }
}

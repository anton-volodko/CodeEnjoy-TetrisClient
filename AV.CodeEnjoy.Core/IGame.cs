using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AV.CodeEnjoy.Core
{
  public interface IGame
  {
    void DeserializeFrom(string serverData);
    string GetRecordedManipulations();
  }
}

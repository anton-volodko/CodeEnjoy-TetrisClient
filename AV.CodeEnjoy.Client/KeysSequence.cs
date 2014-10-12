using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AV.CodeEnjoy.Client
{
  class KeysSequence: Queue<ConsoleKey>
  {
    private static Lazy<KeysSequence> _lazy = new Lazy<KeysSequence>( () => new KeysSequence()); 
    
    private KeysSequence()
    {
      
    }

    public static KeysSequence Instance
    {
      get { return _lazy.Value; }
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cold_Ship
{
  /**
   * An interface to allow an object to be checked by some other elements
   * and this object will be the "watchee", and the "watcher" will react
   * on the condition of the "watchee"
   **/
  public interface IWatchfulConditional
  {
    bool GetCondition();
  }
}

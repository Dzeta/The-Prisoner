using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cold_Ship
{
  public class GameLevel
  {
    protected Character PlayerNode { get; set; }
    protected Camera2D Camera { get; set; }
    public Cold_Ship GameInstance { get; set; }

    protected GameLevel(Cold_Ship gameInstance)
    {
      this.GameInstance = gameInstance;
      this.PlayerNode = gameInstance.Player;
      this.Camera = gameInstance.Camera;
    }
  }
}

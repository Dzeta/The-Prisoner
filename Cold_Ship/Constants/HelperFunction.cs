using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Cold_Ship
{
  public static class HelperFunction
  {
      public static Keys KeyLeft = Keys.A;
      public static Keys KeyUp = Keys.W;
      public static Keys KeyDown = Keys.S;
      public static Keys KeyRight = Keys.D;

      public static Keys KeyJump = Keys.Space;   
      public static Keys KeyUse = Keys.E;
      public static Keys KeySpeed = Keys.LeftShift;

      private static List<Keys> usedKeys;

      public static List<Keys> UsedKeys()
      {
          if (usedKeys == null)
              usedKeys = new List<Keys>();
          else
              usedKeys.Clear();

          usedKeys.Add(KeyLeft);
          usedKeys.Add(KeyUp);
          usedKeys.Add(KeyDown);
          usedKeys.Add(KeyRight);
          usedKeys.Add(KeyJump);
          usedKeys.Add(KeyUse);
          usedKeys.Add(KeySpeed);

          return usedKeys;
      }
  }
}

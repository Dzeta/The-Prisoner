using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Cold_Ship
{
  public class Portal : GenericSprite2D
  {
    public static Texture2D DOOR_LIGHT_GREEN;
    public static Texture2D DOOR_LIGHT_RED;
    public static Texture2D DOOR_TEXTURE;

    public enum PortalType { FORWARD, BACKWARD };

    private PortalType _portalType;
    private bool _isLocked;

    //constructor that initialize the Texture and Position
    public Portal(GameLevel instance, Vector2 position)
      : base(instance, DOOR_LIGHT_RED, position)
    {
      _isLocked = _portalType == PortalType.BACKWARD;
    }

    public bool IsLock() { return this._isLocked; }
    public void Lock() { this._isLocked = true; }
    public void Unlock() { this._isLocked = false; }

    public static Portal GetNewInstance(GameLevel instance, Vector2 position, PortalType type, bool isLocked)
    {
      if (DOOR_LIGHT_RED == null)
        Portal.DOOR_LIGHT_RED = instance.Content.Load<Texture2D>("Objects/doorlight_green");
      if (DOOR_LIGHT_GREEN == null)
        Portal.DOOR_LIGHT_GREEN = instance.Content.Load<Texture2D>("Objects/doorlight_red");
      if (DOOR_TEXTURE == null)
        Portal.DOOR_TEXTURE = instance.Content.Load<Texture2D>("Objects/door");


      Portal _instance = new Portal(instance, position);
      _instance.Texture = Portal.DOOR_TEXTURE;
      _instance._portalType = type;
      _instance._isLocked = isLocked;

      return _instance;
    }

    //draw the portal onto screen
    public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
    {
      if (_isLocked)
        spriteBatch.Draw(Portal.DOOR_LIGHT_GREEN
            , drawPosition + new Vector2(14, -13), Color.White);
      else
        spriteBatch.Draw(Portal.DOOR_LIGHT_RED
            , drawPosition + new Vector2(14, -13), Color.White);

      spriteBatch.Draw(this.Texture, drawPosition, Color.White);
    }
  }
}

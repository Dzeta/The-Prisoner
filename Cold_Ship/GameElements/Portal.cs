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

    public static Vector2 DOOR_SIZE = new Vector2(51, 72);

    private Vector2 _offsetFromParent;
    private bool _isLocked;

    //constructor that initialize the Texture and Position
    public Portal(GameLevel instance, Vector2 position)
      : base(instance, DOOR_TEXTURE, position)
    {
      _isLocked = true;
      _offsetFromParent = new Vector2(10, 10);
    }

    public bool IsLock() { return this._isLocked; }
    public void Lock() { this._isLocked = true; }
    public void Unlock() { this._isLocked = false; }

    public static Portal GetNewInstance(GameLevel instance, Vector2 position, bool isLocked)
    {
      if (DOOR_LIGHT_RED == null)
        Portal.DOOR_LIGHT_RED = instance.Content.Load<Texture2D>("Objects/doorlight_green");
      if (DOOR_LIGHT_GREEN == null)
        Portal.DOOR_LIGHT_GREEN = instance.Content.Load<Texture2D>("Objects/doorlight_red");
      if (DOOR_TEXTURE == null)
        Portal.DOOR_TEXTURE = instance.Content.Load<Texture2D>("Objects/door");

      Rectangle _boundBox = new Rectangle((int)position.X
          , (int)position.Y, (int)DOOR_SIZE.X, (int)DOOR_SIZE.Y);
      Portal _instance = new Portal(instance, position);
      _instance.Texture = Portal.DOOR_TEXTURE;
      _instance._isLocked = isLocked;
      _instance.BoundBox = _boundBox;

      return _instance;
    }

    //draw the portal onto screen
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      if (_isLocked)
        spriteBatch.Draw(Portal.DOOR_LIGHT_GREEN
          , _offsetFromParent + this.Position, Color.White);
      else
        spriteBatch.Draw(Portal.DOOR_LIGHT_RED
          , _offsetFromParent + this.Position, Color.White);
      spriteBatch.End();
      base.Draw(spriteBatch);
    }
  }
}

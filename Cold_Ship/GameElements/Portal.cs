using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

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

    public GameLevel ToGameLevelPortal;
    public Portal GameLevelPortal;

    //constructor that initialize the Texture and Position
    public Portal(GameLevel instance, Vector2 position)
      : base(instance, DOOR_TEXTURE, position)
    {
      _isLocked = true;
      _offsetFromParent = new Vector2(10, 10);
    }

    public bool IsUnlocked() { return !this.IsLocked(); }
    public bool IsLocked() { return this._isLocked; }
    public void Lock() { this._isLocked = true; }
    public void Unlock() { this._isLocked = false; }


    public void WalkThroughPortal(Character player)
    {
      this.ToGameLevelPortal.LoadLevelContentIfHasNotForPlayer(player);
      player.SetPreviousGameLevel(player.CurrentGameLevel);
      player.CurrentGameLevel = this.ToGameLevelPortal;
      this.SpawnPlayer(player);
    }

    public void SpawnPlayer(Character player)
    {
      player.Position = this.GameLevelPortal.Position + new Vector2(0, 9);
    }

    public static Portal GetNewInstance(GameLevel instance, GameLevel takeToLevel, Vector2 position, bool isLocked)
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
      _instance.ToGameLevelPortal = takeToLevel;

      return _instance;
    }

    //draw the portal onto screen
    public override void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
      base.Draw(spriteBatch, position);

      spriteBatch.Begin();
      if (_isLocked)
        spriteBatch.Draw(Portal.DOOR_LIGHT_GREEN
          , _offsetFromParent + position, Color.White);
      else
        spriteBatch.Draw(Portal.DOOR_LIGHT_RED
          , _offsetFromParent + position, Color.White);
      spriteBatch.End();
    }
  }
}

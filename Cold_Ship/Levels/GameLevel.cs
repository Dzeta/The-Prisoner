using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cold_Ship
{
  public class GameLevel
  {
    protected Character PlayerNode { get; set; }
    protected Camera2D Camera { get; set; }
    public Cold_Ship GameInstance { get; set; }

    protected List<Portal> LeveLPortals { get; set; } 
    protected List<PickUpItem> LevelPickUpItems { get; set; } 
    protected List<Platform> LevelPlatforms { get; set; } 
    protected List<Ladder> LevelLadders { get; set; } 
    protected List<InvisibleChatTriggerBox> LevelChatTriggerBoxes { get; set; } 
    protected List<DialogueBubble> LevelDialogueBubbles { get; set; }
    protected List<GenericSprite2D> LevelStaticWorldObjects { get; set; } 

    protected Texture2D LevelBackgroundTexture;
    protected Rectangle WorldBoundingRectangle; // Generally, rectangle width and height replace the old _worldSize variable


    // INTERNVAL OBJECTS
    public SpriteBatch SpriteBatch;
    public ContentManager Content;

    protected GameLevel(Cold_Ship gameInstance)
    {
      this.GameInstance = gameInstance;
      this.PlayerNode = gameInstance.Player;
      this.Camera = gameInstance.Camera;
      this.Content = gameInstance.Content;
      this.SpriteBatch = gameInstance.SpriteBatch;

      LeveLPortals = new List<Portal>();
      LevelPickUpItems = new List<PickUpItem>();
      LevelPlatforms = new List<Platform>(); 
      LevelLadders = new List<Ladder>();
      LevelChatTriggerBoxes = new List<InvisibleChatTriggerBox>();
      LevelDialogueBubbles = new List<DialogueBubble>();
    }

    public Vector2 GetAbsoluteWorldSize()
    {
      return new Vector2(WorldBoundingRectangle.Width
          , WorldBoundingRectangle.Height);
    }

    public virtual void Update(GameTime gameTime)
    {
      Camera.Update(gameTime);

      foreach (Portal portal in LeveLPortals)
        portal.Update(gameTime);

      foreach (PickUpItem item in LevelPickUpItems)
        item.Update(gameTime);

      foreach (Platform platform in LevelPlatforms)
        platform.Update(gameTime);

      foreach (Ladder ladder in LevelLadders)
        ladder.Update(gameTime);
    }
  }
}

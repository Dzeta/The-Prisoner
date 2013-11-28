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
    public Character PlayerNode { get; set; }
    public Camera2D Camera { get; set; }
    public Cold_Ship GameInstance { get; set; }

    public List<Portal> LevelPortals { get; set; } 
    public List<PickUpItem> LevelPickUpItems { get; set; } 
//    public List<Platform> LevelPlatforms { get; set; } 
    public List<Ladder> LevelLadders { get; set; } 
    public List<InvisibleChatTriggerBox> LevelChatTriggerBoxes { get; set; } 
    public List<DialogueBubble> LevelDialogueBubbles { get; set; }
    public List<GenericSprite2D> LevelStaticWorldObjects { get; set; } 

    protected Texture2D LevelBackgroundTexture;
    protected Rectangle WorldBoundingRectangle; // Generally, rectangle width and height replace the old _worldSize variable

    public Portal EntrancePortal;
    public Portal ExitPortal;

    public GameLevel PreviousGameLevel;
    public GameLevel NextGameLevel;

    // INTERNVAL OBJECTS
    public SpriteBatch SpriteBatch;
    public ContentManager Content;

    public GameLevel(Cold_Ship gameInstance, GameLevel prevLevel, GameLevel nextLevel)
    {
      this.GameInstance = gameInstance;
      this.PlayerNode = gameInstance.Player;
      this.Camera = gameInstance.Camera;
      this.Content = gameInstance.Content;
      this.SpriteBatch = gameInstance.SpriteBatch;

      LevelPortals = new List<Portal>();
      LevelPickUpItems = new List<PickUpItem>();
//      LevelPlatforms = new List<Platform>(); 
      LevelLadders = new List<Ladder>();
      LevelChatTriggerBoxes = new List<InvisibleChatTriggerBox>();
      LevelDialogueBubbles = new List<DialogueBubble>();

      PreviousGameLevel = prevLevel;
      NextGameLevel = nextLevel;
    }

    public Vector2 GetScreenSize()
    {
      Vector2 _screenSize = new Vector2(this.GameInstance.Window.ClientBounds.Width
          , this.GameInstance.Window.ClientBounds.Height);
      return _screenSize;
    }
    public Vector2 GetAbsoluteWorldSize()
    {
      return new Vector2(WorldBoundingRectangle.Width
          , WorldBoundingRectangle.Height);
    }

    public virtual void SpawnPlayer(Character player)
    {
      if (!player.HasVisited(this.GetType()))
        this.LoadContent();
    }

    public virtual void LoadContent() { }
    public virtual void Update(GameTime gameTime)
    {
      Camera.Update(gameTime);

      foreach (Portal portal in LevelPortals)
        portal.Update(gameTime);

      foreach (PickUpItem item in LevelPickUpItems)
      {
        item.Update(gameTime);
        if (!item.IsConsumed() && item.CheckCollision(this.PlayerNode))
          this.PlayerNode.PickUp(item);
      }
//
//      foreach (Platform platform in LevelPlatforms)
//        platform.Update(gameTime);
//
//      foreach (Ladder ladder in LevelLadders)
//        ladder.Update(gameTime);
      // Update Dialogues
      foreach (InvisibleChatTriggerBox chatTrigger in LevelChatTriggerBoxes)
      {
        chatTrigger.Update(gameTime);
        if (!chatTrigger.IsConsumed() 
            && chatTrigger.GetHitBox().Intersects(this.PlayerNode.GetPlayerHitBox()))
          chatTrigger.InteractWith(new Vector2(400, 400), this);
      }
    }

    public virtual void Draw()
    {
      SpriteBatch.Begin();
      SpriteBatch.Draw(LevelBackgroundTexture, Vector2.Zero, Color.White);
      SpriteBatch.End();


      foreach (Portal portal in LevelPortals)
        portal.Draw(SpriteBatch);

      foreach (PickUpItem item in LevelPickUpItems)
        if (!item.IsPickedUp())
          item.Draw(SpriteBatch);
//
//      foreach (Platform platform in LevelPlatforms)
//        platform.Update(gameTime);
//
//      foreach (Ladder ladder in LevelLadders)
//        ladder.Update(gameTime);
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace Cold_Ship
{
  public class GameLevel
  {
    public Cold_Ship GameInstance { get; set; }
    public GameLevel(Cold_Ship gameInstance)
    {
      this.GameInstance = gameInstance;
    }
  }

  public class Level_Holding_Cell : GameLevel
  {
    //declare member variables
    public SpriteBatch spriteBatch;
    Vector2 worldSize, screenSize;
    List<Platform> platforms;
    float ground;
    Texture2D statusDisplayTexture;
    SpriteFont font;
    Character playerNode;
    GenericSprite2D backgroundNode;
    Camera2D camera;
    Portal forwardDoor;
    List<Portal> portals;
    List<DialogueBubble> dialogueBubbles;
    List<GenericSprite2D> worldObjects;
    bool visited = false;

    HealthBar _healthBar;
    PickUpItem lighter;

    //declare constructor
    public List<InvisibleChatTriggerBox> AllChatTriggers;
    public Level_Holding_Cell(Cold_Ship theGameInstance, SpriteBatch spriteBatch, Vector2 screenSize)
      : base(theGameInstance)
    {
      this.spriteBatch = spriteBatch;
      platforms = new List<Platform>();
      this.screenSize = screenSize;
      portals = new List<Portal>();
      worldObjects = new List<GenericSprite2D>();

      this.AllChatTriggers = new List<InvisibleChatTriggerBox>();
    }

    //load content
    public void LoadContent(ContentManager Content, Game_Level gameLevel, Game_Level prevGameLevel, double bodyTemperature, double stamina, double staminaLimit)
    {
      //load the needed textures
      Texture2D playerTexture = Content.Load<Texture2D>("Character\\PlayerSpriteSheet");
      Texture2D backgroundTexture = Content.Load<Texture2D>("Backgrounds\\holdingcell_final");
      statusDisplayTexture = Content.Load<Texture2D>("statusDisplay");


      //initialize the world size and the ground coordinate according to the world size
      worldSize = new Vector2(backgroundTexture.Width, backgroundTexture.Height);
      ground = worldSize.Y - 200;

      //load font
      font = Content.Load<SpriteFont>("Fonts\\Score");

      //initialize the needed nodes and camera
      backgroundNode = new GenericSprite2D(backgroundTexture, new Vector2(0, 0), Rectangle.Empty);
      worldObjects.Add(backgroundNode);
      camera = new Camera2D(spriteBatch);
      camera.cameraPosition = new Vector2(0, worldSize.Y - screenSize.Y);

      //initialize the needed platforms
      Texture2D platformTexture = Content.Load<Texture2D>("Textures\\platformTexture");

      //initialize the needed portals
      forwardDoor = new Portal(new Vector2(worldSize.X - 251, worldSize.Y /*- 288*/ - 280), new Vector2(51, 72), Portal.PortalType.FOWARD, Content);
      forwardDoor.canOpen = true;
      portals.Add(forwardDoor);
      worldObjects.AddRange(portals);

      playerNode = new Character(GameInstance, playerTexture, new Vector2(forwardDoor.Position.X - 32 - 200, worldSize.Y - 200 - 64), bodyTemperature, stamina, staminaLimit, 4, 6);
      playerNode._pocketLight = PocketLightSource.GetNewInstance(GameInstance, playerNode);
      playerNode._pocketLight.TurnOn();

      // Load the text with respect to the current player's Position

      if (!visited)
      {
//          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(new Vector2(forwardDoor.Position.X - 200, forwardDoor.Position.Y + 30), StringDialogue.holdingCellIntroduction1));
//          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(new Vector2(forwardDoor.Position.X - 200, forwardDoor.Position.Y + 30), StringDialogue.holdingCellIntroduction2));
//          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(new Vector2(forwardDoor.Position.X - 200, forwardDoor.Position.Y + 30), StringDialogue.holdingCellIntroduction3));
//          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(new Vector2(forwardDoor.Position.X - 200, forwardDoor.Position.Y + 30), StringDialogue.holdingCellIntroduction4));
//          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(new Vector2(forwardDoor.Position.X - 200, forwardDoor.Position.Y + 30), StringDialogue.holdingCellIntroduction5));
//          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(new Vector2(forwardDoor.Position.X - 200, forwardDoor.Position.Y + 30), StringDialogue.holdingCellIntroduction6));
//          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(new Vector2(forwardDoor.Position.X - 200, forwardDoor.Position.Y + 30), StringDialogue.holdingCellIntroduction7));
        AllChatTriggers.Add(
          InvisibleChatTriggerBox.GetNewInstance(new Vector2(486, 336),
            StringDialogue.holdingCellLeaveWithoutLighter, this.playerNode.HasLighter));
      }

      this._healthBar = HealthBar.GetNewInstance(GameInstance, this.playerNode, playerNode.GetHealthAsRatio); 

      Texture2D lighterTexture = Content.Load<Texture2D>("Objects\\lighter");
      if (!visited)
      {
          lighter = new PickUpItem(lighterTexture, new Vector2(forwardDoor.Position.X - 32 - 260, forwardDoor.Position.Y + 55), new Vector2(lighterTexture.Width, lighterTexture.Height), PickUpItem.ItemType.NONE, 100, PickUpItem.ItemEffectDuration.NONE);
          worldObjects.Add(lighter);
      }
      worldObjects.Add(playerNode);
    }

    //unload contents
    public void Unload()
    {
      platforms = new List<Platform>();
      portals = new List<Portal>();
      worldObjects = new List<GenericSprite2D>();
      visited = true;
    }

    //update function
    public double Update(GameTime gameTime, ref float bodyTempTimer, ref float exhaustionTimer, ref KeyboardState oldKeyboardState, ref float jumpTimer, ref Game_Level gameLevel, ref float staminaExhaustionTimer, ref double bodyTemperature, ref double stamina, ref double staminaLimit)
    {
      // Update Dialogues
      foreach (InvisibleChatTriggerBox chatTrigger in AllChatTriggers)
      {
        chatTrigger.Update(gameTime);
        if (!chatTrigger.IsConsumed() 
            && chatTrigger.GetHitBox().Intersects(playerNode.getPlayerHitBox()))
          chatTrigger.InteractWith(new Vector2(forwardDoor.Position.X - 50, forwardDoor.Position.Y), GameInstance);
      }

      //update the player Position with respect to keyboard input and platform collision
      bool useLighter = false;

      this._healthBar.Update(gameTime);

      playerNode.Update(useLighter, gameTime, ref bodyTempTimer, ref exhaustionTimer, ref oldKeyboardState, ref jumpTimer, ground, platforms, null, worldSize, ref staminaExhaustionTimer);

      if (playerNode.Position.X < 250)
      {
        playerNode.Position.X = 250;
      }
      else if (playerNode.Position.X > worldSize.X - 250 - 31)
      {
        playerNode.Position.X = worldSize.X - 250 - 31;
      }

      if (lighter.Position != new Vector2(forwardDoor.Position.X - 32 - 260, forwardDoor.Position.Y + 55))
      {
          //foreach (Portal portal in portals)
          //{
          //    portal.Update(playerNode, ref gameLevel);
          //}
          forwardDoor.Update(playerNode, ref gameLevel);
      }

      lighter.Update(ref playerNode, ref bodyTemperature, ref stamina, ref staminaLimit);

      //update the camera based on the player and world size
      camera.TranslateWithSprite(playerNode, screenSize);
      camera.CapCameraPosition(worldSize, screenSize);

      //return the body temperature
      return playerNode.bodyTemperature;
    }

    //draw funtion
    public void Draw(int framesPerSecond)
    {
      spriteBatch.Begin();

      if (Cold_Ship.DEBUG_MODE)
        foreach (InvisibleChatTriggerBox box in AllChatTriggers)
          spriteBatch.Draw(this.GameInstance.DebugTexture, box.GetHitBox(), Color.Pink);

      //draw the desired nodes onto screen through the camera
      foreach (GenericSprite2D element in worldObjects)
          camera.DrawNode(element);

      this._healthBar.Draw(spriteBatch);
      //draw the fps
      spriteBatch.DrawString(font, framesPerSecond.ToString(), new Vector2(screenSize.X - 50, 25), Color.White);
      //draw the status display and the body temperature
      spriteBatch.Draw(statusDisplayTexture, new Vector2(50, 50), Color.White);
      spriteBatch.DrawString(font, Math.Round(playerNode.bodyTemperature, 2).ToString(), new Vector2(52, 52), Color.Black, 0, new Vector2(0, 0), new Vector2(0.8f, 2), SpriteEffects.None, 0);
      spriteBatch.DrawString(font, Math.Round(playerNode.stamina, 2).ToString(), new Vector2(120, 52), Color.Black, 0, new Vector2(0, 0), new Vector2(1f, 1), SpriteEffects.None, 0);

      // Draw all invisible chat trigger
      if (Cold_Ship.DEBUG_MODE)
        foreach (InvisibleChatTriggerBox invisibleTrigger in AllChatTriggers)
          if (!invisibleTrigger.IsConsumed())
            spriteBatch.Draw(GameInstance.DebugTexture, invisibleTrigger.GetHitBox(), Color.White);

      spriteBatch.End();
    }
  }
}

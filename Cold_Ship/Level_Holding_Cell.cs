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
    public Game1 PrisonerGame { get; set; }
    public GameLevel(Game1 game)
    {
      this.PrisonerGame = game;
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

    PickUpItem lighter;

    //declare constructor
    public List<InvisibleChatTriggerBox> AllChatTriggers;
    public Level_Holding_Cell(Game1 theGame, SpriteBatch spriteBatch, Vector2 screenSize)
      : base(theGame)
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
      Texture2D playerTexture = Content.Load<Texture2D>("PlayerSpriteSheet");
      Texture2D backgroundTexture = Content.Load<Texture2D>("holdingcell_final");
      statusDisplayTexture = Content.Load<Texture2D>("statusDisplay");


      //initialize the world size and the ground coordinate according to the world size
      worldSize = new Vector2(backgroundTexture.Width, backgroundTexture.Height);
      ground = worldSize.Y - 200;

      //load font
      font = Content.Load<SpriteFont>("Score");

      //initialize the needed nodes and camera
      backgroundNode = new GenericSprite2D(backgroundTexture, new Vector2(0, 0), Rectangle.Empty);
      worldObjects.Add(backgroundNode);
      camera = new Camera2D(spriteBatch);
      camera.cameraPosition = new Vector2(0, worldSize.Y - screenSize.Y);

      //initialize the needed platforms
      Texture2D platformTexture = Content.Load<Texture2D>("platformTexture");

      //initialize the needed portals
      forwardDoor = new Portal(new Vector2(worldSize.X - 251, worldSize.Y - 288), new Vector2(51, 72), Portal.PortalType.FOWARD, Content);
      forwardDoor.canOpen = true;
      portals.Add(forwardDoor);
      worldObjects.AddRange(portals);

      playerNode = new Character(playerTexture, new Vector2(forwardDoor.position.X - 32 - 200, worldSize.Y - 200 - 64), bodyTemperature, stamina, staminaLimit, 4, 5);
      // Load the text with respect to the current player's position

      if (!visited)
      {
          AllChatTriggers.Add(new InvisibleChatTriggerBox(new Vector2(forwardDoor.position.X - 200, forwardDoor.position.Y + 30), "Good, you're awake.", false));
          AllChatTriggers.Add(new InvisibleChatTriggerBox(new Vector2(forwardDoor.position.X - 200, forwardDoor.position.Y + 30), "There isn't much time.", false));
          AllChatTriggers.Add(new InvisibleChatTriggerBox(new Vector2(forwardDoor.position.X - 200, forwardDoor.position.Y + 30), "The ship is going down.", false));
          AllChatTriggers.Add(new InvisibleChatTriggerBox(new Vector2(forwardDoor.position.X - 200, forwardDoor.position.Y + 30), "You need to fix it up if you want to live.", false));
          AllChatTriggers.Add(new InvisibleChatTriggerBox(new Vector2(forwardDoor.position.X - 200, forwardDoor.position.Y + 30), "You do want to live, don't you?", false));
          AllChatTriggers.Add(new InvisibleChatTriggerBox(new Vector2(forwardDoor.position.X - 200, forwardDoor.position.Y + 30), "I hear space death isn't very pleasant, though.", false));
          AllChatTriggers.Add(new InvisibleChatTriggerBox(new Vector2(forwardDoor.position.X - 200, forwardDoor.position.Y + 30), "Up to you.", false));
          AllChatTriggers.Add(new InvisibleChatTriggerBox(new Vector2(forwardDoor.position.X - 10, forwardDoor.position.Y + 30), "You definitely should pick that lighter up before you get out of here.", false));
      }


      Texture2D lighterTexture = Content.Load<Texture2D>("lighter");
      if (!visited)
      {
          lighter = new PickUpItem(lighterTexture, new Vector2(forwardDoor.position.X - 32 - 260, forwardDoor.position.Y + 55), new Vector2(lighterTexture.Width, lighterTexture.Height), PickUpItem.ItemType.NONE, 100, PickUpItem.ItemEffectDuration.NONE);
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
        if (chatTrigger.IsPersisted())
          chatTrigger.Update(gameTime);
        if (!chatTrigger.IsConsumed() 
            &&chatTrigger.GetHitBox().Intersects(playerNode.getPlayerHitBox()))
          chatTrigger.InteractWith(new Vector2(forwardDoor.position.X - 50, forwardDoor.position.Y), PrisonerGame);
      }

      //update the player position with respect to keyboard input and platform collision
      bool useLighter = false;

      playerNode.Update(useLighter, gameTime, ref bodyTempTimer, ref exhaustionTimer, ref oldKeyboardState, ref jumpTimer, ground, platforms, null, worldSize, ref staminaExhaustionTimer);

      if (playerNode.position.X < 250)
      {
        playerNode.position.X = 250;
      }
      else if (playerNode.position.X > worldSize.X - 250 - 31)
      {
        playerNode.position.X = worldSize.X - 250 - 31;
      }

      if (lighter.position != new Vector2(forwardDoor.position.X - 32 - 260, forwardDoor.position.Y + 55))
      {
          foreach (Portal portal in portals)
          {
              portal.Update(playerNode, ref gameLevel);
          }
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

      if (Game1.DEBUG_MODE)
        foreach (InvisibleChatTriggerBox box in AllChatTriggers)
          spriteBatch.Draw(this.PrisonerGame.DebugTexture, box.GetHitBox(), Color.Pink);

      //draw the desired nodes onto screen through the camera
      foreach (GenericSprite2D element in worldObjects)
          camera.DrawNode(element);

      //draw the fps
      spriteBatch.DrawString(font, framesPerSecond.ToString(), new Vector2(screenSize.X - 50, 25), Color.White);
      //draw the status display and the body temperature
      spriteBatch.Draw(statusDisplayTexture, new Vector2(50, 50), Color.White);
      spriteBatch.DrawString(font, Math.Round(playerNode.bodyTemperature, 2).ToString(), new Vector2(52, 52), Color.Black, 0, new Vector2(0, 0), new Vector2(0.8f, 2), SpriteEffects.None, 0);
      spriteBatch.DrawString(font, Math.Round(playerNode.stamina, 2).ToString(), new Vector2(120, 52), Color.Black, 0, new Vector2(0, 0), new Vector2(1f, 1), SpriteEffects.None, 0);


      // Draw all invisible chat trigger
      if (Game1.DEBUG_MODE)
        foreach (InvisibleChatTriggerBox invisibleTrigger in AllChatTriggers)
          if (!invisibleTrigger.IsConsumed())
            spriteBatch.Draw(PrisonerGame.DebugTexture, invisibleTrigger.GetHitBox(), Color.White);

      spriteBatch.End();
    }
  }
}

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
  public class Level_Prison_Block : GameLevel
  {
    //declare member variables
    public SpriteBatch spriteBatch;
    SpriteFont manaspace12;

    Vector2 worldSize, screenSize;
    float ground;

    Texture2D statusDisplayTexture;

    Character playerNode;
    Filter shadowFilter;
    GenericSprite2D backgroundNode;

    List<GenericSprite2D> worldObjects;

    List<Platform> platforms;
    List<Portal> portals;
    List<Ladder> ladders;
    Portal forwardDoor, backwardDoor;
    Interactable lightSwitch, generator;
    PickUpItem staminaBooster;

    bool filterOn = true, generatorOn = false;

    //declare constructor
    public Level_Prison_Block(Cold_Ship theGameInstance)
      : base(theGameInstance)
    {
      this.spriteBatch = theGameInstance.spriteBatch;
      this.screenSize = new Vector2(theGameInstance.Window.ClientBounds.Right,
          theGameInstance.Window.ClientBounds.Bottom);
      platforms = new List<Platform>();
      portals = new List<Portal>();
      ladders = new List<Ladder>();
      worldObjects = new List<GenericSprite2D>();

    }

    //load content
    public void LoadContent()
    {
      //load the needed textures
      Texture2D backgroundTexture = GameInstance.Content.Load<Texture2D>("Backgrounds\\prisonblock_final");
      statusDisplayTexture = GameInstance.Content.Load<Texture2D>("statusDisplay");
      playerNode = GameInstance.Player;
      playerNode._pocketLight.TurnOff();

      //initialize the world size and the ground coordinate according to the world size
      worldSize = new Vector2(backgroundTexture.Width, backgroundTexture.Height);
      ground = worldSize.Y - 50;

      //initialize the needed nodes and camera
      backgroundNode = new GenericSprite2D(backgroundTexture, new Vector2(0, 0), Rectangle.Empty);
      worldObjects.Add(backgroundNode);
      GameInstance.Camera.cameraPosition = new Vector2(0, worldSize.Y - screenSize.Y);

      //initialize the needed platforms
      Texture2D platformTexture = GameInstance.Content.Load<Texture2D>("Textures\\platformTexture");

      //initialize the platforms and add them to the list
      platforms.Add(new Platform(platformTexture, new Vector2(790, 20), new Vector2(100, worldSize.Y - 280)));
      platforms.Add(new Platform(platformTexture, new Vector2(375, 20), new Vector2(925, worldSize.Y - 280)));
      platforms.Add(new Platform(platformTexture, new Vector2(615, 20), new Vector2(1337, worldSize.Y - 280)));
      platforms.Add(new Platform(platformTexture, new Vector2(375, 20), new Vector2(100, worldSize.Y - 510)));
      platforms.Add(new Platform(platformTexture, new Vector2(1370, 20), new Vector2(514, worldSize.Y - 510)));
      platforms.Add(new Platform(platformTexture, new Vector2(30, 20), new Vector2(1920, worldSize.Y - 510)));
      platforms.Add(new Platform(platformTexture, new Vector2(33, 20), new Vector2(100, worldSize.Y - 744)));
      platforms.Add(new Platform(platformTexture, new Vector2(727, 20), new Vector2(167, worldSize.Y - 744)));
      platforms.Add(new Platform(platformTexture, new Vector2(773, 20), new Vector2(933, worldSize.Y - 744)));
      platforms.Add(new Platform(platformTexture, new Vector2(205, 20), new Vector2(1742, worldSize.Y - 744)));
      //walls
      //platforms.Add(new Platform(platformTexture, new Vector2(130, 270), new Vector2(963, worldSize.Y - 270)));
      //platforms.Add(new Platform(platformTexture, new Vector2(130, 230), new Vector2(1150, worldSize.Y - 508)));

      //initialize ladders and add them to the list
      Texture2D ladderTexture = GameInstance.Content.Load<Texture2D>("Objects\\ladder");
      ladders.Add(new Ladder(ladderTexture, new Vector2(34, 237), new Vector2(890, worldSize.Y - 284)));
      ladders.Add(new Ladder(ladderTexture, new Vector2(34, 237), new Vector2(1301, worldSize.Y - 284)));
      ladders.Add(new Ladder(ladderTexture, new Vector2(34, 237), new Vector2(478, worldSize.Y - 514)));
      ladders.Add(new Ladder(ladderTexture, new Vector2(34, 237), new Vector2(1887, worldSize.Y - 514)));
      ladders.Add(new Ladder(ladderTexture, new Vector2(34, 239), new Vector2(134, worldSize.Y - 749)));
      ladders.Add(new Ladder(ladderTexture, new Vector2(34, 239), new Vector2(898, worldSize.Y - 749)));
      ladders.Add(new Ladder(ladderTexture, new Vector2(34, 239), new Vector2(1707, worldSize.Y - 749)));

      worldObjects.AddRange(platforms);
      worldObjects.AddRange(ladders);

      //initialize the needed portals
      backwardDoor = new Portal(new Vector2(100, worldSize.Y - 72 - 50), new Vector2(51, 72), Portal.PortalType.BACKWARD, GameInstance.Content);
      forwardDoor = new Portal(new Vector2(worldSize.X - 32 - 75, worldSize.Y - 72 - 50), new Vector2(51, 72), Portal.PortalType.FOWARD, GameInstance.Content);
      forwardDoor.canOpen = true;
      portals.Add(backwardDoor);
      portals.Add(forwardDoor);
      worldObjects.AddRange(portals);

      //initialize the playerNode
      if (GameInstance.Player.GetPreviousLevel is Level_Holding_Cell)
      {
        playerNode.Position = new Vector2(backwardDoor.Position.X + backwardDoor.size.X + 5, worldSize.Y - 64 - 50);
      }
      else if (GameInstance.Player.GetPreviousLevel is Level_Generator)
      {
        playerNode.Position = new Vector2(forwardDoor.Position.X - 32 - 5, worldSize.Y - 64 - 50);
      }

      staminaBooster = new PickUpItem(platformTexture
          , new Vector2(280, worldSize.Y - 772), new Vector2(28, 28)
          , PickUpItem.ItemType.STAMINA, 100, PickUpItem.ItemEffectDuration.TEMPORARY, GameInstance);
      lightSwitch = new Interactable(platformTexture, new Vector2(1643, worldSize.Y - 359), new Vector2(31, 43), Interactable.Type_Of_Interactable.LIGHT_SWITCH);
      generator = new Interactable(GameInstance.Content.Load<Texture2D>("Objects\\generator_off")
          , new Vector2(1807, worldSize.Y - 809), new Vector2(104, 65)
          , Interactable.Type_Of_Interactable.GENERATOR
          , GameInstance.Content.Load<Texture2D>("Objects\\generator_on"));

      worldObjects.Add(staminaBooster);
      //worldObjects.Add(lightSwitch);
      worldObjects.Add(generator);

      worldObjects.Add(playerNode);
    }

    //unload contents
    public void Unload()
    {
      platforms.Clear();
      portals.Clear();
      ladders.Clear();
      worldObjects.Clear();

      platforms = new List<Platform>();
      portals = new List<Portal>();
      ladders = new List<Ladder>();
      worldObjects = new List<GenericSprite2D>();
    }

    //update function
    public double Update(GameTime gameTime, ref float bodyTempTimer, ref float exhaustionTimer, ref KeyboardState oldKeyboardState, ref float jumpTimer, ref Game_Level gameLevel, ref float staminaExhaustionTimer, ref double bodyTemperature, ref double stamina, ref double staminaLimit)
    {
      //update the player Position with respect to keyboard input and platform collision
      Vector2 prevPosition = playerNode.Position;
      playerNode.Update(gameTime, ref bodyTempTimer, ref exhaustionTimer, ref oldKeyboardState, ref jumpTimer, ground, platforms, ladders, worldSize, ref staminaExhaustionTimer);

      //Check the player's collision with the world boundaries
      if (playerNode.Position.X < 100 || playerNode.Position.X + playerNode.playerSpriteSize.X > worldSize.X - 100)
      {
        playerNode.Position.X = prevPosition.X;
      }

      //update portals
      foreach (Portal portal in portals)
      {
        portal.Update(playerNode, ref gameLevel);
      }

      lightSwitch.Update(playerNode, ref generatorOn, ref filterOn, shadowFilter, ref forwardDoor.canOpen);
      generator.Update(playerNode, ref generatorOn, ref filterOn, shadowFilter, ref forwardDoor.canOpen);

      staminaBooster.Update(playerNode, ref bodyTemperature, ref stamina, ref staminaLimit);

      //update the shadowFilter's Position with respect to the playerNode

      //update the camera based on the player and world size
      this.Camera.TranslateWithSprite(playerNode, screenSize);
      this.Camera.CapCameraPosition(worldSize, screenSize);

      //return the body temperature
      return playerNode.bodyTemperature;
    }

    //draw funtion
    public void Draw(int framesPerSecond)
    {
      spriteBatch.Begin();
      ////draw the desired nodes onto screen through the camera
      foreach (GenericSprite2D element in worldObjects)
        this.Camera.DrawNode(element);

      SpriteFont font = GameInstance.MonoMedium;
      //draw the fps
      spriteBatch.DrawString(font, framesPerSecond.ToString(), new Vector2(screenSize.X - 50, 25), Color.White);
      spriteBatch.End();
    }
  }
}

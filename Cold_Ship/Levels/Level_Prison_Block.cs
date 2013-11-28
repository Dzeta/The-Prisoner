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
  public class Level_Prison_Block
  {
    //declare member variables
    public SpriteBatch spriteBatch;
    SpriteFont manaspace12;

    Vector2 worldSize, screenSize;
    float ground;

    Texture2D statusDisplayTexture;

    Camera2D camera;
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

    public List<InvisibleChatTriggerBox> AllChatTriggers;
    bool visited = false;

    Cold_Ship GameInstance;

    //declare constructor
    public Level_Prison_Block(Cold_Ship gameInstance, SpriteBatch spriteBatch, Vector2 screenSize)
    {
      this.spriteBatch = spriteBatch;
      this.GameInstance = gameInstance;
      platforms = new List<Platform>();
      this.screenSize = screenSize;
      portals = new List<Portal>();
      ladders = new List<Ladder>();
      worldObjects = new List<GenericSprite2D>();

      this.AllChatTriggers = new List<InvisibleChatTriggerBox>();
    }

    //load content
    public void LoadContent(ContentManager Content, Game_Level gameLevel, Game_Level prevGameLevel, double bodyTemperature, double stamina, double staminaLimit)
    {
      //load the needed textures
      Texture2D playerTexture = Content.Load<Texture2D>("Character\\PlayerSpriteSheet");
      Texture2D backgroundTexture = Content.Load<Texture2D>("Backgrounds\\prisonblock_bg_02");
      statusDisplayTexture = Content.Load<Texture2D>("statusDisplay");


      //initialize the world size and the ground coordinate according to the world size
      worldSize = new Vector2(backgroundTexture.Width, backgroundTexture.Height);
      ground = worldSize.Y - 50;

      //load manaspace12
      manaspace12 = Content.Load<SpriteFont>("Fonts\\manaspace12");

      //initialize the needed nodes and camera
      backgroundNode = new GenericSprite2D(backgroundTexture, new Vector2(0, 0), Rectangle.Empty);
      worldObjects.Add(backgroundNode);
      camera = new Camera2D(spriteBatch);
      camera.cameraPosition = new Vector2(0, worldSize.Y - screenSize.Y);

      //initialize the needed platforms
      Texture2D platformTexture = Content.Load<Texture2D>("Textures\\platformTexture");

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
      Texture2D barrierTexture = Content.Load<Texture2D>("Objects\\barrier");
      platforms.Add(new Platform(barrierTexture, new Vector2(64, 196), new Vector2(963, worldSize.Y - 50 - 196)));
      platforms.Add(new Platform(barrierTexture, new Vector2(64, 196), new Vector2(1150, worldSize.Y - 508 + 31)));

      //initialize ladders and add them to the list
      Texture2D ladderTexture = Content.Load<Texture2D>("Objects\\ladder");
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
      backwardDoor = new Portal(new Vector2(100, worldSize.Y - 72 - 50), new Vector2(51, 72), Portal.PortalType.BACKWARD, Content);
      forwardDoor = new Portal(new Vector2(worldSize.X - 32 - 75, worldSize.Y - 72 - 50), new Vector2(51, 72), Portal.PortalType.FOWARD, Content);
      forwardDoor.canOpen = true;
      portals.Add(backwardDoor);
      portals.Add(forwardDoor);
      worldObjects.AddRange(portals);

      //initialize the playerNode
      if (prevGameLevel <= gameLevel)
      {
        playerNode = new Character(playerTexture, new Vector2(backwardDoor.Position.X + backwardDoor.size.X + 5, worldSize.Y - 64 - 50), bodyTemperature, stamina, staminaLimit, 4, 6);
      }
      else if (prevGameLevel >= gameLevel)
      {
        playerNode = new Character(playerTexture, new Vector2(forwardDoor.Position.X - 32 - 5, worldSize.Y - 64 - 50), bodyTemperature, stamina, staminaLimit, 4, 6);
      }

      staminaBooster = new PickUpItem(Content.Load<Texture2D>("Objects\\lunchbox"), new Vector2(280, worldSize.Y - 772), new Vector2(28, 28), PickUpItem.ItemType.STAMINA, 100, PickUpItem.ItemEffectDuration.TEMPORARY);
      lightSwitch = new Interactable(Content.Load<Texture2D>("Objects\\lightswitch_off"), new Vector2(1643, worldSize.Y - 357), new Vector2(23, 32), Interactable.Type_Of_Interactable.LIGHT_SWITCH, Content.Load<Texture2D>("Objects\\lightswitch_on"));
      generator = new Interactable(Content.Load<Texture2D>("Objects\\generator_off"), new Vector2(1807, worldSize.Y - 809), new Vector2(104, 65), Interactable.Type_Of_Interactable.GENERATOR, Content.Load<Texture2D>("Objects\\generator_on"));

      if (!visited)
      {
        AddChatTriggers();
      }

      worldObjects.Add(staminaBooster);
      worldObjects.Add(lightSwitch);
      worldObjects.Add(generator);

      worldObjects.Add(playerNode);

      //Initiate the shadow filter
      shadowFilter = new Filter(Content.Load<Texture2D>("shadowFilterLarge"), new Vector2(0, 0));

    }

    private void AddChatTriggers()
    {
      AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(new Vector2(430, 930), StringDialogue.prisonBlockStartingSpeech1));
      AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(new Vector2(430, 930), StringDialogue.prisonBlockStartingSpeech2));

      AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(new Vector2(1680, 245), StringDialogue.prisonBlockGeneratorSpeech1));
      AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(new Vector2(1680, 245), StringDialogue.prisonBlockGeneratorSpeech2));

      AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(new Vector2(1820, 940), StringDialogue.prisonBlockLeavingRoom1, this.lightSwitch.isNotActivated, 1));
      AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(new Vector2(1820, 940), StringDialogue.prisonBlockLeavingRoom2, this.lightSwitch.isNotActivated, 1));
      AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(new Vector2(1820, 940), StringDialogue.prisonBlockLeavingRoom3, this.lightSwitch.isNotActivated, 1));
      AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(new Vector2(1820, 940), StringDialogue.prisonBlockLeavingRoom4, this.lightSwitch.isNotActivated, 1));
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
      // Update Dialogues
      for (int i = 0; i < AllChatTriggers.Count;i++ )
      {
        InvisibleChatTriggerBox chatTrigger = AllChatTriggers.ElementAt(i);
        Vector2 intercomPosition = Vector2.Zero;
        if (i < 2)
          intercomPosition = camera.ApplyTransformations(new Vector2(480, 900));
        else if (i < 4)
          intercomPosition = camera.ApplyTransformations(new Vector2(1765, 205));
        else
          intercomPosition = camera.ApplyTransformations(new Vector2(1885, 895));

        chatTrigger.Update(gameTime);
        if (!chatTrigger.IsConsumed()
            && chatTrigger.GetHitBox().Intersects(playerNode.getPlayerHitBox()))
          chatTrigger.InteractWith(intercomPosition, GameInstance);
      }

      //update the player Position with respect to keyboard input and platform collision
      Vector2 prevPosition = playerNode.Position;
      bool useLighter = filterOn;
      playerNode.Update(useLighter, gameTime, ref bodyTempTimer, ref exhaustionTimer, ref oldKeyboardState, ref jumpTimer, ground, platforms, ladders, worldSize, ref staminaExhaustionTimer);

      //Check the player's collision with the world boundaries
      if (playerNode.Position.X < 100 || playerNode.Position.X + playerNode.playerSpriteSize.X > worldSize.X - 100)
      {
        playerNode.Position.X = prevPosition.X;
      }

      //update portals
      //foreach (Portal portal in portals)
      //{
      //  portal.Update(playerNode, ref gameLevel);
      //}
      forwardDoor.Update(playerNode, ref gameLevel, -20);
      backwardDoor.Update(playerNode, ref gameLevel, 20);

      lightSwitch.Update(playerNode, ref generatorOn, ref filterOn, shadowFilter, ref forwardDoor.canOpen);
      generator.Update(playerNode, ref generatorOn, ref filterOn, shadowFilter, ref forwardDoor.canOpen);

      staminaBooster.Update(ref playerNode, ref bodyTemperature, ref stamina, ref staminaLimit);

      //update the shadowFilter's Position with respect to the playerNode
      shadowFilter.Position = new Vector2((playerNode.Position.X /*+ (playerNode.Texture.Width / 2))*/) - (shadowFilter.Texture.Width / 2),
          (playerNode.Position.Y + (playerNode.playerSpriteSize.Y / 2) - (shadowFilter.Texture.Height / 2)));

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
      ////draw the desired nodes onto screen through the camera
      foreach (GenericSprite2D element in worldObjects)
        camera.DrawNode(element);

       //Draw the showdow filter
      if (filterOn)
      {
          camera.DrawNode(shadowFilter);
      }

      //draw the fps
      spriteBatch.DrawString(manaspace12, framesPerSecond.ToString(), new Vector2(screenSize.X - 50, 25), Color.White);
      //draw the status display and the body temperature
      spriteBatch.Draw(statusDisplayTexture, new Vector2(50, 50), Color.White);
      spriteBatch.DrawString(manaspace12, Math.Round(playerNode.bodyTemperature, 2).ToString(), new Vector2(52, 52), Color.Black, 0, new Vector2(0, 0), new Vector2(0.8f, 2), SpriteEffects.None, 0);
      spriteBatch.DrawString(manaspace12, Math.Round(playerNode.stamina, 2).ToString(), new Vector2(120, 52), Color.Black, 0, new Vector2(0, 0), new Vector2(1f, 1), SpriteEffects.None, 0);

      // Draw all invisible chat trigger
      if (Cold_Ship.DEBUG_MODE)
        foreach (InvisibleChatTriggerBox invisibleTrigger in AllChatTriggers)
          if (!invisibleTrigger.IsConsumed())
            spriteBatch.Draw(GameInstance.DebugTexture, invisibleTrigger.GetHitBox(), Color.White);

      spriteBatch.End();

    }
  }
}

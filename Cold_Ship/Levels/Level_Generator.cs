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
using Microsoft.Xna.Framework.Audio;

namespace Cold_Ship
{
    public class Level_Generator
    {
        //declare member variables
        public SpriteBatch spriteBatch;
        SpriteFont font;

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
        Portal forwardDoor, backwardDoor;
        Interactable lightSwitch, generator, doorSwitch;
        PickUpItem staminaBooster;
        Reactor reactor;

        GenericSprite2D intercom1, intercom2, intercom3;

        bool filterOn = true, generatorOn = false;

        public List<InvisibleChatTriggerBox> AllChatTriggers;
        bool visited = false;

        Cold_Ship GameInstance;
        Cue reactorSound;

        //Computer and screen
        Computer_And_Screen computer;

        //declare constructor
        public Level_Generator(Cold_Ship gameInstance, SpriteBatch spriteBatch, Vector2 screenSize)
        {
          this.GameInstance = gameInstance;
            this.spriteBatch = spriteBatch;
            platforms = new List<Platform>();
            this.screenSize = screenSize;
            portals = new List<Portal>();
            worldObjects = new List<GenericSprite2D>();

            this.AllChatTriggers = new List<InvisibleChatTriggerBox>();
        }

        //load content
        public void LoadContent(ContentManager Content, Game_Level gameLevel, Game_Level prevGameLevel,
                                double bodyTemperature, double stamina, double staminaLimit)
        {
            //load the needed textures
            Texture2D playerTexture = Content.Load<Texture2D>("Character\\PlayerSpriteSheet");
            Texture2D backgroundTexture = Content.Load<Texture2D>("Backgrounds\\engineroom_bg");
            statusDisplayTexture = Content.Load<Texture2D>("statusDisplay");

            //initialize the world size and the ground coordinate according to the world size
            worldSize = new Vector2(backgroundTexture.Width, backgroundTexture.Height);
            ground = worldSize.Y - 50;

            //load font
            font = Content.Load<SpriteFont>("Fonts\\Score");

            //initialize the needed nodes and camera
            //playerNode = new Scene2DNode(playerTexture, new Vector2(35, worldSize.Y - 64));
            backgroundNode = new GenericSprite2D(backgroundTexture, new Vector2(0, 0), Rectangle.Empty);
            worldObjects.Add(backgroundNode);
            shadowFilter = new Filter(Content.Load<Texture2D>("Textures/radius_of_light"), new Vector2(0, 0));
            camera = new Camera2D(spriteBatch);
            camera.cameraPosition = new Vector2(0, worldSize.Y - screenSize.Y);

            reactor = new Reactor(Content, new Vector2(578, 472));
            worldObjects.Add(reactor);

            Texture2D intercomTexture = Content.Load<Texture2D>("Objects\\intercom");
          intercom1 = new GenericSprite2D(intercomTexture, new Vector2(430, 120));
          intercom2 = new GenericSprite2D(intercomTexture, new Vector2(1600, 1900));
          intercom3 = new GenericSprite2D(intercomTexture, new Vector2(1660, 120));

          worldObjects.Add(intercom1);
          worldObjects.Add(intercom2);
          worldObjects.Add(intercom3);

            //initialize the needed platforms
            Texture2D platformTexture = Content.Load<Texture2D>("Textures\\platformTexture");

            //initialize the platforms and add them to the list
            createPlatforms(platformTexture);
            worldObjects.AddRange(platforms);

            //initialize the needed portals
            backwardDoor = new Portal(new Vector2(150, 108), new Vector2(51, 72), Portal.PortalType.BACKWARD, Content);
            forwardDoor = new Portal(new Vector2(worldSize.X - 201, 108), new Vector2(51, 72), Portal.PortalType.FOWARD, Content);
            portals.Add(backwardDoor);
            portals.Add(forwardDoor);
            worldObjects.AddRange(portals);

            //initialize the playerNode
            if (prevGameLevel <= gameLevel)
            {
              playerNode = new Character(GameInstance, playerTexture, new Vector2(backwardDoor.Position.X + backwardDoor.size.X + 5, 116), bodyTemperature, stamina, staminaLimit, 4, 6);
            }
            else if (prevGameLevel >= gameLevel)
            {
                playerNode = new Character(GameInstance, playerTexture, new Vector2(forwardDoor.Position.X - 32 - 35, 116), bodyTemperature, stamina, staminaLimit, 4, 6);
            }

            staminaBooster = new PickUpItem(Content.Load<Texture2D>("Objects\\thermos"), new Vector2(1175, 170), new Vector2(18, 27), PickUpItem.ItemType.STAMINA, 100, PickUpItem.ItemEffectDuration.PERMANENT);
            lightSwitch = new Interactable(Content.Load<Texture2D>("Objects\\lightswitch_off"), new Vector2(130, 1330), new Vector2(23, 32), Interactable.Type_Of_Interactable.LIGHT_SWITCH, Content.Load<Texture2D>("Objects\\lightswitch_on"));
            generator = new Interactable(Content.Load<Texture2D>("Objects\\generator_off"), new Vector2(worldSize.X - 246, ground - 63), new Vector2(103, 63), Interactable.Type_Of_Interactable.GENERATOR, Content.Load<Texture2D>("Objects\\generator_on"));
            doorSwitch = new Interactable(Content.Load<Texture2D>("Objects\\doorswitch_off"), new Vector2(1920, 910), new Vector2(11, 19), Interactable.Type_Of_Interactable.DOOR_SWITCH, Content.Load<Texture2D>("Objects\\doorswitch_on"));
            
            //Initialize computer
            computer = new Computer_And_Screen(Content, new Vector2(worldSize.X - 406, worldSize.Y - 141));

            if (!visited)
            {
              AddChatTriggers();
            }
            
            worldObjects.Add(staminaBooster);
            worldObjects.Add(lightSwitch);
            worldObjects.Add(generator);
            worldObjects.Add(doorSwitch);
            worldObjects.Add(computer.computerNode);
            worldObjects.Add(computer);
            worldObjects.Add(playerNode);

            reactorSound = Sounds.soundBank.GetCue("sound_reactor");
   
        }

        private void AddChatTriggers()
        {
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(intercom1.Position - new Vector2(30, 20), StringDialogue.generatorRoomStartingSpeech1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(intercom1.Position - new Vector2(30, 20), StringDialogue.generatorRoomStartingSpeech2));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(intercom1.Position - new Vector2(30, 20), StringDialogue.generatorRoomStartingSpeech3));

          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(20, 20), StringDialogue.generatorRoomComputerActivityLog1,
                                                                    this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(20, 20), StringDialogue.generatorRoomComputerActivityLog2,
                                                                    this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(20, 20), StringDialogue.generatorRoomComputerActivityLog3,
                                                                    this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(20, 20), StringDialogue.generatorRoomComputerActivityLog4,
                                                                    this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(20, 20), StringDialogue.generatorRoomComputerActivityLog5,
                                                                    this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(20, 20), StringDialogue.generatorRoomComputerActivityLog6,
                                                                    this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(20, 20), StringDialogue.generatorRoomComputerActivityLog7,
                                                                    this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(20, 20), StringDialogue.generatorRoomComputerActivityLog8,
                                                                    this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(20, 20), StringDialogue.generatorRoomComputerActivityLog9,
                                                                    this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(20, 20), StringDialogue.generatorRoomComputerActivityLog10,
                                                                    this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(20, 20), StringDialogue.generatorRoomComputerActivityLog11,
                                                                    this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(20, 20), StringDialogue.generatorRoomComputerActivityLog12,
                                                                    this.generator.isNotActivated, 5000.0f));

          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(20, 20), StringDialogue.generatorRoomErrorCodeReaction,
                                                                    this.generator.isNotActivated, 1));

          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(intercom3.Position - new Vector2(30, 20), StringDialogue.generatorRoomLeavingRoom1,
                                                                    this.forwardDoor.isClosed, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(intercom3.Position - new Vector2(30, 20), StringDialogue.generatorRoomLeavingRoom2,
                                                                    this.forwardDoor.isClosed, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(intercom3.Position - new Vector2(30, 20), StringDialogue.generatorRoomLeavingRoom3,
                                                                    this.forwardDoor.isClosed, 1));
        }

        private void createPlatforms(Texture2D platformTexture)
        {
            Vector2 sizeBasePlatform = new Vector2(120, 20);
            Vector2 sizeSmallPlatform = new Vector2(50, 20);
            platforms.Add(new Platform(platformTexture, new Vector2(420, 20), new Vector2(100, 180)));
            platforms.Add(new Platform(platformTexture, new Vector2(420, 20), new Vector2(1520, 180)));

            platforms.Add(new MovingPlatform(platformTexture, sizeBasePlatform, new Vector2(545, 200), new Vector2(0, 0.5f), new Vector2(0, 270)));
            platforms.Add(new MovingPlatform(platformTexture, sizeBasePlatform, new Vector2(255, 485), new Vector2(0.5f, 0), new Vector2(135, 0)));
            platforms.Add(new MovingPlatform(platformTexture, sizeBasePlatform, new Vector2(120, 555), new Vector2(0, 0.5f), new Vector2(0, 135)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(300, 665)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(430, 755)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(300, 855)));
            platforms.Add(new Platform(platformTexture, sizeSmallPlatform, new Vector2(200, 880)));
            platforms.Add(new Platform(platformTexture, new Vector2(30, 20), new Vector2(100, 980)));
            platforms.Add(new Platform(platformTexture, sizeSmallPlatform, new Vector2(100, 1020)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(300, 1040)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(160, 1140)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(320, 1225)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(415, 1330)));
            platforms.Add(new MovingPlatform(platformTexture, sizeBasePlatform, new Vector2(115, 1400), new Vector2(0.3f, 0), new Vector2(160, 0)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(300, 1490)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(435, 1590)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(250, 1675)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(300, 1770)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(190, 1860)));
            platforms.Add(new Platform(platformTexture, new Vector2(30, 20), new Vector2(100, 1940)));
            platforms.Add(new MovingPlatform(platformTexture, sizeBasePlatform, new Vector2(450, 1850), new Vector2(0.65f, 0), new Vector2(600, 0)));
            platforms.Add(new MovingPlatform(platformTexture, sizeBasePlatform, new Vector2(1225, 1850), new Vector2(0.65f, 0), new Vector2(300, 0)));

            platforms.Add(new MovingPlatform(platformTexture, new Vector2(30, 20), new Vector2(1915, 1830), new Vector2(0, 0.5f), new Vector2(0, 80)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1750, 1780)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1600, 1710)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1510, 1610)));
            platforms.Add(new MovingPlatform(platformTexture, sizeBasePlatform, new Vector2(1630, 1550), new Vector2(0.5f, 0), new Vector2(150, 0)));
            platforms.Add(new MovingPlatform(platformTexture, new Vector2(30, 20), new Vector2(1915, 1355), new Vector2(0, 0.5f), new Vector2(0, 150)));
            platforms.Add(new MovingPlatform(platformTexture, sizeBasePlatform, new Vector2(1720, 1320), new Vector2(0, 0.5f), new Vector2(0, 50)));
            platforms.Add(new Platform(platformTexture, new Vector2(60, 20), new Vector2(1885, 1225)));
            platforms.Add(new MovingPlatform(platformTexture, new Vector2(30, 20), new Vector2(1915, 950), new Vector2(0, 0.5f), new Vector2(0, 200)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1570, 1280)));
            platforms.Add(new MovingPlatform(platformTexture, sizeSmallPlatform, new Vector2(1480, 1150), new Vector2(0, 0.5f), new Vector2(0, 100)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1560, 1100)));
            platforms.Add(new MovingPlatform(platformTexture, sizeSmallPlatform, new Vector2(1480, 950), new Vector2(0, 0.5f), new Vector2(0, 100)));
            platforms.Add(new Platform(platformTexture, sizeSmallPlatform, new Vector2(1560, 900)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1700, 840)));
            platforms.Add(new Platform(platformTexture, sizeSmallPlatform, new Vector2(1850, 750)));
            platforms.Add(new MovingPlatform(platformTexture, sizeSmallPlatform, new Vector2(1680, 680), new Vector2(0.5f, 0), new Vector2(100, 0)));
            platforms.Add(new MovingPlatform(platformTexture, sizeSmallPlatform, new Vector2(1600, 420), new Vector2(0, 0.5f), new Vector2(0, 200)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1450, 440)));
            platforms.Add(new Platform(platformTexture, sizeSmallPlatform, new Vector2(1380, 380)));
            platforms.Add(new MovingPlatform(platformTexture, sizeSmallPlatform, new Vector2(1450, 180), new Vector2(0, 0.5f), new Vector2(0, 150)));
            platforms.Add(new Platform(platformTexture, sizeSmallPlatform, new Vector2(1230, 180)));
            platforms.Add(new Platform(platformTexture, sizeSmallPlatform, new Vector2(1170, 220)));
        }

        //unload contents
        public void Unload()
        {
            platforms.Clear();
            portals.Clear();
            worldObjects.Clear();

            platforms = new List<Platform>();
            portals = new List<Portal>();
            worldObjects = new List<GenericSprite2D>();
        }

        //update function
        public double Update(GameTime gameTime, ref float bodyTempTimer, ref float exhaustionTimer,
                             ref KeyboardState oldKeyboardState, ref float jumpTimer,
                             ref Game_Level gameLevel, ref float staminaExhaustionTimer,
                             ref double bodyTemperature, ref double stamina, ref double staminaLimit)
        {
          if (!reactorSound.IsPlaying)
            reactorSound.Play();

          // Update Dialogues
          for (int i = 0; i < AllChatTriggers.Count; i++)
          {
            InvisibleChatTriggerBox chatTrigger = AllChatTriggers.ElementAt(i);
            Vector2 intercomPosition = Vector2.Zero;
            if (i < 3)
              intercomPosition = camera.ApplyTransformations(intercom1.Position);
            else if (i < 15)
              intercomPosition = camera.ApplyTransformations(computer.Position);
            else if (i < 16)
              intercomPosition = camera.ApplyTransformations(intercom2.Position);
            else
              intercomPosition = camera.ApplyTransformations(intercom3.Position);

            chatTrigger.Update(gameTime);
            if (!chatTrigger.IsConsumed()
                && chatTrigger.GetHitBox().Intersects(playerNode.getPlayerHitBox()))
              chatTrigger.InteractWith(intercomPosition, GameInstance);
          }

            //update the player Position with respect to keyboard input and platform collision
            Vector2 prevPosition = playerNode.Position;
            bool useLighter = filterOn;
            playerNode.Update(useLighter, gameTime, ref bodyTempTimer, ref exhaustionTimer, ref oldKeyboardState, ref jumpTimer, ground, platforms, null, worldSize, ref staminaExhaustionTimer);
            reactor.Update(gameTime);

            //Check the player's collision with the world boundaries
            if (playerNode.Position.X < 100 || playerNode.Position.X + playerNode.playerSpriteSize.X > worldSize.X - 100)
            {
                playerNode.Position.X = prevPosition.X;
            }

            lightSwitch.Update(playerNode, ref generatorOn, ref filterOn, shadowFilter, ref forwardDoor.canOpen);
            generator.Update(playerNode, ref generatorOn, ref filterOn, shadowFilter, ref forwardDoor.canOpen);
            doorSwitch.Update(playerNode, ref generatorOn, ref filterOn, shadowFilter, ref forwardDoor.canOpen);
            staminaBooster.Update(ref playerNode, ref bodyTemperature, ref stamina, ref staminaLimit);

            //update portals
            //foreach (Portal portal in portals)
            //{
            //    portal.Update(playerNode, ref gameLevel);
            //}
            forwardDoor.Update(playerNode, ref gameLevel, -20, 70);
            backwardDoor.Update(playerNode, ref gameLevel, 20);

            //update the shadowFilter's Position with respect to the playerNode
          shadowFilter.Update(gameTime);
            shadowFilter.Position = new Vector2((playerNode.Position.X /*+ (playerNode.Texture.Width / 2))*/) - (shadowFilter.Texture.Width / 2),
                (playerNode.Position.Y + (playerNode.playerSpriteSize.Y / 2) - (shadowFilter.Texture.Height / 2)));


            //update the camera based on the player and world size
            camera.TranslateWithSprite(playerNode, screenSize);
            camera.CapCameraPosition(worldSize, screenSize);

            //update the computer screen
            if (generatorOn)
            {
                computer.Update(gameTime);
            }

            //return the body temperature
            return playerNode.bodyTemperature;
        }

        //draw funtion
        public void Draw(int framesPerSecond)
        {
            spriteBatch.Begin();
            
            foreach (GenericSprite2D element in worldObjects)
                camera.DrawNode(element);

            if (filterOn)
            {
                camera.DrawNode(shadowFilter);
            }
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
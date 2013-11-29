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
    public class Level_Entertainment_Room
    {
        //declare member variables
        public SpriteBatch spriteBatch;
        SpriteFont font;

        Vector2 worldSize, screenSize;
        float ground;

        Camera2D camera;
        Character playerNode;
        Filter shadowFilter;
        GenericSprite2D backgroundNode;

        List<GenericSprite2D> worldObjects;

        List<Platform> platforms;
        List<Portal> portals;
        List<Ladder> ladders;
        Portal forwardDoor, backwardDoor;
        Interactable lightSwitch, generator, puzzleSwitch1, puzzleSwitch2, puzzleSwitch3, puzzleSwitch4, puzzleSwitch5;
        PickUpItem staminaBooster;

        List<Platform> walls = new List<Platform>();

        GenericSprite2D intercom1, intercom2, intercom3;

        bool filterOn = true, generatorOn = false, doorCanOpen = false;

        //List of switches
        List<Interactable> switchPuzzle;

        public List<InvisibleChatTriggerBox> AllChatTriggers;
        bool visited = false;

        Cold_Ship GameInstance;

        //Computer and screen
        Computer_And_Screen computer;

        //declare constructor
        public Level_Entertainment_Room(Cold_Ship gameInstance, SpriteBatch spriteBatch, Vector2 screenSize)
        {
          this.GameInstance = gameInstance;
            this.spriteBatch = spriteBatch;
            platforms = new List<Platform>();
            this.screenSize = screenSize;
            portals = new List<Portal>();
            ladders = new List<Ladder>();
            worldObjects = new List<GenericSprite2D>();
            switchPuzzle = new List<Interactable>();

            this.AllChatTriggers = new List<InvisibleChatTriggerBox>();
        }

        //load content
        public void LoadContent(ContentManager Content, Game_Level gameLevel, Game_Level prevGameLevel, double bodyTemperature, double stamina, double staminaLimit)
        {
            //load the needed textures
            Texture2D playerTexture = Content.Load<Texture2D>("Character\\PlayerSpriteSheet");
            Texture2D backgroundTexture = Content.Load<Texture2D>("Backgrounds\\entertainmentroom_background");

            //initialize the world size and the ground coordinate according to the world size
            worldSize = new Vector2(backgroundTexture.Width, backgroundTexture.Height);
            ground = worldSize.Y - 50;

            //load font
            font = Content.Load<SpriteFont>("Fonts\\manaspace12");

            //initialize the needed nodes and camera
            backgroundNode = new GenericSprite2D(backgroundTexture, new Vector2(0, 0), Rectangle.Empty);
            worldObjects.Add(backgroundNode);

            shadowFilter = new Filter(Content.Load<Texture2D>("Textures/radius_of_light"), new Vector2(0, 0));

            camera = new Camera2D(spriteBatch);
            camera.cameraPosition = new Vector2(0, worldSize.Y - screenSize.Y);

            Texture2D intercomTexture = Content.Load<Texture2D>("Objects\\intercom");
            intercom1 = new GenericSprite2D(intercomTexture, new Vector2(580, worldSize.Y - 140));
            intercom2 = new GenericSprite2D(intercomTexture, new Vector2(worldSize.X - 600, worldSize.Y - 900));
            intercom3 = new GenericSprite2D(intercomTexture, new Vector2(worldSize.X - 770, worldSize.Y - 830));

            worldObjects.Add(intercom1);
            worldObjects.Add(intercom2);
            worldObjects.Add(intercom3);

            //initialize the needed platforms
            Texture2D platformTexture = Content.Load<Texture2D>("Textures\\platformTexture");

            //initialize the platforms and add them to the list
            //Second floor (first floor ceiling)
            platforms.Add(new Platform(platformTexture, new Vector2(110, 20), new Vector2(100, worldSize.Y - 280)));
            platforms.Add(new Platform(platformTexture, new Vector2(675, 20), new Vector2(worldSize.X - 676 - 100, worldSize.Y - 280)));
            // Puzzle platforms
            createPuzzlPlatforms(platformTexture);
            //Third floor (second floor ceiling)
            platforms.Add(new Platform(platformTexture, new Vector2(680, 23), new Vector2(worldSize.X - 923, worldSize.Y - 748)));
            platforms.Add(new Platform(platformTexture, new Vector2(110, 25), new Vector2(worldSize.X - 204, worldSize.Y - 749)));

            //initialize ladders and add them to the list
            //Load ladder Texture
            Texture2D ladderTexture = Content.Load<Texture2D>("Objects\\ladder");
            //First floor
            ladders.Add(new Ladder(ladderTexture, new Vector2(34, 235), new Vector2(100 + 113, worldSize.Y - 285)));
            //Second floor
            ladders.Add(new Ladder(ladderTexture, new Vector2(34, 120), new Vector2(worldSize.X - 243, worldSize.Y - 753)));

            worldObjects.AddRange(platforms);
            worldObjects.AddRange(ladders);

            //initialize the needed portals
            backwardDoor = new Portal(new Vector2(worldSize.X - 40 - 64, worldSize.Y - 50 - 72), new Vector2(51, 72), Portal.PortalType.BACKWARD, Content);
            forwardDoor = new Portal(new Vector2(worldSize.X - 845, worldSize.Y - 822), new Vector2(51, 72), Portal.PortalType.FOWARD, Content);
            portals.Add(backwardDoor);
            portals.Add(forwardDoor);
            worldObjects.AddRange(portals);

            //initialize the playerNode
            if (prevGameLevel <= gameLevel)
            {
              playerNode = new Character(GameInstance, playerTexture, new Vector2(backwardDoor.Position.X - backwardDoor.size.X - 15, worldSize.Y - 64 - 50), bodyTemperature, stamina, staminaLimit, 4, 6);
            }
            else if (prevGameLevel >= gameLevel)
            {
              playerNode = new Character(GameInstance, playerTexture, new Vector2(forwardDoor.Position.X - 32 - 5, worldSize.Y - 64 - 50), bodyTemperature, stamina, staminaLimit, 4, 6);
            }

            //Pickup item
            staminaBooster = new PickUpItem(platformTexture, new Vector2(worldSize.X - 1413, worldSize.Y - 111), new Vector2(28, 28), PickUpItem.ItemType.STAMINA, 100, PickUpItem.ItemEffectDuration.TEMPORARY);

            //initiate the switchPuzzle
            puzzleSwitch1 = new Interactable(Content.Load<Texture2D>("Objects\\doorswitch_off"), new Vector2(400, worldSize.Y - 116), new Vector2(11, 19), Interactable.Type_Of_Interactable.PUZZLE_SWITCH, Content.Load<Texture2D>("Objects\\doorswitch_on"));
            puzzleSwitch2 = new Interactable(Content.Load<Texture2D>("Objects\\doorswitch_off"), new Vector2(440, worldSize.Y - 116), new Vector2(11, 19), Interactable.Type_Of_Interactable.PUZZLE_SWITCH, Content.Load<Texture2D>("Objects\\doorswitch_on"));
            puzzleSwitch3 = new Interactable(Content.Load<Texture2D>("Objects\\doorswitch_off"), new Vector2(480, worldSize.Y - 116), new Vector2(11, 19), Interactable.Type_Of_Interactable.PUZZLE_SWITCH, Content.Load<Texture2D>("Objects\\doorswitch_on"));
            puzzleSwitch4 = new Interactable(Content.Load<Texture2D>("Objects\\doorswitch_off"), new Vector2(520, worldSize.Y - 116), new Vector2(11, 19), Interactable.Type_Of_Interactable.PUZZLE_SWITCH, Content.Load<Texture2D>("Objects\\doorswitch_on"));
            puzzleSwitch5 = new Interactable(Content.Load<Texture2D>("Objects\\doorswitch_off"), new Vector2(560, worldSize.Y - 116), new Vector2(11, 19), Interactable.Type_Of_Interactable.PUZZLE_SWITCH, Content.Load<Texture2D>("Objects\\doorswitch_on"));
            switchPuzzle.Add(puzzleSwitch1);
            switchPuzzle.Add(puzzleSwitch2);
            switchPuzzle.Add(puzzleSwitch3);
            switchPuzzle.Add(puzzleSwitch4);
            switchPuzzle.Add(puzzleSwitch5);
            
            //Generator
            generator = new Interactable(Content.Load<Texture2D>("Objects\\generator_off"), new Vector2(worldSize.X - 550, worldSize.Y - 814), new Vector2(104, 65), Interactable.Type_Of_Interactable.GENERATOR, Content.Load<Texture2D>("Objects\\generator_on"));
            //Door switch
            lightSwitch = new Interactable(Content.Load<Texture2D>("Objects\\lightswitch_off"), new Vector2(130, 330), new Vector2(23, 32), Interactable.Type_Of_Interactable.LIGHT_SWITCH, Content.Load<Texture2D>("Objects\\lightswitch_on"));

            //Initialize the computer and screen
            computer = new Computer_And_Screen(Content, new Vector2(generator.Position.X - 160, generator.Position.Y - 28));

            if (!visited)
            {
              AddChatTriggers();
            }

            worldObjects.Add(staminaBooster);
            worldObjects.Add(generator);
            worldObjects.Add(lightSwitch);
            worldObjects.AddRange(switchPuzzle);

            worldObjects.Add(computer.computerNode);
            worldObjects.Add(computer);

            worldObjects.Add(playerNode);
        }

        private void AddChatTriggers()
        {
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(intercom1.Position + new Vector2(40, 20), StringDialogue.entertainmentRoomSwitchPuzzleHint1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(intercom1.Position + new Vector2(40, 20), StringDialogue.entertainmentRoomSwitchPuzzleHint2));

          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(40, 20), StringDialogue.entertainmentRoomComputerDiary1,
                                                                      this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(40, 20), StringDialogue.entertainmentRoomComputerDiary2,
                                                                      this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(40, 20), StringDialogue.entertainmentRoomComputerDiary3,
                                                                      this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(40, 20), StringDialogue.entertainmentRoomComputerDiary4,
                                                                      this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(40, 20), StringDialogue.entertainmentRoomComputerDiary5,
                                                                      this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(40, 20), StringDialogue.entertainmentRoomComputerDiary6,
                                                                      this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(40, 20), StringDialogue.entertainmentRoomComputerDiary7,
                                                                      this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(40, 20), StringDialogue.entertainmentRoomComputerDiary8,
                                                                      this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(40, 20), StringDialogue.entertainmentRoomComputerDiary9,
                                                                      this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(40, 20), StringDialogue.entertainmentRoomComputerDiary10,
                                                                      this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(40, 20), StringDialogue.entertainmentRoomComputerDiary11,
                                                                      this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(40, 20), StringDialogue.entertainmentRoomComputerDiary12,
                                                                      this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(40, 20), StringDialogue.entertainmentRoomComputerDiary13,
                                                                      this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(40, 20), StringDialogue.entertainmentRoomComputerDiary14,
                                                                      this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(40, 20), StringDialogue.entertainmentRoomComputerDiary15,
                                                                      this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(40, 20), StringDialogue.entertainmentRoomComputerDiary16,
                                                                      this.generator.isNotActivated, 1));
          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(40, 20), StringDialogue.entertainmentRoomComputerDiary17,
                                                                      this.generator.isNotActivated, 5000.0f));

          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(computer.Position + new Vector2(40, 20), StringDialogue.entertainmentRoomErrorCodeReaction,
                                                                      this.generator.isNotActivated, 1));

          AllChatTriggers.Add(InvisibleChatTriggerBox.GetNewInstance(intercom3.Position + new Vector2(20, 20), StringDialogue.entertainmentRoomDoorPuzzleHint,
                                                                      this.forwardDoor.isOpen, 1));
        }

        private void createPuzzlPlatforms(Texture2D platformTexture)
        {
            Vector2 sizeBasePlatform = new Vector2(120, 20);
            Vector2 sizeSmallPlatform = new Vector2(50, 20);
            platforms.Add(new Platform(platformTexture, sizeSmallPlatform, new Vector2(115, 670)));
            platforms.Add(new MovingPlatform(platformTexture, sizeSmallPlatform, new Vector2(160, 590), new Vector2(0.5f, 0), new Vector2(150, 0)));
            platforms.Add(new Platform(platformTexture, sizeSmallPlatform, new Vector2(220, 500)));
            platforms.Add(new MovingPlatform(platformTexture, sizeSmallPlatform, new Vector2(120, 380), new Vector2(0, 0.5f), new Vector2(0, 75)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(420, 580)));
            platforms.Add(new Platform(platformTexture, sizeSmallPlatform, new Vector2(550, 500)));
            platforms.Add(new MovingPlatform(platformTexture, sizeSmallPlatform, new Vector2(600, 550), new Vector2(0.5f, 0), new Vector2(100, 0)));
            platforms.Add(new Platform(platformTexture, sizeSmallPlatform, new Vector2(820, 520)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(700, 390)));
            platforms.Add(new Platform(platformTexture, new Vector2(30, 20), new Vector2(895, 440)));
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


            playerNode.bodyTemperature = bodyTemperature;


          shadowFilter.Update(gameTime);

          // Update Dialogues
          for (int i = 0; i < AllChatTriggers.Count; i++)
          {
            InvisibleChatTriggerBox chatTrigger = AllChatTriggers.ElementAt(i);
            Vector2 intercomPosition = Vector2.Zero;
            if (i < 2)
              intercomPosition = camera.ApplyTransformations(intercom1.Position);
            else if (i < 19)
              intercomPosition = camera.ApplyTransformations(computer.Position);
            else if (i < 20)
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

            puzzleSwitch1.timeCounter++;
            puzzleSwitch2.timeCounter++;
            puzzleSwitch3.timeCounter++;
            puzzleSwitch4.timeCounter++;
            puzzleSwitch5.timeCounter++;
               
            playerNode.Update(filterOn, gameTime, ref bodyTempTimer, ref exhaustionTimer, ref oldKeyboardState, ref jumpTimer, ground, platforms, ladders, worldSize, ref staminaExhaustionTimer);

            //Check the player's collision with the world boundaries
            if (playerNode.Position.X < 100 || playerNode.Position.X + playerNode.playerSpriteSize.X > worldSize.X - 100)
            {
                playerNode.Position.X = prevPosition.X;
            }

            //update portals
            //foreach (Portal portal in portals)
            //{
            //    portal.Update(playerNode, ref gameLevel);
            //}
            forwardDoor.Update(playerNode, ref gameLevel, 20);
            backwardDoor.Update(playerNode, ref gameLevel, -20);

            generator.Update(playerNode, ref generatorOn, ref filterOn, shadowFilter, ref doorCanOpen);
            lightSwitch.Update(playerNode, ref generatorOn, ref filterOn, shadowFilter, ref forwardDoor.canOpen);
            if (generatorOn)
            {
                foreach (Interactable puzzleSwitch in switchPuzzle)
                {
                    puzzleSwitch.Update(playerNode, ref generatorOn, ref filterOn, shadowFilter, ref forwardDoor.canOpen);
                }
                if (puzzleSwitch3.isActivated() && puzzleSwitch5.isActivated())
                    forwardDoor.canOpen = true;
                else
                    forwardDoor.canOpen = false;
            }

            //update the shadowFilter's Position with respect to the playerNode
            shadowFilter.Position = new Vector2((playerNode.Position.X /*+ (playerNode.Texture.Width / 2))*/) - (shadowFilter.Texture.Width / 2),
                (playerNode.Position.Y + (playerNode.playerSpriteSize.Y / 2) - (shadowFilter.Texture.Height / 2)));

            //update the camera based on the player and world size
            camera.TranslateWithSprite(playerNode, screenSize);
            camera.CapCameraPosition(worldSize, screenSize);

            //Update the computer screen
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
            {
                camera.DrawNode(element);
            }

            if (filterOn)
                camera.DrawNode(shadowFilter);

      spriteBatch.DrawString(GameInstance.MonoMedium, Math.Round(playerNode.bodyTemperature, 2).ToString(), new Vector2(52, 52), Color.Red, 0, new Vector2(0, 0), new Vector2(0.8f, 2), SpriteEffects.None, 0);
      spriteBatch.DrawString(GameInstance.MonoMedium, Math.Round(playerNode.stamina, 2).ToString(), new Vector2(120, 52), Color.Yellow, 0, new Vector2(0, 0), new Vector2(1f, 1), SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}

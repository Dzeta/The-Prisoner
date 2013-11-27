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
    public class Level_Common_Room
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
        GenericSprite2D backgroundNode, backgroundFront, backgroundMiddle, backgroundBack;

        List<GenericSprite2D> worldObjects;

        List<Platform> platforms;
        List<Portal> portals;
        List<Ladder> ladders;
        Portal fowardDoor, backwardDoor, roomDoor;
        Interactable lightSwitch, generator, doorSwitch;
        PickUpItem staminaBooster;

        List<GenericSprite2D> openingWindow = new List<GenericSprite2D>();
        List<Platform> walls = new List<Platform>();

        bool filterOn = true, generatorOn = false, doorCanOpen = false;

        //timer and counter for opening window
        float openWindowTimer = 0;
        int windowAnimationCounter = 0;
        //Bool flag to check wether the player is inside the room
        bool insideRoom = false;
        //declare constructor
        public Level_Common_Room(SpriteBatch spriteBatch, Vector2 screenSize)
        {
            this.spriteBatch = spriteBatch;
            platforms = new List<Platform>();
            this.screenSize = screenSize;
            portals = new List<Portal>();
            ladders = new List<Ladder>();
            worldObjects = new List<GenericSprite2D>();

        }

        //load content
        public void LoadContent(ContentManager Content, Game_Level gameLevel, Game_Level prevGameLevel, double bodyTemperature, double stamina, double staminaLimit)
        {
            //load the needed textures
            Texture2D playerTexture = Content.Load<Texture2D>("Character\\PlayerSpriteSheet");
            Texture2D backgroundTexture = Content.Load<Texture2D>("Backgrounds\\messhall_draft");
            Texture2D backgroundFrontTexture = Content.Load<Texture2D>("Backgrounds\\messhall_frontLayer");
            Texture2D backgroundMiddleLayer = Content.Load<Texture2D>("Backgrounds\\messhall_middleLayer");
            Texture2D backgroundBackLayer = Content.Load<Texture2D>("Backgrounds\\messhall_backLayer");
            statusDisplayTexture = Content.Load<Texture2D>("statusDisplay");


            //initialize the world size and the ground coordinate according to the world size
            worldSize = new Vector2(backgroundTexture.Width, backgroundTexture.Height);
            ground = worldSize.Y - 50;

            //load font
            font = Content.Load<SpriteFont>("Fonts\\Score");

            //initialize the needed nodes and camera
            //load the various layers of background node
            backgroundNode = new GenericSprite2D(backgroundTexture, new Vector2(0, 0), Rectangle.Empty);
            backgroundFront = new GenericSprite2D(backgroundFrontTexture, new Vector2(0, 0), Rectangle.Empty);
            backgroundMiddle = new GenericSprite2D(backgroundMiddleLayer, new Vector2(0, 0), Rectangle.Empty);
            backgroundBack = new GenericSprite2D(backgroundBackLayer, new Vector2(0, 0), Rectangle.Empty);
            //worldObjects.Add(backgroundNode);
            shadowFilter = new Filter(Content.Load<Texture2D>("shadowFilterLarge"), new Vector2(0, 0));
            camera = new Camera2D(spriteBatch);
            camera.cameraPosition = new Vector2(0, worldSize.Y - screenSize.Y);

            //initialize the needed platforms
            Texture2D platformTexture = Content.Load<Texture2D>("Textures\\platformTexture");

            //initialize the platforms and add them to the list
            //Second floor (first floor ceiling)
            platforms.Add(new Platform(platformTexture, new Vector2(1339, 20), new Vector2(100, worldSize.Y - 280)));
            platforms.Add(new Platform(platformTexture, new Vector2(473, 20), new Vector2(worldSize.X - 573, worldSize.Y - 280)));
            //platforms.Add(new Platform(platformTexture, new Vector2(619, 20), new Vector2(1345, worldSize.Y - 280)));
            //Third floor (second floor ceiling)
            platforms.Add(new Platform(platformTexture, new Vector2(1125, 20), new Vector2(100, worldSize.Y - 510)));
            platforms.Add(new Platform(platformTexture, new Vector2(683, 20), new Vector2(worldSize.X - 784, worldSize.Y - 510)));
            //platforms.Add(new Platform(platformTexture, new Vector2(50, 20), new Vector2(1920, worldSize.Y - 510)));
            //platforms.Add(new Platform(platformTexture, new Vector2(133, 20), new Vector2(0, worldSize.Y - 744)));
            //platforms.Add(new Platform(platformTexture, new Vector2(727, 20), new Vector2(167, worldSize.Y - 744)));
            //platforms.Add(new Platform(platformTexture, new Vector2(773, 20), new Vector2(933, worldSize.Y - 744)));
            //platforms.Add(new Platform(platformTexture, new Vector2(250, 20), new Vector2(1742, worldSize.Y - 744)));

            //walls
            walls.Add(new Platform(platformTexture, new Vector2(41, 193), new Vector2(worldSize.X - 693, worldSize.Y - 245)));
            walls.Add(new Platform(platformTexture, new Vector2(65, 199), new Vector2(worldSize.X - 353, worldSize.Y - 477)));
            
            

            //initialize ladders and add them to the list
            //Load ladder Texture
            Texture2D ladderTexture = Content.Load<Texture2D>("Objects\\ladder");
            //First floor
            //ladders.Add(new Ladder(ladderTexture, new Vector2(34, 235), new Vector2(890, worldSize.Y - 282)));
            ladders.Add(new Ladder(ladderTexture, new Vector2(34, 239), new Vector2(worldSize.X - 609, worldSize.Y - 286)));
            //Second floor
            ladders.Add(new Ladder(ladderTexture, new Vector2(34, 239), new Vector2(worldSize.X - 823, worldSize.Y - 516)));
            //ladders.Add(new Ladder(ladderTexture, new Vector2(34, 235), new Vector2(1887, worldSize.Y - 512)));
            //ladders.Add(new Ladder(ladderTexture, new Vector2(34, 235), new Vector2(134, worldSize.Y - 747)));
            //ladders.Add(new Ladder(ladderTexture, new Vector2(34, 235), new Vector2(898, worldSize.Y - 749)));
            //ladders.Add(new Ladder(ladderTexture, new Vector2(34, 235), new Vector2(1707, worldSize.Y - 749)));
            worldObjects.AddRange(platforms);
            platforms.AddRange(walls);
            worldObjects.AddRange(ladders);

            //initialize the needed portals
            backwardDoor = new Portal(new Vector2(worldSize.X - 40 - 64, worldSize.Y - 50 - 72), new Vector2(51, 72), Portal.PortalType.BACKWARD, Content);
            fowardDoor = new Portal(new Vector2(120, worldSize.Y - 280 - 72), new Vector2(51, 72), Portal.PortalType.FOWARD, Content);
            roomDoor = new Portal(new Vector2(1185, worldSize.Y - 50 - 72), new Vector2(51, 72), Portal.PortalType.FOWARD, Content);
            portals.Add(backwardDoor);
            portals.Add(fowardDoor);
            worldObjects.AddRange(portals);

            //initialize the playerNode
            if (prevGameLevel <= gameLevel)
            {
                playerNode = new Character(playerTexture, new Vector2(backwardDoor.Position.X - backwardDoor.size.X - 15, worldSize.Y - 64 - 50), bodyTemperature, stamina, staminaLimit, 4, 5);
            }
            else if (prevGameLevel >= gameLevel)
            {
                playerNode = new Character(playerTexture, new Vector2(fowardDoor.Position.X - 32 - 5, worldSize.Y - 64 - 50), bodyTemperature, stamina, staminaLimit, 4, 5);
            }

            //Pickup item
            staminaBooster = new PickUpItem(platformTexture, new Vector2(worldSize.X - 1413, worldSize.Y - 111), new Vector2(28, 28), PickUpItem.ItemType.STAMINA, 100, PickUpItem.ItemEffectDuration.TEMPORARY);
            //Light switch
            lightSwitch = new Interactable(platformTexture, new Vector2(1643, worldSize.Y - 359), new Vector2(31, 43), Interactable.Type_Of_Interactable.LIGHT_SWITCH);
            //Generator
            generator = new Interactable(Content.Load<Texture2D>("Objects\\generator_off"), new Vector2(142, worldSize.Y - 510 - 65), new Vector2(104, 65), Interactable.Type_Of_Interactable.GENERATOR, Content.Load<Texture2D>("Objects\\generator_on"));
            //Door switch
            doorSwitch = new Interactable(Content.Load<Texture2D>("Objects\\doorswitch_off"), new Vector2(worldSize.X - 199, worldSize.Y - 350), new Vector2(11, 19), Interactable.Type_Of_Interactable.DOOR_SWITCH, Content.Load <Texture2D>("Objects\\doorswitch_on"));

            worldObjects.Add(staminaBooster);
            //worldObjects.Add(lightSwitch);
            worldObjects.Add(generator);
            worldObjects.Add(doorSwitch);

            //worldObjects.Add(playerNode);



            //Window opening animation
            openingWindow.Add(new GenericSprite2D(Content.Load<Texture2D>("Objects\\window\\window_01"), new Vector2(worldSize.X - 1513, worldSize.Y - 961), Rectangle.Empty));
            openingWindow.Add(new GenericSprite2D(Content.Load<Texture2D>("Objects\\window\\window_02"), new Vector2(worldSize.X - 1513, worldSize.Y - 961), Rectangle.Empty));
            openingWindow.Add(new GenericSprite2D(Content.Load<Texture2D>("Objects\\window\\window_03"), new Vector2(worldSize.X - 1513, worldSize.Y - 961), Rectangle.Empty));
            openingWindow.Add(new GenericSprite2D(Content.Load<Texture2D>("Objects\\window\\window_04"), new Vector2(worldSize.X - 1513, worldSize.Y - 961), Rectangle.Empty));
            openingWindow.Add(new GenericSprite2D(Content.Load<Texture2D>("Objects\\window\\window_05"), new Vector2(worldSize.X - 1513, worldSize.Y - 961), Rectangle.Empty));
            openingWindow.Add(new GenericSprite2D(Content.Load<Texture2D>("Objects\\window\\window_06"), new Vector2(worldSize.X - 1513, worldSize.Y - 961), Rectangle.Empty));
            openingWindow.Add(new GenericSprite2D(Content.Load<Texture2D>("Objects\\window\\window_07"), new Vector2(worldSize.X - 1513, worldSize.Y - 961), Rectangle.Empty));
            openingWindow.Add(new GenericSprite2D(Content.Load<Texture2D>("Objects\\window\\window_08"), new Vector2(worldSize.X - 1513, worldSize.Y - 961), Rectangle.Empty));
            openingWindow.Add(new GenericSprite2D(Content.Load<Texture2D>("Objects\\window\\window_09"), new Vector2(worldSize.X - 1513, worldSize.Y - 961), Rectangle.Empty));
            openingWindow.Add(new GenericSprite2D(Content.Load<Texture2D>("Objects\\window\\window_10"), new Vector2(worldSize.X - 1513, worldSize.Y - 961), Rectangle.Empty));
            openingWindow.Add(new GenericSprite2D(Content.Load<Texture2D>("Objects\\window\\window_11"), new Vector2(worldSize.X - 1513, worldSize.Y - 961), Rectangle.Empty));
            
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
        bool wallsRetracted = false;
        public double Update(GameTime gameTime, ref float bodyTempTimer, ref float exhaustionTimer, ref KeyboardState oldKeyboardState, ref float jumpTimer, ref Game_Level gameLevel, ref float staminaExhaustionTimer, ref double bodyTemperature, ref double stamina, ref double staminaLimit)
        {
            //Update timer
            openWindowTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //update the player Position with respect to keyboard input and platform collision
            Vector2 prevPosition = playerNode.Position;
            //bool useLighter = filterOn;

            //if (!generatorOn)
            //{
            //    platforms.AddRange(walls);
            //}
            if (generatorOn && !wallsRetracted)
            {
               platforms.RemoveRange(platforms.Count - 2, 2);
               wallsRetracted = true;
            }
            playerNode.Update(!generatorOn, gameTime, ref bodyTempTimer, ref exhaustionTimer, ref oldKeyboardState, ref jumpTimer, ground, platforms, ladders, worldSize, ref staminaExhaustionTimer);

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

            //lightSwitch.Update(playerNode, ref generatorOn, ref filterOn, shadowFilter, ref doorCanOpen);
            generator.Update(playerNode, ref generatorOn, ref filterOn, shadowFilter, ref doorCanOpen);
            doorSwitch.Update(playerNode, ref generatorOn, ref filterOn, shadowFilter, ref fowardDoor.canOpen);
            doorSwitch.Update(playerNode, ref generatorOn, ref filterOn, shadowFilter, ref roomDoor.canOpen);

            if (insideRoom)
            {
                staminaBooster.Update(ref playerNode, ref bodyTemperature, ref stamina, ref staminaLimit);
            }

            //update the shadowFilter's Position with respect to the playerNode
            filterOn = !generatorOn;
            shadowFilter.Position = new Vector2((playerNode.Position.X /*+ (playerNode.Texture.Width / 2))*/) - (shadowFilter.Texture.Width / 2),
                (playerNode.Position.Y + (playerNode.playerSpriteSize.Y / 2) - (shadowFilter.Texture.Height / 2)));


            //update the camera based on the player and world size
            camera.TranslateWithSprite(playerNode, screenSize);
            camera.CapCameraPosition(worldSize, screenSize);
            
            
            //Check if the player is going into/out of the room
            if (new Rectangle((int)playerNode.Position.X, (int)playerNode.Position.Y, (int)playerNode.playerSpriteSize.X, (int)playerNode.playerSpriteSize.Y).Intersects(new Rectangle((int)roomDoor.Position.X, (int)roomDoor.Position.Y, (int)roomDoor.size.X, (int)roomDoor.size.Y)))
            {
                if (roomDoor.canOpen)
                {
                    if (!insideRoom)
                    {
                        insideRoom = true;
                        playerNode.Position.X = roomDoor.Position.X - 32;
                    }
                    else if (insideRoom)
                    {
                        insideRoom = false;
                        playerNode.Position.X = roomDoor.Position.X + roomDoor.size.X;
                    }
                }
            }

            if (insideRoom)
            {
                if (playerNode.Position.X < (worldSize.X - 1431))
                {
                    playerNode.Position.X = worldSize.X - 1431;
                }
            }

            //return the body temperature
            return playerNode.bodyTemperature;
            
        }

        
        //draw funtion
        public void Draw(int framesPerSecond)
        {
            spriteBatch.Begin();
            ////draw the desired nodes onto screen through the camera
            camera.DrawNode(backgroundBack);
            if (generatorOn && openWindowTimer > 1000 && windowAnimationCounter <= 9)
            {
                
                windowAnimationCounter++;
                openWindowTimer = 0;
            }
            camera.DrawNode(openingWindow[windowAnimationCounter]);

            if (insideRoom)
            {
                camera.DrawNode(playerNode);
            }

            camera.DrawNode(backgroundMiddle);
            
            camera.DrawNode(backgroundFront);
            foreach (GenericSprite2D element in worldObjects)
            {
                camera.DrawNode(element);
            }
            camera.DrawNode(roomDoor);
            if (!generatorOn)
            {
                camera.DrawNode(walls[0]);
                camera.DrawNode(walls[1]);
            }
            if (!insideRoom)
            {
                camera.DrawNode(playerNode);
            }

            if (filterOn)
                //camera.DrawNode(shadowFilter);

            //draw the fps
            spriteBatch.DrawString(font, framesPerSecond.ToString(), new Vector2(screenSize.X - 50, 25), Color.White);
            //draw the status display and the body temperature
            spriteBatch.Draw(statusDisplayTexture, new Vector2(50, 50), Color.White);
            spriteBatch.DrawString(font, Math.Round(playerNode.bodyTemperature, 2).ToString(), new Vector2(52, 52), Color.Black, 0, new Vector2(0, 0), new Vector2(0.8f, 2), SpriteEffects.None, 0);
            spriteBatch.DrawString(font, Math.Round(playerNode.stamina, 2).ToString(), new Vector2(120, 52), Color.Black, 0, new Vector2(0, 0), new Vector2(1f, 1), SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}

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

        Texture2D statusDisplayTexture;

        Camera2D camera;
        Character playerNode;
        Filter shadowFilter;
        GenericSprite2D backgroundNode;

        List<GenericSprite2D> worldObjects;

        List<Platform> platforms;
        Platform retractableWall;
        List<Portal> portals;
        List<Ladder> ladders;
        Portal fowardDoor, backwardDoor;
        Interactable lightSwitch, generator, doorSwitch, puzzleSwitch1, puzzleSwitch2, puzzleSwitch3, puzzleSwitch4, puzzleSwitch5;
        PickUpItem staminaBooster;

        List<Platform> walls = new List<Platform>();

        bool filterOn = true, generatorOn = false, doorCanOpen = false;

        //List of switches
        List<Interactable> switchPuzzle;

        //declare constructor
        public Level_Entertainment_Room(SpriteBatch spriteBatch, Vector2 screenSize)
        {
            this.spriteBatch = spriteBatch;
            platforms = new List<Platform>();
            this.screenSize = screenSize;
            portals = new List<Portal>();
            ladders = new List<Ladder>();
            worldObjects = new List<GenericSprite2D>();
            switchPuzzle = new List<Interactable>();

        }

        //load content
        public void LoadContent(ContentManager Content, Game_Level gameLevel, Game_Level prevGameLevel, double bodyTemperature, double stamina, double staminaLimit)
        {
            //load the needed textures
            Texture2D playerTexture = Content.Load<Texture2D>("Character\\PlayerSpriteSheet");
            Texture2D backgroundTexture = Content.Load<Texture2D>("Backgrounds\\entertainmentroom_background");
            statusDisplayTexture = Content.Load<Texture2D>("statusDisplay");

            //initialize the world size and the ground coordinate according to the world size
            worldSize = new Vector2(backgroundTexture.Width, backgroundTexture.Height);
            ground = worldSize.Y - 50;

            //load font
            font = Content.Load<SpriteFont>("Fonts\\Score");

            //initialize the needed nodes and camera
            backgroundNode = new GenericSprite2D(backgroundTexture, new Vector2(0, 0), Rectangle.Empty);
            worldObjects.Add(backgroundNode);

            shadowFilter = new Filter(Content.Load<Texture2D>("shadowFilterLarge"), new Vector2(0, 0));

            camera = new Camera2D(spriteBatch);
            camera.cameraPosition = new Vector2(0, worldSize.Y - screenSize.Y);

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

            //walls
            retractableWall = new Platform(platformTexture, new Vector2(69, 207), new Vector2(worldSize.X - 832, worldSize.Y - 256));


            //initialize ladders and add them to the list
            //Load ladder Texture
            Texture2D ladderTexture = Content.Load<Texture2D>("Objects\\ladder");
            //First floor
            ladders.Add(new Ladder(ladderTexture, new Vector2(34, 235), new Vector2(100 + 113, worldSize.Y - 285)));
            //Second floor
            ladders.Add(new Ladder(ladderTexture, new Vector2(34, 120), new Vector2(worldSize.X - 243, worldSize.Y - 753)));

            worldObjects.AddRange(platforms);
            platforms.Add(retractableWall);
            worldObjects.AddRange(ladders);

            //initialize the needed portals
            backwardDoor = new Portal(new Vector2(worldSize.X - 40 - 64, worldSize.Y - 50 - 72), new Vector2(51, 72), Portal.PortalType.BACKWARD, Content);
            fowardDoor = new Portal(new Vector2(worldSize.X - 845, worldSize.Y - 822), new Vector2(51, 72), Portal.PortalType.FOWARD, Content);
            portals.Add(backwardDoor);
            portals.Add(fowardDoor);
            worldObjects.AddRange(portals);

            //initialize the playerNode
            if (prevGameLevel <= gameLevel)
            {
                playerNode = new Character(playerTexture, /*new Vector2(backwardDoor.Position.X - backwardDoor.size.X - 15, worldSize.Y - 64 - 50)*/new Vector2(200, 650), bodyTemperature, stamina, staminaLimit, 4, 5);
            }
            else if (prevGameLevel >= gameLevel)
            {
                playerNode = new Character(playerTexture, new Vector2(fowardDoor.Position.X - 32 - 5, worldSize.Y - 64 - 50), bodyTemperature, stamina, staminaLimit, 4, 5);
            }

            //Pickup item
            staminaBooster = new PickUpItem(platformTexture, new Vector2(worldSize.X - 1413, worldSize.Y - 111), new Vector2(28, 28), PickUpItem.ItemType.STAMINA, 100, PickUpItem.ItemEffectDuration.TEMPORARY);

            //initiate the switchPuzzle
            puzzleSwitch5 = new Interactable(Content.Load<Texture2D>("Objects\\doorswitch_off"), new Vector2(760, worldSize.Y - 116), new Vector2(11, 19), Interactable.Type_Of_Interactable.PUZZLE_SWITCH, Content.Load<Texture2D>("Objects\\doorswitch_on"));
            puzzleSwitch4 = new Interactable(Content.Load<Texture2D>("Objects\\doorswitch_off"), new Vector2(700, worldSize.Y - 116), new Vector2(11, 19), Interactable.Type_Of_Interactable.PUZZLE_SWITCH, Content.Load<Texture2D>("Objects\\doorswitch_on"));
            puzzleSwitch3 = new Interactable(Content.Load<Texture2D>("Objects\\doorswitch_off"), new Vector2(650, worldSize.Y - 116), new Vector2(11, 19), Interactable.Type_Of_Interactable.PUZZLE_SWITCH, Content.Load<Texture2D>("Objects\\doorswitch_on"));
            puzzleSwitch2 = new Interactable(Content.Load<Texture2D>("Objects\\doorswitch_off"), new Vector2(600, worldSize.Y - 116), new Vector2(11, 19), Interactable.Type_Of_Interactable.PUZZLE_SWITCH, Content.Load<Texture2D>("Objects\\doorswitch_on"));
            puzzleSwitch1 = new Interactable(Content.Load<Texture2D>("Objects\\doorswitch_off"), new Vector2(550, worldSize.Y - 116), new Vector2(11, 19), Interactable.Type_Of_Interactable.PUZZLE_SWITCH, Content.Load<Texture2D>("Objects\\doorswitch_on"));
            switchPuzzle.Add(puzzleSwitch1);
            switchPuzzle.Add(puzzleSwitch2);
            switchPuzzle.Add(puzzleSwitch3);
            switchPuzzle.Add(puzzleSwitch4);
            switchPuzzle.Add(puzzleSwitch5);
            
            //Generator
            generator = new Interactable(Content.Load<Texture2D>("Objects\\generator_off"), new Vector2(worldSize.X - 638, worldSize.Y - 814), new Vector2(104, 65), Interactable.Type_Of_Interactable.GENERATOR, Content.Load<Texture2D>("Objects\\generator_on"));
            //Door switch
            doorSwitch = new Interactable(Content.Load<Texture2D>("Objects\\doorswitch_off"), new Vector2(150, 360), new Vector2(11, 19), Interactable.Type_Of_Interactable.DOOR_SWITCH, Content.Load<Texture2D>("Objects\\doorswitch_on"));

            worldObjects.Add(staminaBooster);
            worldObjects.Add(generator);
            worldObjects.Add(doorSwitch);
            worldObjects.AddRange(switchPuzzle);

            worldObjects.Add(playerNode);
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
        bool wallsRetracted = false, wallRemoved = false;
        public double Update(GameTime gameTime, ref float bodyTempTimer, ref float exhaustionTimer, ref KeyboardState oldKeyboardState, ref float jumpTimer, ref Game_Level gameLevel, ref float staminaExhaustionTimer, ref double bodyTemperature, ref double stamina, ref double staminaLimit)
        {
            //update the player Position with respect to keyboard input and platform collision
            Vector2 prevPosition = playerNode.Position;

            puzzleSwitch1.timeCounter++;
            puzzleSwitch2.timeCounter++;
            puzzleSwitch3.timeCounter++;
            puzzleSwitch4.timeCounter++;
            puzzleSwitch5.timeCounter++;
            if (puzzleSwitch1.puzzleSwitchOn /*&& puzzleSwitch2.puzzleSwitchOn && puzzleSwitch3.puzzleSwitchOn && puzzleSwitch4.puzzleSwitchOn && puzzleSwitch5.puzzleSwitchOn*/)
            {
                wallsRetracted = true;
            }

            if (wallsRetracted && !wallRemoved)
            {
                platforms.RemoveRange(platforms.Count - 1, 1);
                wallRemoved = true;
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
            foreach (Interactable puzzleSwitch in switchPuzzle)
            {
                puzzleSwitch.Update(playerNode, ref generatorOn, ref filterOn, shadowFilter, ref fowardDoor.canOpen);
            }

            //update the shadowFilter's Position with respect to the playerNode
            filterOn = !generatorOn;
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
           
            foreach (GenericSprite2D element in worldObjects)
            {
                camera.DrawNode(element);
            }
            if (!wallsRetracted)
            {
                camera.DrawNode(retractableWall);
            }

            //if (filterOn)
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

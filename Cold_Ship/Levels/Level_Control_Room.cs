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
    public class Level_Control_Room
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
        GenericSprite2D backgroundNode, backgroundbackNode, backgroundFrontNode;

        List<GenericSprite2D> worldObjects;

        List<Platform> platforms;
        Platform retractableWall;
        List<Portal> portals;
        List<Ladder> ladders;
        Portal fowardDoor, backwardDoor, roomDoor;
        Interactable lightSwitch, generator, doorSwitch, puzzleSwitch1, puzzleSwitch2, puzzleSwitch3, puzzleSwitch4, puzzleSwitch5;
        PickUpItem staminaBooster;

        List<GenericSprite2D> openingWindow = new List<GenericSprite2D>();

        List<Platform> walls = new List<Platform>();

        bool filterOn = true, generatorOn = false, doorCanOpen = false;

        //timer and counter for opening window
        float openWindowTimer = 0;
        int windowAnimationCounter = 0;
        //Bool flag to check wether the player is inside the room
        bool insideRoom = false;

        //List of switches
        List<Interactable> switchPuzzle;

        //declare constructor
        public Level_Control_Room(SpriteBatch spriteBatch, Vector2 screenSize)
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
            Texture2D backgroundTexture = Content.Load<Texture2D>("Backgrounds\\controlroom_middle");
            Texture2D backgroundTexture_back = Content.Load<Texture2D>("Backgrounds\\controlroom_back");
            Texture2D backgroundTexture_front = Content.Load<Texture2D>("Backgrounds\\controlroom_front");
            statusDisplayTexture = Content.Load<Texture2D>("statusDisplay");


            //initialize the world size and the ground coordinate according to the world size
            worldSize = new Vector2(backgroundTexture.Width, backgroundTexture.Height);
            ground = worldSize.Y - 50;

            //load font
            font = Content.Load<SpriteFont>("Fonts\\Score");

            //initialize the needed nodes and camera
            backgroundNode = new GenericSprite2D(backgroundTexture, new Vector2(0, 0), Rectangle.Empty);
            backgroundbackNode = new GenericSprite2D(backgroundTexture_back, new Vector2(0, 0), Rectangle.Empty);
            backgroundFrontNode = new GenericSprite2D(backgroundTexture_front, new Vector2(0, 0), Rectangle.Empty);
            

            shadowFilter = new Filter(Content.Load<Texture2D>("shadowFilterLarge"), new Vector2(0, 0));

            camera = new Camera2D(spriteBatch);
            camera.cameraPosition = new Vector2(0, worldSize.Y - screenSize.Y);

            //initialize the needed platforms
            Texture2D platformTexture = Content.Load<Texture2D>("Textures\\platformTexture");

       

            //initialize the needed portals
            backwardDoor = new Portal(new Vector2(worldSize.X - 970, worldSize.Y - 123), new Vector2(51, 72), Portal.PortalType.BACKWARD, Content);
            fowardDoor = new Portal(new Vector2(worldSize.X - 845, worldSize.Y - 822), new Vector2(51, 72), Portal.PortalType.FOWARD, Content);
            
            portals.Add(backwardDoor);
            portals.Add(fowardDoor);
            worldObjects.AddRange(portals);

            //initialize the playerNode
            if (prevGameLevel <= gameLevel)
            {
                playerNode = new Character(playerTexture, new Vector2(backwardDoor.Position.X + backwardDoor.size.X + 10, worldSize.Y - 50 - 64), bodyTemperature, stamina, staminaLimit, 4, 5);
            }
            else if (prevGameLevel >= gameLevel)
            {
                playerNode = new Character(playerTexture, new Vector2(fowardDoor.Position.X - 32 - 5, worldSize.Y - 64 - 50), bodyTemperature, stamina, staminaLimit, 4, 5);
            }


            //Generator
            generator = new Interactable(Content.Load<Texture2D>("Objects\\generator_off"), new Vector2(worldSize.X - 210, worldSize.Y - 116), new Vector2(104, 65), Interactable.Type_Of_Interactable.GENERATOR, Content.Load<Texture2D>("Objects\\generator_on"));
            
            worldObjects.Add(generator);
            



            //Window opening animation
            openingWindow.Add(new GenericSprite2D(Content.Load<Texture2D>("Objects\\window\\controlroom_window_01"), new Vector2(127, 62), Rectangle.Empty));
            openingWindow.Add(new GenericSprite2D(Content.Load<Texture2D>("Objects\\window\\controlroom_window_02"), new Vector2(127, 62), Rectangle.Empty));
            openingWindow.Add(new GenericSprite2D(Content.Load<Texture2D>("Objects\\window\\controlroom_window_03"), new Vector2(127, 62), Rectangle.Empty));
            openingWindow.Add(new GenericSprite2D(Content.Load<Texture2D>("Objects\\window\\controlroom_window_04"), new Vector2(127, 62), Rectangle.Empty));
            openingWindow.Add(new GenericSprite2D(Content.Load<Texture2D>("Objects\\window\\controlroom_window_05"), new Vector2(127, 62), Rectangle.Empty));
            openingWindow.Add(new GenericSprite2D(Content.Load<Texture2D>("Objects\\window\\controlroom_window_06"), new Vector2(127, 62), Rectangle.Empty));
            openingWindow.Add(new GenericSprite2D(Content.Load<Texture2D>("Objects\\window\\controlroom_window_07"), new Vector2(127, 62), Rectangle.Empty));
            openingWindow.Add(new GenericSprite2D(Content.Load<Texture2D>("Objects\\window\\controlroom_window_08"), new Vector2(127, 62), Rectangle.Empty));
            openingWindow.Add(new GenericSprite2D(Content.Load<Texture2D>("Objects\\window\\controlroom_window_09"), new Vector2(127, 62), Rectangle.Empty));
            
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
            //Update timer
            openWindowTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //update the player Position with respect to keyboard input and platform collision
            Vector2 prevPosition = playerNode.Position;
            //bool useLighter = filterOn;

            
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


            generator.Update(playerNode, ref generatorOn, ref filterOn, shadowFilter, ref doorCanOpen);

            

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
            ////draw the desired nodes onto screen through the camera
            camera.DrawNode(backgroundbackNode);
            camera.DrawNode(backgroundNode);
            
            
            if (openWindowTimer > 1000 && windowAnimationCounter < 8 && generatorOn)
            {

                windowAnimationCounter++;
                openWindowTimer = 0;
            }
            camera.DrawNode(openingWindow[windowAnimationCounter]);
            camera.DrawNode(backgroundFrontNode);

         

            foreach (GenericSprite2D element in worldObjects)
            {
                camera.DrawNode(element);
            }
            camera.DrawNode(playerNode);

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

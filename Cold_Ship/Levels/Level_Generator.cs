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
        
        bool filterOn = true, generatorOn = false;

        //declare constructor
        public Level_Generator(SpriteBatch spriteBatch, Vector2 screenSize)
        {
            this.spriteBatch = spriteBatch;
            platforms = new List<Platform>();
            this.screenSize = screenSize;
            portals = new List<Portal>();
            worldObjects = new List<GenericSprite2D>();
        }

        //load content
        public void LoadContent(ContentManager Content, Game_Level gameLevel, Game_Level prevGameLevel, double bodyTemperature, double stamina, double staminaLimit)
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
            shadowFilter = new Filter(Content.Load<Texture2D>("shadowFilterLarge"), new Vector2(0, 0));
            camera = new Camera2D(spriteBatch);
            camera.cameraPosition = new Vector2(0, worldSize.Y - screenSize.Y);

            reactor = new Reactor(Content, new Vector2(578, 472));
            worldObjects.Add(reactor);

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
                playerNode = new Character(playerTexture, new Vector2(backwardDoor.Position.X + backwardDoor.size.X + 5, 116), bodyTemperature, stamina, staminaLimit, 4, 5);
            }
            else if (prevGameLevel >= gameLevel)
            {
                playerNode = new Character(playerTexture, new Vector2(forwardDoor.Position.X - 32 - 35, 116), bodyTemperature, stamina, staminaLimit, 4, 5);
            }

            staminaBooster = new PickUpItem(platformTexture, new Vector2(165, 1750), new Vector2(28, 28), PickUpItem.ItemType.STAMINA, 100, PickUpItem.ItemEffectDuration.TEMPORARY);
            lightSwitch = new Interactable(Content.Load<Texture2D>("Objects\\lightswitch_off"), new Vector2(1900, 425), new Vector2(23, 32), Interactable.Type_Of_Interactable.LIGHT_SWITCH, Content.Load<Texture2D>("Objects\\lightswitch_on"));
            generator = new Interactable(Content.Load<Texture2D>("Objects\\generator_off"), new Vector2(worldSize.X - 246, ground - 63), new Vector2(103, 63), Interactable.Type_Of_Interactable.GENERATOR, Content.Load<Texture2D>("Objects\\generator_on"));
            doorSwitch = new Interactable(Content.Load<Texture2D>("Objects\\doorswitch_off"), new Vector2(1120, 1650), new Vector2(11, 19), Interactable.Type_Of_Interactable.DOOR_SWITCH, Content.Load<Texture2D>("Objects\\doorswitch_on"));

            worldObjects.Add(staminaBooster);
            worldObjects.Add(lightSwitch);
            worldObjects.Add(generator);
            worldObjects.Add(doorSwitch);

            worldObjects.Add(playerNode);
        }

        private void createPlatforms(Texture2D platformTexture)
        {
            Vector2 sizeBasePlatform = new Vector2(120, 20);
            platforms.Add(new Platform(platformTexture, new Vector2(420, 20), new Vector2(100, 180)));
            platforms.Add(new Platform(platformTexture, new Vector2(420, 20), new Vector2(1520, 180)));

            platforms.Add(new MovingPlatform(platformTexture, sizeBasePlatform, new Vector2(545, 200), new Vector2(0, 0.5f), new Vector2(0, 270)));
            platforms.Add(new MovingPlatform(platformTexture, sizeBasePlatform, new Vector2(255, 485), new Vector2(0.5f, 0), new Vector2(135, 0)));
            platforms.Add(new MovingPlatform(platformTexture, sizeBasePlatform, new Vector2(120, 555), new Vector2(0, 0.5f), new Vector2(0, 135)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(300, 665)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(430, 755)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(300, 855)));
            platforms.Add(new Platform(platformTexture, new Vector2(50, 20), new Vector2(200, 880)));
            platforms.Add(new Platform(platformTexture, new Vector2(30, 20), new Vector2(100, 980)));
            platforms.Add(new Platform(platformTexture, new Vector2(50, 20), new Vector2(100, 1020)));
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
        public double Update(GameTime gameTime, ref float bodyTempTimer, ref float exhaustionTimer, ref KeyboardState oldKeyboardState, ref float jumpTimer, ref Game_Level gameLevel, ref float staminaExhaustionTimer, ref double bodyTemperature, ref double stamina, ref double staminaLimit)
        {
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
            foreach (Portal portal in portals)
            {
                portal.Update(playerNode, ref gameLevel);
            }

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
            //draw the desired nodes onto screen through the camera
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
            spriteBatch.End();
        }
    }
}
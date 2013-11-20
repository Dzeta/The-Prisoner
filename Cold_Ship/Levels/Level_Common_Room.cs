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
        GenericSprite2D backgroundNode;

        List<GenericSprite2D> worldObjects;

        List<Platform> platforms;
        List<Portal> portals;
        List<Ladder> ladders;
        Portal fowardDoor, backwardDoor;
        Interactable lightSwitch, generator;
        PickUpItem staminaBooster;

        bool filterOn = true, generatorOn = false, doorCanOpen = false;

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
            Texture2D playerTexture = Content.Load<Texture2D>("PlayerSpriteSheet");
            Texture2D backgroundTexture = Content.Load<Texture2D>("prisonblock_final");
            statusDisplayTexture = Content.Load<Texture2D>("statusDisplay");


            //initialize the world size and the ground coordinate according to the world size
            worldSize = new Vector2(backgroundTexture.Width, backgroundTexture.Height);
            ground = worldSize.Y - 50;

            //load font
            font = Content.Load<SpriteFont>("Score");

            //initialize the needed nodes and camera
            backgroundNode = new GenericSprite2D(backgroundTexture, new Vector2(0, 0), Rectangle.Empty);
            worldObjects.Add(backgroundNode);
            shadowFilter = new Filter(Content.Load<Texture2D>("shadowFilterLarge"), new Vector2(0, 0));
            camera = new Camera2D(spriteBatch);
            camera.cameraPosition = new Vector2(0, worldSize.Y - screenSize.Y);

            //initialize the needed platforms
            Texture2D platformTexture = Content.Load<Texture2D>("platformTexture");

            //initialize the platforms and add them to the list
            platforms.Add(new Platform(platformTexture, new Vector2(890, 20), new Vector2(0, worldSize.Y - 280)));
            platforms.Add(new Platform(platformTexture, new Vector2(375, 20), new Vector2(925, worldSize.Y - 280)));
            platforms.Add(new Platform(platformTexture, new Vector2(619, 20), new Vector2(1345, worldSize.Y - 280)));
            platforms.Add(new Platform(platformTexture, new Vector2(475, 20), new Vector2(0, worldSize.Y - 510)));
            platforms.Add(new Platform(platformTexture, new Vector2(1367, 20), new Vector2(514, worldSize.Y - 510)));
            platforms.Add(new Platform(platformTexture, new Vector2(50, 20), new Vector2(1920, worldSize.Y - 510)));
            platforms.Add(new Platform(platformTexture, new Vector2(133, 20), new Vector2(0, worldSize.Y - 744)));
            platforms.Add(new Platform(platformTexture, new Vector2(727, 20), new Vector2(167, worldSize.Y - 744)));
            platforms.Add(new Platform(platformTexture, new Vector2(773, 20), new Vector2(933, worldSize.Y - 744)));
            platforms.Add(new Platform(platformTexture, new Vector2(250, 20), new Vector2(1742, worldSize.Y - 744)));
            //walls
            platforms.Add(new Platform(platformTexture, new Vector2(130, 270), new Vector2(963, worldSize.Y - 270)));
            platforms.Add(new Platform(platformTexture, new Vector2(130, 230), new Vector2(1150, worldSize.Y - 508)));

            //initialize ladders and add them to the list
            Texture2D ladderTexture = Content.Load<Texture2D>("ladder");
            ladders.Add(new Ladder(ladderTexture, new Vector2(34, 235), new Vector2(890, worldSize.Y - 282)));
            ladders.Add(new Ladder(ladderTexture, new Vector2(34, 235), new Vector2(1301, worldSize.Y - 282)));
            ladders.Add(new Ladder(ladderTexture, new Vector2(34, 235), new Vector2(478, worldSize.Y - 512)));
            ladders.Add(new Ladder(ladderTexture, new Vector2(34, 235), new Vector2(1887, worldSize.Y - 512)));
            ladders.Add(new Ladder(ladderTexture, new Vector2(34, 235), new Vector2(134, worldSize.Y - 747)));
            ladders.Add(new Ladder(ladderTexture, new Vector2(34, 235), new Vector2(898, worldSize.Y - 749)));
            ladders.Add(new Ladder(ladderTexture, new Vector2(34, 235), new Vector2(1707, worldSize.Y - 749)));

            worldObjects.AddRange(platforms);
            worldObjects.AddRange(ladders);

            //initialize the needed portals
            backwardDoor = new Portal(new Vector2(100, worldSize.Y - 64 - 50), new Vector2(32, 64), Portal.PortalType.BACKWARD, Content);
            fowardDoor = new Portal(new Vector2(worldSize.X - 32 - 75, worldSize.Y - 64 - 50), new Vector2(32, 64), Portal.PortalType.FOWARD, Content);
            portals.Add(backwardDoor);
            portals.Add(fowardDoor);
            worldObjects.AddRange(portals);

            //initialize the playerNode
            if (prevGameLevel <= gameLevel)
            {
                playerNode = new Character(playerTexture, new Vector2(backwardDoor.position.X + backwardDoor.size.X + 5, worldSize.Y - 64 - 50), bodyTemperature, stamina, staminaLimit, 4, 5);
            }
            else if (prevGameLevel >= gameLevel)
            {
                playerNode = new Character(playerTexture, new Vector2(fowardDoor.position.X - 32 - 5, worldSize.Y - 64 - 50), bodyTemperature, stamina, staminaLimit, 4, 5);
            }

            staminaBooster = new PickUpItem(platformTexture, new Vector2(280, worldSize.Y - 772), new Vector2(28, 28), PickUpItem.ItemType.STAMINA, 100, PickUpItem.ItemEffectDuration.TEMPORARY);
            lightSwitch = new Interactable(platformTexture, new Vector2(1643, worldSize.Y - 359), new Vector2(31, 43), Interactable.Type_Of_Interactable.LIGHT_SWITCH);
            generator = new Interactable(Content.Load<Texture2D>("generator_off"), new Vector2(1807, worldSize.Y - 809), new Vector2(104, 65), Interactable.Type_Of_Interactable.GENERATOR, Content.Load<Texture2D>("generator_on"));

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
            //update the player position with respect to keyboard input and platform collision
            Vector2 prevPosition = playerNode.position;
            bool useLighter = filterOn;
            playerNode.Update(useLighter, gameTime, ref bodyTempTimer, ref exhaustionTimer, ref oldKeyboardState, ref jumpTimer, ground, platforms, ladders, worldSize, ref staminaExhaustionTimer);

            //Check the player's collision with the world boundaries
            if (playerNode.position.X < 100 || playerNode.position.X + playerNode.playerSpriteSize.X > worldSize.X - 100)
            {
                playerNode.position.X = prevPosition.X;
            }

            //update portals
            foreach (Portal portal in portals)
            {
                portal.Update(playerNode, ref gameLevel);
            }

            lightSwitch.Update(playerNode, ref generatorOn, ref filterOn, shadowFilter, ref doorCanOpen);
            generator.Update(playerNode, ref generatorOn, ref filterOn, shadowFilter, ref doorCanOpen);

            staminaBooster.Update(ref playerNode, ref bodyTemperature, ref stamina, ref staminaLimit);

            //update the shadowFilter's position with respect to the playerNode
            shadowFilter.position = new Vector2((playerNode.position.X /*+ (playerNode.texture.Width / 2))*/) - (shadowFilter.texture.Width / 2),
                (playerNode.position.Y + (playerNode.playerSpriteSize.Y / 2) - (shadowFilter.texture.Height / 2)));


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

            if (filterOn)
                camera.DrawNode(shadowFilter);

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

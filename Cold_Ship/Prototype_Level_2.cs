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
    public class Prototype_Level_2
    {
         //declare member variables
        public SpriteBatch spriteBatch;
        Vector2 worldSize, screenSize;
        List<Platform> platforms;
        float ground;
        Texture2D statusDisplayTexture;
        SpriteFont font;
        Scene2DNode playerNode, backgroundNode, shadowFilter;
        Camera2D camera;
        Portal fowardDoor, backwardDoor;
        List<Portal> portals;
        List<Ladder> ladders;

        //declare constructor
        public Prototype_Level_2(SpriteBatch spriteBatch, Vector2 screenSize)
        {
            this.spriteBatch = spriteBatch;
            platforms = new List<Platform>();
            this.screenSize = screenSize;
            portals = new List<Portal>();
            ladders = new List<Ladder>();
            
        }

        //load content
        public void LoadContent(ContentManager Content, Game_Level gameLevel, Game_Level prevGameLevel, double bodyTemperature, double stamina, double staminaLimit)
        {
            //load the needed textures
            Texture2D playerTexture = Content.Load<Texture2D>("player");
            Texture2D backgroundTexture = Content.Load<Texture2D>("background2");
            statusDisplayTexture = Content.Load<Texture2D>("statusDisplay");


            //initialize the world size and the ground coordinate according to the world size
            worldSize = new Vector2(backgroundTexture.Width, backgroundTexture.Height);
            ground = worldSize.Y;

            //load font
            font = Content.Load<SpriteFont>("Score");

            //initialize the needed nodes and camera
            //playerNode = new Scene2DNode(playerTexture, new Vector2(50, worldSize.Y - 64));
            backgroundNode = new Scene2DNode(backgroundTexture, new Vector2(0, 0));
            shadowFilter = new Scene2DNode(Content.Load<Texture2D>("shadowFilterLarge"), new Vector2(0, 0));
            camera = new Camera2D(spriteBatch);
            camera.cameraPosition = new Vector2(0, worldSize.Y - screenSize.Y);

            //initialize the needed platforms
            Texture2D platformTexture = Content.Load<Texture2D>("platformTexture");
            Platform platform = new Platform(platformTexture, new Vector2(500, 32), new Vector2(0, worldSize.Y - 120));
            Platform platform2 = new Platform(platformTexture, new Vector2(64, 15), new Vector2(600, worldSize.Y - 60));
            Platform platform3 = new Platform(platformTexture, new Vector2(50, 800), new Vector2(800, worldSize.Y - 800));
            Platform platform4 = new Platform(platformTexture, new Vector2(80, 15), new Vector2(120, worldSize.Y - 220));
            Platform platform5 = new Platform(platformTexture, new Vector2(80, 15), new Vector2(50, worldSize.Y - 320));
            Platform platform6 = new Platform(platformTexture, new Vector2(80, 15), new Vector2(140, worldSize.Y - 420));
            Platform platform7 = new Platform(platformTexture, new Vector2(80, 15), new Vector2(200, worldSize.Y - 520));
            Platform platform8 = new Platform(platformTexture, new Vector2(80, 15), new Vector2(100, worldSize.Y - 620));
            platforms.Add(platform);
            platforms.Add(platform2);
            platforms.Add(platform3);
            platforms.Add(platform4);
            platforms.Add(platform5);
            platforms.Add(platform6);
            platforms.Add(platform7);
            platforms.Add(platform8);
            platforms.Add(new Platform(platformTexture, new Vector2(80, 15), new Vector2(20, worldSize.Y - 750)));
            platforms.Add(new Platform(platformTexture, new Vector2(80, 15), new Vector2(100, worldSize.Y - 850)));
            platforms.Add(new Platform(platformTexture, new Vector2(80, 15), new Vector2(200, worldSize.Y - 950)));
            platforms.Add(new Platform(platformTexture, new Vector2(500, 15), new Vector2(400, worldSize.Y - 960)));

            ladders.Add(new Ladder(platformTexture, new Vector2(60, 150), new Vector2(750, worldSize.Y - 180)));
            ladders.Add(new Ladder(platformTexture, new Vector2(40, 120), new Vector2(80, worldSize.Y - 750)));

            //initialize the needed portals
            backwardDoor = new Portal(platformTexture, new Vector2(0, worldSize.Y - 64), new Vector2(32, 64), Portal.PortalType.BACKWARD);
            fowardDoor = new Portal(platformTexture, new Vector2(worldSize.X - 32, worldSize.Y - 64), new Vector2(32, 64), Portal.PortalType.FOWARD);
            portals.Add(backwardDoor);
            portals.Add(fowardDoor);

            //initialize the playerNode
            if (prevGameLevel <= gameLevel)
            {
                playerNode = new Scene2DNode(playerTexture, new Vector2(backwardDoor.position.X + backwardDoor.size.X + 5, worldSize.Y - 64), bodyTemperature, stamina, staminaLimit);
            }
            else if (prevGameLevel >= gameLevel)
            {
                playerNode = new Scene2DNode(playerTexture, new Vector2(fowardDoor.position.X - 32 - 5, worldSize.Y - 64), bodyTemperature, stamina, staminaLimit);
            }
        }

        //update function
        public double Update(GameTime gameTime, ref float bodyTempTimer, ref float exhaustionTimer, ref KeyboardState oldKeyboardState, ref float jumpTimer, ref Game_Level gameLevel, ref float staminaExhaustionTimer, ref double bodyTemperature, ref double stamina, ref double staminaLimit)
        {
            //outdated codes that's now in the Update method
            /*bodyTempTimer += gameTime.ElapsedGameTime.Milliseconds;
            exhaustionTimer += gameTime.ElapsedGameTime.Milliseconds;
            KeyboardState newKeyboardState = Keyboard.GetState();
            playerNode.UpdateKeyboard(oldKeyboardState, newKeyboardState);
            oldKeyboardState = newKeyboardState;
            playerNode.updateBodyTemperature(ref bodyTempTimer, ref exhaustionTimer);*/

            //update the player position with respect to keyboard input and platform collision
            playerNode.Update(gameTime, ref bodyTempTimer, ref exhaustionTimer, ref oldKeyboardState, ref jumpTimer, ground, platforms, ladders, worldSize, ref staminaExhaustionTimer);

            foreach (Portal portal in portals)
            {
                portal.Update(playerNode, ref gameLevel);
            }
            
            //update the shadowFilter's position with respect to the playerNode
            shadowFilter.position = new Vector2((playerNode.position.X /*+ (playerNode.texture.Width / 2))*/) - (shadowFilter.texture.Width / 2),
                (playerNode.position.Y + (playerNode.texture.Height / 2) - (shadowFilter.texture.Height / 2)));


            //update the camera based on the player and world size
            camera.TranslateWithSprite(playerNode, screenSize);
            camera.CapCameraPosition(worldSize, screenSize);

            //return the body temperature
            return playerNode.bodyTemperature;
        }

        //unload contents
        public void Unload()
        {
            platforms = new List<Platform>();
            portals = new List<Portal>();
            ladders = new List<Ladder>();
        }

        //draw funtion
        public void Draw(int framesPerSecond)
        {
            spriteBatch.Begin();
            //draw the desired nodes onto screen through the camera
            camera.DrawNode(backgroundNode);
            camera.DrawNode(playerNode);
            //camera.DrawNode(shadowFilter);
            //draw the platforms
            foreach (Platform platform in platforms)
            {
                camera.DrawPlatform(platform);
            }

            //draw the portals
            foreach (Portal portal in portals)
            {
                camera.DrawPortal(portal);
            }

            foreach (Ladder ladder in ladders)
            {
                camera.DrawLadder(ladder);
            }

            //camera.DrawPlatform(platforms[0]);
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

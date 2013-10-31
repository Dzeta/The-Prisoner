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
    public class Prototype_Level_3
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
        bool filterOn = true, generatorOn = false;
        float filterScale = 1;
        Interactable lightSwitch, generator;
        PickUpItem staminaBooster;

        //declare constructor
        public Prototype_Level_3(SpriteBatch spriteBatch, Vector2 screenSize)
        {
            this.spriteBatch = spriteBatch;
            platforms = new List<Platform>();
            this.screenSize = screenSize;
            portals = new List<Portal>();
        }

        //load content
        public void LoadContent(ContentManager Content, Game_Level gameLevel, Game_Level prevGameLevel, double bodyTemperature, double stamina, double staminaLimit)
        {
            //load the needed textures
            Texture2D playerTexture = Content.Load<Texture2D>("PlayerSpriteSheet");
            Texture2D backgroundTexture = Content.Load<Texture2D>("background3");
            statusDisplayTexture = Content.Load<Texture2D>("statusDisplay");


            //initialize the world size and the ground coordinate according to the world size
            worldSize = new Vector2(backgroundTexture.Width, backgroundTexture.Height);
            ground = worldSize.Y;

            //load font
            font = Content.Load<SpriteFont>("Score");

            //initialize the needed nodes and camera
            //playerNode = new Scene2DNode(playerTexture, new Vector2(35, worldSize.Y - 64));
            backgroundNode = new Scene2DNode(backgroundTexture, new Vector2(0, 0));
            shadowFilter = new Scene2DNode(Content.Load<Texture2D>("shadowFilterLarge"), new Vector2(0, 0));
            camera = new Camera2D(spriteBatch);
            camera.cameraPosition = new Vector2(0, worldSize.Y - screenSize.Y);

            //initialize the needed platforms
            Texture2D platformTexture = Content.Load<Texture2D>("platformTexture");

            //initialize the platforms and add them to the list
            createPlatforms(platformTexture);

            //initialize the needed portals
            backwardDoor = new Portal(platformTexture, new Vector2(0, 286), new Vector2(32, 64), Portal.PortalType.BACKWARD);
            fowardDoor = new Portal(platformTexture, new Vector2(worldSize.X - 32, 351), new Vector2(32, 64), Portal.PortalType.FOWARD);
            portals.Add(backwardDoor);
            portals.Add(fowardDoor);

            //initialize the playerNode
            if (prevGameLevel <= gameLevel)
            {
                playerNode = new Scene2DNode(playerTexture, new Vector2(backwardDoor.position.X + backwardDoor.size.X + 5, 286), bodyTemperature, stamina, staminaLimit, 4, 5);
            }
            else if (prevGameLevel >= gameLevel)
            {
                playerNode = new Scene2DNode(playerTexture, new Vector2(fowardDoor.position.X - 32 - 5, 316), bodyTemperature, stamina, staminaLimit, 4, 5);
            }

            staminaBooster = new PickUpItem(platformTexture, new Vector2(65, 1800), new Vector2(28, 28), PickUpItem.ItemType.STAMINA, 100, PickUpItem.ItemEffectDuration.TEMPORARY);
            lightSwitch = new Interactable(platformTexture, new Vector2(2000, 475), new Vector2(31, 43), Interactable.Type_Of_Interactable.LIGHT_SWITCH);
            generator = new Interactable(Content.Load<Texture2D>("generator_off"), new Vector2(1935, worldSize.Y - 63), new Vector2(103, 63), Interactable.Type_Of_Interactable.GENERATOR, Content.Load<Texture2D>("generator_on"));
        }

        private void createPlatforms(Texture2D platformTexture)
        {
            Vector2 sizeBasePlatform = new Vector2(120, 15);
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(0, 350)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(372, 273)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(712, 220)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1014, 330)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1182, 216)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1336, 370)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1968, 526)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1484, 536)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1000, 550)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(765, 453)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(466, 392)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(540, 550)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(206, 448)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(86, 560)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(0, 805)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(128, 708)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(325, 660)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(560, 790)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(808, 680)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1025, 790)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1225, 628)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1342, 800)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1560, 755)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1765, 650)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1800, 970)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1650, 880)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1260, 955)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(995, 1033)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(747, 962)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(495, 1037)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(290, 890)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(68, 1015)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(288, 1210)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(810, 1150)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1295, 1255)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1410, 1110)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1770, 1230)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1995, 1165)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(2018, 1050)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1610, 1364)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(765, 1365)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(528, 1292)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(50, 1285)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(272, 1555)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(666, 1642)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(952, 1506)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1213, 1528)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1455, 1500)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1626, 1692)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1720, 1565)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1636, 1922)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1444, 1815)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1085, 1750)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(0, 1845)));

            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1925, 570)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1375, 630)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1310, 560)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1230, 430)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1130, 495)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(890, 612)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(870, 390)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(643, 348)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(613, 655)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(365, 513)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(683, 864)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(887, 891)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1120, 885)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1430, 970)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1508, 840)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1435, 707)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1820, 810)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1860, 880)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1925, 762)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1950, 695)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(2010, 1115)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1860, 1023)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1610, 1040)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1600, 1230)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1430, 1300)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1150, 1144)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1075, 1330)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(925, 1245)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(617, 1205)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1520, 1622)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1840, 1485)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1800, 1395)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1863, 1779)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1777, 1688)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1755, 1860)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1530, 1960)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(165, 1905)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(295, 1725)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(420, 1787)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(515, 1867)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(615, 1940)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(752, 1967)));

            platforms.Add(new Platform(platformTexture, new Vector2(50, 15), new Vector2(1592, 472)));
            platforms.Add(new Platform(platformTexture, new Vector2(50, 15), new Vector2(1737, 468)));
            platforms.Add(new Platform(platformTexture, new Vector2(240, 15), new Vector2(1810, 415)));
        }

        //unload contents
        public void Unload()
        {
            platforms = new List<Platform>();
            portals = new List<Portal>();
        }

        //update function
        public double Update(GameTime gameTime, ref float bodyTempTimer, ref float exhaustionTimer, ref KeyboardState oldKeyboardState, ref float jumpTimer, ref Game_Level gameLevel, ref float staminaExhaustionTimer, ref double bodyTemperature, ref double stamina, ref double staminaLimit)
        {
            //update the player position with respect to keyboard input and platform collision
            Vector2 prevPosition = playerNode.position;
            bool useLighter = filterOn;
            playerNode.Update(useLighter, gameTime, ref bodyTempTimer, ref exhaustionTimer, ref oldKeyboardState, ref jumpTimer, ground, platforms, null, worldSize, ref staminaExhaustionTimer);

            //Check the player's collision with the world boundaries
            if (playerNode.position.X < 0 || playerNode.position.X + playerNode.playerSpriteSize.X > worldSize.X)
            {
                playerNode.position.X = prevPosition.X;
            }

            //update portals
            foreach (Portal portal in portals)
            {
                portal.Update(playerNode, ref gameLevel);
            }

            lightSwitch.Update(playerNode, ref generatorOn, ref filterOn, ref filterScale);
            generator.Update(playerNode, ref generatorOn, ref filterOn, ref filterScale);

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
            //draw the desired nodes onto screen through the camera
            camera.DrawNode(backgroundNode);

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
            camera.DrawPickUpItem(staminaBooster);
            camera.DrawInteractable(lightSwitch);
            camera.DrawInteractable(generator);
            camera.DrawPlayerNode(playerNode);



            //camera.DrawPlatform(platforms[0]);
            //camera.DrawNode(shadowFilter);
            if (filterOn)
            {
                camera.DrawFilter(shadowFilter, filterScale);
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

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
            Texture2D playerTexture = Content.Load<Texture2D>("PlayerSpriteSheet");
            Texture2D backgroundTexture = Content.Load<Texture2D>("engineroom_bg");
            statusDisplayTexture = Content.Load<Texture2D>("statusDisplay");

            //initialize the world size and the ground coordinate according to the world size
            worldSize = new Vector2(backgroundTexture.Width, backgroundTexture.Height);
            ground = worldSize.Y - 50;

            //load font
            font = Content.Load<SpriteFont>("Score");

            //initialize the needed nodes and camera
            //playerNode = new Scene2DNode(playerTexture, new Vector2(35, worldSize.Y - 64));
            backgroundNode = new GenericSprite2D(backgroundTexture, new Vector2(0, 0), Rectangle.Empty);
            worldObjects.Add(backgroundNode);
            shadowFilter = new Filter(Content.Load<Texture2D>("shadowFilterLarge"), new Vector2(0, 0));
            camera = new Camera2D(spriteBatch);
            camera.cameraPosition = new Vector2(0, worldSize.Y - screenSize.Y);

            reactor = new Reactor(Content, new Vector2(598, 468));
            worldObjects.Add(reactor);

            //initialize the needed platforms
            Texture2D platformTexture = Content.Load<Texture2D>("platformTexture");

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
                playerNode = new Character(playerTexture, new Vector2(backwardDoor.position.X + backwardDoor.size.X + 5, 116), bodyTemperature, stamina, staminaLimit, 4, 5);
            }
            else if (prevGameLevel >= gameLevel)
            {
                playerNode = new Character(playerTexture, new Vector2(forwardDoor.position.X - 32 - 5, 116), bodyTemperature, stamina, staminaLimit, 4, 5);
            }

            staminaBooster = new PickUpItem(platformTexture, new Vector2(165, 1750), new Vector2(28, 28), PickUpItem.ItemType.STAMINA, 100, PickUpItem.ItemEffectDuration.TEMPORARY);
            lightSwitch = new Interactable(Content.Load<Texture2D>("lightswitch"), new Vector2(1900, 425), new Vector2(23, 32), Interactable.Type_Of_Interactable.LIGHT_SWITCH);
            generator = new Interactable(Content.Load<Texture2D>("generator_off"), new Vector2(worldSize.X - 246, ground - 63), new Vector2(103, 63), Interactable.Type_Of_Interactable.GENERATOR, Content.Load<Texture2D>("generator_on"));
            doorSwitch = new Interactable(Content.Load<Texture2D>("doorswitch"), new Vector2(1120, 1650), new Vector2(11, 19), Interactable.Type_Of_Interactable.DOOR_SWITCH);

            worldObjects.Add(staminaBooster);
            worldObjects.Add(lightSwitch);
            worldObjects.Add(generator);
            worldObjects.Add(doorSwitch);

            worldObjects.Add(playerNode);
        }

        private void createPlatforms(Texture2D platformTexture)
        {
            Vector2 sizeBasePlatform = new Vector2(120, 15);
            platforms.Add(new MovingPlatform(platformTexture, sizeBasePlatform, new Vector2(130, 400), new Vector2(0, 0.3f), new Vector2(100, 100)));
            platforms.Add(new MovingPlatform(platformTexture, sizeBasePlatform, new Vector2(100, 580), new Vector2(-0.3f, 0), new Vector2(100, 100)));
            platforms.Add(new Platform(platformTexture, new Vector2(420, 20), new Vector2(100, 180)));
            platforms.Add(new Platform(platformTexture, new Vector2(420, 20), new Vector2(1520, 180)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(472, 223)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(812, 170)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1014, 280)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1182, 166)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1236, 350)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1868, 476)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1384, 486)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1000, 500)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(865, 403)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(566, 342)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(640, 500)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(380, 398)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(100, 755)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(228, 658)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(425, 610)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(660, 740)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(908, 630)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1025, 740)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1125, 578)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1242, 750)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1460, 705)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1665, 600)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1700, 930)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1550, 830)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1260, 905)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(995, 983)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(847, 912)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(595, 987)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(390, 840)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(168, 965)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(388, 1160)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(910, 1100)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1195, 1205)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1310, 1060)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1670, 1180)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1895, 1115)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1918, 1000)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1510, 1314)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(865, 1315)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(628, 1242)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(150, 1235)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(372, 1505)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(766, 1592)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(952, 1456)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1113, 1478)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1355, 1450)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1526, 1642)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1620, 1515)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1536, 1872)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1344, 1765)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1085, 1700)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(100, 1795)));

            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1825, 520)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1275, 580)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1210, 510)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1130, 380)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1030, 445)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(990, 562)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(970, 340)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(743, 498)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(713, 605)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(465, 463)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(783, 814)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(987, 841)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1020, 835)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1330, 920)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1408, 790)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1335, 657)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1720, 760)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1760, 830)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1825, 712)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1850, 645)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1910, 1065)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1760, 973)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1510, 990)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1500, 1180)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1330, 1250)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1050, 1094)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1075, 1280)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1025, 1195)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(717, 1155)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1420, 1572)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1740, 1435)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1700, 1345)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1763, 1729)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1677, 1638)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1655, 1810)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(1430, 1910)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(265, 1855)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(395, 1675)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(520, 1737)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(615, 1817)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(715, 1890)));
            platforms.Add(new Platform(platformTexture, sizeBasePlatform, new Vector2(852, 1917)));

            platforms.Add(new Platform(platformTexture, new Vector2(50, 15), new Vector2(1492, 422)));
            platforms.Add(new Platform(platformTexture, new Vector2(50, 15), new Vector2(1547, 344)));
            platforms.Add(new Platform(platformTexture, new Vector2(50, 15), new Vector2(1447, 283)));
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
            //update the player position with respect to keyboard input and platform collision
            Vector2 prevPosition = playerNode.position;
            bool useLighter = filterOn;
            playerNode.Update(useLighter, gameTime, ref bodyTempTimer, ref exhaustionTimer, ref oldKeyboardState, ref jumpTimer, ground, platforms, null, worldSize, ref staminaExhaustionTimer);
            reactor.Update(gameTime);

            //Check the player's collision with the world boundaries
            if (playerNode.position.X < 100 || playerNode.position.X + playerNode.playerSpriteSize.X > worldSize.X - 100)
            {
                playerNode.position.X = prevPosition.X;
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
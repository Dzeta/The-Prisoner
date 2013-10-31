#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Cold_Ship
{
    //declare the enum for game levels
    public enum Game_Level { LEVEL_HOLDING_CELL, PROTOTYPE, LEVEL1, LEVEL2 };

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        //declare needed global variables, commented out variables are no longer used
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //Scene2DNode playerNode, backgroundNode;
        public Vector2 screenSize { get; set; }/*, worldSize*/

        // Freeze but display the regular screen
        // Used for dialogue
        // A few unique state that the game can be, these states are linear
        // Which means they cannot be combined togheter with one another
        public enum GameState { Frozen, Paused, Ended, Playing, Initialized }
        private Stack<GameState> _gameState; 
        //SpriteFont font;
        //Texture2D statusDisplayTexture;
        float bodyTempTimer;
        float exhaustionTimer, staminaExhaustionTimer;
        float jumpTimer;
        KeyboardState oldKeyboardState;
        //float ground;
        //Scene2DNode shadowFilter;
        //List<Platform> platforms;
        Prototype_Level prototypeLevel;
        Prototype_Level_2 prototypeLevel2;
        Prototype_Level_3 prototypeLevel3;
        Level_Holding_Cell levelHoldingCell;
        Game_Level gameLevel = Game_Level.LEVEL_HOLDING_CELL;
        Game_Level prevGameLevel = Game_Level.LEVEL_HOLDING_CELL;

        double bodyTemperature = 36;
        double stamina = 100;
        double staminaLimit = 100;

        public Game1() : base() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
          this._gameState = new Stack<GameState>();
          this._gameState.Push(GameState.Initialized);

            
        }

        // Bunch of helper methods to deal with the state of the game at any moment
        public GameState GetCurrentGameState() { return _gameState.Peek(); }
        private void _setCurrentGameState(GameState state) { this._gameState.Push(state); }
        public void ActivateState(GameState state) { this._setCurrentGameState(state); }
        public GameState RestoreLastState() { return this._gameState.Pop(); }
        public bool IsGameState(GameState state) { return this.GetCurrentGameState() == state; }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //initiate screen size
            screenSize = new Vector2(800, 600);
            graphics.PreferredBackBufferWidth = (int)screenSize.X;
            graphics.PreferredBackBufferHeight = (int)screenSize.Y;
            //graphics.IsFullScreen = true;

            //initiate the timers
            bodyTempTimer = 0;
            exhaustionTimer = 0;
            jumpTimer = 0;
            staminaExhaustionTimer = 0;

            //initiate old keyboard state
            oldKeyboardState = Keyboard.GetState();

            //initialize the needed lists
            //platforms = new List<Platform>();

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            prototypeLevel = new Prototype_Level(spriteBatch, screenSize);
            prototypeLevel2 = new Prototype_Level_2(spriteBatch, screenSize);
            prototypeLevel3 = new Prototype_Level_3(spriteBatch, screenSize);
            levelHoldingCell = new Level_Holding_Cell(this, spriteBatch, screenSize);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            //spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            ////load the needed textures
            //Texture2D playerTexture = Content.Load<Texture2D>("player");
            //Texture2D backgroundTexture = Content.Load<Texture2D>("background2");
            //statusDisplayTexture = Content.Load<Texture2D>("statusDisplay");


            ////initialize the world size and the ground coordinate according to the world size
            //worldSize = new Vector2(backgroundTexture.Width, backgroundTexture.Height);
            //ground = worldSize.Y;

            ////load font
            //font = Content.Load<SpriteFont>("Score");
            
            ////initialize the needed nodes and camera
            //playerNode = new Scene2DNode(playerTexture, new Vector2(0, worldSize.Y - 64));
            //backgroundNode = new Scene2DNode(backgroundTexture, new Vector2(0, 0));
            //shadowFilter = new Scene2DNode(Content.Load<Texture2D>("shadowFilterLarge"), new Vector2(0, 0));
            //camera = new Camera2D(spriteBatch);
            //camera.cameraPosition = new Vector2(0, worldSize.Y - screenSize.Y);

            ////initialize the needed platforms
            //Texture2D platformTexture = Content.Load<Texture2D>("platformTexture");
            //Platform platform = new Platform(platformTexture, new Vector2(64, 32), new Vector2(100, worldSize.Y - 80));
            //Platform platform2 = new Platform(platformTexture, new Vector2(64, 150), new Vector2(200, worldSize.Y - 150));
            //Platform platform3 = new Platform(platformTexture, new Vector2(100, 800), new Vector2(300, worldSize.Y - 800));
            //Platform platform4 = new Platform(platformTexture, new Vector2(80, 15), new Vector2(120, worldSize.Y - 250));
            //Platform platform5 = new Platform(platformTexture, new Vector2(80, 15), new Vector2(50, worldSize.Y - 350));
            //Platform platform6 = new Platform(platformTexture, new Vector2(80, 15), new Vector2(140, worldSize.Y - 450));
            //Platform platform7 = new Platform(platformTexture, new Vector2(80, 15), new Vector2(200, worldSize.Y - 550));
            //Platform platform8 = new Platform(platformTexture, new Vector2(80, 15), new Vector2(100, worldSize.Y - 650));
            //platforms.Add(platform);
            //platforms.Add(platform2);
            //platforms.Add(platform3);
            //platforms.Add(platform4);
            //platforms.Add(platform5);
            //platforms.Add(platform6);
            //platforms.Add(platform7);
            //platforms.Add(platform8);
            //platforms.Add(new Platform(platformTexture, new Vector2(80, 15), new Vector2(20, worldSize.Y - 750)));
            //platforms.Add(new Platform(platformTexture, new Vector2(80, 15), new Vector2(100, worldSize.Y - 850)));
            //platforms.Add(new Platform(platformTexture, new Vector2(80, 15), new Vector2(200, worldSize.Y - 950)));
            //platforms.Add(new Platform(platformTexture, new Vector2(500, 15), new Vector2(400, worldSize.Y - 960)));
            switch(gameLevel)
            {
                case Game_Level.PROTOTYPE:
                    prototypeLevel.LoadContent(Content, gameLevel, prevGameLevel, bodyTemperature, stamina, staminaLimit);
                    break;
                case Game_Level.LEVEL1:
                    prototypeLevel2.LoadContent(Content, gameLevel, prevGameLevel, bodyTemperature, stamina, staminaLimit);
                    break;
                case Game_Level.LEVEL2:
                    prototypeLevel3.LoadContent(Content, gameLevel, prevGameLevel, bodyTemperature, stamina, staminaLimit);
                    break;
                case Game_Level.LEVEL_HOLDING_CELL:
                    levelHoldingCell.LoadContent(Content, gameLevel, prevGameLevel, bodyTemperature, stamina, staminaLimit);
                    break;
                    
            }
            //prototypeLevel.LoadContent(Content);
            //prototypeLevel2.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            prototypeLevel.Unload();
            prototypeLevel2.Unload();
            prototypeLevel3.Unload();
            levelHoldingCell.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //outdated codes that's now in the Update method
            /*bodyTempTimer += gameTime.ElapsedGameTime.Milliseconds;
            exhaustionTimer += gameTime.ElapsedGameTime.Milliseconds;
            KeyboardState newKeyboardState = Keyboard.GetState();
            playerNode.UpdateKeyboard(oldKeyboardState, newKeyboardState);
            oldKeyboardState = newKeyboardState;
            playerNode.updateBodyTemperature(ref bodyTempTimer, ref exhaustionTimer);*/


            //playerNode.Update(gameTime, ref bodyTempTimer, ref exhaustionTimer, ref oldKeyboardState, ref jumpTimer, ground, platforms);
            //shadowFilter.position = new Vector2((playerNode.position.X /*+ (playerNode.texture.Width / 2))*/) - (shadowFilter.texture.Width / 2),
            //    (playerNode.position.Y + (playerNode.texture.Height / 2) - (shadowFilter.texture.Height / 2)));


            //update the camera based on the player and world size
            //camera.TranslateWithSprite(playerNode, screenSize);
            //camera.CapCameraPosition(worldSize, screenSize);
            
            switch(gameLevel)
            {
                case Game_Level.PROTOTYPE:
                    //LoadContent();
                    bodyTemperature = prototypeLevel.Update(gameTime, ref bodyTempTimer, ref exhaustionTimer, ref oldKeyboardState, ref jumpTimer, ref gameLevel, ref staminaExhaustionTimer, ref bodyTemperature, ref stamina, ref staminaLimit);
                    break;
                case Game_Level.LEVEL1:
                    //LoadContent();
                    bodyTemperature = prototypeLevel2.Update(gameTime, ref bodyTempTimer, ref exhaustionTimer, ref oldKeyboardState, ref jumpTimer, ref gameLevel, ref staminaExhaustionTimer, ref bodyTemperature, ref stamina, ref staminaLimit);
                    break;
                case Game_Level.LEVEL2:
                    bodyTemperature = prototypeLevel3.Update(gameTime, ref bodyTempTimer, ref exhaustionTimer, ref oldKeyboardState, ref jumpTimer, ref gameLevel, ref staminaExhaustionTimer, ref bodyTemperature, ref stamina, ref staminaLimit);
                    break;
                case Game_Level.LEVEL_HOLDING_CELL:
                    bodyTemperature = levelHoldingCell.Update(gameTime, ref bodyTempTimer, ref exhaustionTimer, ref oldKeyboardState, ref jumpTimer, ref gameLevel, ref staminaExhaustionTimer, ref bodyTemperature, ref stamina, ref staminaLimit); 
                    break;
            }

            if (prevGameLevel != gameLevel)
            {
                UnloadContent();
                LoadContent();
                prevGameLevel = gameLevel;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        //varaibles for capturing the fps
        int frames = 0, framesPerSecond = 0;
        float fpsTimer = 0;
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            //fps calculations
            fpsTimer += gameTime.ElapsedGameTime.Milliseconds;
            frames++;
            if (fpsTimer >= 1000)
            {
                fpsTimer = 0;
                framesPerSecond = frames;
                frames = 0;
            }

            //spriteBatch.Begin();
            ////draw the desired nodes onto screen through the camera
            //camera.DrawNode(backgroundNode);
            //camera.DrawNode(playerNode);
            ////camera.DrawNode(shadowFilter);
            ////draw the platforms
            //foreach (Platform platform in platforms)
            //{
            //    camera.DrawPlatform(platform);
            //}
            //camera.DrawPlatform(platforms[0]);
            //camera.DrawNode(shadowFilter);
            ////draw the fps
            //spriteBatch.DrawString(font, framesPerSecond.ToString(), new Vector2(screenSize.X - 50, 25), Color.White);
            ////draw the status display and the body temperature
            //spriteBatch.Draw(statusDisplayTexture, new Vector2(50, 50), Color.White);
            //spriteBatch.DrawString(font, Math.Round(playerNode.bodyTemperature, 2).ToString(), new Vector2(52, 52), Color.Black, 0, new Vector2(0, 0), new Vector2(0.8f, 2), SpriteEffects.None, 0);
            //spriteBatch.End();

            switch (gameLevel)
            {
                case Game_Level.PROTOTYPE:
                    prototypeLevel.Draw(framesPerSecond);
                    break;
                case Game_Level.LEVEL1:
                    prototypeLevel2.Draw(framesPerSecond);
                    break;
                case Game_Level.LEVEL2:
                    prototypeLevel3.Draw(framesPerSecond);
                    break;
                case Game_Level.LEVEL_HOLDING_CELL:
                    levelHoldingCell.Draw(framesPerSecond);
                    break;
            }
            

            base.Draw(gameTime);
        }
    }
}

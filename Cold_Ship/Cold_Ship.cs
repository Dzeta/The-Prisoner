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
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using GamePad = Microsoft.Xna.Framework.Input.GamePad;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using KeyboardState = Microsoft.Xna.Framework.Input.KeyboardState;

#endregion

namespace Cold_Ship
{
    //declare the enum for game levels
    public enum Game_Level { LEVEL_HOLDING_CELL, LEVEL_PRISON_BLOCKS, LEVEL_GENERATOR, LEVEL_COMMON_ROOM };

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Cold_Ship : Game
    {
        public const bool DEBUG_MODE = true;

        //declare needed global variables, commented out variables are no longer used
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //Scene2DNode playerNode, backgroundNode;
        public Vector2 screenSize { get; set; }/*, worldSize*/
        public Texture2D DebugTexture { get; set; }

        // Freeze but display the regular screen
        // Used for dialogue
        // A few unique state that the game can be, these states are linear
        // Which means they cannot be combined togheter with one another
        // Only frozen is implemented so far, pause still needs work
        public enum GameState { FROZEN, DIALOGUING, PAUSED, ENDED, PLAYING, INTIALIZED }
        private Stack<GameState> _gameState;
        // DIALOGUE USED COMPOENTS
        public List<DialogueBubble> DialogueQueue { get; set; }

        //SpriteFont font;
        //Texture2D statusDisplayTexture;
        float bodyTempTimer;
        float exhaustionTimer, staminaExhaustionTimer;
        float jumpTimer;
        KeyboardState oldKeyboardState;
        Level_Prison_Block prototypeLevel1;
        Level_Generator prototypeLevel2;
        Level_Holding_Cell levelHoldingCell;
        Level_Common_Room levelCommonRoom;

        Game_Level gameLevel = Game_Level.LEVEL_HOLDING_CELL;
        Game_Level prevGameLevel = Game_Level.LEVEL_HOLDING_CELL;


        double bodyTemperature = 36;
        double stamina = 100;
        double staminaLimit = 100;

        public Cold_Ship()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this._gameState = new Stack<GameState>();
            this._gameState.Push(GameState.INTIALIZED);
            this._gameState.Push(GameState.PLAYING);
        }

        // Bunch of helper methods to deal with the state of the game at any moment
        public GameState GetCurrentGameState() { return _gameState.Peek(); }
        private void _setCurrentGameState(GameState state) { this._gameState.Push(state); }
        public void ActivateState(GameState state) { this._setCurrentGameState(state); }
        public GameState RestoreLastState() { return this._gameState.Pop(); }
        public bool GameStateIs(GameState state) { return this.GetCurrentGameState() == state; }


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

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            prototypeLevel1 = new Level_Prison_Block(spriteBatch, screenSize);
            prototypeLevel2 = new Level_Generator(spriteBatch, screenSize);
            levelHoldingCell = new Level_Holding_Cell(this, spriteBatch, screenSize);
            levelCommonRoom = new Level_Common_Room(spriteBatch, screenSize);


            // DIALOGUE USED COMPONENT
            this.DialogueQueue = new List<DialogueBubble>();
            DebugTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //switch (gameLevel)
            //{
            //  case Game_Level.LEVEL_PRISON_BLOCKS:
            prototypeLevel1.LoadContent(Content, gameLevel, prevGameLevel, bodyTemperature, stamina, staminaLimit);
            //    break;
            //  case Game_Level.LEVEL_GENERATOR:
            prototypeLevel2.LoadContent(Content, gameLevel, prevGameLevel, bodyTemperature, stamina, staminaLimit);
            //    break;
            //  case Game_Level.LEVEL_HOLDING_CELL:
            levelHoldingCell.LoadContent(Content, gameLevel, prevGameLevel, bodyTemperature, stamina, staminaLimit);
            //    break;
            //  case Game_Level.LEVEL_COMMON_ROOM:
            levelCommonRoom.LoadContent(Content, gameLevel, prevGameLevel, bodyTemperature, stamina, staminaLimit);
            //    break;
            //}
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            prototypeLevel1.Unload();
            prototypeLevel2.Unload();
            levelHoldingCell.Unload();
            levelCommonRoom.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (this.GameStateIs(GameState.DIALOGUING))
            {
                foreach (DialogueBubble dialogue in DialogueQueue)
                {
                    if (dialogue.IsPlaying())
                    {
                        dialogue.Update(gameTime);
                        _ListenForChatSpeedThrough(dialogue);
                        break;
                    }
                }

                return;
            }
            else if (this.GameStateIs(GameState.PLAYING))
            {
                switch (gameLevel)
                {
                    case Game_Level.LEVEL_PRISON_BLOCKS:
                        bodyTemperature = prototypeLevel1.Update(gameTime, ref bodyTempTimer, ref exhaustionTimer, ref oldKeyboardState, ref jumpTimer, ref gameLevel, ref staminaExhaustionTimer, ref bodyTemperature, ref stamina, ref staminaLimit);
                        break;
                    case Game_Level.LEVEL_GENERATOR:
                        bodyTemperature = prototypeLevel2.Update(gameTime, ref bodyTempTimer, ref exhaustionTimer, ref oldKeyboardState, ref jumpTimer, ref gameLevel, ref staminaExhaustionTimer, ref bodyTemperature, ref stamina, ref staminaLimit);
                        break;
                    case Game_Level.LEVEL_HOLDING_CELL:
                        bodyTemperature = levelHoldingCell.Update(gameTime, ref bodyTempTimer, ref exhaustionTimer, ref oldKeyboardState, ref jumpTimer, ref gameLevel, ref staminaExhaustionTimer, ref bodyTemperature, ref stamina, ref staminaLimit);
                        break;
                    case Game_Level.LEVEL_COMMON_ROOM:
                        bodyTemperature = levelCommonRoom.Update(gameTime, ref bodyTempTimer, ref exhaustionTimer, ref oldKeyboardState, ref jumpTimer, ref gameLevel, ref staminaExhaustionTimer, ref bodyTemperature, ref stamina, ref staminaLimit);
                        break;
                }

                if (prevGameLevel != gameLevel)
                {
                    prevGameLevel = gameLevel;
                }
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

            switch (gameLevel)
            {
                case Game_Level.LEVEL_PRISON_BLOCKS:
                    prototypeLevel1.Draw(framesPerSecond);
                    break;
                case Game_Level.LEVEL_GENERATOR:
                    prototypeLevel2.Draw(framesPerSecond);
                    break;
                case Game_Level.LEVEL_HOLDING_CELL:
                    levelHoldingCell.Draw(framesPerSecond);
                    break;
                case Game_Level.LEVEL_COMMON_ROOM:
                    levelCommonRoom.Draw(framesPerSecond);
                    break;
            }

            // Putting dialogue here cause they need to be appearing on top of everything
            if (this.GameStateIs(GameState.DIALOGUING))
            {
                spriteBatch.Begin();
                foreach (DialogueBubble dialogue in this.DialogueQueue)
                {
                    if (dialogue.IsPlaying())
                    {
                        dialogue.Draw(spriteBatch);
                        break;
                    }
                }
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        private void _ListenForChatSpeedThrough(DialogueBubble currentSpeech)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                currentSpeech.SetPlayThroughSpeed(1);
        }
    }
}

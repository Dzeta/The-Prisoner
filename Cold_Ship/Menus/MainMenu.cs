using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Cold_Ship
{
    class MainMenu : Menu
    {
        private KeyboardState keyboardState;
        private KeyboardState previousKeyboardState;
        private Cold_Ship game;

        public MainMenu(Cold_Ship game, Texture2D background, Texture2D cursorTexture, SpriteFont font, Cue menuClick)
            : base(background, cursorTexture, font, 4, menuClick)
        {
            this.optionMenu = new String[4] { "Start Playing ", "Key Binding", "HighScore (do we need highscores ?)", "Exit" };
            this.optionPosition = new Vector2[4] { new Vector2(350, 260), new Vector2(350, 290), new Vector2(350, 320), new Vector2(350, 350) };
            this.game = game;
            previousKeyboardState = Keyboard.GetState();
        }

        public override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter))
            {
                switch (selectedOption)
                {
                    case 0:
                        game.ActivateState(Cold_Ship.GameState.PLAYING);
                        break;
                    case 1:
                        game.ActivateState(Cold_Ship.GameState.KEY_BINDING);
                        break;
                    case 2:
                        //game.ActivateState(Cold_Ship.GameState.PLAYING);
                        break;
                    case 3:
                        game.Exit();
                        break;
                }
                selectedOption = 0;
            }
            previousKeyboardState = keyboardState;
            base.Update(gameTime);
        }

    }
}

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
    class PauseMenu : Menu
    {
        private KeyboardState keyboardState;
        private KeyboardState previousKeyboardState;
        private Cold_Ship game;

        public PauseMenu(Cold_Ship game, Texture2D background, Texture2D cursorTexture, SpriteFont font, Cue menuClick)
            : base(background, cursorTexture, font, 3, menuClick)
        {
            this.optionMenu = new String[3] { "Resume", "Key Binding", "Go back to Main Menu" };
            this.optionPosition = new Vector2[3] { new Vector2(350, 290), new Vector2(350, 320), new Vector2(350, 350) };
            this.game = game;
        }

        public override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter))
            {
                switch (selectedOption)
                {
                    case 0:
                        game.RestoreLastState();
                        selectedOption = 0;
                        break;
                    case 1:
                        game.ActivateState(Cold_Ship.GameState.KEY_BINDING);
                        selectedOption = 0;
                        break;
                    case 2:
                        game.ActivateState(Cold_Ship.GameState.MENU);
                        selectedOption = 0;
                        break;
                }
            }
            previousKeyboardState = keyboardState;
            base.Update(gameTime);
        }

    }
}

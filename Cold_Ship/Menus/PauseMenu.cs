//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Audio;

//namespace Cold_Ship
//{
//    class PauseMenu : Menu
//    {
//        private KeyboardState keyboardState;
//        private KeyboardState previousKeyboardState;

//        public PauseMenu(Texture2D background, Texture2D cursorTexture, SpriteFont font, Cue menuClick)
//            : base(background, cursorTexture, font, 2, menuClick)
//        {
//            this.optionMenu = new String[2] { "Resume", "Go back to Main Menu" };
//            this.optionPosition = new Vector2[2] { new Vector2(350, 290), new Vector2(350, 320) };
//        }

//        public override void Update(GameTime gameTime, ref Cold_Ship.State currentState)
//        {
//            keyboardState = Keyboard.GetState();

//            if (keyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter))
//            {
//                switch (selectedOption)
//                {
//                    case 0:
//                        currentState = Cold_Ship.State.IN_GAME;
//                        selectedOption = 0;
//                        break;
//                    case 1:
//                        currentState = Cold_Ship.State.MAIN_MENU;
//                        selectedOption = 0;
//                        break;
//                }
//            }
//            previousKeyboardState = keyboardState;
//            base.Update(gameTime, ref currentState);
//        }

//    }
//}

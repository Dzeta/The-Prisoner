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
//    class MainMenu : Menu
//    {
//        private KeyboardState keyboardState;
//        private KeyboardState previousKeyboardState;

//        public MainMenu(Texture2D background, Texture2D cursorTexture, SpriteFont font, Cue menuClick)
//            : base(background, cursorTexture, font, 4, menuClick)
//        {
//            this.optionMenu = new String[4] { "Start Playing ", "Key Binding", "HighScore", "Exit" };
//            this.optionPosition = new Vector2[4] { new Vector2(350, 260), new Vector2(350, 290), new Vector2(350, 320), new Vector2(350, 350) };
//            previousKeyboardState = Keyboard.GetState();
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
//                        break;
//                    case 1:
//                        currentState = Cold_Ship.State.KEY_BINDING;
//                        break;
//                    case 2:
//                        currentState = Cold_Ship.State.HIGHSCORE;
//                        break;
//                    case 3:
//                        currentState = Cold_Ship.State.EXIT;
//                        break;
//                }
//                selectedOption = 0;
//            }
//            previousKeyboardState = keyboardState;
//            base.Update(gameTime, ref currentState);
//        }

//    }
//}

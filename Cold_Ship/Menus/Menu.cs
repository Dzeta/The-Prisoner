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
//    public abstract class Menu
//    {
//        private Texture2D backgroundTexture;
//        private Texture2D cursorTexture;
//        private Vector2 cursorPositionDiff = new Vector2(40, 0);

//        protected int selectedOption = 0;
//        private int nOptions;
//        protected int NOptions { get { return nOptions; } }

//        protected SpriteFont font;
//        private Color fontColor = Color.White;

//        private bool alphaDown = true;

//        private Cue menuClick;

//        protected String[] optionMenu;
//        protected Vector2[] optionPosition;

//        private KeyboardState previousKeyboardState;
//        private KeyboardState keyboardState;

//        public Menu(Texture2D background, Texture2D cursorTexture, SpriteFont font, int nOptions, Cue menuClick)
//        {
//            backgroundTexture = background;
//            this.cursorTexture = cursorTexture;
//            this.font = font;
//            this.nOptions = nOptions;
//            this.menuClick = menuClick;
//            previousKeyboardState = Keyboard.GetState();
//        }

//        public virtual void Update(GameTime gameTime, ref Cold_Ship.State currentState)
//        {
//            keyboardState = Keyboard.GetState();

//            if (keyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down) && selectedOption < nOptions - 1)
//                {
//                    selectedOption++;
//                    fontColor = Color.White;
//                    menuClick.Play();
//                }
//            else if (keyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up) && selectedOption > 0)
//                {
//                    selectedOption--;
//                    fontColor = Color.White;
//                    menuClick.Play();
//                }

//            previousKeyboardState = keyboardState;

//            UpdateFont(3);
//        }

//        protected void UpdateFont(byte colorChange)
//        {
//            if (fontColor.R == 255)
//                alphaDown = true;
//            else if (fontColor.R == 0)
//                alphaDown = false;
//            if (alphaDown)
//                fontColor.R = fontColor.G = (fontColor.B -= colorChange);
//            else
//                fontColor.R = fontColor.G = (fontColor.B += colorChange);
//        }

//        public virtual void Draw(SpriteBatch spriteBatch)
//        {
//            spriteBatch.Draw(backgroundTexture, new Vector2(0, 0), Color.White);

//            for (int i = 0; i < nOptions; i++)
//            {
//                if (i == selectedOption)
//                    spriteBatch.DrawString(font, optionMenu[i], optionPosition[i], fontColor);
//                else
//                    spriteBatch.DrawString(font, optionMenu[i], optionPosition[i], Color.White);
//            }

//            spriteBatch.Draw(cursorTexture, optionPosition[selectedOption] - cursorPositionDiff, Color.White);
//        }

//    }
//}

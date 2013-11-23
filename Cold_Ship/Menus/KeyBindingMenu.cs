﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Cold_Ship
{
    class KeyBindingMenu : Menu
    {
        private KeyboardState keyboardState;
        private KeyboardState previousKeyboardState;

        private Cold_Ship game;

        private bool changingKey = false;

        private float timer = 0;
        private float interval = 80;

        public KeyBindingMenu(Cold_Ship game, Texture2D background, Texture2D cursorTexture, SpriteFont font, Cue menuClick)
            : base(background, cursorTexture, font, 8, menuClick)
        {
            this.optionMenu = new String[8] {   "Move Right       ............ " + HelperFunction.KeyRight.ToString(), 
                                                "Move Left        ............ " + HelperFunction.KeyLeft.ToString(),
                                                "Climb Up         ............ " + HelperFunction.KeyUp.ToString(),
                                                "Climb Down/Duck  ............ " + HelperFunction.KeyDown.ToString(),
                                                "Jump             ............ " + HelperFunction.KeyJump.ToString(),
                                                "Use              ............ " + HelperFunction.KeyUse.ToString(),
                                                "Run              ............ " + HelperFunction.KeySpeed.ToString(),
                                                "Go back to Main Menu" };
            this.optionPosition = new Vector2[8] {  new Vector2(300, 230), new Vector2(300, 260), new Vector2(300, 290), new Vector2(300, 320), 
                                                    new Vector2(300, 350), new Vector2(300, 380), new Vector2(300, 410), new Vector2(300, 460) };
            this.game = game;

            previousKeyboardState = Keyboard.GetState();
        }

        public override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            if (timer < interval)
                timer += gameTime.ElapsedGameTime.Milliseconds;
            else
            {
                if (!changingKey)
                {
                    if (keyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter))
                    {
                        if (selectedOption == NOptions - 1)
                        {
                            game.RestoreLastState();
                            timer = 0;
                            selectedOption = 0;
                        }
                        else
                        {
                            changingKey = true;
                        }
                    }
                    base.Update(gameTime);
                }
                else
                {
                    if (previousKeyboardState.IsKeyUp(Keys.Enter))
                    {
                        if (keyboardState.GetPressedKeys().Length != 0)
                        {
                            Keys newKey = keyboardState.GetPressedKeys()[0];
                            if (!HelperFunction.UsedKeys().Contains(newKey))
                            {
                                switch (selectedOption)
                                {
                                    case 0:
                                            HelperFunction.KeyRight = keyboardState.GetPressedKeys()[0];
                                            this.optionMenu[0] = "Move Right         ............ " + HelperFunction.KeyRight.ToString();
                                            changingKey = false;
                                        break;
                                    case 1:
                                            HelperFunction.KeyLeft = keyboardState.GetPressedKeys()[0];
                                            this.optionMenu[1] = "Move Left          ............ " + HelperFunction.KeyLeft.ToString();
                                            changingKey = false;
                                        break;
                                    case 2:
                                            HelperFunction.KeyUp = keyboardState.GetPressedKeys()[0];
                                            this.optionMenu[2] = "Climb Up           ............ " + HelperFunction.KeyUp.ToString();
                                            changingKey = false;
                                        break;
                                    case 3:
                                            HelperFunction.KeyDown = keyboardState.GetPressedKeys()[0];
                                            this.optionMenu[3] = "Climb Down / Duck  ............ " + HelperFunction.KeyDown.ToString();
                                            changingKey = false;
                                        break;
                                    case 4:
                                            HelperFunction.KeyJump = keyboardState.GetPressedKeys()[0];
                                            this.optionMenu[4] = "Jump               ............ " + HelperFunction.KeyJump.ToString();
                                            changingKey = false;
                                        break;
                                    case 5:
                                            HelperFunction.KeyUse = keyboardState.GetPressedKeys()[0];
                                            this.optionMenu[5] = "Use                ............ " + HelperFunction.KeyUse.ToString();
                                            changingKey = false;
                                        break;
                                    case 6:
                                            HelperFunction.KeySpeed = keyboardState.GetPressedKeys()[0];
                                            this.optionMenu[6] = "Run                ............ " + HelperFunction.KeySpeed.ToString();
                                            changingKey = false;
                                        break;
                                }
                            }
                        }
                    }
                    base.UpdateFont(5);
                }
                previousKeyboardState = keyboardState;
            }
        }

    }
}

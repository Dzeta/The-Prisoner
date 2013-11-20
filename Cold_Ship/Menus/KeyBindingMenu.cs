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
//    class KeyBindingMenu : Menu
//    {
//        private KeyboardState keyboardState;
//        private KeyboardState previousKeyboardState;

//        private bool changingKey = false;

//        private float timer = 0;
//        private float interval = 80;

//        public KeyBindingMenu(Texture2D background, Texture2D cursorTexture, SpriteFont font, Cue menuClick)
//            : base(background, cursorTexture, font, 8, menuClick)
//        {
//            this.optionMenu = new String[8] {   "Go forward  ............ " + HelperClass.UpKey.ToString(), 
//                                                "Go backward ............ " + HelperClass.DownKey.ToString(),
//                                                "Go right    ............ " + HelperClass.RightKey.ToString(),
//                                                "Go left     ............ " + HelperClass.LeftKey.ToString(),
//                                                "Eat food    ............ " + HelperClass.UseKey.ToString(),
//                                                "Throw item  ............ " + HelperClass.ThrowKey.ToString(),
//                                                "Change view ............ " + HelperClass.ChangeCamKey.ToString(),
//                                                "Go back to Main Menu" };
//            this.optionPosition = new Vector2[8] {  new Vector2(300, 230), new Vector2(300, 260), new Vector2(300, 290), new Vector2(300, 320), 
//                                                    new Vector2(300, 350), new Vector2(300, 380), new Vector2(300, 410), new Vector2(300, 460) };
            
//            previousKeyboardState = Keyboard.GetState();
//        }

//        public override void Update(GameTime gameTime, ref Cold_Ship.State currentState)
//        {
//            keyboardState = Keyboard.GetState();

//            if (timer < interval)
//                timer += gameTime.ElapsedGameTime.Milliseconds;
//            else
//            {
//                if (!changingKey)
//                {
//                    if (keyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter))
//                    {
//                        if (selectedOption == NOptions-1)
//                        {
//                            currentState = Cold_Ship.State.MAIN_MENU;
//                            timer = 0;
//                            selectedOption = 0;
//                        }
//                        else
//                        {
//                            changingKey = true;
//                        }
//                    }
//                    base.Update(gameTime, ref currentState);
//                }
//                else
//                {
//                    if (previousKeyboardState.IsKeyUp(Keys.Enter))
//                    {
//                        if (keyboardState.GetPressedKeys().Length != 0)
//                        {
//                            Keys newKey = keyboardState.GetPressedKeys()[0];
//                            switch (selectedOption)
//                            {
//                                case 0:
//                                    if (newKey != HelperClass.DownKey && newKey != HelperClass.RightKey && newKey != HelperClass.LeftKey)
//                                    {
//                                        HelperClass.UpKey = keyboardState.GetPressedKeys()[0];
//                                        this.optionMenu[0] = "Go forward  ............ " + HelperClass.UpKey.ToString();
//                                        changingKey = false;
//                                    }
//                                    break;
//                                case 1:
//                                    if (newKey != HelperClass.UpKey && newKey != HelperClass.RightKey && newKey != HelperClass.LeftKey)
//                                    {
//                                        HelperClass.DownKey = keyboardState.GetPressedKeys()[0];
//                                        this.optionMenu[1] = "Go backward ............ " + HelperClass.DownKey.ToString();
//                                        changingKey = false;
//                                    }
//                                    break;
//                                case 2:
//                                    if (newKey != HelperClass.UpKey && newKey != HelperClass.DownKey && newKey != HelperClass.LeftKey)
//                                    {
//                                        HelperClass.RightKey = keyboardState.GetPressedKeys()[0];
//                                        this.optionMenu[2] = "Go right    ............ " + HelperClass.RightKey.ToString();
//                                        changingKey = false;
//                                    }
//                                    break;
//                                case 3:
//                                    if (newKey != HelperClass.UpKey && newKey != HelperClass.RightKey && newKey != HelperClass.DownKey)
//                                    {
//                                        HelperClass.LeftKey = keyboardState.GetPressedKeys()[0];
//                                        this.optionMenu[3] = "Go left     ............ " + HelperClass.LeftKey.ToString();
//                                        changingKey = false;
//                                    }
//                                    break;
//                                case 4:
//                                    if (newKey != HelperClass.UpKey && newKey != HelperClass.RightKey && newKey != HelperClass.DownKey)
//                                    {
//                                        HelperClass.UseKey = keyboardState.GetPressedKeys()[0];
//                                        this.optionMenu[4] = "Eat food    ............ " + HelperClass.UseKey.ToString();
//                                        changingKey = false;
//                                    }
//                                    break;
//                                case 5:
//                                    if (newKey != HelperClass.UpKey && newKey != HelperClass.RightKey && newKey != HelperClass.DownKey)
//                                    {
//                                        HelperClass.ThrowKey = keyboardState.GetPressedKeys()[0];
//                                        this.optionMenu[5] = "Throw item  ............ " + HelperClass.ThrowKey.ToString();
//                                        changingKey = false;
//                                    }
//                                    break;
//                                case 6:
//                                    if (newKey != HelperClass.UpKey && newKey != HelperClass.RightKey && newKey != HelperClass.DownKey)
//                                    {
//                                        HelperClass.ChangeCamKey = keyboardState.GetPressedKeys()[0];
//                                        this.optionMenu[6] = "Change view ............ " + HelperClass.ChangeCamKey.ToString();
//                                        changingKey = false;
//                                    }
//                                    break;
//                            }
//                        }
//                    }
//                    base.UpdateFont(5);
//                }
//                previousKeyboardState = keyboardState;
//            }
//        }

//    }
//}

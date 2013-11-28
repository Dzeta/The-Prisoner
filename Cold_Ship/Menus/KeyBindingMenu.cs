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
  class KeyBindingMenu : Menu
  {
    private KeyboardState keyboardState;
    private KeyboardState previousKeyboardState;

    private Cold_Ship game;

    private bool changingKey = false;
    private String[] leftOption;
    private float timer = 0;
    private float interval = 80;

    public KeyBindingMenu(Cold_Ship game, Texture2D background, Texture2D cursorTexture, SpriteFont font, Cue menuClick)
      : base(background, cursorTexture, font, 8, menuClick)
    {
      leftOption = new String[7] {
                "Move Right                 ............ ",
                "Move Left                    ............ ",
                "Climb Up                      ............ ",
                "Climb Down/Duck     ............ ",
                "Jump                             ............ ",
                "Use                                ............ ",
                "Run                                ............ " 
            };

      this.optionMenu = new String[8] {   leftOption[0] + HelperFunction.KeyRight.ToString(), 
                                                leftOption[1] + HelperFunction.KeyLeft.ToString(),
                                                leftOption[2] + HelperFunction.KeyUp.ToString(),
                                                leftOption[3] + HelperFunction.KeyDown.ToString(),
                                                leftOption[4] + HelperFunction.KeyJump.ToString(),
                                                leftOption[5] + HelperFunction.KeyUse.ToString(),
                                                leftOption[6] + HelperFunction.KeySpeed.ToString(),
                                                "Go back to Menu" };
      this.optionPosition = new Vector2[8] {  new Vector2(300, 280), new Vector2(300, 310), new Vector2(300, 340), new Vector2(300, 370), 
                                                    new Vector2(300, 400), new Vector2(300, 430), new Vector2(300, 460), new Vector2(350, 510) };
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
            if (keyboardState.IsKeyDown(Keys.Escape))
              changingKey = false;
            else
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
                      this.optionMenu[selectedOption] = leftOption[selectedOption] + HelperFunction.KeyRight.ToString();
                      changingKey = false;
                      break;
                    case 1:
                      HelperFunction.KeyLeft = keyboardState.GetPressedKeys()[0];
                      this.optionMenu[1] = leftOption[selectedOption] + HelperFunction.KeyLeft.ToString();
                      changingKey = false;
                      break;
                    case 2:
                      HelperFunction.KeyUp = keyboardState.GetPressedKeys()[0];
                      this.optionMenu[2] = leftOption[selectedOption] + HelperFunction.KeyUp.ToString();
                      changingKey = false;
                      break;
                    case 3:
                      HelperFunction.KeyDown = keyboardState.GetPressedKeys()[0];
                      this.optionMenu[3] = leftOption[selectedOption] + HelperFunction.KeyDown.ToString();
                      changingKey = false;
                      break;
                    case 4:
                      HelperFunction.KeyJump = keyboardState.GetPressedKeys()[0];
                      this.optionMenu[4] = leftOption[selectedOption] + HelperFunction.KeyJump.ToString();
                      changingKey = false;
                      break;
                    case 5:
                      HelperFunction.KeyUse = keyboardState.GetPressedKeys()[0];
                      this.optionMenu[5] = leftOption[selectedOption] + HelperFunction.KeyUse.ToString();
                      changingKey = false;
                      break;
                    case 6:
                      HelperFunction.KeySpeed = keyboardState.GetPressedKeys()[0];
                      this.optionMenu[6] = leftOption[selectedOption] + HelperFunction.KeySpeed.ToString();
                      changingKey = false;
                      break;
                  }
                }
              }
            }
          }
          this.fontColor = Color.Red;
        }
        previousKeyboardState = keyboardState;
      }
    }

  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Cold_Ship
{
    public class Scene2DNode
    {
        //declare member variables
        public Texture2D texture;
        public Vector2 position;
        public Vector2 velocity;
        public double bodyTemperature;

        //internal member variables
        float normalTempDecreaseRate = -0.005f;
        float exertForceIncreaseRate = 0.002f;
        float exertForceDecreaseRate = -0.05f;
        bool isExertingForce = false;
        bool stoppedExertingForce = false;
        bool isjumping = false;

        //declare constructor
        public Scene2DNode(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
            velocity = new Vector2(0, 0);
            bodyTemperature = 36.5;
        }

        //declare draw method
        public void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            spriteBatch.Draw(texture, drawPosition, Color.White);
        }

        //move the sprite
        public void Move()
        {
            position += velocity;
        }

        //update everything about the Scene2DNode object
        public void Update(GameTime gameTime, ref float bodyTempTimer, ref float exhaustionTimer, ref KeyboardState oldKeyboardState, ref float jumpTimer, float ground)
        {
            //update timers
            float elapsedTime = gameTime.ElapsedGameTime.Milliseconds;
            bodyTempTimer += elapsedTime;
            exhaustionTimer += elapsedTime;
            jumpTimer += elapsedTime;

            //register keyboard inputs
            KeyboardState newKeyboardState = Keyboard.GetState();
            UpdateKeyboard(oldKeyboardState, newKeyboardState, ref jumpTimer);
            Move();
            oldKeyboardState = newKeyboardState;

            //update body temperature
            updateBodyTemperature(ref bodyTempTimer, ref exhaustionTimer);

            //apply gravity
            if (position.Y < ground - texture.Height && jumpTimer > 250)
            {
                velocity = new Vector2(0, 5);
            }
            else if (position.Y > ground - texture.Height)
            {
                isjumping = false;
                position.Y = ground - texture.Height;
            }
        }



        //update the sprite position based on the keyboard inputs
        public void UpdateKeyboard(KeyboardState oldKeyboardState, KeyboardState newKeyboardState, ref float jumpTimer)
        {
            Keys[] keys = newKeyboardState.GetPressedKeys();
            foreach (Keys key in keys)
            {
                switch (key)
                {
                    case Keys.A:
                        if (/*oldKeyboardState.IsKeyDown(Keys.LeftShift) &&*/ newKeyboardState.IsKeyDown(Keys.LeftShift))
                        {
                            isExertingForce = true;
                            stoppedExertingForce = false;
                            position += new Vector2(-5, 0);
                        }
                        else if (oldKeyboardState.IsKeyDown(Keys.LeftShift) && newKeyboardState.IsKeyUp(Keys.LeftShift))
                        {
                            isExertingForce = false;
                            stoppedExertingForce = true;
                            position += new Vector2(-3, 0);
                        }
                        else
                        {
                            isExertingForce = false;
                            //stoppedExertingForce = false;
                            position += new Vector2(-3, 0);
                        }
                        break;
                    case Keys.D:
                        if (/*oldKeyboardState.IsKeyDown(Keys.LeftShift) &&*/ newKeyboardState.IsKeyDown(Keys.LeftShift))
                        {
                            isExertingForce = true;
                            stoppedExertingForce = false;
                            position += new Vector2(5, 0);
                        }
                        else if (oldKeyboardState.IsKeyDown(Keys.LeftShift) && newKeyboardState.IsKeyUp(Keys.LeftShift))
                        {
                            isExertingForce = false;
                            stoppedExertingForce = true;
                            position += new Vector2(3, 0);
                        }
                        else
                        {
                            isExertingForce = false;
                            //stoppedExertingForce = false;
                            position += new Vector2(3, 0);
                        }
                        break;
                    case Keys.Space:
                        if (!isjumping)
                        {
                            position += new Vector2(0, -40);
                            velocity = new Vector2(0, -5);
                            isjumping = true;
                            jumpTimer = 0;
                            bodyTemperature -= 0.005;
                        }
                        break;
                    default:
                        //velocity = new Vector2(0, 0);
                        break;
                }
            }
        }

        //update the body temperature
        public void updateBodyTemperature(ref float refbodyTempTimer, ref float refexhaustionTimer)
        {
            if (refbodyTempTimer > 500)
            {
                if (isExertingForce && !stoppedExertingForce)
                {
                    bodyTemperature += exertForceIncreaseRate;
                    refexhaustionTimer = 0;
                }
                else if (!isExertingForce && stoppedExertingForce && refexhaustionTimer < 3000)
                {
                    bodyTemperature += exertForceDecreaseRate;
                }
                else
                {
                    bodyTemperature += normalTempDecreaseRate;
                    //refexhaustionTimer = 0;
                }
                refbodyTempTimer = 0;
      
            }
        }

        
    }
}

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
        public Vector2 prevPosition;
        public Vector2 velocity;
        public Vector2 playerSpriteSize = new Vector2(32, 64); //for collision once the animation is set up
        public double bodyTemperature;
        public double stamina;
        public double staminaLimit;

        //internal member variables
        float normalTempDecreaseRate = -0.01f;
        float exertForceIncreaseRate = 0.002f;
        float exertForceDecreaseRate = -0.05f;
        bool isExertingForce = false;
        bool stoppedExertingForce = false;
        public bool isjumping = false;

        //declare constructor
        public Scene2DNode(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
            velocity = new Vector2(0, 0);
            this.bodyTemperature = 36;
        }

        //declare constructor with body temperature
        public Scene2DNode(Texture2D texture, Vector2 position, double bodyTemperature, double stamina, double staminaLimit)
        {
            this.texture = texture;
            this.position = position;
            velocity = new Vector2(0, 0);
            this.bodyTemperature = bodyTemperature;
            this.stamina = stamina;
            this.staminaLimit = staminaLimit;
        }

        //declare draw method
        public void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            spriteBatch.Draw(texture, drawPosition, Color.White);
        }


        //draws the shadow filter onto the screen, the size of the filter
        //is changed according to the parameter
        public void DrawFilter(SpriteBatch spriteBatch, Vector2 drawPosition, float scale)
        {
            //update the shadowFilter's position with respect to the playerNode
            if (scale < 1)
            {
                spriteBatch.Draw(texture, drawPosition, Color.White);
            }
            drawPosition = new Vector2((drawPosition.X /*+ (playerNode.texture.Width / 2))*/) + ((texture.Width - texture.Width * scale) / 2),
            (drawPosition.Y /*+ (playerNode.playerSpriteSize.Y / 2)*/ + ((texture.Height - texture.Height * scale) / 2)));
            spriteBatch.Draw(texture, drawPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0);
        }

        //move the sprite
        public void Move()
        {
            position += velocity;
        }

        //update everything about the Scene2DNode object
        public void Update(GameTime gameTime, ref float bodyTempTimer, ref float exhaustionTimer, ref KeyboardState oldKeyboardState, ref float jumpTimer, float ground, List<Platform> platforms, Vector2 worldSize, ref float staminaExhaustionTimer)
        {
            //register the position before updating (prevPosition)
            prevPosition = position;
            //update timers
            float elapsedTime = gameTime.ElapsedGameTime.Milliseconds;
            bodyTempTimer += elapsedTime;
            exhaustionTimer += elapsedTime;
            jumpTimer += elapsedTime;
            staminaExhaustionTimer += elapsedTime;

            //register keyboard inputs
            KeyboardState newKeyboardState = Keyboard.GetState();
            UpdateKeyboard(oldKeyboardState, newKeyboardState, ref jumpTimer);
            //Move();
            oldKeyboardState = newKeyboardState;

            //detect platform collision
            bool jumpable = false;
            foreach (Platform platform in platforms)
            {
                if (!platform.Update(this, prevPosition, jumpTimer, ground, isjumping))
                {
                    isjumping = false;
                }
            }

            //detect world boundary collision
            if (position.X < 0 || position.X + texture.Width > worldSize.X)
            {
                position = prevPosition;
            }

            //update body temperature
            updateBodyTemperature(ref bodyTempTimer, ref exhaustionTimer);

            //recover stamina
            if (staminaExhaustionTimer > 1500)
            {
                stamina += 0.04;
            }
            if (stamina < 0)
            {
                stamina = 0;
                staminaExhaustionTimer = 0;
            }
            else if (stamina > staminaLimit)
            {
                stamina = staminaLimit;
            }

            //apply gravity
            prevPosition = position;
            if (position.Y < ground - texture.Height && jumpTimer > 250)
            {
                velocity = new Vector2(0, 5);
            }
            else if (position.Y > ground - texture.Height)
            {
                isjumping = false;
                position.Y = ground - texture.Height;
            }
            Move();
            foreach (Platform platform in platforms)
            {
                if (!platform.Update(this, prevPosition, jumpTimer, ground, isjumping))
                {
                    isjumping = false;
                }
            }
        }

        //a method that applies gravity to the player sprite
        public void ApplyGravity(float jumpTimer, float ground)
        {
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
                        if (/*oldKeyboardState.IsKeyDown(Keys.LeftShift) &&*/ newKeyboardState.IsKeyDown(Keys.LeftShift) && stamina != 0)
                        {
                            isExertingForce = true;
                            stoppedExertingForce = false;
                            position += new Vector2(-5, 0);
                            stamina -= 1;
                        }
                        else if (oldKeyboardState.IsKeyDown(Keys.LeftShift) && newKeyboardState.IsKeyUp(Keys.LeftShift))
                        {
                            isExertingForce = false;
                            stoppedExertingForce = true;
                            position += new Vector2(-3, 0);
                            stamina -= 0.03;
                        }
                        else
                        {
                            isExertingForce = false;
                            //stoppedExertingForce = false;
                            position += new Vector2(-3, 0);
                            stamina -= 0.03;
                        }
                        break;
                    case Keys.D:
                        if (/*oldKeyboardState.IsKeyDown(Keys.LeftShift) &&*/ newKeyboardState.IsKeyDown(Keys.LeftShift) && stamina != 0)
                        {
                            isExertingForce = true;
                            stoppedExertingForce = false;
                            position += new Vector2(5, 0);
                            stamina -= 1;
                        }
                        else if (oldKeyboardState.IsKeyDown(Keys.LeftShift) && newKeyboardState.IsKeyUp(Keys.LeftShift))
                        {
                            isExertingForce = false;
                            stoppedExertingForce = true;
                            position += new Vector2(3, 0);
                            stamina -= 0.03;
                        }
                        else
                        {
                            isExertingForce = false;
                            //stoppedExertingForce = false;
                            position += new Vector2(3, 0);
                            stamina -= 0.03;
                        }
                        break;
                    case Keys.Space:
                        if (!isjumping && oldKeyboardState.IsKeyUp(Keys.Space) && stamina != 0)
                        {
                            position += new Vector2(0, -40);
                            velocity = new Vector2(0, -5);
                            isjumping = true;
                            jumpTimer = 0;
                            bodyTemperature -= 0.01;
                            stamina -= 0.5;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using OpenTK.Audio.OpenAL;

namespace Cold_Ship
{
    public class Character : GenericSprite2D, IWatchfulConditional
    {
        public static Cold_Ship GameInstance;
        public const float NORMAL_BODY_TEMPERATURE = 37.5f;
        public const float MAXIMUM_ENERGY_LEVEL = 100.0f;

        //declare member variables
        public Vector2 prevPosition;
        public Vector2 velocity;
        public double BodyTemperature;
        public double Energy;
        public Camera2D _camera;

        public GameLevel CurrentGameLevel;
        public GameLevel PreviousGameLevel;

        //animation related variables
        public enum Action_Status { FORWARD = 0, BACKWARD = 1, FORWARD_WITH_LIGHTER = 2, BACKWARD_WITH_LIGHTER = 3, CLIMB = 4 };
        public Action_Status actionStatus;
        public int maxFramesX, maxFramesY, currentFrame;
        public Vector2 playerSpriteSize; //for collision once the animation is set up

        //internal member variables
        float normalTempDecreaseRate = -0.08f;
        float exertForceIncreaseRate = 0.008f;
        float exertForceDecreaseRate = -0.08f;
        bool isExertingForce = false;
        bool stoppedExertingForce = false;
        public bool isjumping = false;
        bool isClimbing = false;
        bool canClimb = false;
        bool gravityIsEnabled = true;

        public Vector2 CameraRelativePosition;

        public PocketLightSource _pocketLight;

      public HealthBar EnergyBar;

      public float bodyTemperatureTimer,
        bodyTempTimer,
        exhaustionTimer,
        jumpTimer,
        staminaExhaustionTimer,
        animationTimer = 150;

        //declare constructor for inheritance
        public Character(Cold_Ship gameInstance, Texture2D texture, Vector2 position)
            : base(texture, position, Rectangle.Empty)
        {
            if (GameInstance == null) GameInstance = gameInstance;
            velocity = new Vector2(0, 0);
        }

        public Camera2D GetCamera() { return this._camera; }

        public static Character GetNewInstance(Cold_Ship gameInstance)
        {
            if (GameInstance == null) GameInstance = gameInstance;
            Texture2D _playerTexture = GameInstance.Content.Load<Texture2D>("Character/PlayerSpriteSheet");
            Character _instance = new Character(gameInstance, _playerTexture, Vector2.Zero);
            _instance.BodyTemperature = NORMAL_BODY_TEMPERATURE;
            _instance.Energy = MAXIMUM_ENERGY_LEVEL;
            _instance.maxFramesX = 4;
            _instance.maxFramesY = 5;
            _instance.currentFrame = 0;
            _instance.actionStatus = Action_Status.FORWARD;
            _instance.animationTimer = 0;
            _instance.playerSpriteSize = new Vector2((float)_playerTexture.Width / _instance.maxFramesX
                , (float)_playerTexture.Height / _instance.maxFramesY);
            _instance.EnergyBar = HealthBar.GetNewInstance(GameInstance, _instance, _instance.GetEnergyAsRatio);
            _instance._camera = GameInstance.Camera;

            return _instance;
        }

        public GameLevel GetPreviousLevel() { return this.PreviousGameLevel; }
        public void SetPreviousLevel(GameLevel level) { this.PreviousGameLevel = level; }

      public float GetEnergyAsRatio()
      {
          return (float)(this.Energy / MAXIMUM_ENERGY_LEVEL);
      }
        //draws the player sprite onto screen
        public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            int line = (int)actionStatus;
            Rectangle rect = new Rectangle(currentFrame 
                * (int)playerSpriteSize.X, line * (int)playerSpriteSize.Y, (int)playerSpriteSize.X, (int)playerSpriteSize.Y);
            spriteBatch.Draw(Texture, drawPosition, rect, Color.White);

            if (_pocketLight != null) _pocketLight.Draw(spriteBatch);
            if (EnergyBar != null) EnergyBar.Draw(spriteBatch);
        }

        //move the sprite
        public void Move()
        {
            Position += velocity;
        }

        //update everything about the Scene2DNode object
        public void Update(GameTime gameTime)
        {
          Vector2 worldSize = this.CurrentGameLevel.GetAbsoluteWorldSize();
            if (_pocketLight != null) _pocketLight.Update(gameTime);
            if (EnergyBar != null) EnergyBar.Update(gameTime);

            //register the Position before updating (prevPosition)
            prevPosition = Position;
            //update timers
            float elapsedTime = gameTime.ElapsedGameTime.Milliseconds;
            bodyTempTimer += elapsedTime;
            exhaustionTimer += elapsedTime;
            jumpTimer += elapsedTime;
            staminaExhaustionTimer += elapsedTime;
            animationTimer += elapsedTime;

            //register keyboard inputs
            KeyboardState newKeyboardState = Keyboard.GetState();
//            UpdateKeyboard(oldKeyboardState, newKeyboardState, ref jumpTimer, ref animationTimer);

//            foreach (Platform platform in platforms)
//            {
//                if (!platform.Update(this, prevPosition, jumpTimer, ground, isjumping))
//                {
//                    isjumping = false;
//                }
//            }

            //detect world boundary collision
            if (Position.X < 0 || Position.X + playerSpriteSize.X > worldSize.X)
            {
                Position = prevPosition;
            }

            //update body temperature
            updateBodyTemperature(ref bodyTempTimer, ref exhaustionTimer);

            //recover Energy
            if (staminaExhaustionTimer > 1500)
            {
                if (Energy < MAXIMUM_ENERGY_LEVEL)
                {
                    Energy += 0.0005;
                }
            }
            if (Energy < 0)
            {
                Energy = 0;

                staminaExhaustionTimer = 700;
            }
            else if (Energy > MAXIMUM_ENERGY_LEVEL + 5)
            {
                //staminaLimit = Energy;
            }
            else if (Energy > MAXIMUM_ENERGY_LEVEL && Energy < MAXIMUM_ENERGY_LEVEL + 5)
            {
               staminaExhaustionTimer = 0;
            }
            else if (Energy > MAXIMUM_ENERGY_LEVEL)
            {
                Energy = MAXIMUM_ENERGY_LEVEL;
            }

            //apply gravity
            prevPosition = Position;
//            if (!isClimbing && (gravityIsEnabled || newKeyboardState.IsKeyDown(HelperFunction.KeyDown)))
//            {
//                Move();
//                if (Position.Y < ground - playerSpriteSize.Y && jumpTimer > 200)
//                {
//                    velocity = new Vector2(0, 5);
//
//                }
//                else if (Position.Y > ground - playerSpriteSize.Y)
//                {
//                    isjumping = false;
//                    Position.Y = ground - playerSpriteSize.Y;
//                }
//
//            }
//            else
//            {
//                if (Position.Y > ground - playerSpriteSize.Y)
//                {
//                    isClimbing = false;
//                    Position.Y = ground - playerSpriteSize.Y;
//                }
//            }
        }

        public bool HasLighter()
        {
          return this._pocketLight != null && !this._pocketLight.IsDisabled();
        }
        public bool GetCondition() { return this.HasLighter(); }

         //update the sprite Position based on the keyboard inputs
        public void UpdateKeyboard(KeyboardState oldKeyboardState, KeyboardState newKeyboardState, ref float jumpTimer, ref float animationTimer)
        {
            Keys[] keys = newKeyboardState.GetPressedKeys();
            foreach (Keys key in keys)
            {
                if (key == HelperFunction.KeyLeft)
                {
                    if (isClimbing) 
                    {
                        Position += new Vector2(-3, 0);
                    }
                    else
                    {
                        if (/*oldKeyboardState.IsKeyDown(Keys.LeftShift) &&*/ newKeyboardState.IsKeyDown(HelperFunction.KeySpeed) && Energy != 0)
                        {
                            isExertingForce = true;
                            stoppedExertingForce = false;
                            Position += new Vector2(-5, 0);
                            Energy -= 0.2;

                            if (!this.HasLighter())
                            {
                                if (actionStatus != Action_Status.BACKWARD)
                                {
                                    actionStatus = Action_Status.BACKWARD;
                                    currentFrame = 0;
                                }
                                else if (animationTimer > 75 && !isjumping)
                                {
                                    currentFrame++;
                                    if (currentFrame >= maxFramesX)
                                    {
                                        currentFrame = 0;
                                    }
                                    animationTimer = 0;
                                }
                            }
                            else if (this.HasLighter())
                            {
                                if (actionStatus != Action_Status.BACKWARD_WITH_LIGHTER)
                                {
                                    actionStatus = Action_Status.BACKWARD_WITH_LIGHTER;
                                    currentFrame = 0;
                                }
                                else if (animationTimer > 75 && !isjumping)
                                {
                                    currentFrame++;
                                    if (currentFrame >= maxFramesX)
                                    {
                                        currentFrame = 0;
                                    }
                                    animationTimer = 0;
                                }
                            }

                        }
                        else if (oldKeyboardState.IsKeyDown(HelperFunction.KeySpeed) && newKeyboardState.IsKeyUp(HelperFunction.KeySpeed))
                        {
                            isExertingForce = false;
                            stoppedExertingForce = true;
                            Position += new Vector2(-3, 0);
                            Energy -= 0.03;
                        }
                        else
                        {
                            isExertingForce = false;
                            //stoppedExertingForce = false;
                            Position += new Vector2(-3, 0);
                            Energy -= 0.03;

                            if (!this.HasLighter())
                            {
                                if (actionStatus != Action_Status.BACKWARD)
                                {
                                    actionStatus = Action_Status.BACKWARD;
                                    currentFrame = 0;
                                }
                                else if (animationTimer > 150 && !isjumping)
                                {
                                    currentFrame++;
                                    if (currentFrame >= maxFramesX)
                                    {
                                        currentFrame = 0;
                                    }
                                    animationTimer = 0;
                                }
                            }
                            else if (this.HasLighter())
                            {
                                if (actionStatus != Action_Status.BACKWARD_WITH_LIGHTER)
                                {
                                    actionStatus = Action_Status.BACKWARD_WITH_LIGHTER;
                                    currentFrame = 0;
                                }
                                else if (animationTimer > 150 && !isjumping)
                                {
                                    currentFrame++;
                                    if (currentFrame >= maxFramesX)
                                    {
                                        currentFrame = 0;
                                    }
                                    animationTimer = 0;
                                }
                            }

                        }
                    }
                }
                else if (key == HelperFunction.KeyRight)
                {
                    if (isClimbing)
                    {
                        Position += new Vector2(3, 0);
                    }
                    else
                    {
                        if (/*oldKeyboardState.IsKeyDown(Keys.LeftShift) &&*/ newKeyboardState.IsKeyDown(HelperFunction.KeySpeed) && Energy != 0)
                        {
                            isExertingForce = true;
                            stoppedExertingForce = false;
                            Position += new Vector2(5, 0);
                            Energy -= 0.2;

                            if (!this.HasLighter())
                            {
                                if (actionStatus != Action_Status.FORWARD)
                                {
                                    actionStatus = Action_Status.FORWARD;
                                    currentFrame = 0;
                                }
                                else if (animationTimer > 75 && !isjumping)
                                {
                                    currentFrame++;
                                    if (currentFrame >= maxFramesX)
                                    {
                                        currentFrame = 0;
                                    }
                                    animationTimer = 0;
                                }
                            }
                            else if (this.HasLighter())
                            {
                                if (actionStatus != Action_Status.FORWARD_WITH_LIGHTER)
                                {
                                    actionStatus = Action_Status.FORWARD_WITH_LIGHTER;
                                    currentFrame = 0;
                                }
                                else if (animationTimer > 75 && !isjumping && !isClimbing)
                                {
                                    currentFrame++;
                                    if (currentFrame >= maxFramesX)
                                    {
                                        currentFrame = 0;
                                    }
                                    animationTimer = 0;
                                }
                            }

                        }
                        else if (oldKeyboardState.IsKeyDown(HelperFunction.KeySpeed) && newKeyboardState.IsKeyUp(HelperFunction.KeySpeed))
                        {
                            isExertingForce = false;
                            stoppedExertingForce = true;
                            Position += new Vector2(3, 0);
                            Energy -= 0.03;
                        }
                        else
                        {
                            isExertingForce = false;
                            //stoppedExertingForce = false;
                            Position += new Vector2(3, 0);
                            Energy -= 0.03;

                            if (!this.HasLighter())
                            {
                                if (actionStatus != Action_Status.FORWARD)
                                {
                                    actionStatus = Action_Status.FORWARD;
                                    currentFrame = 0;
                                }
                                else if (animationTimer > 150 && !isjumping)
                                {
                                    currentFrame++;
                                    if (currentFrame >= maxFramesX)
                                    {
                                        currentFrame = 0;
                                    }
                                    animationTimer = 0;
                                }
                            }
                            else if (this.HasLighter())
                            {
                                if (actionStatus != Action_Status.FORWARD_WITH_LIGHTER)
                                {
                                    actionStatus = Action_Status.FORWARD_WITH_LIGHTER;
                                    currentFrame = 0;
                                }
                                else if (animationTimer > 150 && !isjumping)
                                {
                                    currentFrame++;
                                    if (currentFrame >= maxFramesX)
                                    {
                                        currentFrame = 0;
                                    }
                                    animationTimer = 0;
                                }
                            }
                        }
                    }
                }
                else if (key == HelperFunction.KeyUp)
                {
                    if (canClimb)
                    {
                        isClimbing = true;
                        isjumping = false;
                        if (newKeyboardState.IsKeyDown(HelperFunction.KeySpeed) && Energy != 0)
                        {
                            isExertingForce = true;
                            stoppedExertingForce = false;
                            Position += new Vector2(0, -5);
                            Energy -= 1;
                        }
                        else if (oldKeyboardState.IsKeyDown(HelperFunction.KeySpeed) && newKeyboardState.IsKeyUp(HelperFunction.KeySpeed))
                        {
                            isExertingForce = false;
                            stoppedExertingForce = true;
                            Position += new Vector2(0, -3);
                            Energy -= 0.03;
                        }
                        else
                        {
                            isExertingForce = false;
                            //stoppedExertingForce = false;
                            Position += new Vector2(0, -3);
                            Energy -= 0.03;

                            if (actionStatus != Action_Status.CLIMB)
                            {
                                actionStatus = Action_Status.CLIMB;
                                currentFrame = 0;
                            }
                            else if (animationTimer > 150 && !isjumping)
                            {
                                currentFrame++;
                                if (currentFrame >= 2)
                                {
                                    currentFrame = 0;
                                }
                                animationTimer = 0;
                            }

                        }
                    }
                }
                else if (key == HelperFunction.KeyDown)
                {
                    if (canClimb)
                    {
                        isClimbing = true;
                        isjumping = false;
                        if (newKeyboardState.IsKeyDown(HelperFunction.KeySpeed) && Energy != 0)
                        {
                            isExertingForce = true;
                            stoppedExertingForce = false;
                            Position += new Vector2(0, 5);
                            Energy -= 1;
                        }
                        else if (oldKeyboardState.IsKeyDown(HelperFunction.KeySpeed) && newKeyboardState.IsKeyUp(HelperFunction.KeySpeed))
                        {
                            isExertingForce = false;
                            stoppedExertingForce = true;
                            Position += new Vector2(0, 3);
                            Energy -= 0.03;
                        }
                        else
                        {
                            isExertingForce = false;
                            //stoppedExertingForce = false;
                            Position += new Vector2(0, 3);
                            Energy -= 0.03;

                            if (actionStatus != Action_Status.CLIMB)
                            {
                                actionStatus = Action_Status.CLIMB;
                                currentFrame = 0;
                            }
                            else if (animationTimer > 150 && !isjumping)
                            {
                                currentFrame++;
                                if (currentFrame >= 2)
                                {
                                    currentFrame = 0;
                                }
                                animationTimer = 0;
                            }

                        }
                    }
                }
                else if (key == HelperFunction.KeyJump)
                {
                    if (!isjumping && oldKeyboardState.IsKeyUp(Keys.Space) && Energy != 0)
                    {
                        Position += new Vector2(0, -40);
                        velocity = new Vector2(0, -5);
                        isjumping = true;
                        jumpTimer = 0;
                        BodyTemperature -= 0.01;
                        Energy -= 0.5;
                        isClimbing = false;
                    }
                }
            }
            if (keys.Length == 0)
            {
                currentFrame = 0;
            }
        }

        //update the body temperature
        public void updateBodyTemperature(ref float refbodyTempTimer, ref float refexhaustionTimer)
        {
            if (refbodyTempTimer > 500)
            {
                if (isExertingForce && !stoppedExertingForce)
                {
                    BodyTemperature += exertForceIncreaseRate;
                    refexhaustionTimer = 0;
                }
                else if (!isExertingForce && stoppedExertingForce && refexhaustionTimer < 3000)
                {
                    BodyTemperature += exertForceDecreaseRate;
                }
                else
                {
                    BodyTemperature += normalTempDecreaseRate;
                    //refexhaustionTimer = 0;
                }
                refbodyTempTimer = 0;

            }
        }

        //return player hitbox
        public Rectangle getPlayerHitBox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)playerSpriteSize.X, (int)playerSpriteSize.Y);
        }
    }
}

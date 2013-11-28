using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using KeyboardState = Microsoft.Xna.Framework.Input.KeyboardState;

namespace Cold_Ship
{
  public class Character : Sprite2D, IWatchfulConditional
  {
    public static Texture2D NORMAL_PLAYER_TEXTURE;
    public const float NORMAL_BODY_TEMPERATURE = 37.5f;
    public const float NORMAL_ENERGY_LEVEL = 100.0f;
    public const float NORMAL_PLAYER_MOVEMENT = 5.0f;
    public const float NORMAL_FRAME_SPEED = 350;
    public const float NORMAL_TEMPERATURE_DECREASE_RATE = 5.0f;
    public const float NORMAL_ENERGY_DECREASE_RATE = 5.0f;

    public static Rectangle PLAYER_BOUND_BOX = new Rectangle(0, 0, 32, 64);
    // DEVELOPER CONTENT BASED
    public float FrameSpeedMultipler;
    public float TemperatureMulitpler;
    public float EnergyMultiplier;
    public float VelocityMultiplier;

    public Stack<float> FrameSpeedMultiplierStack;
    public Stack<float> TemperatureMultiplierStack;
    public Stack<float> EnergyMultiplierStack;
    public Stack<float> VelocityMultiplierStack;

    // GAME OBJECT BASED
    public Cold_Ship GameInstance;
    public Vector2 Velocity;
    public double BodyTemperature;
    public double Energy;
    public Camera2D _camera;

    public Stack<GameLevel> GameLevelWalkThrough;
    public GameLevel CurrentGameLevel;

    public Queue<PickUpItem> PickUpItems;

    private float _frameTimer;

    //animation related variables
    public enum ActionStatus { FORWARD = 0, BACKWARD = 1, 
        FORWARD_WITH_LIGHTER = 2, BACKWARD_WITH_LIGHTER = 3, CLIMB = 4 };

    public ActionStatus CurrentActionStatus;
    public int maxFramesX, maxFramesY, currentFrame;
    public Vector2 playerSpriteSize; //for collision once the animation is set up

    bool isExertingForce = false;
    bool stoppedExertingForce = false;
    public bool isjumping = false;
    bool isClimbing = false;
    bool canClimb = false;
    bool gravityIsEnabled = true;

    public Vector2 CameraRelativePosition;

    public PocketLightSource PocketLight;

    public HealthBar EnergyBar;

    public float bodyTemperatureTimer,
      bodyTempTimer,
      exhaustionTimer,
      jumpTimer,
      staminaExhaustionTimer,
      animationTimer = 150;

    //declare constructor for inheritance
    private Character(Texture2D texture
      , Rectangle boundBox)
      : base(texture, Vector2.Zero, boundBox)
    {

      this.maxFramesX = 4;
      this.maxFramesY = 5;
      this.currentFrame = 0;

      GameLevelWalkThrough = new Stack<GameLevel>();
      this.ResetPlayerProperties();
    }

    public void ResetPlayerProperties()
    {
      FrameSpeedMultiplierStack = new Stack<float>();
      TemperatureMultiplierStack = new Stack<float>();
      EnergyMultiplierStack = new Stack<float>();
      VelocityMultiplierStack = new Stack<float>();
      PickUpItems = new Queue<PickUpItem>();

      FrameSpeedMultiplierStack.Push(1);
      TemperatureMultiplierStack.Push(1);
      EnergyMultiplierStack.Push(1);
      VelocityMultiplierStack.Push(1);

      this.FrameSpeedMultipler = 1;
      this.TemperatureMulitpler = 1;
      this.EnergyMultiplier = 1;
      this.VelocityMultiplier = 1;

      this.Velocity = new Vector2(NORMAL_PLAYER_MOVEMENT, 0);
      this.BodyTemperature = NORMAL_PLAYER_MOVEMENT;
      this.Energy = NORMAL_ENERGY_LEVEL;
    }

    public float GetFrameSpeedMultiplier()
    {
      float _speed = 0;
      foreach (float f in FrameSpeedMultiplierStack)
        _speed += f;
      return _speed;
    }

    public float GetTemperatureMultiplier()
    {
      float _speed = 0;
      foreach (float f in TemperatureMultiplierStack)
        _speed += f;
      return _speed;
    }

    public float GetEnergyMultiplier()
    {
      float _speed = 0;
      foreach (float f in EnergyMultiplierStack)
        _speed += f;
      return _speed;
    }

    public float GetVelocityMultiplier()
    {
      float _speed = 0;
      foreach (float f in FrameSpeedMultiplierStack)
        _speed += f;
      return _speed;
    }

    public bool HasVisited(Type gameLevelType)
    {
      foreach (GameLevel level in GameLevelWalkThrough)
      {
        if (level != null 
            && gameLevelType == level.GetType())
          return true;
      }

      return false;
    }

    public Camera2D GetCamera() { return this._camera; }

    public static Character GetNewInstance(Cold_Ship gameInstance, Camera2D camera)
    {
      Texture2D _playerTexture = gameInstance.Content.Load<Texture2D>("Character/PlayerSpriteSheet");
      Character _instance = new Character(_playerTexture, PLAYER_BOUND_BOX);
      _instance.GameInstance = gameInstance;
      _instance.BodyTemperature = NORMAL_BODY_TEMPERATURE;
      _instance.Energy = NORMAL_ENERGY_LEVEL;
      _instance.maxFramesX = 4;
      _instance.maxFramesY = 5;
      _instance.currentFrame = 0;
      _instance.CurrentActionStatus = ActionStatus.FORWARD;
      _instance.animationTimer = 0;
      _instance.playerSpriteSize = new Vector2((float)_playerTexture.Width / _instance.maxFramesX
          , (float)_playerTexture.Height / _instance.maxFramesY);
      _instance.EnergyBar = HealthBar.GetNewInstance(gameInstance, _instance, _instance.GetEnergyAsRatio);
      _instance._camera = camera;
      camera.PlayerFocus = _instance;

      return _instance;
    }

    public GameLevel GetCurrentGameLevel()
    {
      return this.CurrentGameLevel;
    }

    public GameLevel GetPreviousGameLevel()
    {
      return this.GameLevelWalkThrough.Peek();
    }

    public void SetPreviousGameLevel(GameLevel level)
    {
      this.GameLevelWalkThrough.Push(level);
    }

    public float GetEnergyAsRatio()
    {
      return (float)(this.Energy / NORMAL_ENERGY_LEVEL);
    }

    public void DrawEnvironment(SpriteBatch spriteBatch)
    {
      // Draw the world
      this.CurrentGameLevel.Draw();
    }

    //draws the player sprite onto screen
    public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
    {
      int line = (int)CurrentActionStatus;
      Rectangle rect = new Rectangle(currentFrame
          * (int)playerSpriteSize.X, line 
          * (int)playerSpriteSize.Y
          , (int)playerSpriteSize.X, (int)playerSpriteSize.Y);

      spriteBatch.Begin();
      spriteBatch.Draw(Texture, drawPosition, rect, Color.White);
      spriteBatch.End();
//      GameInstance.Camera.DrawNode(this);

      if (PocketLight != null) PocketLight.Draw(spriteBatch, drawPosition);
      if (EnergyBar != null) EnergyBar.Draw(spriteBatch);

    }

    public void TakePortal(Portal portal)
    {
      portal.WalkThroughPortal(this);
    }

    public void PickUp(PickUpItem item)
    {
      this.PickUpItems.Enqueue(item);
      item.PickUpBy(this);
    }

    public void SpawnIn(GameLevel level)
    {
      this.SetPreviousGameLevel(this.CurrentGameLevel);
      this.CurrentGameLevel = level;
      this.CurrentGameLevel.LoadLevelContentIfHasNotForPlayer(this);
    }

    //move the sprite
    private void _MoveHorizontally(GameTime gameTime)
    {
      this._frameTimer += gameTime.ElapsedGameTime.Milliseconds;
      if (Keyboard.GetState().IsKeyDown(HelperFunction.KeyRight))
      {
        this.Position += this.Velocity * VelocityMultiplier;
        this.CurrentActionStatus = ActionStatus.FORWARD;
        
      }
      else if (Keyboard.GetState().IsKeyDown(HelperFunction.KeyLeft))
      {
        this.Position -= this.Velocity * VelocityMultiplier;
        this.CurrentActionStatus = ActionStatus.BACKWARD;
      }

      if (this._frameTimer
          >= this.GetFrameSpeedMultiplier()*NORMAL_FRAME_SPEED)
      {
        if (Keyboard.GetState().IsKeyDown(HelperFunction.KeyLeft)
            || Keyboard.GetState().IsKeyDown(HelperFunction.KeyRight))
        {
        this.currentFrame = ++this.currentFrame % maxFramesX;
        this._frameTimer = 0;
          
        }
      else
        {
          this.currentFrame = 0;

        }
      }
    }

    public bool WantToTakePortal(Portal portal)
    {
      return (this.CheckCollision(portal)
          && Keyboard.GetState().IsKeyDown(HelperFunction.KeyUp));
    }

    //update everything about the Scene2DNode object
    public void Update(GameTime gameTime)
    {
      // Update the world only the player is in currently
      this.CurrentGameLevel.Update(gameTime);

      if (PocketLight != null) PocketLight.Update(gameTime);
      if (EnergyBar != null) EnergyBar.Update(gameTime);

      this._MoveHorizontally(gameTime);

      //            foreach (Platform platform in platforms)
      //            {
      //                if (!platform.Update(this, prevPosition, jumpTimer, ground, isjumping))
      //                {
      //                    isjumping = false;
      //                }
      //            }

      //detect world boundary collision
      //      if (Position.X < 0 || Position.X + playerSpriteSize.X > worldSize.X)
      //      {
      //        Position = prevPosition;
      //      }

      //      //recover Energy
      //      if (staminaExhaustionTimer > 1500)
      //      {
      //        if (Energy < NORMAL_ENERGY_LEVEL)
      //        {
      //          Energy += 0.0005;
      //        }
      //      }
      //      if (Energy < 0)
      //      {
      //        Energy = 0;
      //
      //        staminaExhaustionTimer = 700;
      //      }
      //      else if (Energy > NORMAL_ENERGY_LEVEL + 5)
      //      {
      //        //staminaLimit = Energy;
      //      }
      //      else if (Energy > NORMAL_ENERGY_LEVEL && Energy < NORMAL_ENERGY_LEVEL + 5)
      //      {
      //        staminaExhaustionTimer = 0;
      //      }
      //      else if (Energy > NORMAL_ENERGY_LEVEL)
      //      {
      //        Energy = NORMAL_ENERGY_LEVEL;
      //      }

      //apply gravity
      //      prevPosition = Position;
      //            if (!isClimbing && (gravityIsEnabled || newKeyboardState.IsKeyDown(HelperFunction.KeyDown)))
      //            {
      //                Move();
      //                if (Position.Y < ground - playerSpriteSize.Y && jumpTimer > 200)
      //                {
      //                    Velocity = new Vector2(0, 5);
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
      return this.PocketLight != null && !this.PocketLight.IsDisabled();
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
                if (CurrentActionStatus != ActionStatus.BACKWARD)
                {
                  CurrentActionStatus = ActionStatus.BACKWARD;
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
                if (CurrentActionStatus != ActionStatus.BACKWARD_WITH_LIGHTER)
                {
                  CurrentActionStatus = ActionStatus.BACKWARD_WITH_LIGHTER;
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
                if (CurrentActionStatus != ActionStatus.BACKWARD)
                {
                  CurrentActionStatus = ActionStatus.BACKWARD;
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
                if (CurrentActionStatus != ActionStatus.BACKWARD_WITH_LIGHTER)
                {
                  CurrentActionStatus = ActionStatus.BACKWARD_WITH_LIGHTER;
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
                if (CurrentActionStatus != ActionStatus.FORWARD)
                {
                  CurrentActionStatus = ActionStatus.FORWARD;
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
                if (CurrentActionStatus != ActionStatus.FORWARD_WITH_LIGHTER)
                {
                  CurrentActionStatus = ActionStatus.FORWARD_WITH_LIGHTER;
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
                if (CurrentActionStatus != ActionStatus.FORWARD)
                {
                  CurrentActionStatus = ActionStatus.FORWARD;
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
                if (CurrentActionStatus != ActionStatus.FORWARD_WITH_LIGHTER)
                {
                  CurrentActionStatus = ActionStatus.FORWARD_WITH_LIGHTER;
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

              if (CurrentActionStatus != ActionStatus.CLIMB)
              {
                CurrentActionStatus = ActionStatus.CLIMB;
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

              if (CurrentActionStatus != ActionStatus.CLIMB)
              {
                CurrentActionStatus = ActionStatus.CLIMB;
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
            Velocity = new Vector2(0, -5);
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

    public Vector2 GetSize()
    {
      Vector2 _size = new Vector2(this.BoundBox.Width, this.BoundBox.Height);

      return _size;
    }

    public Rectangle GetDefaultBoundingBox()
    {
      Rectangle _box = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Texture.Width, this.Texture.Height);
      return _box;
    }
  }
}

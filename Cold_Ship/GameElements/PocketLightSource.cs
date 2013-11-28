using System.Runtime.Remoting.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Cold_Ship
{
  public class PocketLightSource : PickUpItem
  {
    public const float GLOW_INTERVAL = 200;
    public const int GLOW_TICK_SCALE = 5;
    public const float MAX_SCALE_FACTOR = 0.7f; // Empirical measure
    public const float TICK_INTERVAL = 200;
    public const float SNAPSHOT_TICK = 400;

    private float _tickTimer;
    private Vector2 _scale = new Vector2(MAX_SCALE_FACTOR, MAX_SCALE_FACTOR);
    private int _scaleCount;
    private Vector2 _scaleOffset;
    private Vector2 _positionOffset;
    private Vector2 _positionOffsetLeft = new Vector2(0, -10); // The offset of left
    private Vector2 _positionOffsetRight = new Vector2(35, -10); // The offset of right
    private Vector2 _actualFacingOffset;

    private enum LightState { ON, OFF, NOLIGHTING }

    private Vector2[] _positionBuffer;
    private float _positionBufferTimer;

    private LightState _lightState;
    private Texture2D _lightOn;
    private Texture2D _lightOff;
    private float _lightSwitchInterval = 200;
    private float _lightSwitchTimer;

    private enum Facing { LEFT, RIGHT }

    private Texture2D _trail1;
    private Texture2D _trail2;

    private Facing _facing;

    private PocketLightSource(GameLevel instance, Texture2D texture, Vector2 position)
      : base(instance, texture, position)
    {
      this._facing = Facing.RIGHT;
      this._scaleCount = 0;
      this._actualFacingOffset = _positionOffsetRight;
      this._scaleOffset = (new Vector2(this.Texture.Width * (1 - this._scale.X)
          , this.Texture.Height * (1 - this._scale.Y))) / 2;
      this._positionBuffer = new Vector2[2];

      this._lightState = LightState.ON;
    }

    public bool IsTurnedOff() { return this._lightState == LightState.OFF; }
    public bool IsTurnedOn() { return this._lightState == LightState.ON; }
    public bool IsDisabled() { return this._lightState == LightState.NOLIGHTING; } 
    public void TurnOff() { this._lightState = LightState.OFF; }
    public void TurnOn() { this._lightState = LightState.ON; }
    public void TurnDisable() { this._lightState = LightState.NOLIGHTING; }

    public static PocketLightSource GetNewInstance(GameLevel instance, Vector2 position)
    {
      Texture2D _texture = instance.Content.Load<Texture2D>("Objects/lighter");
      Texture2D _textureLightOn = instance.Content.Load<Texture2D>("Textures/radius_of_light");
      Texture2D _textureLightOff = instance.Content.Load<Texture2D>("Textures/radius_of_light_off");

      PocketLightSource _instance = new PocketLightSource(instance, _texture, position);
      _instance._lightOn = _textureLightOn;
      _instance._lightOff = _textureLightOff;
      _instance.Size = new Vector2(5, 11);
      return _instance;
    }

    private void _UpdatePositionBuffer(Vector2 pos)
    {
      this._positionBuffer[1] = this._positionBuffer[0];
      this._positionBuffer[0] = pos;
    }

    public void Update(GameTime gameTime)
    {
      if (this._lightState == LightState.NOLIGHTING) return;

      this._tickTimer += gameTime.ElapsedGameTime.Milliseconds;
      this._lightSwitchTimer += gameTime.ElapsedGameTime.Milliseconds;
      this._positionBufferTimer += gameTime.ElapsedGameTime.Milliseconds;
      this._positionOffset = new Vector2(_lightOff.Width / 2
          , _lightOff.Height / 2);

      if (Keyboard.GetState().IsKeyDown(Keys.D))
        this._facing = Facing.RIGHT;
      else if (Keyboard.GetState().IsKeyDown(Keys.A))
        this._facing = Facing.LEFT;

      if (Keyboard.GetState().IsKeyDown(Keys.D)
        || Keyboard.GetState().IsKeyDown(Keys.A))
      {
        if (this._positionBufferTimer >= SNAPSHOT_TICK)
        {
          this._UpdatePositionBuffer(this._owner.Position - this._positionOffset + _actualFacingOffset + _scaleOffset);
          this._positionBufferTimer = 0;
        }
      }

      if (this._lightState == LightState.ON)
        this.Texture = this._lightOn;
      else if (this._lightState == LightState.OFF)
        this.Texture = this._lightOff;

      if (this._lightState == LightState.ON)
      {

        if (this._tickTimer >= TICK_INTERVAL)
        {
          float _x = (float)(Math.Sin(_scaleCount) / Math.PI) / GLOW_TICK_SCALE;
          float _y = (float)(Math.Cos(_scaleCount) / Math.PI) / GLOW_TICK_SCALE;
          this._scale += new Vector2(_x, _y);
          this._tickTimer = 0;
          this._scaleCount++;
          this._scaleOffset = (new Vector2(this.Texture.Width * (1 - this._scale.X)
              , this.Texture.Height * (1 - this._scale.Y))) / 2;
        }
      }

      if (this._facing == Facing.RIGHT)
        _actualFacingOffset = _positionOffsetRight;
      else
        _actualFacingOffset = _positionOffsetLeft;

      if (Keyboard.GetState().IsKeyDown(Keys.L) && this._lightState == LightState.ON &&
          this._lightSwitchTimer >= this._lightSwitchInterval)
      {
        this._lightState = LightState.OFF;
        this._lightSwitchTimer = 0;
      }
      else if (Keyboard.GetState().IsKeyDown(Keys.L) && this._lightState == LightState.OFF &&
               this._lightSwitchTimer >= this._lightSwitchInterval)
      {
        this._lightState = LightState.ON;
        this._lightSwitchTimer = 0;
      }

//      Vector2 cameraToPlayer = new Vector2(this._owner.Position.X - CurrentGameLevel.Camera.CameraPosition.X,
//        this._owner.Position.Y - CurrentGameLevel.Camera.CameraPosition.Y);
      Vector2 cameraToPlayer = CurrentGameLevel.Camera.CameraPosition;

      this.Position = cameraToPlayer - this._positionOffset + _actualFacingOffset + _scaleOffset;
    }

    public override void PickUpBy(Character player)
    {
      player.PocketLight = this;
      this.TurnDisable();
      base.PickUpBy(player);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (this._lightState == LightState.NOLIGHTING) return;

      if (!this.IsConsumed())
      {
        base.Draw(spriteBatch);
        return;
      }

      spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
      if (this._facing == Facing.RIGHT)
      {
        spriteBatch.Draw(this.Texture, this.Position, null, Color.White, 0, Vector2.Zero, _scale,
          SpriteEffects.FlipHorizontally, 1);
      }
      else
      {
        spriteBatch.Draw(this.Texture, this.Position, null, Color.White, 0, Vector2.Zero, _scale,
          SpriteEffects.None, 1);
      }

      spriteBatch.End();
    }
  }
}

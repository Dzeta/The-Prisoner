using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Cold_Ship
{
  public class PocketLightSource : GenericSprite2D
  {
    public static Cold_Ship GameInstance;

    public const float GLOW_INTERVAL = 200;
    public const int GLOW_TICK_SCALE = 10;
    public const float MAX_SCALE_FACTOR = 0.8f; // Empirical measure
    public const float TICK_INTERVAL = 200;

    private float _tickTimer;
    private Vector2 _scale = new Vector2(MAX_SCALE_FACTOR, MAX_SCALE_FACTOR);
    private int _scaleCount;
    private Vector2 _scaleOffset;
    private Vector2 _positionOffset;
    private Vector2 _positionOffsetLeft = new Vector2(-5, -50); // The offset of left
    private Vector2 _positionOffsetRight = new Vector2(40, -50); // The offset of right
    private Vector2 _actualFacingOffset;

    private enum LightState { ON, OFF, NOLIGHTING }


    private LightState _lightState;
    private Texture2D _lightOn;
    private Texture2D _lightOff;
    private float _lightSwitchInterval = 200;
    private float _lightSwitchTimer;

    private enum Facing { LEFT, RIGHT }

    private Facing _facing;

    private Character _owner;
    private Texture2D _darkCurtain;

    private PocketLightSource(Character owner, Texture2D tex)
      : base(tex)
    {
      this._owner = owner;
      this._facing = Facing.RIGHT;
      this._positionOffset = new Vector2(tex.Width / 2, tex.Height / 2);
      this._scaleCount = 0;
      this._actualFacingOffset = _positionOffsetRight;
      this._scaleOffset = (new Vector2(this.Texture.Width * (1 - this._scale.X)
          , this.Texture.Height * (1 - this._scale.Y))) / 2;
      this.Position = this._owner.Position - this._positionOffset + _actualFacingOffset + this._scaleOffset;

      this._lightState = LightState.ON;
    }

    public void TurnOff() { this._lightState = LightState.OFF; }
    public void TurnOn() { this._lightState = LightState.ON; }
    public void TurnDisable() { this._lightState = LightState.NOLIGHTING; }

    public static PocketLightSource GetNewInstance(Cold_Ship instance, Character character)
    {
      if (PocketLightSource.GameInstance == null) GameInstance = instance;
      Texture2D _texture = instance.Content.Load<Texture2D>("Textures/radius_of_light");
      Texture2D _textureLightOff = instance.Content.Load<Texture2D>("Textures/radius_of_light_off");

      PocketLightSource _instance = new PocketLightSource(character, _texture);
      _instance._lightOn = _texture;
      _instance._lightOff = _textureLightOff;

      return _instance;
    }

    public void Update(GameTime gameTime)
    {
      if (this._lightState == LightState.NOLIGHTING) return;

      this._tickTimer += gameTime.ElapsedGameTime.Milliseconds;
      this._lightSwitchTimer += gameTime.ElapsedGameTime.Milliseconds;

      if (Keyboard.GetState().IsKeyDown(Keys.D))
        this._facing = Facing.RIGHT;
      else if (Keyboard.GetState().IsKeyDown(Keys.A))
        this._facing = Facing.LEFT;

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


      this.Position = this._owner.Position - this._positionOffset + _actualFacingOffset + _scaleOffset;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (this._lightState == LightState.NOLIGHTING) return;

      spriteBatch.End();

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

      spriteBatch.Begin();
    }
  }
}

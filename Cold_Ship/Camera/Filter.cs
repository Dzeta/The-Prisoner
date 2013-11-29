using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cold_Ship
{
    public class Filter : GenericSprite2D
    {
      public const float GLOW_INTERVAL = 200;
      public const int GLOW_TICK_SCALE = 10;
      public const float MAX_SCALE_FACTOR = 0.8f;
      public const float TICK_INTERVAL = 200;
      public const float SNAPSHOT_TICK = 400;

      private float _tickTimer;
      private Vector2 _scale = new Vector2(MAX_SCALE_FACTOR, MAX_SCALE_FACTOR);
      private int _scaleCount;
      private Vector2 _scaleOffset;
      private Vector2 _positionOffset;
      private Vector2 _positionOffsetLeft = new Vector2(-5, -50);
      private Vector2 _positionOffsetRight = new Vector2(40, -50);
      private Vector2 _actualFacingOffset;

//      private enum LightState { }

      private Texture2D _lightOn;
      private Texture2D _lightOff;
      private float _lightSwitchInterval = 200;
      private float _lightSwitchTimer;

      private enum Facing { LEFT, RIGHT }

      private Facing _facing;

        public float filterScale = 1;

        public Filter(Texture2D texture, Vector2 position)
            : base(texture, position, Rectangle.Empty)
        {
        }

      public void Update(GameTime gameTime)
      {
        this._tickTimer += gameTime.ElapsedGameTime.Milliseconds;
        this._lightSwitchTimer += gameTime.ElapsedGameTime.Milliseconds;

        if (Keyboard.GetState().IsKeyDown(HelperFunction.KeyLeft))
          this._facing = Facing.LEFT;
        else if (Keyboard.GetState().IsKeyDown(HelperFunction.KeyRight))
          this._facing = Facing.RIGHT;

        if (this._tickTimer >= TICK_INTERVAL)
        {
          float _x = (float) (Math.Sin(_scaleCount)/Math.PI)/GLOW_TICK_SCALE;
          float _y = (float) (Math.Cos(_scaleCount)/Math.PI)/GLOW_TICK_SCALE;
          this._scale += new Vector2(_x, _y)*100;
          this._tickTimer = 0;
          this._scaleCount++;
          this._scaleOffset = (new Vector2(this.Texture.Width * (1-this._scale.X), this.Texture.Height*(1-this._scale.Y)))/2;
        }

        if (this._facing == Facing.RIGHT)
        {
          _actualFacingOffset = _positionOffsetRight;
        }
        else
        {
          _actualFacingOffset = _positionOffsetLeft;
        }
      }

        //draws the shadow filter onto the screen, the size of the filter
        //is changed according to the parameter
        public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            if (filterScale < 1)
            {
                base.Draw(spriteBatch, drawPosition);
            }

            drawPosition = new Vector2(
                (drawPosition.X) + ((Texture.Width - Texture.Width * filterScale) / 2),
                (drawPosition.Y + ((Texture.Height - Texture.Height * filterScale) / 2)));

          if (this._facing == Facing.RIGHT)
          {
            drawPosition += new Vector2(40, -50) + this._scale;
            spriteBatch.Draw(
              Texture, drawPosition, new Rectangle(0, 0, Texture.Width, Texture.Height)
              , Color.White, 0f, new Vector2(0, 0), filterScale
              , SpriteEffects.FlipHorizontally, 0);
          }
          else
          {
            drawPosition += new Vector2(-10, -50) + this._scale;
            spriteBatch.Draw(
              Texture, drawPosition, new Rectangle(0, 0, Texture.Width, Texture.Height)
              , Color.White, 0f, new Vector2(0, 0), filterScale
              , SpriteEffects.None, 0);
          }

//          spriteBatch.Draw(this.Texture, drawPosition 
//                , new Rectangle(0, 0, Texture.Width, Texture.Height)
//                , Color.White, 0, Vector2.Zero, _scale, SpriteEffects.None, 1);
        }
    }
}

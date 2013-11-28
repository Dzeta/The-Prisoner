using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cold_Ship
{
  public class GenericAnimated2DSprite : GenericSprite2D
  {
    public static Queue<Texture2D> ANIMATION_TEXTURES_FOR_LOADING;

    protected Queue<Texture2D> Textures; 
    protected int _FrameSwitchTime = 200; // Time between drawing
    protected int _FrameTimer;
    protected bool loopAnimation;

    public GenericAnimated2DSprite(/*GameLevel instance,*/ Queue<Texture2D> textures, Vector2 position, bool repeat)
      : base(textures.Peek(), position)
    {
      this.Textures = textures;
      this.Texture = textures.Dequeue();
      if (repeat)
      {
          this.Textures.Enqueue(this.Texture);
      }
      loopAnimation = repeat;
    }

    //public override void Update(GameTime gameTime)
    //{
    //    if (loopAnimation)
    //    {
    //        _FrameTimer += gameTime.ElapsedGameTime.Milliseconds;
    //        if (_FrameTimer >= _FrameSwitchTime)
    //        {
    //            _FrameTimer = 0;
    //            this.Texture = Textures.Dequeue();
    //            this.Textures.Enqueue(this.Texture);
    //        }
    //    }
    //    else if (!loopAnimation)
    //    {
    //        _FrameTimer += gameTime.ElapsedGameTime.Milliseconds;
    //        if (_FrameTimer >= _FrameSwitchTime)
    //        {
    //            _FrameTimer = 0;
    //            this.Texture = Textures.Dequeue();
    //        }
    //    }
    //}

//    // CAN BE OVERWRITE IF NEEDED, SO FAR THEY DO NOTHING
//    public override void Draw(SpriteBatch spriteBatch
//      , Vector2 drawPosition)
//    {
//      base.Draw(spriteBatch, drawPosition);
//    }

//    public override void Draw(SpriteBatch spriteBatch)
//    {
//      base.Draw(spriteBatch);
//    }

    }
}

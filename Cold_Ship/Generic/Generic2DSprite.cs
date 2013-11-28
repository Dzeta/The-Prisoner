using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cold_Ship;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cold_Ship
{
  public class Sprite2D
  {
    public Texture2D Texture;
    public Rectangle BoundBox;
    public Vector2 Position;

    protected Vector2 Size;

    protected Sprite2D(Texture2D texture, Vector2 position) : this(texture, position, Rectangle.Empty) { }

    protected Sprite2D(Texture2D texture, Vector2 position, Rectangle boundBox)
    {
      this.Texture = texture;
      this.Position = position;
      this.BoundBox = boundBox;
    }

    public virtual void Update(GameTime gameTime) { }

    public virtual void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
      spriteBatch.Begin();
      spriteBatch.Draw(Texture, position, Color.White);
      if (Cold_Ship.DEBUG_MODE)
        spriteBatch.Draw(Cold_Ship.DEBUG_TEXTURE
            , this.GetBoundingBox(), null, Color.White
            , 0, Vector2.Zero, SpriteEffects.None, 1);
      spriteBatch.End();
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      spriteBatch.Draw(Texture, this.Position, Color.White);
      if (Cold_Ship.DEBUG_MODE)
        spriteBatch.Draw(Cold_Ship.DEBUG_TEXTURE
            , this.GetBoundingBox(), null, Color.White
            , 0, Vector2.Zero, SpriteEffects.None, 1);
      spriteBatch.End();

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

    public Rectangle GetBoundingBox()
    {
      if (this.BoundBox.Width == 0 && this.BoundBox.Height == 0)
        return this.GetDefaultBoundingBox();
      return new Rectangle((int)this.Position.X
          , (int)this.Position.Y, this.BoundBox.Width
          , this.BoundBox.Height);
    }

    public bool CheckCollision(Sprite2D sprite)
    {
      return this.GetBoundingBox()
          .Intersects(sprite.GetBoundingBox());
    }
  }


}
public class GenericSprite2D : Sprite2D
{
  public GameLevel CurrentGameLevel; // A reference to this object/sprite exists in which level

  protected GenericSprite2D(GameLevel instance, Texture2D texture)
    : this(instance, texture, Vector2.One, Rectangle.Empty) { }
  public GenericSprite2D(GameLevel instance, Texture2D texture, Vector2 position)
    : this(instance, texture, position, Rectangle.Empty) { }

  protected GenericSprite2D(GameLevel instance, Texture2D texture, Vector2 position, Rectangle boundBox)
    : base(texture, position, boundBox)
  {
    this.CurrentGameLevel = instance;
  }
}

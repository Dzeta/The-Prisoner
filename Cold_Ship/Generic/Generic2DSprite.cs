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
      spriteBatch.End(); 
      if (Cold_Ship.DEBUG_MODE && this.GetBoundingBox().Width < 600)
        DebugSprite.GetNewInstance(this).Draw(spriteBatch, position);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      spriteBatch.Draw(Texture, this.Position, Color.White);
      spriteBatch.End();
      if (Cold_Ship.DEBUG_MODE && this.GetBoundingBox().Width < 600)
        DebugSprite.GetNewInstance(this).Draw(spriteBatch);
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
      return this.GetBoundingBox().Intersects(sprite.GetBoundingBox());
    }

    public bool CheckCollision(Rectangle boundBox)
    {
      return this.GetBoundingBox().Intersects(boundBox);
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


  public class DebugSprite : Sprite2D
  {
    public static Texture2D DEBUG_TEXTURE;

    public Sprite2D _owner;
    public DebugSprite(Cold_Ship gameInstance)
      : base(DEBUG_TEXTURE, Vector2.Zero, Rectangle.Empty)
    {
      if (DEBUG_TEXTURE == null)
        DEBUG_TEXTURE = gameInstance.Content.Load<Texture2D>("Textures/debug_textures");
    }

    protected DebugSprite(Sprite2D owner)
      : base(DEBUG_TEXTURE, owner.Position, owner.BoundBox)
    {
      this._owner = owner;
    }

    public static DebugSprite GetNewInstance(Sprite2D owner)
    {
      return new DebugSprite(owner);
    }

    public override void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
      Rectangle _dest = new Rectangle((int)position.X, (int)position.Y, this.GetDefaultBoundingBox().Width, this.GetDefaultBoundingBox().Height);
      spriteBatch.Begin();
      spriteBatch.Draw(DEBUG_TEXTURE
          , _dest, null, Color.White
          , 0, Vector2.Zero, SpriteEffects.None, 1);
      spriteBatch.End();
    }
  }
}


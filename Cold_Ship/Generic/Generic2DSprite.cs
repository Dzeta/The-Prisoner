using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cold_Ship
{
    // Generic Sprite 2D class for inheritance
    public class GenericSprite2D
    {
      public GameLevel CurrentGameLevel; // A refernece to this object/sprite exists in which level
        public Texture2D Texture;
        public Vector2 Position;
        public Rectangle BoundBox;

        protected GenericSprite2D(GameLevel instance, Texture2D texture) 
            : this(instance, texture, Vector2.One, Rectangle.Empty) { }
        public GenericSprite2D(GameLevel instance, Texture2D texture, Vector2 position) 
            : this(instance, texture, position, Rectangle.Empty) { }
        protected GenericSprite2D(GameLevel instance, Texture2D texture
            , Vector2 position, Rectangle boundBox)
        {
            this.Texture = texture;
            this.Position = position;
            this.BoundBox = boundBox;
          this.CurrentGameLevel = instance;
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
        return this.BoundBox;
      }

        public bool CheckCollision(GenericSprite2D sprite)
        {
            return this.BoundBox.Intersects(sprite.BoundBox);
        }

      public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            spriteBatch.Draw(Texture, drawPosition, Color.White);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, this.Position, Color.White);
        }
    }
}

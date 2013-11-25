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
        public Texture2D Texture;
        public Vector2 Position;
        public Rectangle BoundBox;

        public GenericSprite2D(Texture2D texture) : this(texture, Vector2.One, Rectangle.Empty) { }
        public GenericSprite2D(Texture2D texture, Vector2 position) : this(texture, position, Rectangle.Empty) { }

        public GenericSprite2D(Texture2D texture, Vector2 position, Rectangle boundBox)
        {
            this.Texture = texture;
            this.Position = position;
            this.BoundBox = boundBox;
        }

        public bool CheckCollision(GenericSprite2D sprite)
        {
            return this.BoundBox.Intersects(sprite.BoundBox);
        }

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

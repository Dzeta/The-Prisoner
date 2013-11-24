using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace Cold_Ship
{
    public class Ladder : GenericSprite2D
    {
        //declare member variables
        public Vector2 size;
        
        private Rectangle collision;
        //declare constructor
        public Ladder(Texture2D texture, Vector2 size, Vector2 position)
            : base(texture, position, Rectangle.Empty)
        {
            this.size = size;
            collision = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }

        //update method that handles the collisions
        public bool CanClimb(Character player)
        {
            return collision.Contains(new Rectangle((int)player.position.X + 8, (int)player.position.Y + (int)player.playerSpriteSize.Y - 5, (int)player.playerSpriteSize.X - 16, 5));
        }

        public bool isOnTop(Character player)
        {
            return ((player.position.X >= position.X-8 && player.position.X + 32 <= position.X + size.X+8)
               && player.position.Y + 64 >= position.Y && player.position.Y < position.Y);
        }

        //draw method that draws the platform onto the screen
        public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            spriteBatch.Draw(texture, drawPosition, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
        }
    }
}

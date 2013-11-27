using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace Cold_Ship
{
    public class Platform : GenericSprite2D
    {
        //declare member variables
        public Vector2 size;
        public Rectangle collisionUp, collisionDown, collisionLeft, collisionRight;

        //optional parameter(usefulness to be determined)
        public Vector2 collisionUpPosition, collisionDownPosition, collisionLeftPosition, collisionRightPosition;

        //declare constructor
        public Platform(Texture2D texture, Vector2 size, Vector2 position)
            : base(texture, position, Rectangle.Empty)
        {
            this.size = size;
            collisionUp = new Rectangle((int)(position.X /*+ (1 * (size.X / 10))*/), (int)(position.Y), (int)size.X/*(8 * (size.X / 10))*/, (int)size.Y / 6);
            collisionDown = new Rectangle((int)(position.X + (1 * (size.X / 10))), (int)(position.Y + (5 * (size.Y / 6))), (int)(8 * (size.X / 10)), (int)size.Y / 6);
            collisionLeft = new Rectangle((int)position.X, (int)(position.Y + (1 * (size.Y / 15))), (int)size.X / 1000, (int)(size.Y));
            //collisionLeft = new Rectangle((int)Position.X, (int)(Position.Y /*+ (1 * (size.Y / 6))*/), (int)size.X / 10, (int)(size.Y));
            collisionRight = new Rectangle((int)(position.X + (7 * (size.X / 8))), (int)(position.Y + (1 * (size.Y / 15))), (int)size.X / 10, (int)(size.Y));
        }

        //update method that handles the collisions
        public virtual bool Update(Character player, Vector2 prevPosition, float jumpTimer, float ground, bool isJumping)
        {
            bool canJump = true;
            if (new Rectangle((int)player.Position.X /*+ 8*/, (int)player.Position.Y /*+ (int)player.playerSpriteSize.Y - 5*/, (int)player.playerSpriteSize.X /*- 16*/, (int)player.playerSpriteSize.Y/*5*/).Intersects(collisionUp))
            {
                player.Position = prevPosition;
                canJump = false;
            }
            if (new Rectangle((int)player.Position.X /*+ 7*/, (int)player.Position.Y, (int)player.playerSpriteSize.X /*- 7*/, (int)player.playerSpriteSize.Y).Intersects(collisionLeft))
            {
                player.Position.X = prevPosition.X;
            }
            if (new Rectangle((int)player.Position.X/* + 7*/, (int)player.Position.Y, (int)player.playerSpriteSize.X /*- 7*/, (int)player.playerSpriteSize.Y).Intersects(collisionRight))
            {
                player.Position = prevPosition;
            }
            else if (new Rectangle((int)player.Position.X /*+ 7*/, (int)player.Position.Y, (int)player.playerSpriteSize.X /*- 7*/, (int)player.playerSpriteSize.Y).Intersects(collisionDown))
            {
                player.Position = prevPosition;
            }
            return canJump;
        }

        //draw method that draws the platform onto the screen
        public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            spriteBatch.Draw(Texture, drawPosition, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
        }
    }
}

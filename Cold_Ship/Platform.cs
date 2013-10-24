using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace Cold_Ship
{
    public class Platform
    {
        //declare member variables
        public Texture2D texture;
        public Vector2 position;
        public Vector2 size;
        public Rectangle collisionUp, collisionDown, collisionLeft, collisionRight;
        
        //optional parameter(usefulness to be determined)
        public Vector2 collisionUpPosition, collisionDownPosition, collisionLeftPosition, collisionRightPosition;

        //declare constructor
        public Platform(Texture2D texture, Vector2 size, Vector2 position)
        {
            this.texture = texture;
            this.size = size;
            this.position = position;
            collisionUp = new Rectangle((int)(position.X /*+ (1 * (size.X / 10))*/), (int)(position.Y), (int)size.X/*(8 * (size.X / 10))*/, (int)size.Y / 6);
            collisionDown = new Rectangle((int)(position.X + (1 * (size.X / 10))), (int)(position.Y + (5 * (size.Y / 6))), (int)(8 * (size.X / 10)), (int)size.Y / 6);
            collisionLeft = new Rectangle((int)position.X, (int)(position.Y + (1 * (size.Y / 15))), (int)size.X / 1000, (int)(size.Y));
            //collisionLeft = new Rectangle((int)position.X, (int)(position.Y /*+ (1 * (size.Y / 6))*/), (int)size.X / 10, (int)(size.Y));
            collisionRight = new Rectangle((int)(position.X + (7 * (size.X / 8))), (int)(position.Y + (1 * (size.Y / 15))), (int)size.X / 10, (int)(size.Y));
        }

        //update method that handles the collisions
        public bool Update(Scene2DNode player, Vector2 prevPosition, float jumpTimer, float ground, bool isJumping)
        {
            bool canJump = true;
            if (new Rectangle((int)player.position.X, (int)player.position.Y, (int)player.playerSpriteSize.X, (int)player.playerSpriteSize.Y).Intersects(collisionUp))
            {

                player.position = prevPosition;

                if (player.position.X + player.playerSpriteSize.X > collisionUp.X + collisionUp.Width / 5 && player.position.X < collisionUp.X + collisionUp.Width / 5 * 4)
                {
                    canJump = false;
                }
                }
            if (new Rectangle((int)player.position.X, (int)player.position.Y, (int)player.playerSpriteSize.X, (int)player.playerSpriteSize.Y).Intersects(collisionLeft))
            {

                player.position.X = prevPosition.X;
                //player.ApplyGravity(jumpTimer, ground);
            }
            if (new Rectangle((int)player.position.X, (int)player.position.Y, (int)player.playerSpriteSize.X, (int)player.playerSpriteSize.Y).Intersects(collisionRight))
            {
                player.position = prevPosition;
                //player.ApplyGravity(jumpTimer, ground);
            }
            // if (new Rectangle((int)player.position.X, (int)player.position.Y, (int)player.texture.Width, (int)player.texture.Height).Intersects(collisionUp))
            //{
            //    player.position.Y = prevPosition.Y;
            //    return false;
            //}
            else if (new Rectangle((int)player.position.X, (int)player.position.Y, (int)player.playerSpriteSize.X, (int)player.playerSpriteSize.Y).Intersects(collisionDown))
            {
                player.position = prevPosition;
            }
            return canJump;
            //if (new Rectangle((int)player.position.X, (int)player.position.Y, (int)player.texture.Width, (int)player.texture.Height).Intersects(new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y)))
            //{
            //    player.position = prevPosition;
            //}
        }

        //draw method that draws the platform onto the screen
        public void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            spriteBatch.Draw(texture, drawPosition, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
        }
    }
}

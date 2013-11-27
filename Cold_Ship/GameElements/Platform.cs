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
        public Rectangle collisionUpLeft, collisionUpRight, collisionDownLeft, collisionDownRight;

        //optional parameter(usefulness to be determined)
        public Vector2 collisionUpPosition, collisionDownPosition, collisionLeftPosition, collisionRightPosition;

        //declare constructor
        public Platform(Texture2D texture, Vector2 size, Vector2 position)
            : base(texture, position, Rectangle.Empty)
        {
            this.size = size;
            //original
            //collisionUp = new Rectangle((int)(position.X), (int)(position.Y), (int)size.X, (int)size.Y / 6);
            //collisionDown = new Rectangle((int)(position.X), (int)(position.Y + (5 * (size.Y / 6))), (int)size.X, (int)size.Y / 6);
            //collisionLeft = new Rectangle((int)(position.X - (2 * (size.X / 100))), (int)(position.Y + (2 * (size.Y / 6))), (int)size.X * 3 / 100, (int)(size.Y * 2 / 6));
            //collisionRight = new Rectangle((int)(position.X + (99 * (size.X / 100))), (int)(position.Y + (2 * (size.Y / 6))), (int)size.X * 3 / 100, (int)(size.Y * 2 / 6));
            
            //update1
            //collisionUp = new Rectangle((int)(position.X + (size.X * 1 / 10)), (int)(position.Y), (int)(size.X * 8 / 10), (int)(size.Y / 10));
            //collisionDown = new Rectangle((int)(position.X + (size.X * 1 / 10)), (int)(position.Y + (9 * (size.Y / 10))), (int)(size.X * 8 / 10), (int)size.Y / 10);
            //collisionLeft = new Rectangle((int)(position.X), (int)(position.Y + (size.Y / 10)), (int)(size.X * 3 / 10), (int)(size.Y * 8 / 10));
            //collisionRight = new Rectangle((int)(position.X + (size.X * 7 / 10)), (int)(position.Y + (size.Y / 10)), (int)(size.X * 3 / 10), (int)(size.Y * 8 / 10));

            //collisionUpLeft = new Rectangle((int)(position.X), (int)(position.Y), (int)(size.X / 10), (int)(size.Y / 10));
            //collisionUpRight = new Rectangle((int)(position.X + (size.X * 9 / 10)), (int)(position.Y), (int)(size.X / 10), (int)(size.Y / 10));
            //collisionDownLeft = new Rectangle((int)(position.X), (int)(position.Y + (size.Y * 9 / 10)), (int)(size.X / 10), (int)(size.Y / 10));
            //collisionDownRight = new Rectangle((int)(position.X + (size.X * 9 / 10)), (int)(position.Y + (size.Y * 9 / 10)), (int)(size.X / 10), (int)(size.Y / 10));

            //update2
            collisionUp = new Rectangle((int)(position.X + 5), (int)(position.Y), (int)(size.X - 10), (int)(size.Y / 10));
            collisionDown = new Rectangle((int)(position.X + 5), (int)(position.Y + (9 * (size.Y / 10))), (int)(size.X - 10), (int)size.Y / 10);
            collisionLeft = new Rectangle((int)(position.X), (int)(position.Y + 5), (int)(size.X * 3 / 10), (int)(size.Y - 10));
            collisionRight = new Rectangle((int)(position.X + (size.X * 7 / 10)), (int)(position.Y + 5), (int)(size.X * 3 / 10), (int)(size.Y - 10));

            collisionUpLeft = new Rectangle((int)(position.X), (int)(position.Y), (int)(5), (int)(5));
            collisionUpRight = new Rectangle((int)(position.X + size.X - 5), (int)(position.Y), (int)(5), (int)(5));
            collisionDownLeft = new Rectangle((int)(position.X), (int)(position.Y + size.Y - 5), (int)(5), (int)(5));
            collisionDownRight = new Rectangle((int)(position.X + size.X - 5), (int)(position.Y + size.Y - 5), (int)(5), (int)(5));
        }

        //update method that handles the collisions
        public virtual bool Update(Character player, Vector2 prevPosition, float jumpTimer, float ground, bool isJumping)
        {
            bool canJump = true;
            bool touchUp = false, touchDown = false, touchLeft = false, touchRight = false;
            bool touchUpLeft = false, touchUpRight = false, touchDownLeft = false, touchDownRight = false;
            if (new Rectangle((int)player.Position.X /*+ 7*/, (int)player.Position.Y, (int)player.playerSpriteSize.X /*- 7*/, (int)player.playerSpriteSize.Y).Intersects(collisionLeft))
            {
                //Console.Out.WriteLine("Left hit box touched!");
                //player.Position.X = prevPosition.X;
                touchLeft = true;
            }
            if (new Rectangle((int)player.Position.X/* + 7*/, (int)player.Position.Y, (int)player.playerSpriteSize.X /*- 7*/, (int)player.playerSpriteSize.Y).Intersects(collisionRight))
            {
                //Console.Out.WriteLine("Right hit box touched!");
                //player.Position.X = prevPosition.X;
                touchRight = true;
            }
            
            if (new Rectangle((int)player.Position.X /*+ 8*/, (int)player.Position.Y /*+ (int)player.playerSpriteSize.Y - 5*/, (int)player.playerSpriteSize.X /*- 16*/, (int)player.playerSpriteSize.Y/*5*/).Intersects(collisionUp))
            {
                //Console.Out.WriteLine("Top hit box touched!");
                //player.Position.Y = prevPosition.Y;
                //canJump = false;
                touchUp = true;
            }
            if (new Rectangle((int)player.Position.X /*+ 7*/, (int)player.Position.Y, (int)player.playerSpriteSize.X /*- 7*/, (int)player.playerSpriteSize.Y).Intersects(collisionDown))
            {
                //Console.Out.WriteLine("Bottom hit box touched!");
                //player.Position.Y = prevPosition.Y;
                touchDown = true;
            }
            if (new Rectangle((int)player.Position.X, (int)player.Position.Y, (int)player.playerSpriteSize.X, (int)player.playerSpriteSize.Y).Intersects(collisionUpLeft))
            {
                //Console.Out.WriteLine("Bottom hit box touched!");
                touchUpLeft = true;
            }
            if (new Rectangle((int)player.Position.X, (int)player.Position.Y, (int)player.playerSpriteSize.X, (int)player.playerSpriteSize.Y).Intersects(collisionUpRight))
            {
                //Console.Out.WriteLine("Bottom hit box touched!");
                touchUpRight = true;
            }
            if (new Rectangle((int)player.Position.X, (int)player.Position.Y, (int)player.playerSpriteSize.X, (int)player.playerSpriteSize.Y).Intersects(collisionDownLeft))
            {
                //Console.Out.WriteLine("Bottom hit box touched!");
                touchDownLeft = true;
            }
            if (new Rectangle((int)player.Position.X, (int)player.Position.Y, (int)player.playerSpriteSize.X, (int)player.playerSpriteSize.Y).Intersects(collisionDownRight))
            {
                //Console.Out.WriteLine("Bottom hit box touched!");
                touchDownRight = true;
            }


            if (touchUp && (touchUpLeft || touchUpRight))
            {
                Console.Out.WriteLine("Top!");
                player.Position.Y = prevPosition.Y;
                canJump = false;
            }
            else if ((touchLeft && (touchUpLeft || touchDownLeft)))
            {
                Console.Out.WriteLine("Left Side!");
                player.Position.X = collisionLeft.X - 32;
            }
            else if ((touchRight && (touchUpRight || touchDownRight) && !touchDown))
            {
                Console.Out.WriteLine("Right Side!");
                player.Position.X = collisionRight.X + collisionRight.Width;
            }
            else if (touchDown && (touchUpLeft || touchUpRight))
            {
                Console.Out.WriteLine("Down!");
                player.Position.Y = prevPosition.Y;
            }
            else if (touchUpLeft || touchUpRight || touchDownLeft || touchDownRight)
            {
                player.Position = prevPosition;
            }
            else if (touchUp)
            {
                player.Position.Y = prevPosition.Y;
                canJump = false;
            }
            else if (touchDown)
            {
                player.Position.Y = prevPosition.Y;
            }
            else if (touchRight)
            {
                player.Position.X = prevPosition.X;
            }
            else if (touchLeft)
            {
                player.Position.X = prevPosition.X;
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

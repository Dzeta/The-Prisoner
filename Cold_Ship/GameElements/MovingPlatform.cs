using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cold_Ship
{
    class MovingPlatform : Platform
    {
        private Vector2 startPosition;
        private Vector2 limits;
        private Vector2 velocity;

        public MovingPlatform(Texture2D texture, Vector2 size, Vector2 position, Vector2 movingDirection, Vector2 limits)
            : base(texture, size, position)
        {
            startPosition = new Vector2(position.X, position.Y);
            velocity = movingDirection;
            this.limits = limits;
        }

        public override bool Update(Character player, Vector2 prevPosition, float jumpTimer, float ground, bool isJumping)
        {
            bool ret = false;
            if (Position.X + velocity.X > startPosition.X + limits.X || Position.X + velocity.X < startPosition.X)
                velocity.X = -velocity.X;
            if (Position.Y + velocity.Y > startPosition.Y + limits.Y || Position.Y + velocity.Y < startPosition.Y)
                velocity.Y = -velocity.Y;

            Position += velocity;

            //update2
            collisionUp = new Rectangle((int)(Position.X + 5), (int)(Position.Y), (int)(size.X - 10), (int)(size.Y / 10));
            collisionDown = new Rectangle((int)(Position.X + 5), (int)(Position.Y + (9 * (size.Y / 10))), (int)(size.X - 10), (int)size.Y / 10);
            collisionLeft = new Rectangle((int)(Position.X), (int)(Position.Y + 5), (int)(size.X * 3 / 10), (int)(size.Y - 10));
            collisionRight = new Rectangle((int)(Position.X + (size.X * 7 / 10)), (int)(Position.Y + 5), (int)(size.X * 3 / 10), (int)(size.Y - 10));

            collisionUpLeft = new Rectangle((int)(Position.X), (int)(Position.Y), (int)(5), (int)(5));
            collisionUpRight = new Rectangle((int)(Position.X + size.X - 5), (int)(Position.Y), (int)(5), (int)(5));
            collisionDownLeft = new Rectangle((int)(Position.X), (int)(Position.Y + size.Y - 5), (int)(5), (int)(5));
            collisionDownRight = new Rectangle((int)(Position.X + size.X - 5), (int)(Position.Y + size.Y - 5), (int)(5), (int)(5));

            //collisionUp = new Rectangle((int)(Position.X + (size.X * 1 / 10)), (int)(Position.Y), (int)(size.X * 8 / 10), (int)(size.Y / 10));
            //collisionDown = new Rectangle((int)(Position.X + (size.X * 1 / 10)), (int)(Position.Y + (9 * (size.Y / 10))), (int)(size.X * 8 / 10), (int)size.Y / 10);
            //collisionLeft = new Rectangle((int)(Position.X), (int)(Position.Y + (size.Y / 10)), (int)(size.X * 3 / 10), (int)(size.Y * 8 / 10));
            //collisionRight = new Rectangle((int)(Position.X + (size.X * 7 / 10)), (int)(Position.Y + (size.Y / 10)), (int)(size.X * 3 / 10), (int)(size.Y * 8 / 10));

            //collisionUpLeft = new Rectangle((int)(Position.X), (int)(Position.Y), (int)(size.X / 10), (int)(size.Y / 10));
            //collisionUpRight = new Rectangle((int)(Position.X + (size.X * 9 / 10)), (int)(Position.Y), (int)(size.X / 10), (int)(size.Y / 10));
            //collisionDownLeft = new Rectangle((int)(Position.X), (int)(Position.Y + (size.Y * 9 / 10)), (int)(size.X / 10), (int)(size.Y / 10));
            //collisionDownRight = new Rectangle((int)(Position.X + (size.X * 9 / 10)), (int)(Position.Y + (size.Y * 9 / 10)), (int)(size.X / 10), (int)(size.Y / 10));


            //collisionUp = new Rectangle((int)(Position.X), (int)(Position.Y), (int)size.X, (int)size.Y / 6);
            //collisionDown = new Rectangle((int)(Position.X + (1 * (size.X / 10))), (int)(Position.Y + (5 * (size.Y / 6))), (int)(8 * (size.X / 10)), (int)size.Y / 6);
            //collisionLeft = new Rectangle((int)Position.X, (int)(Position.Y + (1 * (size.Y / 15))), (int)size.X / 1000, (int)(size.Y));
            //collisionRight = new Rectangle((int)(Position.X + (7 * (size.X / 8))), (int)(Position.Y + (1 * (size.Y / 15))), (int)size.X / 10, (int)(size.Y));

            ret = base.Update(player, prevPosition, jumpTimer, ground, isJumping);

            // TODO something to change there to make the player follow the platform smoothly
            if (new Rectangle((int)player.Position.X, (int)player.Position.Y, (int)player.playerSpriteSize.X, (int)player.playerSpriteSize.Y + 5).Intersects(collisionUp))
                player.Position += velocity;

            return ret;
        }
    }
}

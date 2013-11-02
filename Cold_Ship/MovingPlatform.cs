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

        public override bool Update(Scene2DNode player, Vector2 prevPosition, float jumpTimer, float ground, bool isJumping)
        {
            if (position.X + velocity.X > startPosition.X + limits.X || position.X + velocity.X < startPosition.X - limits.X)
                velocity.X = -velocity.X;
            if (position.Y + velocity.Y > startPosition.Y + limits.Y || position.Y + velocity.Y < startPosition.Y - limits.Y)
                velocity.Y = -velocity.Y;

            position += velocity;

            collisionUp = new Rectangle((int)(position.X), (int)(position.Y), (int)size.X, (int)size.Y / 6);
            collisionDown = new Rectangle((int)(position.X + (1 * (size.X / 10))), (int)(position.Y + (5 * (size.Y / 6))), (int)(8 * (size.X / 10)), (int)size.Y / 6);
            collisionLeft = new Rectangle((int)position.X, (int)(position.Y + (1 * (size.Y / 15))), (int)size.X / 1000, (int)(size.Y));
            collisionRight = new Rectangle((int)(position.X + (7 * (size.X / 8))), (int)(position.Y + (1 * (size.Y / 15))), (int)size.X / 10, (int)(size.Y));

            // TODO something to change there to make the player follow the platform smoothly
            if (new Rectangle((int)player.position.X, (int)player.position.Y, (int)player.playerSpriteSize.X, (int)player.playerSpriteSize.Y).Intersects(collisionUp))
                player.position += velocity;

            return base.Update(player, prevPosition, jumpTimer, ground, isJumping);
        }
    }
}

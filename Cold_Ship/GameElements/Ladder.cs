using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace Cold_Ship
{
    public class Ladder : GenericSprite2D
    {
      public static Texture2D LADDER_TEXTURE;
        public Vector2 size;
        
        private Rectangle collision;
        //declare constructor
        public Ladder(GameLevel instance, Vector2 position)
            : base(instance, LADDER_TEXTURE, position)
        {
            this.size = size;
            collision = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }

      public static Ladder GetNewInstance(GameLevel instance, Vector2 position)
      {
        if (LADDER_TEXTURE == null)
          Ladder.LADDER_TEXTURE = instance.Content.Load<Texture2D>("Objects/ladder");

        Ladder _instance = new Ladder(instance, position);
        _instance.Texture = LADDER_TEXTURE;
        _instance.BoundBox = new Rectangle(
          0, 0, LADDER_TEXTURE.Width, LADDER_TEXTURE.Height);

        return _instance;
      }

        //update method that handles the collisions
        public bool CanClimb(Character player)
        {
            return collision.Contains(new Rectangle((int)player.Position.X + 8, (int)player.Position.Y + (int)player.playerSpriteSize.Y - 5, (int)player.playerSpriteSize.X - 16, 5));
        }

        public bool isOnTop(Character player)
        {
            return ((player.Position.X >= Position.X-8 && player.Position.X + 32 <= Position.X + size.X+8)
               && player.Position.Y + 64 >= Position.Y && player.Position.Y < Position.Y);
        }

        //draw method that draws the platform onto the screen
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, this.Position, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
        }
    }
}

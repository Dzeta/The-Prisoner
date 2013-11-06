using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace Cold_Ship
{
    public class Portal : GenericSprite2D
    {
        //declare member data
        public Vector2 size;
        public enum PortalType { FOWARD, BACKWARD };
        PortalType portalType;


        //constructor that initialize the texture and position
        public Portal(Texture2D texture, Vector2 position, Vector2 size, PortalType portalType ) :
            base(texture, position, Rectangle.Empty)
        {
            this.size = size;
            this.portalType = portalType;
        }

        //check is the player has interacted with the portal
        public void Update(Character playerNode, ref Game_Level gameLevel, bool doorCanOpen)
        {
            if (new Rectangle((int)playerNode.position.X, (int)playerNode.position.Y, (int)playerNode.playerSpriteSize.X, (int)playerNode.playerSpriteSize.Y) .Intersects(new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y)))
            {
                if (portalType == PortalType.FOWARD)
                {
                    if(doorCanOpen)
                        gameLevel += 1;
                }
                
                if (portalType == PortalType.BACKWARD)
                {
                    gameLevel -= 1;
                }

                if (gameLevel < 0)
                {
                    gameLevel = 0;
                }
            }
        }

        //draw the portal onto screen
        public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            spriteBatch.Draw(texture, drawPosition, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
        }
    }
}

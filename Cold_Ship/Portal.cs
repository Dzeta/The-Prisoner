using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Cold_Ship
{
    public class Portal : GenericSprite2D
    {
        //declare member data
        public Vector2 size;
        public enum PortalType { FOWARD, BACKWARD };
        PortalType portalType;
        Texture2D doorLightGreen, doorLightRed;
        public bool canOpen;

        //constructor that initialize the texture and position
        public Portal(Vector2 position, Vector2 size, PortalType portalType, ContentManager Content) :
            base(Content.Load<Texture2D>("door"), position, Rectangle.Empty)
        {
            this.size = size;
            this.portalType = portalType;
            doorLightGreen = Content.Load<Texture2D>("doorlight_green");
            doorLightRed = Content.Load<Texture2D>("doorlight_red");
            canOpen = (portalType == PortalType.BACKWARD)
                                ? true
                                : false;
        }

        //check is the player has interacted with the portal
        public void Update(Character playerNode, ref Game_Level gameLevel)
        {
            if (new Rectangle((int)playerNode.position.X, (int)playerNode.position.Y, (int)playerNode.playerSpriteSize.X, (int)playerNode.playerSpriteSize.Y) .Intersects(new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y)))
            {
                if (portalType == PortalType.FOWARD)
                {
                    if(canOpen)
                        gameLevel += 1;
                }
                
                else if (portalType == PortalType.BACKWARD)
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
            if(canOpen)
                spriteBatch.Draw(doorLightGreen, drawPosition + new Vector2(14, -13), Color.White);
            else
                spriteBatch.Draw(doorLightRed, drawPosition + new Vector2(14, -13), Color.White);

            spriteBatch.Draw(texture, drawPosition, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
        }
    }
}

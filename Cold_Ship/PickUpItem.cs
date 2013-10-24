using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace Cold_Ship
{
    public class PickUpItem : Scene2DNode
    {
        //item type enum
        public enum ItemType {LIGHT, STAMINA, TEMPERATURE};

        //declare member data
        public ItemType itemType;
        public float effect;
        public Vector2 size;

        //constructor
        public PickUpItem(Texture2D texture, Vector2 position, Vector2 size, ItemType itemType, float effect) :
            base(texture, position)
        {
            this.itemType = itemType;
            this.effect = effect;
            this.size = size;
        }

        //update method
        public void Update(ref Scene2DNode playerNode)
        {
            if (new Rectangle((int)playerNode.position.X, (int)playerNode.position.Y, (int)playerNode.texture.Width, (int)playerNode.texture.Height).Intersects(new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y)))
            {
                if (itemType == ItemType.STAMINA)
                {
                    playerNode.staminaLimit += effect;
                    playerNode.stamina = playerNode.staminaLimit;
                }

                else if (itemType == ItemType.TEMPERATURE)
                {
                    playerNode.bodyTemperature += effect;
                }

                //temporary fix
                position = new Vector2(2048, 2048);
            }
        }


        //draw the item onto screen
        public void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            spriteBatch.Draw(texture, drawPosition, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace Cold_Ship
{
    public class PickUpItem : GenericSprite2D
    {
        //item type enum
        public enum ItemType {LIGHT, STAMINA, TEMPERATURE, NONE};
        public enum ItemEffectDuration { TEMPORARY, PERMANENT, NONE };

        //declare member data
        public ItemType itemType;
        public ItemEffectDuration itemEffectDuration;
        public float effect;
        public Vector2 size;

        //constructor
        public PickUpItem(Texture2D texture, Vector2 position, Vector2 size, ItemType itemType, float effect, ItemEffectDuration itemEffectDuration) :
            base(texture, position, Rectangle.Empty)
        {
            this.itemType = itemType;
            this.effect = effect;
            this.size = size;
            this.itemEffectDuration = itemEffectDuration;
        }

        //update method
        public void Update(Character playerNode, ref double bodyTemperature, ref double stamina, ref double staminaLimit)
        {
            if (new Rectangle((int)playerNode.Position.X, (int)playerNode.Position.Y, (int)playerNode.playerSpriteSize.X, (int)playerNode.playerSpriteSize.Y).Intersects(new Rectangle((int)Position.X, (int)Position.Y, (int)size.X, (int)size.Y)))
            {
                if (itemType == ItemType.STAMINA)
                {
                    if (itemEffectDuration == ItemEffectDuration.PERMANENT)
                    {
                        playerNode.staminaLimit += effect;
                        playerNode.stamina = playerNode.staminaLimit;
                        staminaLimit = stamina = playerNode.staminaLimit;
                    }
                    else if (itemEffectDuration == ItemEffectDuration.TEMPORARY)
                    {
                        //playerNode.staminaLimit += effect;
                        //playerNode.stamina = playerNode.staminaLimit;
                        playerNode.stamina += effect;
                    }
                }

                else if (itemType == ItemType.TEMPERATURE)
                {
                    playerNode.bodyTemperature += effect;
                }

                //temporary fix
                Position = new Vector2(2048, 2048);
            }
        }


        //draw the item onto screen
        public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            spriteBatch.Draw(Texture, drawPosition, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
        }
    }
}

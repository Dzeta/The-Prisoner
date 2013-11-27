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
        public enum ItemType {LIGHT, STAMINA, TEMPERATURE, LIGHTER, NONE};
        public enum ItemEffectDuration { TEMPORARY, PERMANENT, NONE };

        //declare member data
        public ItemType itemType;
        public ItemEffectDuration itemEffectDuration;
        public float effect;
        public Vector2 size;


        //constructor
        public PickUpItem(GameLevel instance, Texture2D texture
            , Vector2 position, ItemType itemType) 
                : base(instance, texture, position, Rectangle.Empty)
        {
            this.itemType = itemType;
            this.effect = effect;
            this.size = size;
            this.itemEffectDuration = itemEffectDuration;
        }

      public static PickUpItem GetNewInstance(GameLevel instance, Vector2 positian)
      {


        
      }

      public void PickUpBy(Character player)
      {
              if (itemType == ItemType.LIGHTER)
              {
                player._pocketLight = PocketLightSource.GetNewInstance(CurrentGameLevel.GameInstance, player);
                player._pocketLight.TurnDisable();
              }
              else if (itemType == ItemType.STAMINA)
                {
                    if (itemEffectDuration == ItemEffectDuration.PERMANENT)
                    {
                        player.Energy = Character.MAXIMUM_ENERGY_LEVEL;
                    }
                    else if (itemEffectDuration == ItemEffectDuration.TEMPORARY)
                    {
                        player.Energy += effect;
                    }
                }

                else if (itemType == ItemType.TEMPERATURE)
                {
                    player.BodyTemperature += effect;
                }

                //temporary fix
                Position = new Vector2(2048, 2048);
        
      }

        //update method
        public void Update()
        {
        }

        //draw the item onto screen
        public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            spriteBatch.Draw(Texture, drawPosition, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
        }
    }
}

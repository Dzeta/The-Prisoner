using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace Cold_Ship
{
    public class PickUpItem : GenericSprite2D
    {
      public static Texture2D LIGHT_TEXTURE;
      public static Texture2D STAMINA_TEXTURE;
      public static Texture2D TEMPERATURE_TEXTURE;
      public static Texture2D LIGHTER_TEXTURE;


      protected Character _owner;
        //item type enum
        public enum ItemEffectDuration { TEMPORARY, PERMANENT, NONE };

      public Vector2 Size;
        //declare member data
        public ItemEffectDuration itemEffectDuration;
        public float effect;


        //constructor
        protected PickUpItem(GameLevel instance, Texture2D texture
            , Vector2 position)
                : base(instance, texture, position, Rectangle.Empty) { }

      protected static PickUpItem GetNewInstance(GameLevel instance, Vector2 position)
      {
        if (LIGHT_TEXTURE == null)
          LIGHT_TEXTURE = instance.Content.Load<Texture2D>("Objects/lighter");
        if (STAMINA_TEXTURE == null)
          STAMINA_TEXTURE = instance.Content.Load<Texture2D>("Objects/lighter");
        if (TEMPERATURE_TEXTURE == null)
          TEMPERATURE_TEXTURE = instance.Content.Load<Texture2D>("Objects/lighter");
        if (LIGHTER_TEXTURE == null)
          LIGHTER_TEXTURE = instance.Content.Load<Texture2D>("Objects/lighter");

        Texture2D _texture = LIGHT_TEXTURE;

        PickUpItem _instance = new PickUpItem(instance, _texture, position);

        return _instance;
      }

      public bool IsPickedUp() { return this._owner != null; }
      public bool IsConsumed() { return this.IsPickedUp(); }

      public virtual void PickUpBy(Character player)
      {
        this._owner = player;
//              if (itemType == ItemType.LIGHTER)
//              {
//                player.PocketLight = PocketLightSource.GetNewInstance(CurrentGameLevel.GameInstance, player);
//                player.PocketLight.TurnDisable();
//              }
//              else if (itemType == ItemType.STAMINA)
//                {
//                    if (itemEffectDuration == ItemEffectDuration.PERMANENT)
//                    {
//                        player.Energy = Character.NORMAL_ENERGY_LEVEL;
//                    }
//                    else if (itemEffectDuration == ItemEffectDuration.TEMPORARY)
//                    {
//                        player.Energy += effect;
//                    }
//                }
//
//                else if (itemType == ItemType.TEMPERATURE)
//                {
//                    player.BodyTemperature += effect;
//                }
//
//                //temporary fix
//                Position = new Vector2(2048, 2048);

      }

        //update method
        public void Update(GameTime gameTime)
        {
        }

        //draw the item onto screen
        public override void Draw(SpriteBatch spriteBatch)
        {
          spriteBatch.Begin();
            spriteBatch.Draw(Texture, this.Position, new Rectangle(0, 0, (int)Size.X, (int)Size.Y), Color.White);
          spriteBatch.End();

          base.Draw(spriteBatch);
        }
    }
}

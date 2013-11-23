using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cold_Ship
{
    public class PocketLightSource : GenericSprite2D
    {
        public static Cold_Ship GameInstance;

        private Character _owner;

        private PocketLightSource(Character owner, Texture2D tex) : base(tex)
        {
            this.position = owner.position;
        }

        public static PocketLightSource GetNewInstance(Cold_Ship instance, Character character)
        {
            if (PocketLightSource.GameInstance == null) GameInstance = instance;
            Texture2D _texture = instance.Content.Load<Texture2D>("Texture\\radius_of_light");

            PocketLightSource _instance = new PocketLightSource(character, _texture);
            return _instance;
        }

        public void Update(GameTime gameTime)
        {
            this.position = this._owner.position;
        }
    }
}

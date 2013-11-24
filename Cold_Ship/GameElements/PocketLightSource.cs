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

        private Vector2 _positionOffset;
        private Character _owner;
        private Texture2D _darkCurtain;

        private PocketLightSource(Character owner, Texture2D tex)
            : base(tex)
        {
            this._owner = owner;
            this._positionOffset = new Vector2(tex.Width / 2, tex.Height / 2);
            this.position = owner.position - this._positionOffset;
        }

        public static PocketLightSource GetNewInstance(Cold_Ship instance, Character character)
        {
            if (PocketLightSource.GameInstance == null) GameInstance = instance;
            Texture2D _texture = instance.Content.Load<Texture2D>("Textures/raidus_of_light_with_alpha");

            PocketLightSource _instance = new PocketLightSource(character, _texture);
            _instance._darkCurtain = _texture;
            //_instance._darkCurtain = new Texture2D(instance.GraphicsDevice, 1, 1);
            //_instance._darkCurtain.SetData(new[] { Color.Blue });
            return _instance;
        }

        public void Update(GameTime gameTime)
        {
            this.position = this._owner.position - this._positionOffset;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();

            RenderTarget2D mask = new RenderTarget2D(GameInstance.graphics.GraphicsDevice,
                GameInstance.Window.ClientBounds.Width, GameInstance.Window.ClientBounds.Height,
                false, SurfaceFormat.Color, DepthFormat.None);
            //GameInstance.graphics.GraphicsDevice.SetRenderTarget(mask);
            //GameInstance.graphics.GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(this._darkCurtain, GameInstance.Window.ClientBounds, Color.White);
            spriteBatch.End();

            //BlendState bs = new BlendState();
            //bs.ColorSourceBlend = Blend.Zero;
            //bs.ColorDestinationBlend = Blend.SourceAlpha;
            //bs.ColorBlendFunction = BlendFunction.Add;

            //bs.AlphaSourceBlend = Blend.Zero;
            //bs.AlphaDestinationBlend = Blend.SourceAlpha;
            //bs.AlphaBlendFunction = BlendFunction.Add;

            //spriteBatch.Begin(SpriteSortMode.FrontToBack, bs);
            //spriteBatch.Draw(this.texture, this.position, Color.White);
            //spriteBatch.End();

            //GameInstance.graphics.GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin();
        }
    }
}

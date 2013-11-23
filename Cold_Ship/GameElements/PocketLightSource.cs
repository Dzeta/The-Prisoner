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
    private Texture2D _darkCurtain;

    private PocketLightSource(Character owner, Texture2D tex)
      : base(tex)
    {
      this._owner = owner;
      this.position = owner.position;
    }

    public static PocketLightSource GetNewInstance(Cold_Ship instance, Character character)
    {
      if (PocketLightSource.GameInstance == null) GameInstance = instance;
      Texture2D _texture = instance.Content.Load<Texture2D>("Textures\\radius_of_light");

      PocketLightSource _instance = new PocketLightSource(character, _texture);
      _instance._darkCurtain = new Texture2D(instance.GraphicsDevice, 1, 1);
      _instance._darkCurtain.SetData(new[] { Color.Blue });
      return _instance;
    }

    public void Update(GameTime gameTime)
    {
      this.position = this._owner.position;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.GraphicsDevice.BlendState = BlendState.AlphaBlend;
      spriteBatch.Draw(this._darkCurtain, GameInstance.Window.ClientBounds, Color.White);
      spriteBatch.Draw(this.texture, this.position, Color.Black);
    }
  }
}

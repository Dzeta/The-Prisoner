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
      Texture2D _texture = instance.Content.Load<Texture2D>("Textures/radius_of_light_with_alpha");

      PocketLightSource _instance = new PocketLightSource(character, _texture);
      _instance._darkCurtain = GameInstance.Content.Load<Texture2D>("Textures/mask");

      return _instance;
    }

    public void Update(GameTime gameTime)
    {
      this.position = this._owner.position - this._positionOffset;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.End();

      spriteBatch.Begin();

      spriteBatch.Draw(this._darkCurtain, GameInstance.Window.ClientBounds, Color.White);
      spriteBatch.Draw(this.texture, this.position, Color.White);
      spriteBatch.End();

      spriteBatch.Begin();
    }
  }
}

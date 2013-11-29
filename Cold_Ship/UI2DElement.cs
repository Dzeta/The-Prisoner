using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Cold_Ship
{
  public abstract class UI2DElement : GenericSprite2D
  {
    protected Cold_Ship GameInstance;
    protected UI2DElement(Texture2D texture, Vector2 position) 
      : base(texture, position) {}


    public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
    {
      base.Draw(spriteBatch, drawPosition);
    }
  }

  public class HealthBar : UI2DElement
  {
    private const float SCALE_FACTOR = 2.5f; // Scale down the hp and energy bar
    private Func<float> _state;

    protected Character _player;
    private Texture2D _textureBar;
    private Texture2D _textureBarFill;

    private Rectangle _barSpriteDestination;
    private Rectangle _barSpriteSource;
    private Rectangle _barFillRectangle;


    private Vector2 _offsetFromPlayer = new Vector2(10, 20);

    private HealthBar(Texture2D texture, Vector2 position) 
      : base(texture, position) { }

    public static HealthBar GetNewInstance(Cold_Ship gameInstance
      , Character player, Func<float> state)
    {
      Texture2D _highlight = gameInstance
        .Content.Load<Texture2D>("Textures/prisoner-energy-bar-highlight");
      Texture2D _bar = gameInstance
        .Content.Load<Texture2D>("Textures/prisoner-energy-bar");
      HealthBar _instance = new HealthBar(_bar, player.Position - (new Vector2(0, 40)));
      _instance._player = player;
      _instance._textureBar = _bar;
      _instance._textureBarFill = _highlight;
      _instance._state = state;
      _instance._barSpriteDestination = 
          new Rectangle((int)_instance._player.Position.X, (int)_instance._player.Position.Y
          , (int)(_bar.Width / SCALE_FACTOR), (int)(_bar.Height / 2 / SCALE_FACTOR));
      _instance._barSpriteSource = 
          new Rectangle(0, (int)(_bar.Height / 2), _bar.Width, _bar.Height / 2);
      _instance.Texture = _instance._textureBar;
      _instance._barFillRectangle = new Rectangle(_instance._barSpriteDestination.X + 3
          , _instance._barSpriteDestination.Y, (int) ((_instance._state()*_bar.Width - (6))/SCALE_FACTOR)
          , (int) (_bar.Height/2/SCALE_FACTOR));

      if (_instance.GameInstance == null) 
        _instance.GameInstance = gameInstance;

      return _instance;
    }

    public void Update(GameTime gameTime)
    {
      this._barSpriteDestination = 
          new Rectangle((int)(this._player.Position.X - this._offsetFromPlayer.X), (int)(this._player.Position.Y - this._offsetFromPlayer.Y)
          , (int)(this._textureBar.Width / SCALE_FACTOR), (int)(this._textureBar.Height / 2 / SCALE_FACTOR));

      this._barSpriteSource = new Rectangle(0, this._textureBar.Height / 2
        , this._textureBar.Width, this._textureBar.Height / 2);

      this._barFillRectangle = new Rectangle(this._barSpriteDestination.X + 3
        , this._barSpriteDestination.Y, (int) ((this._state()*this._textureBar.Width - (6))/SCALE_FACTOR)
        , (int) (this._textureBar.Height/2/SCALE_FACTOR));
    }

    public override void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
      // Draw the background
      spriteBatch.Draw(this.Texture, this._barSpriteDestination
        , this._barSpriteSource, Color.White);

      float _healthPercent = this._state() * 100;
      spriteBatch.DrawString(GameInstance.MonoMedium
        , _healthPercent.ToString().Substring(0, (int)MathHelper.Min(_healthPercent.ToString().Length, 4)) + " %"
        , new Vector2(this._barSpriteDestination.X - 35, this._barSpriteDestination.Y - 4), Color.White
        , 0, Vector2.Zero, 0.5f, SpriteEffects.None, 1);

      spriteBatch.Draw(this._textureBarFill, this._barFillRectangle, null, Color.White);
    }
  }
}

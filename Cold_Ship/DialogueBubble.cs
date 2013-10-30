using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Cold_Ship
{
  public class DialogueBubble : GenericSprite2D
  {
    public const int LINE_FEED_LENGTH = 20; // Empirical number
    // Sense of typing the character out on the screen
    public const int CHAR_FEED_INTERVAL = 200; // Empirical number

        
    // AUTIO ENGINES
    public static AudioEngine engine { get; set; }
    public static SoundBank soundBank { get; set; }
    public static WaveBank waveBank { get; set; }
        
    private static Texture2D _bubbleTextureMono;
    private static Texture2D _bubbleTextureHasMore;
    private static SpriteFont _mono8;

    // All number below are determined empirically
    private static Vector2 _textWhenBubbleIsTop = new Vector2(12, 12);
    private static Vector2 _textWhenBubbleIsBottom = new Vector2(12, 32);
    private static Vector2 _bubbleLeftTop = new Vector2(-150, -250);
    private static Vector2 _bubbleRightTop = new Vector2(-20, -250);
    private static Vector2 _bubbleLeftBottom = new Vector2(-150, -170);
    private static Vector2 _bubbleRightBottom = new Vector2(-20, -170);

    private static Rectangle _bubbleSpriteRightBottom = new Rectangle(186, 0, 186, 60);
    private static Rectangle _bubbleSpriteRightTop = new Rectangle(186, 60, 186, 60);
    private static Rectangle _bubbleSpriteLeftBottom = new Rectangle(0, 0, 186, 60);
    private static Rectangle _bubbleSpriteLeftTop = new Rectangle(0, 60, 186, 60);
    private Rectangle _sourceRectangle;
    private Vector2 _textStartPosition;

    private Texture2D _bubbleTexture;

    public enum XPositionRelativeToCenter { Left, Right }
    public enum YPositionRelativeToCenter { Top, Bottom }
    private XPositionRelativeToCenter _xPosition;
    private YPositionRelativeToCenter _yPosition;

    private int _totalMessageRow;
    private Vector2 _rowOneStartPosition;
    private Vector2 _rowTwoStartPosition;

    private int _currentRow = 0; // Always start at 0
    private int _currentRowCharPosition; // Always start at 0
    private List<string> _scroller;
    private int _charDisplayTimer;

    private bool _isPlaying;
    private Game1 _gameLevel;

    // The collision box of the dialogue
    private static Rectangle _boundBox = new Rectangle(0, 0, 186, 60);

    private DialogueBubble(Game1 game, Texture2D texture, Vector2 speakerPosition, List<string> scroller, XPositionRelativeToCenter xPos, YPositionRelativeToCenter yPos) : base(texture, speakerPosition, _boundBox)
    {
      if (_bubbleTextureMono == null)
        _bubbleTextureMono = texture;

      this._scroller = scroller;
      this._xPosition = xPos;
      this._yPosition = yPos;
      // Start texture for bubble
      this._bubbleTexture = (scroller.Count > 1) ? _bubbleTextureHasMore : _bubbleTextureMono;
      this._gameLevel = game;

      if (xPos == XPositionRelativeToCenter.Left)
      {
        if (yPos == YPositionRelativeToCenter.Top)
        {
          this.position = Vector2.Add(speakerPosition, _bubbleRightBottom);
          this._sourceRectangle = _bubbleSpriteRightBottom;
          this._textStartPosition = _textWhenBubbleIsBottom;
        }
        else
        {
          this.position = Vector2.Add(speakerPosition, _bubbleRightTop);
          this._sourceRectangle = _bubbleSpriteRightTop;
          this._textStartPosition = _textWhenBubbleIsTop;
        }
      }
      else // xPos is at Right
      {
        if (yPos == YPositionRelativeToCenter.Top)
        {
          this.position = Vector2.Add(speakerPosition, _bubbleLeftBottom);
          this._sourceRectangle = _bubbleSpriteLeftBottom;
          this._textStartPosition = _textWhenBubbleIsBottom;
        }
        else
        {
          this.position = Vector2.Add(speakerPosition, _bubbleLeftTop);
          this._sourceRectangle = _bubbleSpriteLeftTop;
          this._textStartPosition = _textWhenBubbleIsTop;
        }
      }
    }

    public static DialogueBubble GetNewInstance(Game1 level, Vector2 speakerPosition, Rectangle windowBound, string msg)
    {
      if (_bubbleTextureMono == null)
        _bubbleTextureMono = level.Content.Load<Texture2D>("speech");
      if (_bubbleTextureHasMore == null)
        _bubbleTextureHasMore = level.Content.Load<Texture2D>("speech-has-more");
      if (_mono8 == null)
        _mono8 = level.Content.Load<SpriteFont>("manaspace0");
      if (engine == null)
      {
        engine = new AudioEngine("Content\\SOUND_SPEECH_ENGINE.xgs");
        soundBank = new SoundBank(engine, "Content\\SOUND_SPEECH_SOUNDBANK.xsb");
        waveBank = new WaveBank(engine, "Content\\SOUND_SPEECH_WAVEBANK.xwb");
      }

      // We gonna place the bubble based on the player's position left or right of the screen middle
      XPositionRelativeToCenter xPos = speakerPosition.X - windowBound.Center.X >= 0
        ? XPositionRelativeToCenter.Right
        : XPositionRelativeToCenter.Left;
      YPositionRelativeToCenter yPos = speakerPosition.Y - windowBound.Center.Y >= 0
        ? YPositionRelativeToCenter.Bottom
        : YPositionRelativeToCenter.Top;

      return new DialogueBubble(level, _bubbleTextureMono, speakerPosition, DialogueBubble.ComputeScroller(msg), xPos, yPos);
    }

    public static DialogueBubble GetNewInstance(Game1 level, Vector2 speakerPosition, XPositionRelativeToCenter xPos,
      YPositionRelativeToCenter yPos, string msg)
    {
      if (_bubbleTextureMono == null)
        _bubbleTextureMono = level.Content.Load<Texture2D>("speech");
      if (_bubbleTextureHasMore == null)
        _bubbleTextureHasMore = level.Content.Load<Texture2D>("speech-has-more");
      if (_mono8 == null)
        _mono8 = level.Content.Load<SpriteFont>("manaspace0");
      if (engine == null)
      {
        engine = new AudioEngine("SOUND_SPEECH_ENGINE.xgs");
        soundBank = new SoundBank(engine, "SOUND_SPEECH_SOUNDBANK.xsb");
        waveBank = new WaveBank(engine, "SOUND_SPEECH_WAVEBANK.xwb");
      }

      return new DialogueBubble(level, _bubbleTextureMono, speakerPosition, DialogueBubble.ComputeScroller(msg), xPos, yPos);
    }

    public bool IsPlaying()
    {
      return this._isPlaying;
    }

    public void Play()
    {
      this._isPlaying = true;
      this._gameLevel.ActivateState(Game1.GameState.Frozen);
    }

    public void Pause()
    {
      this._isPlaying = false;
    }

    public void Update(GameTime gameTime)
    {
      _charDisplayTimer += gameTime.ElapsedGameTime.Milliseconds;
      if (_charDisplayTimer >= CHAR_FEED_INTERVAL)
      {
        if (_currentRow < _scroller.Count && _currentRowCharPosition < _scroller[_currentRow].Length)
        {
          DialogueBubble.soundBank.PlayCue("sound-next-char");
          _currentRowCharPosition++;
          _charDisplayTimer = 0;
        }
        else if (_currentRow < _scroller.Count - 1)
        {
          DialogueBubble.soundBank.PlayCue("sound-next-chat");
          _currentRow++;
          _currentRowCharPosition = 0;
        }
        else
        {
          _isPlaying = false;
        }
      } 

    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_bubbleTexture, this.position, this._sourceRectangle, Color.White);
      spriteBatch.DrawString(_mono8, this._scroller[this._currentRow].Substring(0, _currentRowCharPosition), Vector2.Add(this.position, _textStartPosition), Color.White);
      
    }

    private static List<string> ComputeScroller(string message)
    {
      List<string> localScroller = new List<string>();
      string[] words = message.Split(' ');
      for (int wordIndex = 0; wordIndex < words.Length; )
      {
        string lineFeed = "";
        do
        {
          lineFeed += " " + words[wordIndex];
          wordIndex++;
        } while (lineFeed.Length < LINE_FEED_LENGTH && wordIndex < words.Length);

        localScroller.Add(lineFeed);
      }

      return localScroller;
    }
  }
}

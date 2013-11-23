using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cold_Ship
{

  public class InvisibleBox
  {
    private Rectangle _hitBox;
    private Vector2 _position;

    public InvisibleBox(Rectangle hitBox, Vector2 position)
    {
      this._hitBox = hitBox;
      this._position = position;
    }

    public virtual Rectangle GetHitBox()
    {
      return new Rectangle((int)this._position.X + this._hitBox.X, (int)this._position.Y + this._hitBox.Y, this._hitBox.Width, this._hitBox.Height);
    }
  }

  public class InvisibleChatTriggerBox : InvisibleBox
  {
    private int _timeOutTime = 1000; // Timer between reappearance of the invisible box in a persisted
    private int _timeOutTimer = 0;
    private string _msg;
    private static Rectangle _hitBox = new Rectangle(0, 0, 32, 32);
    private bool _isPersisted;
    private bool _isConsumed;

    private IWatchfulConditional _watchee;

    private InvisibleChatTriggerBox(Vector2 position, string msg, bool isPersistant)
      : base(_hitBox, position)
    {
      this._msg = msg;
      this._isPersisted = isPersistant;
      this._isConsumed = false;
      // this._timeOutTime = this._msg.Length * DialogueBubble.DEFAULT_PLAY_THROUGH_SPEED;
    }

    public static InvisibleChatTriggerBox GetNewInstance(Vector2 pos, string m, bool p)
    {
      return new InvisibleChatTriggerBox(pos, m, p);
    }

    public static InvisibleChatTriggerBox GetNewInstance(Vector2 pos,
      string m, IWatchfulConditional w)
    {
      InvisibleChatTriggerBox _instance = new InvisibleChatTriggerBox(pos, m, false);
      _instance._watchee = w;
      return _instance;
    }

    public void SetConsumed(bool isConsumed) { this._isConsumed = true; }
    public bool IsPersisted() { return this._isPersisted; }
    public string GetMessage() { return this._msg; }
    public bool IsWatchful() { return this._watchee != null; }

    public bool IsConsumed()
    {
      return (this._isConsumed || this.IsWatchful() && this._watchee.GetCondition());
    }

    public void Update(GameTime gameTime)
    {
      if (this.IsConsumed() && !this.IsPersisted()) return;

      _timeOutTimer += gameTime.ElapsedGameTime.Milliseconds;
      if (_timeOutTimer >= _timeOutTime)
      {
        if (this._isPersisted)
        {
          this._isConsumed = false;
          _timeOutTimer = 0;
        }
      }
    }

    public void InteractWith(Character player, Cold_Ship gameLevel)
    {
      this.InteractWith(player.position, gameLevel);
    }

    public void InteractWith(Vector2 position, Cold_Ship gameLevel)
    {
      if (this.IsConsumed()) return;

      DialogueBubble dialogue = DialogueBubble.GetNewInstance(gameLevel, position,
        new Rectangle(0, 0, (int)gameLevel.screenSize.X, (int)gameLevel.screenSize.Y), this._msg);
      this._isConsumed = true;
      dialogue.Play();
      gameLevel.DialogueQueue.Add(dialogue);
    }
  }
}

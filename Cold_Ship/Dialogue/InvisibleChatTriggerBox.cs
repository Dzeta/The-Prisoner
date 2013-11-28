﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    private int _timeOutInterval = 1000; // Timer between reappearance of the invisible box in a persisted
    private int _timeOutTimer = 0;
    private string _msg;
    private static Rectangle _hitBox = new Rectangle(0, 0, 32, 32);
    private bool _isPersisted;
    private bool _isConsumed;
    private Func<bool> _condition; 

    private IWatchfulConditional _watchee;

    private InvisibleChatTriggerBox(Vector2 position, string msg)
      : base(_hitBox, position)
    {
      this._msg = msg;
      this._isConsumed = false;
//      this._timeOutInterval = this._msg.Length * DialogueBubble.DEFAULT_PLAY_THROUGH_SPEED;
    }

    public static InvisibleChatTriggerBox GetNewInstance(Vector2 pos, string m)
    {
      return InvisibleChatTriggerBox.GetNewInstance(pos, m, false);
    }

    public static InvisibleChatTriggerBox GetNewInstance(Vector2 pos, string m, bool p)
    {
      InvisibleChatTriggerBox _instance = new InvisibleChatTriggerBox(pos, m);
      _instance._isPersisted = p;
      return _instance;
    }

    public static InvisibleChatTriggerBox GetNewInstance(Vector2 pos, string m, Func<bool> cond)
    {
      InvisibleChatTriggerBox _instance = new InvisibleChatTriggerBox(pos, m);
      _instance._condition = cond;
      return _instance;
    }

    public static InvisibleChatTriggerBox GetNewInstance(Vector2 pos, string m, IWatchfulConditional w)
    {
      InvisibleChatTriggerBox _instance = new InvisibleChatTriggerBox(pos, m);
      _instance._watchee = w;
      return _instance;
    }

    public string GetMessage() { return this._msg; }
    public void SetConsumed() { this._isConsumed = true; }
    public bool IsPersisted() { return this._isPersisted; }
    public bool IsConsumed() { return this._isConsumed; }
    public bool IsWatchful() { return this._watchee != null; }
    public bool IsConditional() { return this._condition != null; }

    private bool _Respawn()
    {
      if (this._isPersisted)
        return true;
      else
      {
        if (this.IsWatchful())
          return !this._watchee.GetCondition();
        else if (this.IsConditional())
          return !this._condition();
        else
          return !this._isConsumed;
      }
    }

    public void Update(GameTime gameTime)
    {
      if (this.IsConditional() && this._condition())
        this._isConsumed = true;

      if (this.IsWatchful() && this._watchee.GetCondition())
        this._isConsumed = true;

      if (this.IsConsumed() && this._Respawn())
      {
        _timeOutTimer += gameTime.ElapsedGameTime.Milliseconds;

        if (_timeOutTimer >= _timeOutInterval)
        {
          this._isConsumed = false;
          _timeOutTimer = 0;
        }
      }
    }

    public void InteractWith(Character player, GameLevel gameLevel)
    {
      this.InteractWith(player.Position, gameLevel);
    }

    public void InteractWith(Vector2 position, GameLevel gameLevel)
    {
      DialogueBubble dialogue = DialogueBubble.GetNewInstance(gameLevel, position,
        new Rectangle(0, 0, (int)gameLevel.GetScreenSize().X
            , (int)gameLevel.GetScreenSize().Y), this._msg);
      this._isConsumed = true;
      dialogue.Play();
      gameLevel.LevelDialogueBubbles.Add(dialogue);
    }
  }
}

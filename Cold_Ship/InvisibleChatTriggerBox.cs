using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
    private string _msg;
    private static Rectangle _hitBox = new Rectangle(0, 0, 32, 32);
    private bool _isPersisted;
    private bool _isConsumed;

    public InvisibleChatTriggerBox(Vector2 position, string msg, bool isPersistant) : base(_hitBox, position)
    {
      this._msg = msg;
      this._isPersisted = isPersistant;
      this._isConsumed = false;
    }

    public string GetMessage() { return this._msg; }

    public void InteractWith(Scene2DNode player, Game1 gameLevel)
    {
      if (!this._isConsumed || !this._isPersisted)
      {
        DialogueBubble dialogue = DialogueBubble.GetNewInstance(gameLevel, player.position,
          new Rectangle(0, 0, (int) gameLevel.screenSize.X, (int) gameLevel.screenSize.Y), this._msg); 
        dialogue.Play();
        gameLevel.DialogueQueue.Add(dialogue);
        if (!this._isPersisted) this._isConsumed = true;
      }
    }
    
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace Cold_Ship
{
  public class Level_Holding_Cell : GameLevel
  {
    float ground;
    GenericSprite2D backgroundNode;

    PickUpItem lighter;

    public Level_Holding_Cell(Cold_Ship gameInstance)
      : base(gameInstance) { }

    //load content
    public void LoadContent()
    {
      LevelBackgroundTexture = Content.Load<Texture2D>("Backgrounds/holdingcell_final");
      this.WorldBoundingRectangle = new Rectangle(0, 0
          , LevelBackgroundTexture.Width, LevelBackgroundTexture.Height);

      Portal _forwardPortal = new Portal(
          this.GetAbsoluteWorldSize() - new Vector2(251, 288)
          , new Vector2(51, 72), Portal.PortalType.FORWARD
          , GameInstance.Content);

      LeveLPortals.Add(_forwardPortal);
      this.PlayerNode.Position = _forwardPortal.Position - new Vector2(200, 200);

      // Load the text with respect to the current player's Position

      Vector2 _trigger1PositionOffset = new Vector2(-200, 30);
      Vector2 _trigger2PositionOffset = new Vector2(-10, 30);
        LevelChatTriggerBoxes.Add(
          InvisibleChatTriggerBox.GetNewInstance(
          _forwardPortal.Position + _trigger1PositionOffset
          , "Good, you're awake."));
        LevelChatTriggerBoxes.Add(
          InvisibleChatTriggerBox.GetNewInstance(
          _forwardPortal.Position + _trigger1PositionOffset
          , "There isn't much time."));
        LevelChatTriggerBoxes.Add(
          InvisibleChatTriggerBox.GetNewInstance(
          _forwardPortal.Position + _trigger1PositionOffset
          , "The ship is going down."));
        LevelChatTriggerBoxes.Add
          (InvisibleChatTriggerBox.GetNewInstance(
          _forwardPortal.Position + _trigger1PositionOffset
          , "You need to fix it up if you want to live."));
        LevelChatTriggerBoxes.Add(
          InvisibleChatTriggerBox.GetNewInstance(
          _forwardPortal.Position + _trigger1PositionOffset
          , "You do want to live, don't you?"));
        LevelChatTriggerBoxes.Add(
          InvisibleChatTriggerBox.GetNewInstance(
          _forwardPortal.Position + _trigger1PositionOffset
          , "I hear space death isn't very pleasant, though."));
      LevelChatTriggerBoxes.Add( 
        InvisibleChatTriggerBox.GetNewInstance(
        _forwardPortal.Position + _trigger1PositionOffset
        , "Up to you.")); 
      LevelChatTriggerBoxes.Add( 
        InvisibleChatTriggerBox.GetNewInstance(
        _forwardPortal.Position + _trigger2PositionOffset
        , "You definitely should pick that lighter up before you get out of here."
        , this.PlayerNode.HasLighter));

    Texture2D lighterTexture = this.Content.Load<Texture2D>("Objects/lighter");
       PickUpItem lighter = new PickUpItem(this, 
           lighterTexture, _forwardPortal.Position + new Vector2(-288, 50)
           , new Vector2(lighterTexture.Width, lighterTexture.Height)
           , PickUpItem.ItemType.LIGHTER, 100, PickUpItem.ItemEffectDuration.NONE, GameInstance);
      LevelPickUpItems.Add(lighter);
    }

    //unload contents
    public void Unload() { }

    //update function
    public override void Update(GameTime gameTime)
    {
      // Update Dialogues
      foreach (InvisibleChatTriggerBox chatTrigger in LevelChatTriggerBoxes)
      {
        chatTrigger.Update(gameTime);
        if (!chatTrigger.IsConsumed() 
            && chatTrigger.GetHitBox().Intersects(this.PlayerNode.getPlayerHitBox()))
          chatTrigger.InteractWith(new Vector2(400, 400), GameInstance);
      }

      this.PlayerNode.Update(gameTime);

      if (this.PlayerNode.Position.X < 250)
      {
        this.PlayerNode.Position.X = 250;
      }
      else if (this.PlayerNode.Position.X 
          > this.GetAbsoluteWorldSize().X - 250 - 31)
      {
        this.PlayerNode.Position.X = this.GetAbsoluteWorldSize().Y - 250 - 31;
      }
    }

    //draw funtion
    public void Draw(int framesPerSecond)
    {
      SpriteBatch.Begin();

      if (Cold_Ship.DEBUG_MODE)
        foreach (InvisibleChatTriggerBox box in LevelChatTriggerBoxes)
          SpriteBatch.Draw(this.GameInstance.DebugTexture, box.GetHitBox(), Color.Pink);

      //draw the desired nodes onto screen through the camera
      foreach (GenericSprite2D element in LevelStaticWorldObjects)
          this.Camera.DrawNode(element);

      // Draw all invisible chat trigger
      if (Cold_Ship.DEBUG_MODE)
        foreach (InvisibleChatTriggerBox invisibleTrigger in LevelChatTriggerBoxes)
          if (!invisibleTrigger.IsConsumed())
            SpriteBatch.Draw(GameInstance.DebugTexture, invisibleTrigger.GetHitBox(), Color.White);

      SpriteBatch.End();
    }
  }
}

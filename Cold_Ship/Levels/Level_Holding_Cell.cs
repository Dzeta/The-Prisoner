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
    GenericSprite2D backgroundNode;
    public Level_Holding_Cell(Cold_Ship gameInstance, GameLevel prevLevel, GameLevel nextLevel)
      : base(gameInstance, prevLevel, nextLevel) { }

    public override void LoadContent()
    {
      LevelBackgroundTexture = Content.Load<Texture2D>("Backgrounds/holdingcell_final");
      this.WorldBoundingRectangle = new Rectangle(0, 0
          , LevelBackgroundTexture.Width, LevelBackgroundTexture.Height);

      Vector2 _portalPosition = this.GetAbsoluteWorldSize() - new Vector2(251, 273);
      this.ExitPortal = Portal.GetNewInstance(this, _portalPosition, true);
      LevelPortals.Add(this.ExitPortal);

//        LevelChatTriggerBoxes.Add(
//          InvisibleChatTriggerBox.GetNewInstance(
//          _forwardPortal.Position + _trigger1PositionOffset
//          , "Good, you're awake."));
//        LevelChatTriggerBoxes.Add(
//          InvisibleChatTriggerBox.GetNewInstance(
//          _forwardPortal.Position + _trigger1PositionOffset
//          , "There isn't much time."));
//        LevelChatTriggerBoxes.Add(
//          InvisibleChatTriggerBox.GetNewInstance(
//          _forwardPortal.Position + _trigger1PositionOffset
//          , "The ship is going down."));
//        LevelChatTriggerBoxes.Add
//          (InvisibleChatTriggerBox.GetNewInstance(
//          _forwardPortal.Position + _trigger1PositionOffset
//          , "You need to fix it up if you want to live."));
//        LevelChatTriggerBoxes.Add(
//          InvisibleChatTriggerBox.GetNewInstance(
//          _forwardPortal.Position + _trigger1PositionOffset
//          , "You do want to live, don't you?"));
//        LevelChatTriggerBoxes.Add(
//          InvisibleChatTriggerBox.GetNewInstance(
//          _forwardPortal.Position + _trigger1PositionOffset
//          , "I hear space death isn't very pleasant, though."));
//      LevelChatTriggerBoxes.Add( 
//        InvisibleChatTriggerBox.GetNewInstance(
//        _forwardPortal.Position + _trigger1PositionOffset
//        , "Up to you.")); 
//      LevelChatTriggerBoxes.Add( 
//        InvisibleChatTriggerBox.GetNewInstance(
//        _forwardPortal.Position + _trigger2PositionOffset
//        , "You definitely should pick that lighter up before you get out of here."
//        , this.PlayerNode.HasLighter));

      LevelPickUpItems.Add(PocketLightSource.GetNewInstance(this, _portalPosition + new Vector2(-288, 50)));
    }

    public override void SpawnPlayer(Character player)
    {
      base.SpawnPlayer(player);

      player.Position = new Vector2(300, 337);
    }

    //unload contents
    public void Unload() { }

    //update function
    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

//      if (this.PlayerNode.Position.X < 250)Vjjjj
//      {
//        this.PlayerNode.Position.X = 250;
//      }
//      else if (this.PlayerNode.Position.X 
//          > this.GetAbsoluteWorldSize().X - 250 - 31)
//      {
//        this.PlayerNode.Position.X = this.GetAbsoluteWorldSize().Y - 250 - 31;
//      }
    }


    //draw funtion
    public override void Draw()
    {
      SpriteBatch.Begin();

      if (Cold_Ship.DEBUG_MODE)
        foreach (InvisibleChatTriggerBox box in LevelChatTriggerBoxes)
          SpriteBatch.Draw(this.GameInstance.DebugTexture, box.GetHitBox(), Color.Pink);

      //draw the desired nodes onto screen through the camera
//      foreach (GenericSprite2D element in LevelStaticWorldObjects)
//          this.Camera.DrawNode(element);

      // Draw all invisible chat trigger
      if (Cold_Ship.DEBUG_MODE)
        foreach (InvisibleChatTriggerBox invisibleTrigger in LevelChatTriggerBoxes)
          if (!invisibleTrigger.IsConsumed())
            SpriteBatch.Draw(GameInstance.DebugTexture, invisibleTrigger.GetHitBox(), Color.White);

      SpriteBatch.End();

      base.Draw();
    }
  }
}

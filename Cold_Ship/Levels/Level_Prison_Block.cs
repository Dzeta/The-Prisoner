using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cold_Ship.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace Cold_Ship
{
  public class Level_Prison_Block : GameLevel
  {
    //declare member variables
    public SpriteBatch spriteBatch;
    Interactable lightSwitch, generator;
    PickUpItem staminaBooster;

    //declare constructor
    public Level_Prison_Block(Cold_Ship gameInstance, GameLevel prevLevel, GameLevel nextLevel) : base(gameInstance, prevLevel, nextLevel) { }

    //load content
    public override void LoadContent()
    {
      //load the needed textures
      LevelBackgroundTexture = GameInstance.Content.Load<Texture2D>("Backgrounds/prisonblock_final");
      LevelBackgroundNode = new GenericSprite2D(this, LevelBackgroundTexture, Vector2.Zero);
      this.WorldBoundingRectangle = new Rectangle(0, 0
          , LevelBackgroundTexture.Width, LevelBackgroundTexture.Height);

//      //initialize the platforms and add them to the list
//      platforms.Add(new Platform(platformTexture, new Vector2(790, 20), new Vector2(100, worldSize.Y - 280)));
//      platforms.Add(new Platform(platformTexture, new Vector2(375, 20), new Vector2(925, worldSize.Y - 280)));
//      platforms.Add(new Platform(platformTexture, new Vector2(615, 20), new Vector2(1337, worldSize.Y - 280)));
//      platforms.Add(new Platform(platformTexture, new Vector2(375, 20), new Vector2(100, worldSize.Y - 510)));
//      platforms.Add(new Platform(platformTexture, new Vector2(1370, 20), new Vector2(514, worldSize.Y - 510)));
//      platforms.Add(new Platform(platformTexture, new Vector2(30, 20), new Vector2(1920, worldSize.Y - 510)));
//      platforms.Add(new Platform(platformTexture, new Vector2(33, 20), new Vector2(100, worldSize.Y - 744)));
//      platforms.Add(new Platform(platformTexture, new Vector2(727, 20), new Vector2(167, worldSize.Y - 744)));
//      platforms.Add(new Platform(platformTexture, new Vector2(773, 20), new Vector2(933, worldSize.Y - 744)));
//      platforms.Add(new Platform(platformTexture, new Vector2(205, 20), new Vector2(1742, worldSize.Y - 744)));
      //walls
      //platforms.Add(new Platform(platformTexture, new Vector2(130, 270), new Vector2(963, worldSize.Y - 270)));
      //platforms.Add(new Platform(platformTexture, new Vector2(130, 230), new Vector2(1150, worldSize.Y - 508)));

      LevelLadders.Add(Ladder.GetNewInstance(this, new Vector2(890, this.GetAbsoluteWorldSize().Y - 284)));
      LevelLadders.Add(Ladder.GetNewInstance(this, new Vector2(1301, this.GetAbsoluteWorldSize().Y - 284)));
      LevelLadders.Add(Ladder.GetNewInstance(this, new Vector2(478, this.GetAbsoluteWorldSize().Y - 514)));
      LevelLadders.Add(Ladder.GetNewInstance(this, new Vector2(1887, this.GetAbsoluteWorldSize().Y - 514)));
      LevelLadders.Add(Ladder.GetNewInstance(this, new Vector2(134, this.GetAbsoluteWorldSize().Y - 749)));
      LevelLadders.Add(Ladder.GetNewInstance(this, new Vector2(898, this.GetAbsoluteWorldSize().Y - 749)));
      LevelLadders.Add(Ladder.GetNewInstance(this, new Vector2(1707, this.GetAbsoluteWorldSize().Y - 749)));

      Vector2 _entrancePosition = new Vector2(80, this.GetAbsoluteWorldSize().Y - 122);

      this.EntrancePortal = Portal.GetNewInstance(this, PreviousGameLevel,  _entrancePosition, true);
      this.ExitPortal = Portal.GetNewInstance(this, NextGameLevel, this.GetAbsoluteWorldSize() + new Vector2(-32 - 75, -72 - 50), true);

      PreviousGameLevel.ExitPortal.GameLevelPortal = this.EntrancePortal;
      this.EntrancePortal.GameLevelPortal = PreviousGameLevel.ExitPortal;

      this.LevelPortals.Add(EntrancePortal);
      this.LevelPortals.Add(ExitPortal);

//      staminaBooster = new PickUpItem(platformTexture
//          , new Vector2(280, worldSize.Y - 772), new Vector2(28, 28)
//          , PickUpItem.ItemType.STAMINA, 100, PickUpItem.ItemEffectDuration.TEMPORARY, GameInstance);
//      lightSwitch = new Interactable(this, platformTexture
//          , new Vector2(1643, worldSize.Y - 359), new Vector2(31, 43)
//          , Interactable.Type_Of_Interactable.LIGHT_SWITCH);
//      generator = new Interactable(this, GameInstance.Content.Load<Texture2D>("Objects\\generator_off")
//          , new Vector2(1807, worldSize.Y - 809), new Vector2(104, 65)
//          , Interactable.Type_Of_Interactable.GENERATOR
//          , GameInstance.Content.Load<Texture2D>("Objects\\generator_on"));
//
    }

    //unload contents
    public void Unload()
    {
//      platforms.Clear();
//      portals.Clear();
//      ladders.Clear();
//      worldObjects.Clear();
//
//      worldObjects = new List<GenericSprite2D>();
    }

    public void Update(GameTime gameTime)
    {
      base.Update(gameTime);
//      playerNode.Update(gameTime);
//
//      //Check the player's collision with the world boundaries
//      if (playerNode.Position.X < 100 || playerNode.Position.X + playerNode.playerSpriteSize.X > worldSize.X - 100)
//      {
//        playerNode.Position.X = prevPosition.X;
//      }
//
//      lightSwitch.Update(playerNode, ref generatorOn, ref filterOn, shadowFilter, ref forwardDoor.canOpen);
//      generator.Update(playerNode, ref generatorOn, ref filterOn, shadowFilter, ref forwardDoor.canOpen);
//
//      staminaBooster.Update(playerNode);
//
//      //update the camera based on the player and world size
    }
  }
}

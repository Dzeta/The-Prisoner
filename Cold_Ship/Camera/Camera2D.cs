using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace Cold_Ship
{
  public class Camera2D
  {
    public static Cold_Ship GameInstance;
    public Character PlayerFocus;
    public GenericSprite2D ObjectFocus;
    public Vector2 ScreenSize;
    public Vector2 CameraPosition;

    //declare constructor
    public Camera2D(Cold_Ship gameInstance)
    {
      if (GameInstance == null) GameInstance = gameInstance;
      CameraPosition = new Vector2(0, 0);
      PlayerFocus = GameInstance.Player;
      ScreenSize = new Vector2(
          gameInstance.Window.ClientBounds.X
          , gameInstance.Window.ClientBounds.Y);

    }

    public void Update(GameTime gameTime)
    {
      this.TranslateWithSprite(this.PlayerFocus, this.ScreenSize);
      this.CapCameraPosition(this.PlayerFocus.CurrentGameLevel.GetAbsoluteWorldSize()
          , this.ScreenSize);
    }

    //transform the node's coordinates with respect to the camera
    public Vector2 ApplyTransformations(Vector2 nodePosition)
    {
      Vector2 finalPosition = nodePosition - CameraPosition;
      //you can apply transformation here
      //.....................
      //..................
      return finalPosition;
    }

    //move the camera
    public void Translate(Vector2 moveVector)
    {
      CameraPosition += moveVector;
    }

    //move the camera with respect to a sprite
    public void TranslateWithSprite(Sprite2D node, Vector2 screenSize)
    {
      //horizontal transformations
      if (ApplyTransformations(node.Position).X > screenSize.X / 3 * 1.5f)
      {
        CameraPosition.X = node.Position.X - (screenSize.X / 3 * 1.5f);
      }
      else if (ApplyTransformations(node.Position).X < screenSize.X / 3 * 1.2f)
      {
        CameraPosition.X = node.Position.X - (screenSize.X / 3 * 1.2f);
      }
      //vertical transformations
      if (ApplyTransformations(node.Position).Y < screenSize.Y / 3 * 1.2f)
      {
        CameraPosition.Y = node.Position.Y - (screenSize.Y / 3 * 1.2f);
      }
      else if (ApplyTransformations(node.Position).Y > screenSize.Y / 3 * 1.5f)
      {
        CameraPosition.Y = node.Position.Y - (screenSize.Y / 3 * 1.5f);
      }
    }

    //cap the camera Position
    public void CapCameraPosition(Vector2 worldSize, Vector2 screenSize)
    {
      if (CameraPosition.X < 0)
        CameraPosition.X = 0;
      if (CameraPosition.X + screenSize.X > worldSize.X)
        CameraPosition.X = worldSize.X - screenSize.X;
      if (CameraPosition.Y + screenSize.Y > worldSize.Y)
        CameraPosition.Y = worldSize.Y - screenSize.Y;
      if (CameraPosition.Y < 0)
        CameraPosition.Y = 0;
    }

    //draw the node on screen with respect to the camera
    public void DrawNode(Sprite2D node)
    {
      Vector2 drawPosition = ApplyTransformations(node.Position);
      node.Draw(GameInstance.SpriteBatch, drawPosition);
    }
  }
}

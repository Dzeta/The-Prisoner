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
        //declare member variables
        public SpriteBatch spriteBatch;
        //top left corner of the camera
        public Vector2 cameraPosition;
        
        //declare constructor
        public Camera2D(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            cameraPosition = new Vector2(0, 0);
        }

        //transform the node's coordinates with respect to the camera
        public Vector2 ApplyTransformations(Vector2 nodePosition)
        {
            Vector2 finalPosition = nodePosition - cameraPosition;
            //you can apply transformation here
            //.....................
            //..................
            return finalPosition;
        }

        //move the camera
        public void Translate(Vector2 moveVector)
        {
            cameraPosition += moveVector;
        }

        //move the camera with respect to a sprite
        public void TranslateWithSprite(Scene2DNode node, Vector2 screenSize)
        {
            //horizontal transformations
            if (ApplyTransformations(node.position).X > screenSize.X / 3 * 1.5f)
            {
                cameraPosition.X = node.position.X - (screenSize.X / 3 * 1.5f);
            }
            else if (ApplyTransformations(node.position).X < screenSize.X / 3 * 1.2f)
            {
                cameraPosition.X = node.position.X - (screenSize.X / 3 * 1.2f);
            }
            //vertical transformations
            if (ApplyTransformations(node.position).Y < screenSize.Y / 3 * 1.2f)
            {
                cameraPosition.Y = node.position.Y - (screenSize.Y / 3 * 1.2f);
            }
            else if (ApplyTransformations(node.position).Y > screenSize.Y / 3 * 1.5f)
            {
                cameraPosition.Y = node.position.Y - (screenSize.Y / 3 * 1.5f);
            }
        }

        //cap the camera position
        public void CapCameraPosition(Vector2 worldSize, Vector2 screenSize)
        {
            if (cameraPosition.X < 0)
                cameraPosition.X = 0;
            if (cameraPosition.X + screenSize.X > worldSize.X)
                cameraPosition.X = worldSize.X - screenSize.X;
            if (cameraPosition.Y + screenSize.Y > worldSize.Y)
                cameraPosition.Y = worldSize.Y - screenSize.Y;
            if (cameraPosition.Y < 0)
                cameraPosition.Y = 0;
        }

        //draw the node on screen with respect to the camera
        public void DrawNode(Scene2DNode node)
        {
            Vector2 drawPosition = ApplyTransformations(node.position);
            node.Draw(spriteBatch, drawPosition);
        }


        //draw the player sprite onto screen with respect to the camera
        public void DrawPlayerNode(Scene2DNode node)
        {
            Vector2 drawPosition = ApplyTransformations(node.position);
            node.DrawPlayer(spriteBatch, drawPosition);
        }


        //draw the shadow filter onto screen wtih respect to the camera
        public void DrawFilter(Scene2DNode node, float scale)
        {
            Vector2 drawPosition = ApplyTransformations(node.position);
            node.DrawFilter(spriteBatch, drawPosition, scale);
        }


        //draw the platform on screen with respect to the camera
        public void DrawPlatform(Platform platform)
        {
            Vector2 drawPosition = ApplyTransformations(platform.position);
            platform.Draw(spriteBatch, drawPosition);
        }

        //draw the portal on screen with respect to the camera
        public void DrawPortal(Portal portal)
        {
            Vector2 drawPosition = ApplyTransformations(portal.position);
            portal.Draw(spriteBatch, drawPosition);
        }

        //draw the pickUpItem on screen with respect to the camera
        public void DrawPickUpItem(PickUpItem item)
        {
            Vector2 drawPosition = ApplyTransformations(item.position);
            item.Draw(spriteBatch, drawPosition);
        }

        public void DrawLadder(Ladder ladder)
        {
            Vector2 drawPosition = ApplyTransformations(ladder.position);
            ladder.Draw(spriteBatch, drawPosition);
        }

        public void DrawInteractable(Interactable interactable)
        {
            Vector2 drawPosition = ApplyTransformations(interactable.position);
            interactable.Draw(spriteBatch, drawPosition);
        }
    }
}

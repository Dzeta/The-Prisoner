﻿using System;
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
            if (ApplyTransformations(node.position).X > screenSize.X / 2)
            {
                cameraPosition.X = node.position.X - (screenSize.X / 2);
            }
            ////vertical transformations
            //if (ApplyTransformations(node.position).Y > screenSize.Y / 2)
            //{
            //    cameraPosition.X = node.position.Y - (screenSize.Y / 2);
            //}
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
        }

        //draw the node on screen with respect to the camera
        public void DrawNode(Scene2DNode node)
        {
            Vector2 drawPosition = ApplyTransformations(node.position);
            node.Draw(spriteBatch, drawPosition);
        }
    }
}
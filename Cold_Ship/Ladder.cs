﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace Cold_Ship
{
    public class Ladder
    {
        //declare member variables
        public Texture2D texture;
        public Vector2 position;
        public Vector2 size;
        
        private Rectangle collision;
        //declare constructor
        public Ladder(Texture2D texture, Vector2 size, Vector2 position)
        {
            this.texture = texture;
            this.size = size;
            this.position = position;
            collision = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }

        //update method that handles the collisions
        public bool Update(Scene2DNode player)
        {
            return collision.Intersects(new Rectangle((int)player.position.X, (int)player.position.Y, (int)player.playerSpriteSize.X, (int)player.playerSpriteSize.Y));
        }

        //draw method that draws the platform onto the screen
        public void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            spriteBatch.Draw(texture, drawPosition, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
        }
    }
}
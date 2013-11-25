using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cold_Ship
{
    public class Reactor : GenericSprite2D
    {
        Texture2D[] textures = new Texture2D[3];
        int currentTexture = 0;
        float timer = 0;
        float timerChangeState = 2000;

        public Reactor(ContentManager Content, Vector2 position)
            : base(Content.Load<Texture2D>("Textures\\reactor_01"), position, Rectangle.Empty)
        {
            textures[0] = Content.Load<Texture2D>("Textures\\reactor_01");
            textures[1] = Content.Load<Texture2D>("Textures\\reactor_02");
            textures[2] = Content.Load<Texture2D>("Textures\\reactor_03");
        }

        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;
            if (timer > timerChangeState)
            {
                timer = 0;
                currentTexture++;

                if (currentTexture > 2)
                    currentTexture = 0;

                this.Texture = textures[currentTexture];
            }
        }
    }
}

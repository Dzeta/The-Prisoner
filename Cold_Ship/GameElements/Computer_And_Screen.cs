using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cold_Ship
{
    public class Computer_And_Screen : GenericSprite2D
    {
        public Texture2D[] screen_Textures = new Texture2D[13];
        public GenericSprite2D computerNode;
        public int currentTexture = 0;
        public float timer = 0;
        public float timerChangeState = 1350;
        public Vector2 screenPosition;

        public Computer_And_Screen(ContentManager Content, Vector2 position)
            : base(Content.Load<Texture2D>("Objects\\Computer_Screen\\computerscreen_on_01"), position, Rectangle.Empty)
        {
            screen_Textures[0] = Content.Load<Texture2D>("Objects\\Computer_Screen\\computerscreen_on_01");
            screen_Textures[1] = Content.Load<Texture2D>("Objects\\Computer_Screen\\computerscreen_on_02");
            screen_Textures[2] = Content.Load<Texture2D>("Objects\\Computer_Screen\\computerscreen_on_03");
            screen_Textures[3] = Content.Load<Texture2D>("Objects\\Computer_Screen\\computerscreen_on_04");
            screen_Textures[4] = Content.Load<Texture2D>("Objects\\Computer_Screen\\computerscreen_on_05");
            screen_Textures[5] = Content.Load<Texture2D>("Objects\\Computer_Screen\\computerscreen_on_06");
            screen_Textures[6] = Content.Load<Texture2D>("Objects\\Computer_Screen\\computerscreen_on_07");
            screen_Textures[7] = Content.Load<Texture2D>("Objects\\Computer_Screen\\computerscreen_on_08");
            screen_Textures[8] = Content.Load<Texture2D>("Objects\\Computer_Screen\\computerscreen_on_09");
            screen_Textures[9] = Content.Load<Texture2D>("Objects\\Computer_Screen\\computerscreen_on_10");
            screen_Textures[10] = Content.Load<Texture2D>("Objects\\Computer_Screen\\computerscreen_on_11");
            screen_Textures[11] = Content.Load<Texture2D>("Objects\\Computer_Screen\\computerscreen_on_12");
            screen_Textures[12] = Content.Load<Texture2D>("Objects\\Computer_Screen\\computerscreen_on_13");
            screenPosition = new Vector2(position.X + 7, position.Y + 6);
            computerNode = new GenericSprite2D(Content.Load<Texture2D>("Objects\\computer"), position);
        }

        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;
            if (timer > timerChangeState)
            {
                timer = 0;
                currentTexture++;

                if (currentTexture > 12)
                    currentTexture = 12;

                this.Texture = screen_Textures[currentTexture];
            }
        }
    }
}

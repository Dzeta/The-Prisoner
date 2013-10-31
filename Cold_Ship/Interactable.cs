using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Cold_Ship
{
    public class Interactable : Scene2DNode
    {
        //declare enum for type of interactable
        public enum Type_Of_Interactable {GENERATOR, LIGHT_SWITCH};
       
        //declare member variables
        public Vector2 size;
        public Type_Of_Interactable typeOfInteractable;
        public Texture2D altTexture;

        //Constructor
        public Interactable(Texture2D texture, Vector2 position, Vector2 size, Type_Of_Interactable type, Texture2D altTexture = null) :
            base(texture, position)
        {
            this.size = size;
            this.typeOfInteractable = type;
            this.altTexture = altTexture;
        }

        //Update function (detect collision, etc.)
        public void Update(Scene2DNode playerNode, ref bool generatorOn, ref bool filterOn, ref float filterScale)
        {
            if (new Rectangle((int)playerNode.position.X, (int)playerNode.position.Y, (int)playerNode.playerSpriteSize.X, (int)playerNode.playerSpriteSize.Y).Intersects(new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y)))
            {
                KeyboardState keyboard = Keyboard.GetState();
                if (keyboard.IsKeyDown(Keys.E))
                {
                    switch (typeOfInteractable)
                    {
                        case Type_Of_Interactable.GENERATOR:
                            generatorOn = true;
                            if (altTexture != null)
                            {
                                texture = altTexture;
                            }
                            break;
                        case Type_Of_Interactable.LIGHT_SWITCH:
                            if (generatorOn)
                            {
                                filterOn = false;
                            }
                            break;
                    }
                }
            }
        }

        //draw the item onto screen
        public void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            spriteBatch.Draw(texture, drawPosition, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
        }
    }
}

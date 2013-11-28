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
    public class Interactable : GenericSprite2D
    {
        //declare enum for type of interactable
        public enum Type_Of_Interactable {GENERATOR, LIGHT_SWITCH, DOOR_SWITCH};

        //declare member variables
        public Vector2 size;
        public Type_Of_Interactable typeOfInteractable;
        public Texture2D altTexture;

        //Constructor
        public Interactable(GameLevel instance, Texture2D texture, Vector2 position, Vector2 size, Type_Of_Interactable type, Texture2D altTexture = null) :
            base(instance, texture, position, Rectangle.Empty)
        {
            this.size = size;
            this.typeOfInteractable = type;
            this.altTexture = altTexture;
        }


        //Update function (detect collision, etc.)
        public void Update(Character playerNode, ref bool generatorOn, ref bool doorCanOpen)
        {
            if (new Rectangle((int)playerNode.Position.X, (int)playerNode.Position.Y, (int)playerNode.playerSpriteSize.X, (int)playerNode.playerSpriteSize.Y).Intersects(new Rectangle((int)Position.X, (int)Position.Y, (int)size.X, (int)size.Y)))
            {
                KeyboardState keyboard = Keyboard.GetState();
                if (keyboard.IsKeyDown(HelperFunction.KeyUse))
                {
                    switch (typeOfInteractable)
                    {
                        case Type_Of_Interactable.GENERATOR:
                            generatorOn = true;
                            if (altTexture != null)
                            {
                                Texture = altTexture;
                            }
                            break;
                        case Type_Of_Interactable.LIGHT_SWITCH:
                            if (generatorOn)
                            {
                                if (altTexture != null)
                                {
                                    Texture = altTexture;
                                }
                            }
                            break;
                        case Type_Of_Interactable.DOOR_SWITCH:
                            if (generatorOn)
                            {
                                doorCanOpen = true;
                                if (altTexture != null)
                                {
                                    Texture = altTexture;
                                }
                            }
                            break;
                    }
                }
            }
        }

        //draw the item onto screen
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, this.Position, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
        }
    }
}

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
        public enum Type_Of_Interactable {GENERATOR, LIGHT_SWITCH, DOOR_SWITCH, PUZZLE_SWITCH};
        private bool activated;
        public int timeCounter;

        //declare member variables
        public Vector2 size;
        public Type_Of_Interactable typeOfInteractable;
        public Texture2D altTexture, tempTexture;

        //Constructor
        public Interactable(Texture2D texture, Vector2 position, Vector2 size, Type_Of_Interactable type, Texture2D altTexture = null) :
            base(texture, position, Rectangle.Empty)
        {
            this.size = size;
            this.typeOfInteractable = type;
            this.altTexture = altTexture;
            activated = false;
            tempTexture = texture;
            timeCounter = 200;
        }

        public bool isActivated() { return activated; }
        public bool isNotActivated() { return !activated; }

        //Update function (detect collision, etc.)
        public void Update(Character playerNode, ref bool generatorOn, ref bool filterOn, Filter filter, ref bool doorCanOpen)
        {
            if (new Rectangle((int)playerNode.Position.X, (int)playerNode.Position.Y, (int)playerNode.playerSpriteSize.X, (int)playerNode.playerSpriteSize.Y).Intersects(new Rectangle((int)Position.X, (int)Position.Y, (int)size.X, (int)size.Y)))
            {
                KeyboardState keyboard = Keyboard.GetState();
                if (keyboard.IsKeyDown(HelperFunction.KeyUse))
                {
                    switch (typeOfInteractable)
                    {
                        case Type_Of_Interactable.GENERATOR:
                            if (!activated)
                            {
                              Sounds.soundBank.PlayCue("sound_generator");
                              generatorOn = true;
                              activated = true;
                              if (altTexture != null)
                              {
                                Texture = altTexture;
                              }
                            }
                            break;
                        case Type_Of_Interactable.LIGHT_SWITCH:
                            if (generatorOn)
                            {
                              if (!activated)
                              {
                                Sounds.soundBank.PlayCue("sound_switch");
                                activated = true;
                                filterOn = false;
                                if (altTexture != null)
                                {
                                  Texture = altTexture;
                                }
                              }
                            }
                            break;
                        case Type_Of_Interactable.DOOR_SWITCH:
                            if (generatorOn)
                            {
                              if (!activated)
                              {
                                Sounds.soundBank.PlayCue("sound_switch");
                                activated = true;
                                doorCanOpen = true;
                                if (altTexture != null)
                                {
                                  Texture = altTexture;
                                }
                              }
                            }
                            break;
                        case Type_Of_Interactable.PUZZLE_SWITCH:
                            if ((altTexture != null) && (activated == false) && timeCounter > 25)
                            {
                                Sounds.soundBank.PlayCue("sound_switch");
                                Texture = altTexture;
                                activated = true;
                                timeCounter = 0;
                            }
                            else if (activated == true && timeCounter > 25)
                            {
                                Sounds.soundBank.PlayCue("sound_switch");
                                Texture = tempTexture;
                                activated = false;
                                timeCounter = 0;
                            }
                            break;
                    }
                }
            }
        }

        //draw the item onto screen
        public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            spriteBatch.Draw(Texture, drawPosition, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
        }
    }
}

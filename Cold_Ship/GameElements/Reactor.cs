using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cold_Ship.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cold_Ship
{
    public class Reactor : GenericAnimated2DSprite
    {
      public static int REACTOR_SWITCH_TIMER = 2000;

      public Reactor(GameLevel instance, Queue<Texture2D> textures
        , Vector2 position) : base(instance, textures, position)
      {
        this._FrameSwitchTime = REACTOR_SWITCH_TIMER;
      }

      public static Reactor GetNewInstance(GameLevel instance, Vector2 position)
      {
        if (Reactor.ANIMATION_TEXTURES_FOR_LOADING == null)
        {
          Queue<Texture2D> _textures = new Queue<Texture2D>();
          _textures.Enqueue(instance.Content.Load<Texture2D>("Textures/reactor_01"));
          _textures.Enqueue(instance.Content.Load<Texture2D>("Textures/reactor_02"));
          _textures.Enqueue(instance.Content.Load<Texture2D>("Textures/reactor_03"));
        }

        Reactor _instance = new Reactor(instance, Reactor.ANIMATION_TEXTURES_FOR_LOADING, position);
        return _instance;
      }
    }
}

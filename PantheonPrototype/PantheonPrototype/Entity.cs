using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PantheonPrototype
{
    class Entity
    {
        /// <summary>
        /// The location of the entity.
        /// </summary>
        protected Rectangle location;

        /// <summary>
        /// The visual representation of the entity.
        /// </summary>
        protected Sprite sprite;

        /// <summary>
        /// Public access to the location vector.                               
        /// </summary>
        public Rectangle Location
        {
            get { return location; }
            set { location = value; }
        }

        /// <summary>
        /// Public access to the sprite.
        /// </summary>
        public Sprite Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        //Collision
        public bool collidesWith(Entity other)
        {
            return true;
        }
    }
}

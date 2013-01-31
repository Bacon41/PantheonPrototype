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
        /// The location and bounds of the entity.
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


        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime)
        {
        }

        /// <summary>
        /// Detects collisions between another entity.
        /// 
        /// Uses the rectangle defining sprite to detect collisions.
        /// </summary>
        /// <param name="other">The entity to detect collision with.</param>
        /// <returns>True if the bounding rectangles overlap.</returns>
        public bool collidesWith(Entity other)
        {
            return this.location.Intersects(other.Location);
        }
    }
}

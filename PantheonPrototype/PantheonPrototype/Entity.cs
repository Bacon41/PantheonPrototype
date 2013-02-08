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
    /// <summary>
    /// The Entity defines an object within the game. It has physical presence
    /// in the game as a Sprite and can collide with other Entities.
    /// </summary>
    class Entity
    {
        /// <summary>
        /// The location and bounds of the entity.
        /// </summary>
        protected Rectangle location;

        public Rectangle Location
        {
            get { return location; }
            set { location = value; }
        }

        protected Rectangle prevLocation;

        public Rectangle PrevLocation
        {
            get { return prevLocation; }
            set { prevLocation = value; }
        }

        /// <summary>
        /// A string representing the current state.
        /// </summary>
        protected string currentState;

        public string CurrentState {
            get
            {
                return currentState;
            }
            set
            {
                //Change the sprite state
                sprite.changeState(value);

                //Change the current state
                currentState = value;
            }
        }

        /// <summary>
        /// The visual representation of the entity.
        /// </summary>
        protected Sprite sprite;

        public Sprite Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        /// <summary>
        /// Constructs a basic entity.
        /// </summary>
        public Entity()
        {
            this.sprite = new Sprite();
            this.location = new Rectangle(0, 0, 40, 40);
            this.prevLocation = this.location;

            currentState ="Default";
        }

        /// <summary>
        /// Loads any assets this particular entity needs.
        /// </summary>
        public virtual void Load(ContentManager contentManager)
        {
        }

        /// <summary>
        /// Updates the entity... yeah.
        /// </summary>
        /// <param name="gameTime">Even has the option for frame rate, pretty cool eh?</param>
        /// <param name="gameRefence">The ugly global game reference of doom.</param>
        public virtual void Update(GameTime gameTime, Pantheon gameRefence)
        {
            this.sprite.Update(gameTime);
        }

        /// <summary>
        /// Draws the entity to its current location.
        /// </summary>
        /// <param name="canvas">An initialized sprite batch to draw the sprite upon.</param>
        public virtual void Draw(SpriteBatch canvas)
        {
            this.sprite.Draw(canvas, location);
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

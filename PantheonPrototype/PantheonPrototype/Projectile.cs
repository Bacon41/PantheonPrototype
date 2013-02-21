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
using FuncWorks.XNA.XTiled;

namespace PantheonPrototype
{
    /// <summary>
    /// Contains all that is necessary to create a projectile in the game.
    /// May be used for bullets, fireballs, and maybe even eggs.
    /// 
    /// When given an initial location and velocity, the projectile shall
    /// continue onwards unless told otherwise.
    /// </summary>
    class Projectile : Entity
    {
        /// <summary>
        /// This is the velocity of the projectile. This class causes the projectile
        /// to move by this amount every Update cycle.
        /// </summary>
        protected Vector2 velocity;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        protected int timeToLive;

        protected bool toDestroy;

        public bool ToDestroy
        {
            get { return toDestroy; }
        }

        /// <summary>
        /// A pass through constructor for the Entity class
        /// </summary>
        /// <param name="location">The location of the entity relative to global space. Note that the reference point of the entity is the center of the bounding box.</param>
        /// <param name="drawBox">The box to which the sprite will be drawn. Only the width and height will be used.</param>
        /// <param name="boundingBox">The bounding box of the entity relative to the upper right hand corner of the entity.</param>
        public Projectile(Vector2 location, Rectangle drawBox, Rectangle boundingBox)
            : base(location, drawBox, boundingBox)
        {
            toDestroy = false;
            timeToLive = 250;
        }

        /// <summary>
        /// Loads the projectile object.
        /// </summary>
        /// <param name="contentManager">The content manager for loading resources.</param>
        public override void Load(ContentManager contentManager)
        {
            base.Load(contentManager);
        }

        /// <summary>
        /// Updates the projectile each frame.
        /// </summary>
        /// <param name="gameTime">The amount of time since the last call of Update.</param>
        /// <param name="gameReference">A reference to the entire game.</param>
        public override void Update(GameTime gameTime, Pantheon gameReference)
        {
            base.Update(gameTime, gameReference);

            //Update the location
            this.Location = Location + velocity;

            timeToLive -= gameTime.ElapsedGameTime.Milliseconds;
            if (timeToLive <= 0)
            {
                toDestroy = true;
            }
        }

        /// <summary>
        /// Draws the projectile.
        /// </summary>
        /// <param name="canvas">The spritebatch onto which to draw the projectile.</param>
        public override void Draw(SpriteBatch canvas)
        {
            base.Draw(canvas);
        }
    }
}
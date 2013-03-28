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
    /// This class contains methods to run basic cutscenes (such as level loading).
    /// </summary>
    public class CutsceneManager
    {
        protected Texture2D hideTexture;
        protected Rectangle hideRect;
        protected int hideOffset;
        protected bool cutcsenePlaying;

        public bool CutscenePlaying
        {
            get { return cutcsenePlaying; }
        }

        protected bool cutsceneEnded;

        public bool CutsceneEnded
        {
            get { return cutsceneEnded; }
        }

        public CutsceneManager(GraphicsDevice graphicsDevice)
        {
            this.hideTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            this.hideTexture.SetData(new[] { Color.Black });
            this.hideRect = Rectangle.Empty;
            this.hideOffset = -20;
            this.cutcsenePlaying = false;
            this.cutsceneEnded = false;
        }
        
        /// <summary>
        /// The method to make sure that the cutsecen actually moves.
        /// </summary>
        /// <param name="gameTime">Time since last update.</param>
        public void Update(GameTime gameTime, Pantheon gameReference)
        {
            cutsceneEnded = false;
            if (hideRect.Height < 0 || hideRect.Height > gameReference.GraphicsDevice.Viewport.Height)
            {
                if (hideRect.Height < 0)
                {
                    gameReference.ControlManager.enableControls();
                }
                cutcsenePlaying = false;
                cutsceneEnded = true;
                hideRect = Rectangle.Empty;
            }
            else if (cutcsenePlaying)
            {
                hideRect.Height += hideOffset;
            }
        }

        public void PlayLevelLoad(Pantheon gameReference)
        {
            cutcsenePlaying = true;
<<<<<<< HEAD
            gameReference.ControlManager.disableControls(false);
=======
            gameReference.controlManager.disableControls(false);
>>>>>>> origin
            hideRect.Width = gameReference.GraphicsDevice.Viewport.Width;
            hideRect.Height = gameReference.GraphicsDevice.Viewport.Height;
            hideOffset = -20;
        }

        public void PlayLevelEnd(Pantheon gameReference)
        {
            cutcsenePlaying = true;
<<<<<<< HEAD
            gameReference.ControlManager.disableControls(false);
=======
            gameReference.controlManager.disableControls(false);
>>>>>>> origin
            hideRect.Width = gameReference.GraphicsDevice.Viewport.Width;
            hideRect.Height = 0;
            hideOffset = 20;
        }

        /// <summary>
        /// The method to draw anything to cover the screen.
        /// </summary>
        /// <param name="spriteBatch">The object for drawing.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(hideTexture, hideRect, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}

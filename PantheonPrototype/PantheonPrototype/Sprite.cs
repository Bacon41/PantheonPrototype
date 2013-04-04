using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PantheonPrototype
{
    /// <summary>
    /// Provides a basic class for managing a sprite on screen. This class
    /// contains a loading method, a potential state system, and a drawing
    /// method.
    /// </summary>
    public class Sprite
    {
        /// <summary>
        /// The sprite's image property.
        /// </summary>
        private Texture2D image;

        /// <summary>
        /// The number of frame rows in the sprite.
        /// </summary>
        private int rows;

        /// <summary>
        /// The number of frame columns in the sprite.
        /// </summary>
        private int columns;

        /// <summary>
        /// The total number of frames in the sprite.
        /// </summary>
        private int totalFrames;

        /// <summary>
        /// The increment to the next frame.
        /// </summary>
        private int incrementor;

        protected float rotation;

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        /// <summary>
        /// A structure to store the frames for a given state in the sprite.
        /// </summary>
        private struct FrameRange
        {
            public int first;
            public int last;
            public bool looping;
            public bool sweeping;
        }

        /// <summary>
        /// The frame ranges for each given state.
        /// </summary>
        private Dictionary<string, FrameRange> stateRange;

        /// <summary>
        /// The current state of the sprite. (Defines the current animation.)
        /// </summary>
        private string currentState;

        /// <summary>
        /// The current frame in the sprite's animation.
        /// </summary>
        private int currentFrame;

        /// <summary>
        /// The current opacity of the sprite.
        /// </summary>
        private int opacity;
        
        /// <summary>
        /// Gets the current opacity or sets the opacity
        /// Set takes an integer from 0 (transparent) to 100 (fully opaque)
        /// </summary>
        public int Opacity
        {
            get { return opacity; }
            set { opacity = (int)(value * (2.55)); }
        }

        public Sprite()
        {
            stateRange = new Dictionary<string, FrameRange>();
            this.incrementor = 1;
        }

        public Sprite(Texture2D image, int rows, int columns)
        {
            //Set the rotation of the sprite
            this.rotation = 0;
            loadSprite(image, rows, columns, 30);
            this.incrementor = 1;
        }

        public Sprite(Texture2D image, int rows, int columns, bool backAndForth)
        {
            //Set the rotation of the sprite
            this.rotation = 0;
            loadSprite(image, rows, columns, 30);
            this.incrementor = 1;
            
        }

        /// <summary>
        /// Loads a sprite with an image.
        /// 
        /// Note: This function will clear any classes
        /// </summary>
        /// <param name="image">The image to be used for the sprite.</param>
        /// <param name="rows">The number of frame rows in the image.</param>
        /// <param name="columns">The number of frame columns in the image.</param>
        /// <param name="frameRate">The frame rate at which the sprite updates.</param>
        public void loadSprite(Texture2D image, int rows, int columns, int frameRate)
        {
            //Get the image
            this.image = image;

            //Get the number of rows and columns
            this.rows = rows;
            this.columns = columns;

            //Get the total number of frames
            this.totalFrames = rows * columns;

            //Initialize to the first frame
            this.currentFrame = 0;

            //Create a temporary frame range for the default state
            FrameRange temp;
            temp.first = 0;
            temp.last = totalFrames - 1;
            temp.looping = true;
            temp.sweeping = false;

            //Clear the stateRange with a blank dictionary
            stateRange = new Dictionary<string, FrameRange>();

            //Create the default state in case the user doesn't specify one
            this.stateRange.Add("default", temp);

            //Set the current state to default
            this.currentState = "default";

            //Set the default opacity to fully opaque
            Opacity = 100;
        }

        /// <summary>
        /// Creates a new state and adds it to the sprite.
        /// 
        /// Deletes the default state the first time this is called. If the
        /// current state is the default state, the method shall check to see
        /// if the current frame is outside of the new range and shall adjust
        /// so that it is not.
        /// </summary>
        /// <param name="state">The name for the new state.</param>
        /// <param name="first">The first frame of the new state.</param>
        /// <param name="last">The last frame of the new state.</param>
        /// <param name="looping">Whether or not the animation of the current state should loop.</param>
        /// <param name="sweeping">Whether or not the animation of the current state plays back and forth.</param>
        public void addState(string state, int first, int last, bool looping, bool sweeping)
        {
            //Initialize a range object
            FrameRange range;
            range.first = first;
            range.last = last;
            range.looping = looping;
            range.sweeping = sweeping;

            if (stateRange.ContainsKey("default"))
            {
                //Remove the default state if one has been specified. 
                stateRange.Remove("default");

                //Make sure to change the current state and frame to
                //reasonable values.
                if (currentState == "default")
                {
                    currentState = state;

                    if (currentFrame < range.first || currentFrame > range.last)
                    {
                        currentFrame = range.first;
                    }
                }
            }

            //Add the new state
            stateRange.Add(state, range);
        }

        /// <summary>
        /// Removes the specified state from the sprite.
        /// </summary>
        /// <param name="state">The name of the state to remove.</param>
        public void removeState(string state)
        {
            stateRange.Remove(state);
        }

        /// <summary>
        /// This method returns the name of the curren state.
        /// </summary>
        /// <returns>The current state of the sprite.</returns>
        public string getState()
        {
            return currentState;
        }

        /// <summary>
        /// Changes the current animation state of the sprite.
        /// </summary>
        /// <param name="state">The new state for the sprite.</param>
        /// <returns>False if the passed state does not exist.</returns>
        public bool changeState(string state)
        {
            if (stateRange.ContainsKey(state) && this.currentState != state)
            {
                currentState = state;
                this.incrementor = 1;
                this.currentFrame = this.stateRange[currentState].first;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Update(GameTime gameTime)
        {
            //Increment the current frame
            currentFrame += incrementor;

            // Not looping? At last frame? Stop moving.
            if (!stateRange[currentState].looping && currentFrame == stateRange[currentState].last)
            {
                incrementor = 0;
            }

            // Frame sweeping? At last frame? Go backwards.
            if (stateRange[currentState].sweeping && currentFrame >= stateRange[currentState].last)
            {
                incrementor = -Math.Abs(incrementor);
            }

            // Frame sweeping? At first frame? Go forwards.
            if (stateRange[currentState].sweeping && currentFrame <= stateRange[currentState].first)
            {
                incrementor = Math.Abs(incrementor);
            }

            // Not frame sweeping? After last frame? Go to first frame.
            if (!stateRange[currentState].sweeping && currentFrame > stateRange[currentState].last)
            {
                currentFrame = stateRange[currentState].first;
            }
        }

        /// <summary>
        /// Draws the sprite to the screen at the given location.
        /// </summary>
        /// <param name="canvas">An initialized SpriteBatch to which the sprite will be drawn.</param>
        /// <param name="location">The location as a vector to which the sprite should be drawn.</param>
        /// <param name="origin">The origin around which the sprite should rotate</param>
        public void Draw(SpriteBatch canvas, Rectangle location, Vector2 origin)
        {
            if (image == null) return;

            //Calculate frame specific information
            int width = image.Width / columns;
            int height = image.Height / rows;
            int row = (int)((float)currentFrame / (float)columns);
            int column = currentFrame % columns;

            //Calculate the source and destination rectangles for the current frame
            Rectangle sourceRectangle = new Rectangle(column * width, row * height, width, height);
            Rectangle destinationRectangle = location;

            //Calculate the origin relative to the scale of the destination rectangle
            Vector2 realOrigin = new Vector2(
                origin.X * sourceRectangle.Width / destinationRectangle.Width,
                origin.Y * sourceRectangle.Height / destinationRectangle.Height);

            //Draw the correct frame of the image
            canvas.Draw(image, destinationRectangle, sourceRectangle, new Color(opacity, opacity, opacity, opacity), rotation,
                realOrigin, SpriteEffects.None, (float)((Math.Atan(location.Bottom) / Math.PI + .5) * -0.1 + 0.1));
        }
    }
}

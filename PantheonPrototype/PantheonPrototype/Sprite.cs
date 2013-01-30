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
    class Sprite
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
        /// A structure to store the frames for a given state in the sprite.
        /// </summary>
        private struct FrameRange
        {
            public int first;
            public int last;
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
        /// Loads a sprite with an image.
        /// 
        /// Note: This function will clear any classes
        /// </summary>
        /// <param name="image">The image to be used for the sprite.</param>
        /// <param name="rows">The number of frame rows in the image.</param>
        /// <param name="columns">The number of frame columns in the image.</param>
        public void loadSprite(Texture2D image, int rows, int columns)
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
            temp.last = totalFrames;

            //Create the default state in case the user doesn't specify one
            this.stateRange.Add("default", temp);

            //Set the current state to default
            this.currentState = "default";
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
        /// <param name="range">The frame range for the new state.</param>
        public void addState(string state, int first, int last)
        {
            //Initialize a range object
            FrameRange range;
            range.first = first;
            range.last = last;

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
        /// <param name="state"></param>
        public void removeState(string state)
        {
            stateRange.Remove(state);
        }

        /// <summary>
        /// Changes the current animation state of the sprite.
        /// </summary>
        /// <param name="state">The new state for the sprite.</param>
        /// <returns>False if the passed state does not exist.</returns>
        public bool changeState(string state)
        {
            if (stateRange.ContainsKey(state))
            {
                currentState = state;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void update()
        {
            //Increment the current frame
            currentFrame++;

            //Loop around if the frame range for the current state has been exhausted.
            if (currentFrame < stateRange[currentState].last)
            {
                currentFrame = stateRange[currentState].first;
            }
        }

        /// <summary>
        /// Draws the sprite to the screen at the given location.
        /// </summary>
        /// <param name="canvas">An initialized SpriteBatch to which the sprite will be drawn.</param>
        /// <param name="location">The location as a vector to which the sprite should be drawn.</param>
        public void draw(SpriteBatch canvas, Vector2 location)
        {
            //Calculate frame specific information
            int width = image.Width / columns;
            int height = image.Height / rows;
            int row = (int)((float)currentFrame / (float)columns);
            int column = currentFrame % columns;

            //Calculate the source and destination rectangles for the current frame
            Rectangle sourceRectangle = new Rectangle(column * width, row * height, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            //Draw the correct frame of the image
            canvas.Draw(image, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}

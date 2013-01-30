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

namespace ProofOfConcept
{
    /// <summary>
    /// The class that contains all of the information for the camera's rotation, position, zoom, and related.
    /// </summary>
    public class Camera
    {
        // The information needed for the camera to draw properly
        protected float zoom;
        public Matrix Transform;
        public Vector2 Position;
        protected float rotation;

        int WIDTH;
        int HEIGHT;

        /// <summary>
        /// The constructor that reads in the initial camera frame size and initializes the other information.
        /// </summary>
        /// <param name="w">Screen width.</param>
        /// <param name="h">Screen height.</param>
        public Camera(int w, int h)
        {
            // Saving all of the values
            zoom = 1f;
            rotation = 0.0f;
            Position = Vector2.Zero;

            WIDTH = w;
            HEIGHT = h;
        }

        /// <summary>
        /// Gets and sets the zoom.
        /// </summary>
        public float Zoom
        {
            // Does not allow negative zoom
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; }
        }

        /// <summary>
        /// Gets and sets the rotation.
        /// </summary>
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        /// <summary>
        /// An additional way to move the camera.
        /// </summary>
        /// <param name="offset">The offest for the camera</param>
        public void Move(Vector2 offset)
        {
            Position += offset;
        }

        /// <summary>
        /// Getting and setting the postition directly.
        /// </summary>
        public Vector2 Pos
        {
            get { return Position; }
            set { Position = value; }
        }

        /// <summary>
        /// The method to get the matrix that is the transformed screen based on zooms, shifts, rotations, and the like.
        /// </summary>
        /// <returns>The transformation matrix that is a result of the shifts and whatnot.</returns>
        public Matrix getTransformation()
        {
            // Calculating and returning the transformation maxtrix based on the shift, rotation, and zoom
            Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(WIDTH * 0.5f, HEIGHT * 0.5f, 0));
            return Transform;
        }
    }
}

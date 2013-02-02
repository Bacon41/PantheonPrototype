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

namespace CustomLoad
{
    public class Map
    {
        int width;
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        int height;
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        int[] textureGrid;
        public int[] TextureGrid
        {
            get { return textureGrid; }
            set { textureGrid = value; }
        }

        int[] collisionGrid;
        public int[] CollisionGrid
        {
            get { return collisionGrid; }
            set { collisionGrid = value; }
        }

        private Texture2D testSheet;

        public Map()
        {
        }

        public void Load(ContentManager contentManager)
        {
            testSheet = contentManager.Load<Texture2D>("testImg");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    spriteBatch.Draw(testSheet, new Rectangle(64 * x, 64 * y, 64, 64),
                        new Rectangle(64 * textureGrid[width * y + x], 0, 64, 64), Color.White);
                }
            }
        }
    }
}

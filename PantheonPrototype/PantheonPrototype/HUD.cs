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
    /// This is the wrapper class for all information that will go
    /// into the HUD. It will have HUDItems in it, thad can be added
    /// at will and drawn all at once.
    /// </summary>
    class HUD
    {
        /// <summary>
        /// All of the different HUDItems that will need to be drawn.
        /// More or less can be registered.
        /// </summary>
        protected List<HUDItem> hudItems;
        protected ContentManager content;
        protected GraphicsDevice graphicsDevice;
        protected Texture2D backing;
        protected Texture2D background;
        protected Vector2 HUDcoords;
        protected int SCREEN_WIDTH;
        protected int SCREEN_HEIGHT;
        protected int danger;

        public HUD(GraphicsDevice graphicsDevice, ContentManager Content, int WIDTH, int HEIGHT)
        {
            this.graphicsDevice = graphicsDevice;
            content = Content;
            SCREEN_WIDTH = WIDTH;
            SCREEN_HEIGHT = HEIGHT;

            backing = Content.Load<Texture2D>("HUDbacking");
            background = Content.Load<Texture2D>("HUDbackground");
            hudItems = new List<HUDItem>();
            HUDcoords = new Vector2(0, SCREEN_HEIGHT - background.Height - 20);

            danger = 0;

            AddItem("ArmorBar", 5, 46);
            AddItem("IndicatorG", 230, 10);
            AddItem("IndicatorY", 230, 10);
            AddItem("IndicatorR", 230, 10);
            AddItem("IndicatorD", 230, 10);
            AddItem("IndicatorEmpty", 230, 10);
        }

        /// <summary>
        /// This is the method to add more items to the HUD. It will probably have
        /// parameters later, but I'm not sure what yet.
        /// Accepts a string that defines an image in the preloaded content.
        /// </summary>
        public void AddItem(String img, int x, int y)
        {
            hudItems.Add(new HUDItem(content, img, (int)HUDcoords.X + x, (int)HUDcoords.Y + y));
        }

        /// <summary>
        /// The method to update all of the HUDItems' information.
        /// </summary>
        /// <param name="gameTime">The object that holds all the time information.</param>
        public void Update(GameTime gameTime, Pantheon gameReference, Level level) 
        {
            PlayerCharacter player = (PlayerCharacter)(level.Entities["character"]);
            // Set the width of the Armor Bar with respect to the current percent of the player's armor.
            try
            {
                hudItems[0].Coordinates = new Rectangle(hudItems[0].Coordinates.X, hudItems[0].Coordinates.Y, (int)(hudItems[0].DefaultWidth * ((float)player.CurrentArmor / player.TotalArmor)), hudItems[0].Coordinates.Height);
                
                // Get the Shield item
                Shield shield = (Shield)player.EquippedItems["shield"];

                // Update the Shield indicator
                int shieldPercent = (int)(100 * (float)shield.CurrentShield / shield.TotalShield);
                hudItems[1].SetOpacity((int)((shieldPercent - 55) * 5.66)); // (% - min) * (255 / (max - min))

                if (shieldPercent > 55)
                {
                    hudItems[2].SetOpacity((int)(255 - (shieldPercent - 55) * 5.66));
                }
                else
                {
                    hudItems[2].SetOpacity((int)((shieldPercent - 10) * 5.66));
                }

                if (shieldPercent > 10)
                {
                    hudItems[3].SetOpacity((int)(255 - (shieldPercent - 10) * 5.66));
                }
                else
                {
                    hudItems[3].SetOpacity(0);
                }

                if (shieldPercent <= 10 && shieldPercent > 0)
                {
                    hudItems[4].SetOpacity(danger);
                    danger = (danger + (30 - (shieldPercent * 2))) % 256; 
                }
                else
                {
                    hudItems[4].SetOpacity(0);
                    danger = 0;
                }

                if (shieldPercent == 0)
                {
                    hudItems[5].SetOpacity(255);
                }
                else
                {
                    hudItems[5].SetOpacity(0);
                }
                

            }
            catch (DivideByZeroException)
            {
                Console.Write("Total armor or shield is zero!");
            }
        }

        /// <summary>
        /// The method to draw all of the different HUDItems.
        /// </summary>
        /// <param name="spriteBatch">The object that we will use to draw to the screen.</param>
        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Begin();

            string title = "Pantheon DRAGON SPLEAN";

            spriteBatch.DrawString(font, title, new Vector2(1, 1), Color.DarkGray);
            spriteBatch.DrawString(font, title, new Vector2(0, 0), Color.LightGray);

            // Draw the Backing
            spriteBatch.Draw(backing, new Rectangle((int)HUDcoords.X, (int)HUDcoords.Y, background.Width, background.Height), Color.White);

            // Draw the Armor Bar (Must be done first due to shape)
            spriteBatch.Draw(hudItems[0].Image, hudItems[0].Coordinates, hudItems[0].Opacity);
            
            // Draw the Background
            spriteBatch.Draw(background, new Rectangle((int)HUDcoords.X, (int)HUDcoords.Y, background.Width, background.Height), Color.White);

            // Draw all the remaining items
            for (int i = 1; i < hudItems.Count; i++)
            {
                hudItems[i].Draw(spriteBatch);
            }
            
            spriteBatch.End();
        }
    }
}

﻿using System;
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
    /// This class keeps track of inventory stuff.
    /// </summary>
    class Inventory
    {
        public List<Rectangle> locationBoxes;
        public List<Rectangle> equippedBoxes;
        public Rectangle infoBox;
        protected Texture2D inventorySelector;
        private Color color;

        private int SCREEN_WIDTH;
        private int SCREEN_HEIGHT;
        private int selected;
        private int hoveredOver;

        public Inventory(int SCREEN_WIDTH, int SCREEN_HEIGHT, ContentManager Content)
        {
            locationBoxes = new List<Rectangle>();
            equippedBoxes = new List<Rectangle>();
            infoBox = new Rectangle();

            this.SCREEN_WIDTH = SCREEN_WIDTH;
            this.SCREEN_HEIGHT = SCREEN_HEIGHT;

            SetBoxes();

            selected = -1;
            hoveredOver = -1;

            color = new Color(34, 167, 222, 50);

            inventorySelector = Content.Load<Texture2D>("InvSelect");
        }

        /// <summary>
        /// The inventory Selector png
        /// </summary>
        public Texture2D InventorySelector
        {
            get { return inventorySelector; }
        }

        /// <summary>
        /// Sets the selected item in the inventory
        /// </summary>
        public int Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        /// <summary>
        /// Sets the color for the hover box
        /// </summary>
        public Color HColor
        {
            set { color = value; }
        }

        /// <summary>
        /// Updates which item is being hovered over
        /// </summary>
        public int HoveredOver
        {
            get { return hoveredOver; }
            set { hoveredOver = value; }
        }


        /// <summary>
        /// Basically hardcodes the different square areas for item's HUDrepresentation to go.
        /// </summary>
        private void SetBoxes()
        {
            // Add Inventory slots
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    locationBoxes.Add(new Rectangle((int)((.025 * SCREEN_WIDTH) + i * (.1 * SCREEN_WIDTH)), (int)((.041 * SCREEN_HEIGHT) + j * (.167 * SCREEN_HEIGHT)),
                        (int)(.1 * SCREEN_WIDTH), (int)((.167 * SCREEN_HEIGHT))));
                }
            }

            // Add Equipped slots
            for (int i = 0; i < 2; i++)
            {
                equippedBoxes.Add(new Rectangle((int)((.025 * SCREEN_WIDTH) + i * (.1 * SCREEN_WIDTH)), (int)(.792 * SCREEN_HEIGHT), 
                    (int)(.1 * SCREEN_WIDTH), (int)(.167 * SCREEN_HEIGHT)));
            }
            for (int i = 0; i < 4; i++)
            {
                equippedBoxes.Add(new Rectangle((int)((.301 * SCREEN_WIDTH) + i * (.1 * SCREEN_WIDTH)), (int)(.792 * SCREEN_HEIGHT),
                    (int)(.1 * SCREEN_WIDTH), (int)(.167 * SCREEN_HEIGHT)));
            }
            equippedBoxes.Add(new Rectangle((int)(.775 * SCREEN_WIDTH), (int)(.792 * SCREEN_HEIGHT), (int)(.1 * SCREEN_WIDTH), (int)(.167 * SCREEN_HEIGHT)));

            // Add info screen

            infoBox = new Rectangle((int)(.695 * SCREEN_WIDTH), (int)(.053 * SCREEN_HEIGHT), (int)(.305 * SCREEN_WIDTH), (int)(.417 * SCREEN_HEIGHT));
        }

        public bool SwapFromEquipped(int index)
        {
            Item temp;
            bool returnval;
            int count = 0;
            foreach (Item item in PlayerCharacter.inventory.unequipped)
            {
                if (item.isNull || item.Equals(PlayerCharacter.inventory.unequipped.ElementAt(selected)))
                {
                    break;
                }
                count++;
            }
            if (count >= 24)
            {
                return false;
            }
            else
            {
                if (!PlayerCharacter.inventory.unequipped.ElementAt(count).isNull)
                {
                    temp = PlayerCharacter.inventory.unequipped.ElementAt(count);
                    returnval = true;
                }
                else
                {
                    temp = new Item();
                    returnval = false;
                }

                PlayerCharacter.inventory.unequipped.RemoveAt(count);
                PlayerCharacter.inventory.unequipped.Insert(count, PlayerCharacter.inventory.equipped.ElementAt(index));
                PlayerCharacter.inventory.equipped.RemoveAt(index);
                PlayerCharacter.inventory.equipped.Insert(index, temp);
                return returnval;
            }

        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont Font)
        {
            if (hoveredOver != -1)
            {
                if (selected != -1)
                {
                    if (selected < 24)
                    {
                        if (hoveredOver >= 24)
                        {
                            spriteBatch.Draw(inventorySelector, equippedBoxes[hoveredOver - 24], color);
                        }
                    }
                    else
                    {
                        if (hoveredOver < 24 && PlayerCharacter.inventory.unequipped.ElementAt(hoveredOver).isNull)
                        {
                            spriteBatch.Draw(inventorySelector, locationBoxes[hoveredOver], color);
                        }
                    }
                }
                else
                {
                    if (!PlayerCharacter.inventory.unequipped.Union(PlayerCharacter.inventory.equipped).ElementAt(hoveredOver).isNull)
                    {
                        if (hoveredOver < 24)
                        {
                            spriteBatch.Draw(inventorySelector, locationBoxes.ElementAt(hoveredOver), color);
                        }
                        else
                        {
                            spriteBatch.Draw(inventorySelector, equippedBoxes.ElementAt(hoveredOver - 24), color);
                        }
                    }
                }
            }
            if (selected != -1)
            {
                if (selected < 24)
                {
                    spriteBatch.Draw(inventorySelector, locationBoxes[selected], Color.White);
                }
                else
                {
                    spriteBatch.Draw(inventorySelector, equippedBoxes[selected - 24], Color.White);
                }
            }
            int count = 0;

            foreach (Item item in PlayerCharacter.inventory.unequipped)
            {
                if (!item.isNull)
                {
                    spriteBatch.Draw(item.HUDRepresentation, locationBoxes[count], Color.White);
                }
                count++;
            }

            count = 0;
            foreach (Item item in PlayerCharacter.inventory.equipped)
            {
                if (!item.isNull)
                {
                    spriteBatch.Draw(item.HUDRepresentation, equippedBoxes[count], Color.White);
                }
                count++;
            }

            if (selected != -1)
            {
                if (selected < 24)
                {
                    spriteBatch.DrawString(Font, PlayerCharacter.inventory.unequipped.ElementAt(selected).Info, new Vector2(infoBox.X, infoBox.Y), Color.White);
                }
                else
                {
                    spriteBatch.DrawString(Font, PlayerCharacter.inventory.equipped.ElementAt(selected - 24).Info, new Vector2(infoBox.X, infoBox.Y), Color.White);
                }
            }
        }
    }
}

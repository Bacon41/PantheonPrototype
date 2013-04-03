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
    /// This class keeps track of inventory stuff.
    /// </summary>
    class Inventory
    {
        public List<Rectangle> locationBoxes;
        public List<Rectangle> equippedBoxes;
        public List<int> types;
        public Rectangle infoBox;
        public Rectangle movingBox;
        protected Rectangle trashBox;
        public Item tempStorage;
        protected Texture2D inventorySelector;
        protected Texture2D trashCan;
        protected Texture2D nullImage;
        private Color color;
        protected Color trashColor;

        private int SCREEN_WIDTH;
        private int SCREEN_HEIGHT;
        private int selected;
        private int hoveredOver;

        public Inventory(int SCREEN_WIDTH, int SCREEN_HEIGHT, Pantheon gameReference)
        {
            locationBoxes = new List<Rectangle>();
            equippedBoxes = new List<Rectangle>();
            types = new List<int>();
            infoBox = new Rectangle();
            movingBox = new Rectangle();
            trashBox = new Rectangle();

            tempStorage = new Item();

            this.SCREEN_WIDTH = SCREEN_WIDTH;
            this.SCREEN_HEIGHT = SCREEN_HEIGHT;

            SetBoxes();

            selected = -1;
            hoveredOver = -1;

            color = new Color(34, 167, 222, 50);
            trashColor = Color.White;

            inventorySelector = gameReference.Content.Load<Texture2D>("InvSelect");
            trashCan = gameReference.Content.Load<Texture2D>("TrashCan");
            nullImage = new Texture2D(gameReference.GraphicsDevice, 1,1);
            nullImage.SetData(new[] { new Color(0,0,0,0) });
        }

        /// <summary>
        /// The inventory Selector png
        /// </summary>
        public Texture2D InventorySelector
        {
            get { return inventorySelector; }
        }

        /// <summary>
        /// A Blank image
        /// </summary>
        public Texture2D NullImage
        {
            get { return nullImage; }
        }

        /// <summary>
        /// A trash can texture
        /// </summary>
        public Texture2D TrashCan
        {
            get { return trashCan; }
        }

        public Rectangle TrashBox
        {
            get { return trashBox; }
        }

        public Color TrashColor
        {
            get { return trashColor; }
            set { trashColor = value; }
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
            // Initialize size of movingBox
            movingBox = new Rectangle(0, 0, (int)(.05 * SCREEN_WIDTH), (int)((.0835 * SCREEN_HEIGHT)));

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

            // Adding types to all the different boxes. Look under Items.Type for clarification
            for (int i = 0; i < 24; i++)
            {
                types.Add(0xF);
            }
            types.Add(0x8);
            types.Add(0x8);
            for (int i = 26; i < 30; i++)
            {
                types.Add(0x4);
            }
            types.Add(0x2);

            // Setting the box for the trash icon
            trashBox = new Rectangle((int)(.915 * SCREEN_WIDTH), (int)(.83375 * SCREEN_HEIGHT), (int)(.05 * SCREEN_WIDTH), (int)(.0835 * SCREEN_HEIGHT));
        }

        public void Move(Pantheon gameReference)
        {
            Item temp;
 
            if (PlayerCharacter.inventory.unequipped.Union(PlayerCharacter.inventory.equipped).ElementAt(hoveredOver).isNull)
            {
                if (hoveredOver < 24)
                {
                    PlayerCharacter.inventory.unequipped.RemoveAt(hoveredOver);
                    PlayerCharacter.inventory.unequipped.Insert(hoveredOver, tempStorage);
                    tempStorage = new Item();
                    ((PlayerCharacter)gameReference.player).ArmedItem = PlayerCharacter.inventory.equipped.ElementAt(((PlayerCharacter)gameReference.player).CurrentArmedItem);
                }
                else if (hoveredOver >= 24)
                {
                    PlayerCharacter.inventory.equipped.RemoveAt(hoveredOver - 24);
                    PlayerCharacter.inventory.equipped.Insert(hoveredOver - 24, tempStorage);
                    tempStorage = new Item();
                    ((PlayerCharacter)gameReference.player).ArmedItem = PlayerCharacter.inventory.equipped.ElementAt(((PlayerCharacter)gameReference.player).CurrentArmedItem);
                }
                selected = -1;
            }
            else
            {
                if (hoveredOver < 24)
                {
                    temp = PlayerCharacter.inventory.unequipped.ElementAt(hoveredOver);
                    PlayerCharacter.inventory.unequipped.RemoveAt(hoveredOver);
                    PlayerCharacter.inventory.unequipped.Insert(hoveredOver, tempStorage);
                    tempStorage = temp;
                }
                else if (hoveredOver >= 24)
                {
                    temp = PlayerCharacter.inventory.equipped.ElementAt(hoveredOver - 24);
                    PlayerCharacter.inventory.equipped.RemoveAt(hoveredOver - 24);
                    PlayerCharacter.inventory.equipped.Insert(hoveredOver - 24, tempStorage);
                    tempStorage = temp;
                }
                selected = hoveredOver;
            }

        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont Font)
        {
            if (hoveredOver != -1)
            {
                spriteBatch.Draw(inventorySelector, locationBoxes.Union(equippedBoxes).ElementAt(hoveredOver), color);                
            }


            int count = 0;

            count = 0;
            foreach (Item item in PlayerCharacter.inventory.unequipped.Union(PlayerCharacter.inventory.equipped))
            {
                if (!item.isNull)
                {
                        spriteBatch.Draw(item.HUDRepresentation, locationBoxes.Union(equippedBoxes).ElementAt(count), Color.White);
                }
                count++;
            }

            if (hoveredOver != -1)
            {
                if (hoveredOver < 24)
                {
                    spriteBatch.DrawString(Font, PlayerCharacter.inventory.unequipped.ElementAt(hoveredOver).Info, new Vector2(infoBox.X, infoBox.Y), Color.White);
                }
                else
                {
                    spriteBatch.DrawString(Font, PlayerCharacter.inventory.equipped.ElementAt(hoveredOver - 24).Info, new Vector2(infoBox.X, infoBox.Y), Color.White);
                }
            }
        }
    }
}

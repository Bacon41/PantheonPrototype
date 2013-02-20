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
    /// This is the class for drawing our pause menu.
    /// </summary>
    class Menu
    {
        protected Dictionary<string, MenuItem> items;
        protected Dictionary<string, MenuItem> inventoryButtons;
        protected Rectangle mainBackgroundRect;
        protected Texture2D mainBackgroundTex;
        protected Texture2D inventoryBackground;
        protected Texture2D inventoryBackgroundTex;
        protected String menuState;

        protected int SCREEN_WIDTH;
        protected int SCREEN_HEIGHT;

        public String MenuState
        {
            get { return menuState; }
            set { menuState = value; }
        }

        public Menu()
        {
            items = new Dictionary<string, MenuItem>();
            inventoryButtons = new Dictionary<string, MenuItem>();
            menuState = "main";
        }

        public void Load(Pantheon gameReference)
        {
            mainBackgroundRect = new Rectangle((gameReference.GraphicsDevice.Viewport.Width - 300) / 2,
                (gameReference.GraphicsDevice.Viewport.Height - 400) / 2, 300, 400);
            
            mainBackgroundTex = new Texture2D(gameReference.GraphicsDevice, 1, 1);
            mainBackgroundTex.SetData(new[] { Color.White });

            inventoryBackground = gameReference.Content.Load<Texture2D>("InventoryBackground");
            inventoryBackgroundTex = gameReference.Content.Load<Texture2D>("InventoryBackgroundTexture");

            SCREEN_WIDTH = gameReference.GraphicsDevice.Viewport.Width;
            SCREEN_HEIGHT = gameReference.GraphicsDevice.Viewport.Height;
            
            loadDefaultMenu(gameReference);
        }

        /// <summary>
        /// This is the method to set up a basic menu with items in it (resume / exit).
        /// </summary>
        /// <param name="gameReference">The whole game.</param>
        private void loadDefaultMenu(Pantheon gameReference)
        {
            MenuItem resumeItem = new MenuItem("Resume", new Rectangle((mainBackgroundRect.Width - 250) / 2 + mainBackgroundRect.X,
                mainBackgroundRect.Y + 25, 250, 50));
            resumeItem.Load(gameReference);
            items.Add("resume", resumeItem);

            MenuItem exitItem = new MenuItem("Exit", new Rectangle((mainBackgroundRect.Width - 250) / 2 + mainBackgroundRect.X,
                mainBackgroundRect.Y + 100, 250, 50));
            exitItem.Load(gameReference);
            items.Add("exit", exitItem);

            MenuItem inventoryItem = new MenuItem("Inventory", new Rectangle((mainBackgroundRect.Width - 250) / 2 + mainBackgroundRect.X,
                mainBackgroundRect.Y + 175, 250, 50));
            inventoryItem.Load(gameReference);
            items.Add("inventory", inventoryItem);

            // This is not the default menu, but I wasn't sure where else to put it.

            MenuItem equipInventory = new MenuItem("Equip/Use", new Rectangle(480, 230, 150, 50));
            equipInventory.Load(gameReference);
            inventoryButtons.Add("equip", equipInventory);

            MenuItem trashInventory = new MenuItem("Trash", new Rectangle(640, 230, 150, 50));
            trashInventory.Load(gameReference);
            inventoryButtons.Add("trash", trashInventory);

            MenuItem resumeInventory = new MenuItem("Resume", new Rectangle(480, 305, 150, 50));
            resumeInventory.Load(gameReference);
            inventoryButtons.Add("resumeInv", resumeInventory);

            MenuItem mainInventory = new MenuItem("Main Menu", new Rectangle(640, 305, 150, 50));
            mainInventory.Load(gameReference);
            inventoryButtons.Add("mainMenu", mainInventory);
        }

        /// <summary>
        /// The method for updating the menu items.
        /// </summary>
        /// <param name="gameTime">How much time has elapsed.</param>
        /// <param name="gameReference">The reference to everything.</param>
        public void Update(GameTime gameTime, Pantheon gameReference)
        {
            switch (menuState)
            {
                case "main":
                    foreach (string itemName in this.items.Keys)
                    {
                        // Update every Button
                        this.items[itemName].Update(gameTime, gameReference);

                        // If mouse is on a button, Update the isSelected variable
                        if (this.items[itemName].DrawBox.Contains((int)gameReference.controlManager.actions.CursorPosition.X,
                            (int)gameReference.controlManager.actions.CursorPosition.Y))
                        {
                            this.items[itemName].IsSelected = true;
                        }
                        else
                        {
                            this.items[itemName].IsSelected = false;
                        }
                    }

                    if (gameReference.controlManager.actions.Attack)
                    {
                        if (items["resume"].DrawBox.Contains((int)gameReference.controlManager.actions.CursorPosition.X,
                            (int)gameReference.controlManager.actions.CursorPosition.Y))
                        {
                            gameReference.controlManager.actions.Pause = false;
                        }
                        if (items["exit"].DrawBox.Contains((int)gameReference.controlManager.actions.CursorPosition.X,
                            (int)gameReference.controlManager.actions.CursorPosition.Y))
                        {
                            gameReference.Exit();
                        }
                        if (items["inventory"].DrawBox.Contains((int)gameReference.controlManager.actions.CursorPosition.X,
                            (int)gameReference.controlManager.actions.CursorPosition.Y))
                        {
                            menuState = "inventory";
                        }
                    }
                    break;
                case "inventory":
                    foreach (string itemName in this.inventoryButtons.Keys)
                    {
                        // Update every Button
                        this.inventoryButtons[itemName].Update(gameTime, gameReference);

                        // If mouse is on a button, Update the isSelected variable
                        if (this.inventoryButtons[itemName].DrawBox.Contains((int)gameReference.controlManager.actions.CursorPosition.X,
                            (int)gameReference.controlManager.actions.CursorPosition.Y))
                        {
                            this.inventoryButtons[itemName].IsSelected = true;
                        }
                        else
                        {
                            this.inventoryButtons[itemName].IsSelected = false;
                        }
                    }

                    if (gameReference.controlManager.actions.Attack)
                    {
                        if (inventoryButtons["resumeInv"].DrawBox.Contains((int)gameReference.controlManager.actions.CursorPosition.X,
                            (int)gameReference.controlManager.actions.CursorPosition.Y))
                        {
                            gameReference.controlManager.actions.Pause = false;
                        }
                        if (inventoryButtons["mainMenu"].DrawBox.Contains((int)gameReference.controlManager.actions.CursorPosition.X,
                            (int)gameReference.controlManager.actions.CursorPosition.Y))
                        {
                            menuState = "main";
                        }
                        
                    }

                    break;
            }
        }

        /// <summary>
        /// The method for drawing the whole menu.
        /// </summary>
        /// <param name="spriteBatch">What is used to draw.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            switch (menuState)
            {
                case "main":
                    spriteBatch.Draw(mainBackgroundTex, mainBackgroundRect, Color.White);
                    foreach (string itemName in this.items.Keys)
                    {
                        this.items[itemName].Draw(spriteBatch);
                    }
                    break;
                case "inventory": 
                    spriteBatch.Draw(inventoryBackgroundTex, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);
                    spriteBatch.Draw(inventoryBackground, new Rectangle( 0, 0, SCREEN_WIDTH, SCREEN_HEIGHT) , Color.White);

                    foreach (string itemName in this.inventoryButtons.Keys)
                    {
                        this.inventoryButtons[itemName].Draw(spriteBatch);
                    }
                    break;
            }

            spriteBatch.End();
        }
    }
}

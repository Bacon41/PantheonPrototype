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
        protected Dictionary<string, MenuItem> splashScreenButtons;
        protected Dictionary<string, MenuItem> creditsButtons;
        protected Rectangle mainBackgroundRect;
        protected Rectangle splashScreenRect;
        protected Rectangle creditsRect;
        protected Texture2D splashScreen;
        protected Texture2D splashScreenMask;
        protected Texture2D splashShine;
        protected String splashText;
        protected bool first;
        protected Texture2D mainBackgroundTex;
        protected Texture2D inventoryBackground;
        protected Texture2D inventoryBackgroundTex;
        protected String menuState;
        protected Inventory inventory;
        QuestManager questManager;

        protected int SCREEN_WIDTH;
        protected int SCREEN_HEIGHT;
        protected int offset;

        private String creditText;

        public String MenuState
        {
            get { return menuState; }
            set { menuState = value; }
        }

        public Menu(int SCREEN_WIDTH, int SCREEN_HEIGHT)
        {
            items = new Dictionary<string, MenuItem>();
            inventoryButtons = new Dictionary<string, MenuItem>();
            splashScreenButtons = new Dictionary<string, MenuItem>();
            creditsButtons = new Dictionary<string, MenuItem>();
            menuState = "start";

            this.SCREEN_WIDTH = SCREEN_WIDTH;
            this.SCREEN_HEIGHT = SCREEN_HEIGHT;
            offset = 0;

            // These are the current credits. Feel free to change them if you want to.
            creditText = "Pantheon is a game made for LeTourneau University's\n" +
                    "Game Project class. It was made by students as\n" +
                    "a demonstration of the game engine they created\n" +
                    "The five and a half students that created this game are:\n\n" +
                    "SpenSer Bray (Comedic Comment Writing Dragon)\n" +
                    "Bob \"He's a Chicken\" Charney (Music)\n" +
                    "Michael \"Summit\" Eaton (3D Art)\n" +
                    "Hazen Johnson (Git Wizard)\n" +
                    "Terry \"Bacon\" Penner (Canadian Code and Story Writer)\n" +
                    "Tumbler Terrall (Git Dolphin)\n";

        }

        public void Load(Pantheon gameReference)
        {
            mainBackgroundRect = new Rectangle((int)(gameReference.GraphicsDevice.Viewport.Width - .35 * SCREEN_WIDTH) / 2,
                (gameReference.GraphicsDevice.Viewport.Height - 400) / 2, (int)(.35 * SCREEN_WIDTH), 400);
            
            mainBackgroundTex = new Texture2D(gameReference.GraphicsDevice, 1, 1);
            mainBackgroundTex.SetData(new[] { Color.White });

            splashScreen = gameReference.Content.Load<Texture2D>("Menu/PantheonText");
            splashScreenMask = gameReference.Content.Load<Texture2D>("Menu/PantheonTextMask");
            splashShine = gameReference.Content.Load<Texture2D>("Menu/Shine");
            splashText = "The abridged...";
            splashScreenRect = new Rectangle((int)(SCREEN_WIDTH/2 - (SCREEN_WIDTH * .75)/2),
                200, (int)(SCREEN_WIDTH * .75), (int)(SCREEN_HEIGHT * .33));
            creditsRect = new Rectangle((int)(SCREEN_WIDTH / 2 - (SCREEN_WIDTH * .75) / 2),
                50, (int)(SCREEN_WIDTH * .75), (int)(SCREEN_HEIGHT * .33));
            
            inventoryBackground = gameReference.Content.Load<Texture2D>("Inventory/InventoryBackground");
            inventoryBackgroundTex = gameReference.Content.Load<Texture2D>("Inventory/InventoryBackgroundTexture");

            inventory = new Inventory(SCREEN_WIDTH, SCREEN_HEIGHT, gameReference);
            
            loadDefaultMenu(gameReference);

            questManager = gameReference.QuestManager;
            first = true;
        }

        /// <summary>
        /// This is the method to set up a basic menu with items in it (resume / exit).
        /// </summary>
        /// <param name="gameReference">The whole game.</param>
        private void loadDefaultMenu(Pantheon gameReference)
        {
            MenuItem resumeItem = new MenuItem("Resume", new Rectangle((int)(50 - (15 * SCREEN_WIDTH) / SCREEN_WIDTH),
                ((mainBackgroundRect.Y * 100)/ SCREEN_HEIGHT) + 3, 30, 7), new Vector2(SCREEN_WIDTH, SCREEN_HEIGHT));
            resumeItem.Load(gameReference);
            items.Add("resume", resumeItem);
            MenuItem exitItem = new MenuItem("Exit", new Rectangle((int)(50 - (15 * SCREEN_WIDTH) / SCREEN_WIDTH),
                ((mainBackgroundRect.Y * 100) / SCREEN_HEIGHT) + 13, 30, 7), new Vector2(SCREEN_WIDTH, SCREEN_HEIGHT));
            exitItem.Load(gameReference);
            items.Add("exit", exitItem);

            MenuItem inventoryItem = new MenuItem("Inventory", new Rectangle((int)(50 - (15 * SCREEN_WIDTH) / SCREEN_WIDTH),
                ((mainBackgroundRect.Y * 100) / SCREEN_HEIGHT) + 23, 30, 7), new Vector2(SCREEN_WIDTH, SCREEN_HEIGHT));
            inventoryItem.Load(gameReference);
            items.Add("inventory", inventoryItem);

            // This following is not the default menu, but I wasn't sure where else to put it.
            // Inventory screen:

            MenuItem equipInventory = new MenuItem("Use", new Rectangle(67, 48, 31, 6), new Vector2(SCREEN_WIDTH, SCREEN_HEIGHT));
            equipInventory.Load(gameReference);
            inventoryButtons.Add("use", equipInventory);

            MenuItem resumeInventory = new MenuItem("Resume", new Rectangle(67, 57, 31, 6), new Vector2(SCREEN_WIDTH, SCREEN_HEIGHT));
            resumeInventory.Load(gameReference);
            inventoryButtons.Add("resumeInv", resumeInventory);

            MenuItem mainInventory = new MenuItem("Main Menu", new Rectangle(67, 66, 31, 6), new Vector2(SCREEN_WIDTH, SCREEN_HEIGHT));
            mainInventory.Load(gameReference);
            inventoryButtons.Add("mainMenu", mainInventory);

            // Start screen:

            MenuItem startGame = new MenuItem("Start New Game", new Rectangle((int)(50 - (15 * SCREEN_WIDTH) / SCREEN_WIDTH),
                (int)(((splashScreenRect.Y + splashScreen.Height) / (SCREEN_HEIGHT + 0.0)) * 100) + 25, 30, 7), new Vector2(SCREEN_WIDTH, SCREEN_HEIGHT));
            startGame.Load(gameReference);
            splashScreenButtons.Add("start", startGame);

            MenuItem loadGame = new MenuItem("Credits", new Rectangle((int)(50 - (15 * SCREEN_WIDTH) / SCREEN_WIDTH),
                (int)(((splashScreenRect.Y + splashScreen.Height) / (SCREEN_HEIGHT + 0.0)) * 100) + 35, 30, 7), new Vector2(SCREEN_WIDTH, SCREEN_HEIGHT));
            loadGame.Load(gameReference);
            splashScreenButtons.Add("credits", loadGame);

            MenuItem quitGame = new MenuItem("Quit", new Rectangle((int)(50 - (15 * SCREEN_WIDTH) / SCREEN_WIDTH),
                (int)(((splashScreenRect.Y + splashScreen.Height) / (SCREEN_HEIGHT + 0.0)) * 100) + 45, 30, 7), new Vector2(SCREEN_WIDTH, SCREEN_HEIGHT));
            quitGame.Load(gameReference);
            splashScreenButtons.Add("quit", quitGame);

            // Crdit Screen

            MenuItem back = new MenuItem("Back", new Rectangle((int)(50 - (15 * SCREEN_WIDTH) / SCREEN_WIDTH),
                (int)(((splashScreenRect.Y + splashScreen.Height) / (SCREEN_HEIGHT + 0.0)) * 100) + 45, 30, 7), new Vector2(SCREEN_WIDTH, SCREEN_HEIGHT));
            back.Load(gameReference);
            creditsButtons.Add("back", back);
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
                        if (this.items[itemName].DrawBox.Contains((int)gameReference.ControlManager.actions.CursorPosition.X,
                            (int)gameReference.ControlManager.actions.CursorPosition.Y))
                        {
                            this.items[itemName].IsSelected = true;
                        }
                        else
                        {
                            this.items[itemName].IsSelected = false;
                        }
                    }
                    if (gameReference.ControlManager.actions.MenuSelect)
                    {
                        if (items["resume"].DrawBox.Contains((int)gameReference.ControlManager.actions.CursorPosition.X,
                            (int)gameReference.ControlManager.actions.CursorPosition.Y))
                        {
                            gameReference.ControlManager.actions.Pause = false;
                        }
                        if (items["exit"].DrawBox.Contains((int)gameReference.ControlManager.actions.CursorPosition.X,
                            (int)gameReference.ControlManager.actions.CursorPosition.Y))
                        {
                            gameReference.Exit();
                        }
                        if (items["inventory"].DrawBox.Contains((int)gameReference.ControlManager.actions.CursorPosition.X,
                            (int)gameReference.ControlManager.actions.CursorPosition.Y))
                        {
                            menuState = "inventory";
                        }
                    }
                    break;

                case "inventory":

                    if (inventory.Selected == -1)
                    {
                        gameReference.IsMouseVisible = true;
                        inventoryButtons["mainMenu"].IsDisabled = false;
                        inventoryButtons["resumeInv"].IsDisabled = false;
                    }
                    else
                    {
                        gameReference.IsMouseVisible = false;
                        inventoryButtons["mainMenu"].IsDisabled = true;
                        inventoryButtons["resumeInv"].IsDisabled = true;
                    }

                    if ((inventory.tempStorage.type & Item.Type.USEABLE) > 0)
                    {
                        inventoryButtons["use"].IsDisabled = false;
                    }
                    else
                    {
                        inventoryButtons["use"].IsDisabled = true;
                    }

                    inventory.movingBox.X = (int)(gameReference.ControlManager.actions.CursorPosition.X - (.05 * SCREEN_WIDTH)/2);
                    inventory.movingBox.Y = (int)(gameReference.ControlManager.actions.CursorPosition.Y - (.0835 * SCREEN_HEIGHT)/2);


                    int count = 0;
                    foreach (string itemName in this.inventoryButtons.Keys)
                    {
                        // Update every Button
                        this.inventoryButtons[itemName].Update(gameTime, gameReference);

                        // If mouse is on a button, Update the isSelected variable
                        if (this.inventoryButtons[itemName].DrawBox.Contains((int)gameReference.ControlManager.actions.CursorPosition.X,
                            (int)gameReference.ControlManager.actions.CursorPosition.Y))
                        {
                            this.inventoryButtons[itemName].IsSelected = true;
                        }
                        else
                        {
                            this.inventoryButtons[itemName].IsSelected = false;
                        }
                    }
                    // If you click...
                    if (gameReference.ControlManager.actions.MenuSelect)
                    {
                        if (inventoryButtons["resumeInv"].DrawBox.Contains((int)gameReference.ControlManager.actions.CursorPosition.X,
                            (int)gameReference.ControlManager.actions.CursorPosition.Y))
                        {
                            // Disable if in the middle of transfering an item.
                            if (!inventoryButtons["resumeInv"].IsDisabled)
                            {
                                gameReference.ControlManager.actions.Pause = false;
                            }
                        }
                        if (inventoryButtons["mainMenu"].DrawBox.Contains((int)gameReference.ControlManager.actions.CursorPosition.X,
                            (int)gameReference.ControlManager.actions.CursorPosition.Y))
                        {
                            // Disable if in the middle of transfering an item.
                            if (!inventoryButtons["mainMenu"].IsDisabled)
                            {
                                menuState = "main";
                            }
                        }
                        if (inventoryButtons["use"].DrawBox.Contains((int)gameReference.ControlManager.actions.CursorPosition.X,
                            (int)gameReference.ControlManager.actions.CursorPosition.Y))
                        {
                            
                        }
                        count = 0;
                        if (inventory.Selected != -1)
                        {
                            if (inventory.HoveredOver != -1)
                            {
                                inventory.Move(gameReference);
                            }
                        }
                        else if (inventory.HoveredOver != -1)
                        {
                            inventory.Selected = inventory.HoveredOver;
                            inventory.tempStorage = PlayerCharacter.inventory.unequipped.Union(PlayerCharacter.inventory.equipped).ElementAt(inventory.Selected);
                            if (inventory.Selected < 24)
                            {
                                PlayerCharacter.inventory.unequipped.RemoveAt(inventory.Selected);
                                PlayerCharacter.inventory.unequipped.Insert(inventory.Selected, new Item());
                            }
                            else
                            {
                                PlayerCharacter.inventory.equipped.RemoveAt(inventory.Selected - 24);
                                PlayerCharacter.inventory.equipped.Insert(inventory.Selected - 24, new Item());
                            }
                        }

                        // Trash the selected item if you click on the trash can
                        if (inventory.TrashBox.Contains((int)gameReference.ControlManager.actions.CursorPosition.X,
                            (int)gameReference.ControlManager.actions.CursorPosition.Y) && !inventory.tempStorage.isNull)
                        {
                            inventory.tempStorage = new Item();
                            inventory.Selected = -1;
                            ((PlayerCharacter)gameReference.player).ArmedItem = PlayerCharacter.inventory.equipped.ElementAt(((PlayerCharacter)gameReference.player).CurrentArmedItem);
                        }
                  
                    }
                    // Right click to de-select
                    if (gameReference.ControlManager.actions.Deselect)
                    {
                        if (inventory.Selected != -1)
                        {
                            if (inventory.Selected < 24)
                            {
                                PlayerCharacter.inventory.unequipped.RemoveAt(inventory.Selected);
                                PlayerCharacter.inventory.unequipped.Insert(inventory.Selected, inventory.tempStorage);
                            }
                            else
                            {
                                PlayerCharacter.inventory.equipped.RemoveAt(inventory.Selected - 24);
                                PlayerCharacter.inventory.equipped.Insert(inventory.Selected - 24, inventory.tempStorage);
                            }
                            inventory.tempStorage = new Item();
                            inventory.Selected = -1;
                        }
                    }

                    count = 0;
                    foreach (Rectangle box in (inventory.locationBoxes.Union(inventory.equippedBoxes)))
                    {
                        if (inventory.Selected == -1)
                        {
                            if (box.Contains((int)gameReference.ControlManager.actions.CursorPosition.X,
                                (int)gameReference.ControlManager.actions.CursorPosition.Y) && 
                                (!PlayerCharacter.inventory.unequipped.Union(PlayerCharacter.inventory.equipped).ElementAt(count).isNull))
                            {
                                inventory.HoveredOver = count;
                                break;
                            }
                            inventory.HoveredOver = -1;
                        }
                        else
                        {
                            if (box.Contains((int)gameReference.ControlManager.actions.CursorPosition.X,
                                (int)gameReference.ControlManager.actions.CursorPosition.Y) &&
                                (inventory.tempStorage.type & inventory.types.ElementAt(count)) > 0)
                            {
                                inventory.HoveredOver = count;
                                break;
                            }
                            inventory.HoveredOver = -1;
                        }
                        count++;
                    }

                    if (inventory.Selected == -1)
                    {
                        inventory.HColor = new Color(0, 50, 255, 150);
                    }
                    else
                    {
                        inventory.HColor = new Color(34, 255, 50, 255);
                    }

                    if (inventory.TrashBox.Contains((int)gameReference.ControlManager.actions.CursorPosition.X,
                            (int)gameReference.ControlManager.actions.CursorPosition.Y) && !inventory.tempStorage.isNull)
                    {
                        inventory.TrashColor = Color.Turquoise;
                    }
                    else
                    {
                        inventory.TrashColor = Color.White;
                    }

                    break;
                case "start":
                    gameReference.ControlManager.disableControls(true);
                    foreach (string itemName in this.splashScreenButtons.Keys)
                    {
                        // Update every Button
                        this.splashScreenButtons[itemName].Update(gameTime, gameReference);

                        // If mouse is on a button, Update the isSelected variable
                        if (this.splashScreenButtons[itemName].DrawBox.Contains((int)gameReference.ControlManager.actions.CursorPosition.X,
                            (int)gameReference.ControlManager.actions.CursorPosition.Y))
                        {
                            this.splashScreenButtons[itemName].IsSelected = true;
                        }
                        else
                        {
                            this.splashScreenButtons[itemName].IsSelected = false;
                        }
                    }
                    if (gameReference.ControlManager.actions.MenuSelect)
                    {
                        if (splashScreenButtons["start"].DrawBox.Contains((int)gameReference.ControlManager.actions.CursorPosition.X,
                            (int)gameReference.ControlManager.actions.CursorPosition.Y))
                        {
                            gameReference.ControlManager.actions.Pause = false;
                            gameReference.ControlManager.enableControls();
                            gameReference.StartGame();
                        }
                        if (splashScreenButtons["credits"].DrawBox.Contains((int)gameReference.ControlManager.actions.CursorPosition.X,
                           (int)gameReference.ControlManager.actions.CursorPosition.Y))
                        {
                            menuState = "credits";
                        }
                        if (splashScreenButtons["quit"].DrawBox.Contains((int)gameReference.ControlManager.actions.CursorPosition.X,
                            (int)gameReference.ControlManager.actions.CursorPosition.Y))
                        {
                            gameReference.Exit();
                        }
                        count = 0;
                    }
                    offset = (offset + 50) % 12000;

                    if (offset > 5000 && first)
                    {
                        splashText = "The abridged...";
                    }
                    if (offset > 5250 && first)
                    {
                        splashText = "The v abridged...";
                    }
                    if (offset > 5500 && first)
                    {
                        splashText = "The ve abridged...";
                    }
                    if (offset > 5750 && first)
                    {
                        splashText = "The ver abridged...";
                    }
                    if (offset > 6000 && first)
                    {
                        splashText = "The very abridged...";
                        first = false;
                    }
                    break;
                case "credits":
                    foreach (string itemName in this.creditsButtons.Keys)
                    {
                        // Update every Button
                        this.creditsButtons[itemName].Update(gameTime, gameReference);

                        // If mouse is on a button, Update the isSelected variable
                        if (this.creditsButtons[itemName].DrawBox.Contains((int)gameReference.ControlManager.actions.CursorPosition.X,
                            (int)gameReference.ControlManager.actions.CursorPosition.Y))
                        {
                            this.creditsButtons[itemName].IsSelected = true;
                        }
                        else
                        {
                            this.creditsButtons[itemName].IsSelected = false;
                        }
                    }
                    if (gameReference.ControlManager.actions.MenuSelect)
                    {
                        if (creditsButtons["back"].DrawBox.Contains((int)gameReference.ControlManager.actions.CursorPosition.X,
                            (int)gameReference.ControlManager.actions.CursorPosition.Y))
                        {
                            menuState = "start";
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// The method for drawing the whole menu.
        /// </summary>
        /// <param name="spriteBatch">What is used to draw.</param>
        public void Draw(SpriteBatch spriteBatch, SpriteFont Font, SpriteFont Splash)
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

                    // Printing out the quest info.
                    int lineNum = 0;
                    for (int x = 0; x < questManager.quests.Count; x++)
                    {
                        spriteBatch.DrawString(Font, questManager.quests.ElementAt(x).QuestTitle,
                            new Vector2(41, 41 * lineNum), Color.Red);
                        lineNum++;

                        for (int y = 0; y < questManager.quests.ElementAt(x).CurrentObjectives.Count; y++)
                        {
                            spriteBatch.DrawString(Font, "      " + questManager.quests.ElementAt(x).CurrentObjectives.ElementAt(y).ObjectiveName,
                                new Vector2(41, 41 * lineNum), Color.Orange);
                            lineNum++;
                            spriteBatch.DrawString(Font, "          " + questManager.quests.ElementAt(x).CurrentObjectives.ElementAt(y).ObjectiveText,
                                new Vector2(41, 41 * lineNum), Color.Azure);
                            lineNum++;
                        }

                        for (int y = 0; y < questManager.quests.ElementAt(x).CompletedObjectives.Count; y++)
                        {
                            spriteBatch.DrawString(Font, "      " + questManager.quests.ElementAt(x).CompletedObjectives.ElementAt(y).ObjectiveName,
                                new Vector2(41, 41 * lineNum), Color.DarkGray);
                            lineNum++;
                        }
                    }

                    break;
                case "inventory":
                    spriteBatch.Draw(inventoryBackgroundTex, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);

                    inventory.Draw(spriteBatch, Font);

                    spriteBatch.Draw(inventoryBackground, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);

                    spriteBatch.Draw(inventory.TrashCan, inventory.TrashBox, inventory.TrashColor);

                    foreach (string itemName in this.inventoryButtons.Keys)
                    {
                        this.inventoryButtons[itemName].Draw(spriteBatch);
                    }

                    if (!(inventory.tempStorage == null) && !inventory.tempStorage.isNull)
                    {
                        spriteBatch.Draw(inventory.tempStorage.HUDRepresentation, inventory.movingBox, Color.White);
                    }

                    break;
                case "start":
                    spriteBatch.Draw(splashScreen, splashScreenRect, Color.White);
                    spriteBatch.Draw(splashShine, new Rectangle (splashScreenRect.X + offset - 4000, splashScreenRect.Y, splashScreenRect.Width, splashScreenRect.Height), Color.White);
                    spriteBatch.Draw(splashScreenMask, new Rectangle(0, splashScreenRect.Y, SCREEN_WIDTH, splashScreenRect.Height), Color.White);
                    spriteBatch.DrawString(Splash, splashText,new Vector2(splashScreenRect.X + (splashScreenRect.Width/2) - (Splash.MeasureString(splashText).X/2), splashScreenRect.Y+splashScreenRect.Height), Color.Turquoise);
                    foreach (string itemName in this.splashScreenButtons.Keys)
                    {
                        this.splashScreenButtons[itemName].Draw(spriteBatch);
                    }
                    break;
                case "credits":
                    spriteBatch.DrawString(Font, creditText, new Vector2(creditsRect.X + (creditsRect.Width / 2) - (Font.MeasureString(creditText).X / 2), creditsRect.Y + creditsRect.Height), Color.White);
                    foreach (string itemName in this.creditsButtons.Keys)
                    {
                        this.creditsButtons[itemName].Draw(spriteBatch);
                    }
                    break;
            }

            spriteBatch.End();
        }
    }
}

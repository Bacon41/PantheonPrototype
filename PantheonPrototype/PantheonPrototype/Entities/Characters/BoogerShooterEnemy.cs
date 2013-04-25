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
    class BoogerShooterEnemy : EnemyNPC
    {
        /// <summary>
        /// The constructor for the Butterfly enemy, currently just sets up the base class.
        /// </summary>
        /// <param name="location">The initial position of the Butterfly.</param>
        public BoogerShooterEnemy(Vector2 location, ContentManager Content)
            : base(location, new Rectangle(0, 0, 150, 90), new Rectangle(0, 0, 140, 80))
        {
            facing = Direction.Left;
            currentState = "Move";
            changeDirection = TimeSpan.FromSeconds(3);
            TotalArmor = 30;
            CurrentArmor = 30;

            EquippedItems.Add("weapon", new BoogerGun(Content));
            ArmedItem = EquippedItems["weapon"];
            speed = 5;
        }

        /// <summary>
        /// Load the butterfly.
        /// </summary>
        /// <param name="contentManager">So you can load things with the pipeline.</param>
        public override void Load(ContentManager contentManager)
        {
            base.Load(contentManager);

            Texture2D spriteTex;

            //Load the image
            spriteTex = contentManager.Load<Texture2D>("Sprites/BoogerShooterSprite");

            this.Sprite.loadSprite(spriteTex, 13, 8, 30);

            this.Sprite.addState("Move Back", 0, 7, true, false);
            this.Sprite.addState("Move Back Right", 8, 15, true, false);
            this.Sprite.addState("Move Back Left", 56, 63, true, false);
            this.Sprite.addState("Move Left", 48, 55, true, false);
            this.Sprite.addState("Move Right", 24, 31, true, false);
            this.Sprite.addState("Move Forward Right", 24, 31, true, false);
            this.Sprite.addState("Move Forward Left", 40, 47, true, false);
            this.Sprite.addState("Move Forward", 32, 39, true, false);
            this.Sprite.addState("Die Back", 64, 68, false, false);
            this.Sprite.addState("Die Back Right", 69, 73, false, false);
            this.Sprite.addState("Die Back Left", 99, 104, false, false);
            this.Sprite.addState("Die Left", 94, 98, false, false);
            this.Sprite.addState("Die Right", 74, 78, false, false);
            this.Sprite.addState("Die Forward Right", 79, 83, false, false);
            this.Sprite.addState("Die Forward Left", 89, 93, false, false);
            this.Sprite.addState("Die Forward", 84, 88, false, false);


            //Load the interaction information
            // DO IT --
            // ADD IT --
            // ETC --

            velocity = Vector2.Zero;
        }

        /// <summary>
        /// UPDATE THE BUTTERFLY CLASS FOR THE WIN
        /// </summary>
        /// <param name="gameTime">The game time object for letting you know how old you've gotten since starting the game.</param>
        /// <param name="gameReference">Game reference of doom</param>
        public override void Update(GameTime gameTime, Pantheon gameReference)
        {
            base.Update(gameTime, gameReference);

            if (!isRoaming)
            {

                this.EquippedItems["weapon"].activate(gameReference, this);
                if (((Weapon)this.EquippedItems["weapon"]).CurrentAmmo != 0)
                {
                    gameReference.audioManager.playSoundEffect(this.EquippedItems["weapon"].soundCueName);
                }
                
                if (((Weapon)this.EquippedItems["weapon"]).CurrentAmmo == 0 && !((Weapon)this.EquippedItems["weapon"]).Reloading)
                {
                    ((Weapon)this.EquippedItems["weapon"]).Reload(gameTime);
                }
            }

            if (currentState.Equals("Die"))
            {

            }
        }

        /// <summary>
        /// DRAW THE BUTTERFULLYMONK
        /// </summary>
        /// <param name="canvas">An initialized SpriteBatch.</param>
        public override void Draw(SpriteBatch canvas)
        {
            base.Draw(canvas);
        }
    }
}

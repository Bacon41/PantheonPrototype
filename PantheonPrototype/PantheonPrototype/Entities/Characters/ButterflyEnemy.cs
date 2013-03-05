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
    class ButterflyEnemy : EnemyCharacter
    {
        /// <summary>
        /// The constructor for the Butterfly enemy, currently just sets up the base class.
        /// </summary>
        /// <param name="location">The initial position of the Butterfly.</param>
        public ButterflyEnemy(Vector2 location)
            : base(location, new Rectangle(0, 0, 40, 40), new Rectangle(15, 25, 10, 10))
        {
            facing = Direction.Left;
            currentState = "Move";
            changeDirection = TimeSpan.FromSeconds(3);

            EquippedItems.Add("weapon", new Weapon());
            ArmedItem = EquippedItems["weapon"];
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
            spriteTex = contentManager.Load<Texture2D>("butterfly");

            this.Sprite.loadSprite(spriteTex, 4, 3, 30);

            this.Sprite.addState("Move Forward", 0, 2);
            this.Sprite.addState("Move Left", 3, 5);
            this.Sprite.addState("Move Right", 6, 8);
            this.Sprite.addState("Move Back", 9, 11);

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

            switch (facing)
            {
                case Direction.Left:
                    velocity = new Vector2(-3, 0);
                    sprite.changeState(currentState + " Left");
                    break;
                case Direction.Right:
                    velocity = new Vector2(3, 0);
                    sprite.changeState(currentState + " Right");
                    break;
                case Direction.forward:
                    velocity = new Vector2(0, 3);
                    sprite.changeState(currentState + " Forward");
                    break;
                case Direction.back:
                    velocity = new Vector2(0, -3);
                    sprite.changeState(currentState + " Back");
                    break;
            }

            changeDirection = changeDirection.Subtract(gameTime.ElapsedGameTime);
            if (changeDirection.CompareTo(TimeSpan.Zero) <= 0)
            {
                switchDirection();
                changeDirection = TimeSpan.FromSeconds(3);
            }

            this.EquippedItems["weapon"].activate(gameReference, this);
            ((Weapon)this.EquippedItems["weapon"]).CurrentAmmo = ((Weapon)this.EquippedItems["weapon"]).TotalAmmo;
        }

        /// <summary>
        /// Randomly switch directions.
        /// </summary>
        private void switchDirection()
        {
            int dir = new Random().Next(4);
            switch (dir)
            {
                case 0:
                    facing = Direction.back;
                    break;
                case 1:
                    facing = Direction.Left;
                    break;
                case 2:
                    facing = Direction.forward;
                    break;
                case 3:
                    facing = Direction.Right;
                    break;
                default:
                    break;
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

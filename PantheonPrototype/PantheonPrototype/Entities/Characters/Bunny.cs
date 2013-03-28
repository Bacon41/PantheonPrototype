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
    /// Super important bunny rabbit for testing porpoises.
    /// </summary>
    class BunnyNPC : EnemyNPC
    {
        public static int counter = 0;

        public BunnyNPC(Vector2 location)
            : base(location, new Rectangle(0, 0, 60,60), new Rectangle (10, 10, 40, 40))
        {
            facing = Direction.Right;
            currentState = "Move";
            changeDirection = TimeSpan.FromSeconds(1);

            currentArmor = 10;
            totalArmor = 10;

            characteristics.Add("Enemy");
        }

        public override void Load(ContentManager contentManager)
        {
            base.Load(contentManager);

            Texture2D image;

            image = contentManager.Load<Texture2D>("Bunnies");

            this.Sprite.loadSprite(image, 12, 12, 30);

            this.Sprite.addState("Move Right", 0, 34, true);
            this.Sprite.addState("Move Left", 35, 69, true);
            this.Sprite.addState("Move Forward", 70, 104, true);
            this.Sprite.addState("Move Back", 105, 139, true);

            velocity = Vector2.Zero;
        }

        public override void Interact()
        {
            base.Interact();
        }

        public override void Update(GameTime gameTime, Pantheon gameReference)
        {
            base.Update(gameTime, gameReference);

            if (currentArmor <= 0)
            {
                currentState = "Die";
                toDestroy = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}

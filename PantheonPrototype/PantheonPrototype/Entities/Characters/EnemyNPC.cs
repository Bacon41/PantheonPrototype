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
    class EnemyNPC : NPCCharacter
    {
        public EnemyNPC(Vector2 location, Rectangle drawBox, Rectangle boundingBox)
            : base(location, drawBox, boundingBox)
        {
            comfortZone = new Rectangle((int)location.X + drawBox.X - 200, (int)location.Y + drawBox.Y - 200, 400, 400);

            characteristics.Add("Enemy");
        }

        public override void Update(GameTime gameTime, Pantheon gameReference)
        {
            base.Update(gameTime, gameReference);
            if (currentArmor <= 0)
            {
                String command = "Die ";
                command += HamburgerHelper.GetDirection(Facing);

                Console.WriteLine(command);
                CurrentState = command;

                if (sprite.isComplete())
                {
                    toDestroy = true;
                }
            }

            if (!isRoaming)
            {
                if (!gameReference.player.CurrentState.Contains("Die"))
                {
                    this.EquippedItems["weapon"].activate(gameReference, this);
                }
            }

            if (((Weapon)this.EquippedItems["weapon"]).CurrentAmmo == 0 && !((Weapon)this.EquippedItems["weapon"]).Reloading)
            {
                ((Weapon)this.EquippedItems["weapon"]).Reload(gameTime);
            }

            if (currentState.Contains("Die"))
            {
                gameReference.audioManager.playSoundEffect("Death");
            }
        }
    }
}

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
    // Currenly Obsolete. Possibly will store Inventory. We'll see.
    class Player : CharacterEntity
    {
        Player() : base(Vector2.Zero, Rectangle.Empty, Rectangle.Empty, "Player of Awesomeness Who Definitely Eats Sushi")
        {
            TotalArmor = 100;
            CurrentArmor = 100;
            
        }
    }
}

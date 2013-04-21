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

namespace Test
{
    public class XMLTest12
    {
        public string InnerName;

        public struct frameRange
        {
            public int beginFrame;
            public int endFrame;
            public bool looping;
            public bool sweeping;

            public string ToString()
            {
                return beginFrame + ", " + endFrame + " " + looping + " " + sweeping;
            }
        }

        public frameRange Range;
    }
}

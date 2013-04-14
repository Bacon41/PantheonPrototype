using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FuncWorks.XNA.XTiled;

namespace Test
{
    public class XMLTest
    {
        public string Name;
        public List<string> List;

        public string MapPath;

        private Map map;

        public Map getMap()
        {
            return map;
        }

        public void Load(ContentManager content)
        {
            // Load the map thingy
            map = content.Load<Map>(MapPath);
        }
    }
}
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

namespace LevelLoad
{
    /// <summary>
    /// Loads a level.
    /// 
    /// Contains all the information necessary to construct a level. Acts as an interface for an
    /// XML document which will fill all the public members of the class.
    /// 
    /// Note that the Load function must be called after the public members are filled so that some
    /// members such as the level map may be loaded from the given paths in the XML file.
    /// </summary>
    public class LevelLoader
    {
    }
}

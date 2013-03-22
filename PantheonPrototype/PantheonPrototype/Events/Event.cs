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
    /// A basic event.
    /// 
    /// Consists of a type and a list of strings. The type allows the event manager to know where
    /// to send the event. The list of strings gives more specific information about the event.
    /// 
    /// Essentially, the type is an identifier of what type of event this is, and thus an
    /// identification of the receivers. The list of strings is essentially a payload for the event.
    /// </summary>
    class Event
    {
    }
}

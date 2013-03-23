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
    /// Distributes events to the correct locations in Pantheon.
    /// 
    /// Classes should register functions with the EventManager to handle specific types of events.
    /// When an event occurs, the type is identified, and all registered functions are called. They
    /// are passed the event, whose payload gives any other relevant information to the recipient.
    /// </summary>
    class EventManager
    {
    }
}

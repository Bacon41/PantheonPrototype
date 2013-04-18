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
    public class Event
    {
        /// <summary>
        /// Added temporary reference to game for the lolz.
        /// </summary>
        public Pantheon gameReference;

        /// <summary>
        /// The type of the event.
        /// </summary>
        private string type;

        public string Type
        {
            get { return type; }
            set { this.type = value; }
        }

        /// <summary>
        /// One of those joyous global reference thingies protected by a healthy degree of encapsulation...
        /// 
        /// Truthfully, this should have bitten us by now... but it hasn't. So much for a bad coding practice.
        /// 
        /// JINX!
        /// </summary>
        public Pantheon GameReference;

        /// <summary>
        /// A list of all the event specific information given in the event.
        /// 
        /// Takes the form of a Dictionary with a string mapping to a string.
        /// </summary>
        public Dictionary<string, string> payload;

        /// <summary>
        /// Empty constructor that makes an empty event.
        /// </summary>
        public Event()
        {
            payload = new Dictionary<string, string>();
        }

        /// <summary>
        /// Takes parameters and fully initializes an event.
        /// </summary>
        /// <param name="type">The type of the event.</param>
        /// <param name="payload">The payload of the event as a Dictionary collection object.</param>
        public Event(string type, Dictionary<string, string> payload)
        {
            this.type = type;
            this.payload = payload;
        }
    }
}

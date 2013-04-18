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
    /// The delegate definition for a function that handles events.
    /// </summary>
    /// <param name="eventInfo">All the information about the event.</param>
    public delegate void HandleEvent(Event eventInfo);

    /// <summary>
    /// Distributes events to the correct locations in Pantheon.
    /// 
    /// Classes should register functions with the EventManager to handle specific types of events.
    /// When an event occurs, the type is identified, and all registered functions are called. They
    /// are passed the event, whose payload gives any other relevant information to the recipient.
    /// </summary>
    public class EventManager
    {
        /// <summary>
        /// Event manager passes the game reference into events as they pass through it.
        /// That way, we can inject Pantheon references where ever we want... somewhat
        /// like the Bunyaviridae virus... except a lot less like a living organism.
        /// </summary>
        public Pantheon GameReference;

        /// <summary>
        /// A dictionary that maps each type of event to a list of delegate functions to call when
        /// that specific type of event occurs.
        /// </summary>
        private Dictionary<string, List<HandleEvent>> eventHandlers;

        public EventManager(Pantheon gameReference)
        {
            eventHandlers = new Dictionary<string, List<HandleEvent>>();

            this.GameReference = gameReference;
        }

        /// <summary>
        /// Registers an event handling function with the EventManager class. When the EventManager
        /// receives an event of the given type, the given handler will be called.
        /// </summary>
        /// <param name="type">A string denoting the event type to trigger the handler.</param>
        /// <param name="handler">The handler function declared in the form: void EventHandler (Event eventInfo);</param>
        public void register(string type, HandleEvent handler)
        {
            if (!eventHandlers.Keys.Contains(type))
            {
                eventHandlers.Add(type, new List<HandleEvent>());
            }

            Console.WriteLine("Handler registered for the \"" + type + "\" event");

            eventHandlers[type].Add(handler);
        }

        /// <summary>
        /// Unregisters the given event handler with the given type of event notification.
        /// </summary>
        /// <param name="type">The type of event from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the notification list.</param>
        public void unregister(string type, HandleEvent handler)
        {
            Console.WriteLine("Handler unregistered for " + type);

            List<HandleEvent> handlerList = eventHandlers[type];

            handlerList.Remove(handler);
        }

        /// <summary>
        /// Notifies the appropriate handlers of an Event.
        /// </summary>
        /// <param name="eventInfo">Information about the event.</param>
        public void notify(Event eventInfo)
        {
            Console.WriteLine("Event occuring of type \"" + eventInfo.Type + "\"");

            // Inject that global reference non-Bunyaviridae-like thingy
            eventInfo.GameReference = this.GameReference;

            try
            {
                // Useless statement
                if (eventHandlers[eventInfo.Type] == null)
                {
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unhandled event was encountered for the \"" + eventInfo.Type + "\" type of event.");
                return;
            }

            foreach (HandleEvent handler in eventHandlers[eventInfo.Type])
            {
                handler(eventInfo);
            }
        }
    }
}

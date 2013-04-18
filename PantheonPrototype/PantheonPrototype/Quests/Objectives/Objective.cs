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
    /// Contains the individual logic for deciding if a condition has been met
    /// based on game events.
    /// </summary>
    public class Objective
    {
        /// <summary>
        /// The id of the objective within the quest.
        /// </summary>
        public int id
        {
            get;
            set;
        }

        /// <summary>
        /// A list of objectives which should activate when this one is finished.
        /// </summary>
        public List<int> nextObjectives;

        /// <summary>
        /// The event type that should trigger the objective.
        /// </summary>
        public string EventType
        {
            get;
            set;
        }

        /// <summary>
        /// A simple flag used for testing completion.
        /// </summary>
        protected enum condition { initialized, active, complete };
        protected condition state;

        /// <summary>
        /// Constructor... yeah
        /// </summary>
        public Objective(int id)
        {
            nextObjectives = new List<int>();

            state = condition.initialized;

            this.id = id;
        }

        /// <summary>
        /// Sets the objective to a starting state. This way, an objective may be
        /// predictable when reused.
        /// 
        /// Notably registers event handlers.
        /// </summary>
        /// <param name="gameReference">A reference to the game so that the event
        /// manager is accessible. Also, other initializing actions may be taken.</param>
        public virtual void Initialize(Pantheon gameReference)
        {
            // Register the objective's handler in the event manager
            HandleEvent eventHandler = this.HandleNotification;
            gameReference.EventManager.register(this.EventType, eventHandler);

            state = condition.active;
        }

        /// <summary>
        /// An event handler meant to be registered with the event manager.
        /// </summary>
        /// <param name="eventinfo">The event data passed to the handler</param>
        public virtual void HandleNotification(Event eventinfo)
        {
            state = condition.complete;
        }

        /// <summary>
        /// Determines if an objective is complete.
        /// </summary>
        /// <returns>Returns true if objective is complete.</returns>
        public virtual bool Complete()
        {
            return state == condition.complete;
        }

        /// <summary>
        /// Called after the objective has been completed. Performs any transition
        /// events before the quest moves to the next objective.
        /// 
        /// Notably cleans up registered event handlers.
        /// </summary>
        public virtual void WrapUp(Pantheon gameReference)
        {
            HandleEvent eventHandler = this.HandleNotification;
            gameReference.EventManager.unregister(this.EventType, eventHandler);
        }

        /// <summary>
        /// Used for time sensitive objectives.
        /// </summary>
        /// <param name="gameTime">Time since the last update cycle.</param>
        public virtual void Update(GameTime gameTime)
        {
        }
    }
}

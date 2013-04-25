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
    /// Contains all the objectives relevant to a given Quest.
    /// Basically goes from objective to objective, updating
    /// itself according to the current objective and the incoming
    /// event.
    /// </summary>
    public class Quest
    {
        /// <summary>
        /// The title. Be creative. I think you'll figure it out.
        /// </summary>
        private string questTitle;

        public string QuestTitle
        {
            get { return questTitle; }
            set { questTitle = value; }
        }

        /// <summary>
        /// The objectives for this quest.
        /// </summary>
        public List<Objective> objectives;

        /// <summary>
        /// A list of current objectives.
        /// </summary>
        private List<Objective> currentObjectives;

        public List<Objective> CurrentObjectives
        {
            get { return currentObjectives; }
        }

        private List<Objective> completedObjectives;

        public List<Objective> CompletedObjectives
        {
            get { return completedObjectives; }
        }

        public Quest()
        {
            objectives = new List<Objective>();
            currentObjectives = new List<Objective>();

            //if (currentObjectives.Contains(objectives[4+2])) Console.WriteLine("DRAGONS");

            // Statically set objectives id to their index in the objective list.
            // This isn't very useful right now, but it's a reminder of how I coded things to work.
            for (int i = 0; i < objectives.Count; i++)
            {
                objectives[i].id = i;
            }

            Console.WriteLine("Creating a quest");
        }

        /// <summary>
        /// Initializes all current objectives and sets the current objective to the
        /// first one given.
        /// </summary>
        /// <param name="gameReference">Game reference... why not.</param>
        public void Initialize(Pantheon gameReference)
        {
            for (int i = 0; i < currentObjectives.Count; i++)
            {
                currentObjectives[i].Initialize(gameReference);
            }

            setCurrentObjective(0);
        }

        /// <summary>
        /// Checks to see if any of the current objectives need to be registered
        /// or deregistered for events.
        /// </summary>
        /// <param name="gameTime">Time since the last update cycle.</param>
        public void Update(GameTime gameTime, Pantheon gameReference)
        {
            // Go backwards through the array so deletions don't break anything
            for (int i = currentObjectives.Count-1; i > -1; i--)
            {
                // If the objective is complete
                if (currentObjectives[i].Complete())
                {
                    // Run the transition trigger
                    currentObjectives[i].WrapUp(gameReference);

                    // Activate the appropriate next objectives
                    if (currentObjectives[i].nextObjectives.Count > 0)
                    {
                        foreach (int objectiveId in currentObjectives[i].nextObjectives)
                        {
                            currentObjectives.Add(objectives[objectiveId]);
                            objectives[objectiveId].Initialize(gameReference);
                        }
                    }

                    // The objective is now complete
                    completedObjectives.Add(currentObjectives[i]);

                    // Remove this objective from the current objectives
                    currentObjectives.RemoveAt(i);
                }

                // If the objective isn't complete
                else
                {
                    // Update the current objective
                    currentObjectives[i].Update(gameTime);
                }
            }
        }

        /// <summary>
        /// Adds the specified index to the current objective list.
        /// 
        /// Note: Assumes that the objective has already been initialized.
        /// </summary>
        /// <param name="index">The index indicating the objective to add from the objective list.</param>
        public void setCurrentObjective(int index)
        {
            currentObjectives.Add(objectives[index]);
        }
    }
}

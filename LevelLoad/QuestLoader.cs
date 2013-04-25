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

namespace LevelLoad
{
    /// <summary>
    /// Contains all characteristics needed to load quests.
    /// </summary>
    public class QuestLoader
    {
        /// BEGIN: XML Parsing Section

        /// <summary>
        /// The title of the quest.
        /// </summary>
        public string QuestTitle;

        /// <summary>
        /// The number of objectives in this quest.
        /// </summary>
        public int NumberOfObjectives;

        /// <summary>
        /// The objectives for the quest.
        /// </summary>
        public List<ObjectiveLoader> Objectives;

        /// <summary>
        /// A list of the initial objectives of the quest.
        /// These are given by listing the ids of each objective.
        /// </summary>
        public List<int> InitialObjectives;

        /// END: XML Parsing Section

        public override string ToString()
        {
            string questString = "";

            questString += NumberOfObjectives + ";";

            questString += InitialObjectives[0];

            for (int i = 1; i < InitialObjectives.Count; i++)
            {
                questString += "," + InitialObjectives[i];
            }

            questString += ";";

            questString += QuestTitle;

            return questString;
        }
    }
}

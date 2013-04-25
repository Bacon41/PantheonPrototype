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
    /// Loads the objective thingies. Creativity ftw.
    /// </summary>
    public class ObjectiveLoader
    {
        /// BEGIN: XML Parsing Section

        /// <summary>
        /// The index/id of the objective in the quest. May be
        /// referenced by other objectives.
        /// </summary>
        public int Id;

        /// <summary>
        /// The title of the objective.
        /// </summary>
        public string ObjectiveTitle;

        /// <summary>
        /// Text explaining exactly what the player is supposed to do.
        /// </summary>
        public string ObjectiveText;

        /// <summary>
        /// Defines the type of objective (Trigger, Speak, Kill).
        /// </summary>
        public string Type;

        /// <summary>
        /// Defines the target for the objective. (e.g. the suspicious
        /// platipus eating your foot)
        /// </summary>
        public string Target;

        /// <summary>
        /// List of objectives to be activated once this objective has been completed.
        /// </summary>
        public List<int> NextObjectives;

        /// END: XML Parsing Section

        public override string ToString()
        {
            string objectiveString = "";

            objectiveString += Id + ";";
            objectiveString += Type + ";";
            objectiveString += Target + ";";

            foreach (int objId in NextObjectives)
            {
                objectiveString += objId + ",";
            }
            objectiveString += ";";
            objectiveString += ObjectiveTitle + ";" + ObjectiveText;

            return objectiveString;
        }
    }
}

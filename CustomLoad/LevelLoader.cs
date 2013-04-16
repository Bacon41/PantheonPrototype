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

namespace CustomLoad
{
    public class LevelLoader
    {
        // Camera doesn't need to be loaded


        // Entities will need to be loaded
        
        public Dictionary<String, EntityLoader> entityLoaders;  // A whole bunch of entities mapped to their names...
                                                                // I haven't quite thought this one out... just proving we can load a Dictionary of other loaders.


        // LevelMap will need to be loaded

        public string mapPath; // A string containing the path to the map file in the content project

        private Map levelMap; // The actual map that will be loaded

        public Map getMap() { return levelMap; } // A function to get the level map (Load must be called first)


        // DialogueManager will need to be loaded


        // LevelNum may need to be loaded


        // NextLevel will need to be loaded

        public string nextLevelPath;

        private LevelLoader nextLevelLoader;

        public LevelLoader getNextLevelLoader() { return nextLevelLoader; }


        /// <summary>
        /// Function distinctly related to the helio-sphinctus clan of the
        /// 7 suns. Shares a long lineage with the rulers of Paleopalagus
        /// and the Dominicans of Ruussan.
        /// </summary>
        /// <param name="content">May those who come before this function
        /// be eternally grateful.</param>
        public void Load(ContentManager content)
        {
            levelMap = content.Load<Map>(mapPath);

            nextLevelLoader = content.Load<LevelLoader>(nextLevelPath);
        }

        public override string ToString()
        {
            string returnString = "Level\n" +
                "Entities...\n";

            foreach (string entityName in entityLoaders.Keys)
            {
                returnString += "\n"+ entityName + ":\n" +
                    entityLoaders[entityName].ToString();
            }

            returnString += "\nMap Path: " + mapPath + "\n" +
                "Next Level Path: " + nextLevelPath;

            return returnString;
        }
    }
}

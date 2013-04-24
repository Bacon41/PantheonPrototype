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
        /// BEGIN: XML Parsing Section

        /// <summary>
        /// Path to the TMX map for this level.
        /// </summary>
        public string MapPath;

        /// <summary>
        /// Contains information to construct the player entity.
        /// </summary>
        public PlayerLoader Player;

        /// <summary>
        /// Contains information to construct all the enemy entities for this level.
        /// 
        /// Of the form <key, value> = <string, EnemyLoader>
        /// </summary>
        public Dictionary<string, EnemyLoader> Enemies;

        /// <summary>
        /// Contains information to construct all the NPC entities for this level.
        /// 
        /// Of the form <key, value> = <string, NPCLoader>
        /// </summary>
        public Dictionary<string, NPCLoader> NPCs;

        /// <summary>
        /// Contains information to construct all dialogue
        /// </summary>
        public DialogueLoader Dialogue;

        /// <summary>
        /// Contains information to construct all quests.
        /// </summary>
        public QuestLoader Quests;

        /// <summary>
        /// Path to the next level XML.
        /// </summary>
        public string NextLevelPath;

        /// END: XML Parsing Section

        /// <summary>
        /// The map member into which the map located at MapPath should be loaded.
        /// </summary>
        private Map levelMap;

        /// <summary>
        /// Loads all dependent content in the Level.
        /// 
        /// Notably the levelMap.
        /// </summary>
        public void Load(ContentManager contentManager)
        {
            levelMap = contentManager.Load<Map>(MapPath);
        }

        /// <summary>
        /// Gets the map member loaded from the MapPath member.
        /// 
        /// Note: the load function should be called before this function. Otherwise the map will
        /// not be inititialized.
        /// </summary>
        /// <returns>A map object.</returns>
        public Map GetLevelMap()
        {
            return levelMap;
        }

        /// <summary>
        /// Gets the next level.
        /// </summary>
        /// <param name="contentManager">Content manager used to load the next level.</param>
        /// <returns>An uninitialized level. Must still call the Load function.</returns>
        public LevelLoader GetNextLevel(ContentManager contentManager)
        {
            return contentManager.Load<LevelLoader>(NextLevelPath);
        }
    }
}

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
    class WinObjective : Objective
    {
        /// <summary>
        /// The id of the quest this objective belongs to
        /// </summary>
        private int questId;

        /// <summary>
        /// Constructs a new win objective that terminates a quest.
        /// </summary>
        /// <param name="id">The id of the objective in the objective list.</param>
        /// <param name="QuestId">The id of the quest to which the objective belongs.</param>
        public WinObjective(int id, int QuestId) : base (id)
        {
            this.questId = QuestId;
        }

        /// <summary>
        /// Causes the quest to be completed.
        /// </summary>
        /// <param name="gameReference"></param>
        public override void Initialize(Pantheon gameReference)
        {
            Dictionary<string,string> payload = new Dictionary<string,string>();
            payload.Add("QuestId", questId + "");
            Event closeQuestEvent = new Event("CloseQuest", payload);
            gameReference.EventManager.notify(closeQuestEvent);
        }
    }
}

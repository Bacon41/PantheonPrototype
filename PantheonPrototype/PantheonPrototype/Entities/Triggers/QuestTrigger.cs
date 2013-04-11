﻿using System;
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
    /// Target for an objective for that one quest that's used for testing but eventually for other quests too.
    /// </summary>
    class QuestTrigger : Trigger
    {
        public QuestTrigger(Rectangle locationBox, Pantheon gameReference, string name)
            : base(locationBox, gameReference, name)
        {
            HandleEvent temp = AlertQuests;
            gameReference.EventManager.register("Trigger", temp);
        }

        public override void Update(GameTime gameTime, Pantheon gameReference)
        {
            base.Update(gameTime, gameReference);
        }

        public void AlertQuests(Event eventInfo)
        {
        }
    }
}

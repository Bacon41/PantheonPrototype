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
    /// Describes the possible actions that the player may accomplish through some input.
    /// </summary>
    public struct ControlActions
    {
        public bool MoveForward;
        public bool MoveBackward;
        public bool MoveLeft;
        public bool MoveRight;

        public bool Attack;
        public bool Shield;

        public bool Pause;

        public Vector2 CursorPosition;
    }

    /// <summary>
    /// This is the class that handles the input. It will keep track of all
    /// possible input devices and will return whether or not the button for
    /// a particular action (whatever button that is) is pushed.
    /// </summary>
    class ControlManager
    {
        /// <summary>
        /// The current active actions from the player input.
        /// </summary>
        public ControlActions actions;

        public ControlManager()
        {
            reset();
        }

        public void Update()
        {
            //Reset the appropriate actions
            reset();

            //Get the current keyboard state
            KeyboardState keyboard = Keyboard.GetState();

            //Get the mouse state
            MouseState mouse = Mouse.GetState();

            //Get the gamepad state
            GamePadState gamepad = GamePad.GetState(PlayerIndex.One);

            if(keyboard.IsKeyDown(Keys.Up)) { actions.MoveForward = true; }
            if (keyboard.IsKeyDown(Keys.Down)) { actions.MoveBackward = true; }
            if (keyboard.IsKeyDown(Keys.Left)) { actions.MoveLeft = true; }
            if (keyboard.IsKeyDown(Keys.Right)) { actions.MoveRight = true; }
            if (keyboard.IsKeyDown(Keys.Escape) && !actions.Pause) { actions.Pause = true; }
            if (keyboard.IsKeyDown(Keys.Space) && !actions.Shield) { actions.Shield = true; }
            if (mouse.LeftButton == ButtonState.Pressed) { actions.Attack = true; }
            if (gamepad.Buttons.Back == ButtonState.Pressed) { actions.Pause = true; }

            actions.CursorPosition = new Vector2(mouse.X, mouse.Y);
        }

        /// <summary>
        /// Resets all the thingies.
        /// </summary>
        private void reset()
        {
            actions.MoveForward = false;
            actions.MoveBackward = false;
            actions.MoveLeft = false;
            actions.MoveRight = false;

            actions.Attack = false;
            actions.Shield = false;

            actions.Pause = false;

            actions.CursorPosition = new Vector2(0, 0);
        }
    }
}

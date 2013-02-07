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
        public bool Zoom;
        public bool Interact;

        public Vector2 CursorPosition;
    }

    /// <summary>
    /// This struct is for the gamepad's controls, which will be set to a default button pattern. 
    /// If we need to check buttons pressed, then it will be easier to access these than to type the full path
    /// If the user wishes to edit the controls, then we update the struct's Button assignment
    /// </summary>
    public struct GamePadControls
    {       
        public ButtonState MeleeAttack;
        public ButtonState Shield;
        public ButtonState Zoom;
        public ButtonState Interact;
        public ButtonState Pause;
        public ButtonState Magic; //might not be a separate button
        public Vector2 Movement;
        //whatever the right thumbstick would be
        public Vector2  ZoomControl;

        //trigggers are floats
        public float Shoot;
        public float Aim;

        //add more when remember what we need

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
        public GamePadControls gamepadControls;

        public ControlManager()
        {
            setDefaultControlScheme();
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
            mouseTest(mouse);
            
            //Get the gamepad state
            GamePadState gamepad = GamePad.GetState(PlayerIndex.One);
            if (gamepad.IsConnected)
            {
                if (gamepadControls.Pause == ButtonState.Pressed) { actions.Pause = true; }
                
            }
            //Set action true if keyboard button is pressed
            if (keyboard.IsKeyDown(Keys.W)) { actions.MoveForward = true; }
            if (keyboard.IsKeyDown(Keys.S)) { actions.MoveBackward = true; }
            if (keyboard.IsKeyDown(Keys.A)) { actions.MoveLeft = true; }
            if (keyboard.IsKeyDown(Keys.D)) { actions.MoveRight = true; }
            if (keyboard.IsKeyDown(Keys.Escape) && !actions.Pause) { actions.Pause = true; }
            if (keyboard.IsKeyDown(Keys.Space) && !actions.Shield) { actions.Shield = true; }

            //
            if (mouse.LeftButton == ButtonState.Pressed) { actions.Attack = true; }

            actions.CursorPosition = new Vector2(mouse.X, mouse.Y);
            Console.WriteLine(actions.CursorPosition);
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


        /// <summary>
        /// Set the default button scheme for the XBOX360 gamepad controller
        /// </summary>
        private void setDefaultControlScheme()
        {
            //TODO: Edit button scheme if buttons need to be changed
            GamePadState gameDefault = GamePad.GetState(PlayerIndex.One);
            gamepadControls.MeleeAttack = gameDefault.Buttons.X;
            gamepadControls.Pause = gameDefault.Buttons.Start;
            gamepadControls.Movement = gameDefault.ThumbSticks.Left;
            gamepadControls.Interact = gameDefault.Buttons.A;
            gamepadControls.Shoot = gameDefault.Triggers.Right;
            gamepadControls.Aim = gameDefault.Triggers.Left;
            gamepadControls.Shield = gameDefault.Buttons.Y;
            gamepadControls.Magic = gameDefault.Buttons.B;
            gamepadControls.ZoomControl = gameDefault.ThumbSticks.Right;
        }

        /// <summary>
        /// This function will change the Control binding of a single button to
        /// another button on the gamepad.
        /// </summary>
        /// <param name="oldbutton">The action that corresponds to a button that will be changed</param>
        /// <param name="newButton">The button on the gamepad that the action will now be set to</param>
        /// <param name="gamepad">The gamepadState that is already in place</param>
        public void changeControlBindings(ButtonState oldButton, GamePadButtons newButton, GamePadState gamepad)
        {
            //TODO:
            
        }

        /// <summary>
        /// This method was written to test the input information being received from the mouse, 
        /// rather than testing within the code.  
        /// </summary>
        /// <param name="mouse">MouseState from user's mouse</param>
        public void mouseTest(MouseState mouse)
        {
            //test mouse location by outputing to console the location
            
        }
   
    }
}

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
        public bool isControlEnabled;
        public bool isMouseClickEnabled;
        public bool isMotionEnabled;
        public bool MoveForward;
        public bool MoveBackward;
        public bool MoveLeft;
        public bool MoveRight;
        public bool beingDamaged;

        public bool Attack;
        public bool Shield;
        public bool Aim;

        public bool Pause;
        public bool Zoom;
        public bool Interact;
        public bool Reload;

        public bool MenuSelect;

        public bool CursorEnabled;

        public Vector2 CursorPosition;
        public Vector2 CursorDirection;
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
    /// 
    /// </summary>
    public struct MouseAndKeyboardControls
    {
        public Keys MoveUpKey;
        public Keys MoveDownKey;
        public Keys MoveLeftKey;
        public Keys MoveRightKey;

        public Keys MeleeAttackKey;
        public Keys PauseKey;
        public Keys ShieldKey;
        public Keys InteractKey;
        public Keys ReloadKey;

        public ButtonState AttackMouseButton;
        public Keys WeaponSwapButton;
        public MouseState ZoomButton;
        //public Keys ZoomButton;
        //public Keys AttackButton;
        public Keys AimKey;

        public ButtonState MenuSelectKey;
        public ButtonState PrevMenuSelectKey;

        public Keys TakeDamage;
    }

    /// <summary>
    /// This is the class that handles the input. It will keep track of all
    /// possible input devices and will return whether or not the button for
    /// a particular action (whatever button that is) is pushed.
    /// </summary>
    public class ControlManager
    {
        /// <summary>
        /// The current active actions from the player input.
        /// </summary>
        public ControlActions actions;
        public GamePadControls gamepadControls;
        public MouseAndKeyboardControls keyboardAndMouse;

        private KeyboardState keyboard;
        private KeyboardState oldKeyboard;

        private MouseState mouse;
        private MouseState oldMouse;

        public ControlManager()
        {
            actions.Shield = false;
            actions.isControlEnabled = true;
            actions.isMotionEnabled = true;

            keyboard = Keyboard.GetState();
            oldKeyboard = keyboard;

            mouse = Mouse.GetState();
            oldMouse = mouse;

            setDefaultGamepadControlScheme();
            setDefaultMouseAndKeyboardControlScheme();            
            reset();
        }

        public void Update()
        {
            //Reset the appropriate actions
            reset();

            //Get the current keyboard state
            keyboard = Keyboard.GetState();           
            
            //Get the mouse state
            mouse = Mouse.GetState();
            updateMouse(mouse);
            
            //Get the gamepad state
            GamePadState gamepad = GamePad.GetState(PlayerIndex.One);
            if (actions.isControlEnabled == true)
            {
                if (gamepad.IsConnected)
                {
                    //No longer using a mouse
                    actions.CursorEnabled = false;

                    if (gamepadControls.Pause == ButtonState.Pressed) { actions.Pause = true; }
                }
                else
                {
                    //We are using a mouse
                    actions.CursorEnabled = true;
                }
                //Set action true if keyboard button is pressed
                if (actions.isMotionEnabled)
                {
                    if (keyboard.IsKeyDown(keyboardAndMouse.MoveUpKey) || keyboard.IsKeyDown(Keys.Down)) { actions.MoveForward = true; }
                    if (keyboard.IsKeyDown(keyboardAndMouse.MoveDownKey) || keyboard.IsKeyDown(Keys.Up)) { actions.MoveBackward = true; }
                    if (keyboard.IsKeyDown(keyboardAndMouse.MoveLeftKey) || keyboard.IsKeyDown(Keys.Left)) { actions.MoveLeft = true; }
                    if (keyboard.IsKeyDown(keyboardAndMouse.MoveRightKey) || keyboard.IsKeyDown(Keys.Right)) { actions.MoveRight = true; }
                }

                if (keyboard.IsKeyDown(keyboardAndMouse.InteractKey) && !oldKeyboard.IsKeyDown(keyboardAndMouse.InteractKey)) { actions.Interact = true; }
                if (keyboard.IsKeyDown(keyboardAndMouse.ShieldKey) && !oldKeyboard.IsKeyDown(keyboardAndMouse.ShieldKey)) { actions.Shield = !actions.Shield; }
                if (keyboard.IsKeyDown(keyboardAndMouse.ReloadKey)) { actions.Reload = true; }
                if (keyboard.IsKeyDown(keyboardAndMouse.TakeDamage)) { actions.beingDamaged = true; }
                //if (mouse.LeftButton == ButtonState.Pressed) { actions.Attack = true; }
                if (keyboard.IsKeyDown(keyboardAndMouse.AimKey)) { actions.Aim = true; }

                if (!actions.Pause)
                {
                    if (keyboardAndMouse.AttackMouseButton == ButtonState.Pressed) { actions.Attack = true; }
                }
            }
            if (keyboard.IsKeyDown(keyboardAndMouse.PauseKey) && !oldKeyboard.IsKeyDown(keyboardAndMouse.PauseKey))
            { actions.Pause = !actions.Pause; }

            if (actions.Pause && actions.isMouseClickEnabled)
            {
                if (keyboardAndMouse.MenuSelectKey == ButtonState.Pressed && keyboardAndMouse.PrevMenuSelectKey != ButtonState.Pressed)
                { actions.MenuSelect = true; }
                else { actions.MenuSelect = false; }
            }

            actions.CursorPosition = new Vector2(mouse.X, mouse.Y);

            oldKeyboard = keyboard;
            oldMouse = mouse;
        }

        /// <summary>
        /// Resets all the thingies.
        /// </summary>
        private void reset()
        {
            actions.beingDamaged = false;
            actions.MoveForward = false;
            actions.MoveBackward = false;
            actions.MoveLeft = false;
            actions.MoveRight = false;
            actions.Reload = false;

            actions.Shield = false;
            actions.Aim = false;
            actions.Attack = false;

            actions.CursorPosition = Vector2.Zero;
        }

        /// <summary>
        /// Allows you to disable the controlls including or exluding the mouse click. True will allow the mouse click, false will disable the mouse click.
        /// </summary>
        /// <param name="mouse">Whether or not the mouse click will be allowed</param>
        public void disableControls(bool mouse)
        {
            if (!mouse)
            {
                actions.isMouseClickEnabled = false;
            }
            else
            {
                actions.isMouseClickEnabled = true;
            }
            actions.isControlEnabled = false;
        }

        public void enableControls()
        {
            actions.isControlEnabled = true;
            actions.isMouseClickEnabled = true;
        }

        public void disableMotion()
        {
            actions.isMotionEnabled = false;
        }

        public void enableMotion()
        {
            actions.isMotionEnabled = true;
        }


        /// <summary>
        /// Set the default button scheme for the XBOX360 gamepad controller
        /// </summary>
        private void setDefaultGamepadControlScheme()
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

        private void setDefaultMouseAndKeyboardControlScheme()
        {
            keyboardAndMouse.MoveUpKey = Keys.S;
            keyboardAndMouse.MoveDownKey = Keys.W;
            keyboardAndMouse.MoveLeftKey = Keys.A;
            keyboardAndMouse.MoveRightKey = Keys.D;

            keyboardAndMouse.PauseKey = Keys.Escape;
            keyboardAndMouse.ShieldKey = Keys.Space;
            keyboardAndMouse.InteractKey = Keys.E;
            keyboardAndMouse.ReloadKey = Keys.R;

            keyboardAndMouse.AimKey = Keys.LeftShift;

            //temporary: REMOVE THIS ONCE COMBAT WORKS
            keyboardAndMouse.TakeDamage = Keys.Tab;
        }


        /// <summary>
        /// This function will change the Control binding of a single button to
        /// another button on the gamepad.
        /// </summary>
        /// <param name="oldbutton">The action that corresponds to a button that will be changed</param>
        /// <param name="newButton">The button on the gamepad that the action will now be set to</param>
        /// <param name="gamepad">The gamepadState that is already in place</param>
        public void changeGamepadBindings(ButtonState oldButton, GamePadButtons newButton, GamePadState gamepad)
        {
            //TODO:
            
        }

        public void changeKeyControlBindings(Keys oldKey, Keys newKey, KeyboardState keyboard)
        {
            oldKey = newKey;
        }

        /// <summary>
        /// This method was written to test the input information being received from the mouse, 
        /// rather than testing within the code.  
        /// </summary>
        /// <param name="mouse">MouseState from user's mouse</param>
        public void updateMouse(MouseState mouse)
        {
            keyboardAndMouse.AttackMouseButton = mouse.LeftButton;
            keyboardAndMouse.PrevMenuSelectKey = keyboardAndMouse.MenuSelectKey;
            keyboardAndMouse.MenuSelectKey = mouse.LeftButton;
        }
   
    }
}

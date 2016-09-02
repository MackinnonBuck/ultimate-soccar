using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Core
{
    public class Input
    {
        /// <summary>
        /// Used for passing mouse input information to an event.
        /// </summary>
        public class MouseInputEventArgs : EventArgs
        {
            public MouseButtons Button { get; private set; }

            public MouseInputEventArgs(MouseButtons button)
            {
                Button = button;
            }
        }

        /// <summary>
        /// used for passing keyboard input information to an event.
        /// </summary>
        public class KeyboardInputEventArgs : EventArgs
        {
            public Keys Key { get; private set; }

            public KeyboardInputEventArgs(Keys key)
            {
                Key = key;
            }
        }

        /// <summary>
        /// Represents the various buttons on a mouse.
        /// </summary>
        public enum MouseButtons
        {
            LEFT = 0,
            MIDDLE = 1,
            RIGHT = 2
        }

        static Input _instance;

        /// <summary>
        /// The global Input instance.
        /// </summary>
        public static Input Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Input();

                return _instance;
            }
        }

        /// <summary>
        /// The position of the mouse.
        /// </summary>
        public Vector2 MousePosition { get; private set; }

        /// <summary>
        /// The speed of the mouse, in pixels per update.
        /// </summary>
        public Vector2 MouseSpeed { get; private set; }

        /// <summary>
        /// The position of the mouse relative to the scene.
        /// </summary>
        public Vector2 SceneMousePosition
        {
            get
            {
                return Vector2.Transform(MousePosition, Matrix.Invert(App.Instance.Scene.Camera.ViewMatrix));
            }
        }

        /// <summary>
        /// The position of the wheel, relative to when the game started.
        /// </summary>
        public int WheelPosition { get; private set; }

        /// <summary>
        /// The speed of the mouse wheel, in ticks per update.
        /// </summary>
        public int WheelSpeed { get; private set; }

        bool[] pressedMouseButtons;
        Keys[] pressedKeys;

        /// <summary>
        /// Called when a mouse button is first pressed.
        /// </summary>
        public event EventHandler<MouseInputEventArgs> OnMousePressed;

        /// <summary>
        /// Called while a mouse button is held.
        /// </summary>
        public event EventHandler<MouseInputEventArgs> OnMouseDown;

        /// <summary>
        /// Called when a mouse button is released.
        /// </summary>
        public event EventHandler<MouseInputEventArgs> OnMouseReleased;

        /// <summary>
        /// Called when a key is first pressed.
        /// </summary>
        public event EventHandler<KeyboardInputEventArgs> OnKeyPressed;

        /// <summary>
        /// Called when a key is held.
        /// </summary>
        public event EventHandler<KeyboardInputEventArgs> OnKeyDown;

        /// <summary>
        /// Called when a key is released.
        /// </summary>
        public event EventHandler<KeyboardInputEventArgs> OnKeyReleased;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private Input()
        {
            MousePosition = Mouse.GetState().Position.ToVector2();
            MouseSpeed = Vector2.Zero;

            WheelPosition = Mouse.GetState().ScrollWheelValue;
            WheelSpeed = 0;

            pressedMouseButtons = new bool[3];
            pressedKeys = new Keys[0];
        }

        /// <summary>
        /// Updates all input information.
        /// </summary>
        internal void Refresh()
        {
            MouseState currentMouseState = Mouse.GetState();
            KeyboardState currentKeyboardState = Keyboard.GetState();

            Vector2 currentMousePosition = Mouse.GetState().Position.ToVector2();
            MouseSpeed = currentMousePosition - MousePosition;
            MousePosition = currentMousePosition;

            bool[] currentMouseButtons = {
                currentMouseState.LeftButton == ButtonState.Pressed,
                currentMouseState.MiddleButton == ButtonState.Pressed,
                currentMouseState.RightButton == ButtonState.Pressed
            };
            
            int currentWheelPosition = Mouse.GetState().ScrollWheelValue;
            WheelSpeed = currentWheelPosition - WheelPosition;
            WheelPosition = currentWheelPosition;

            for (int i = 0; i < currentMouseButtons.Length; i++)
            {
                MouseInputEventArgs eventArgs = new MouseInputEventArgs((MouseButtons)i);

                if (currentMouseButtons[i])
                {
                    OnMouseDown?.Invoke(this, eventArgs);

                    if (!pressedMouseButtons[i])
                        OnMousePressed?.Invoke(this, eventArgs);
                }
                else
                {
                    if (pressedMouseButtons[i])
                        OnMouseReleased?.Invoke(this, eventArgs);
                }
            }

            pressedMouseButtons = currentMouseButtons;

            Keys[] currentKeys = currentKeyboardState.GetPressedKeys();

            foreach (Keys key in currentKeys)
            {
                KeyboardInputEventArgs eventArgs = new KeyboardInputEventArgs(key);

                OnKeyDown?.Invoke(this, eventArgs);

                if (!pressedKeys.Contains(key))
                    OnKeyPressed?.Invoke(this, eventArgs);
            }

            foreach (Keys key in pressedKeys)
            {
                if (!currentKeys.Contains(key))
                    OnKeyReleased?.Invoke(this, new KeyboardInputEventArgs(key));
            }

            pressedKeys = currentKeys;
        }

        /// <summary>
        /// Used for determining if the given mouse button is down.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool IsMouseButtonDown(MouseButtons button)
        {
            return pressedMouseButtons[(int)button];
        }

        /// <summary>
        /// Used for determining if the given key is down.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyDown(Keys key)
        {
            return pressedKeys.Contains(key);
        }
    }
}

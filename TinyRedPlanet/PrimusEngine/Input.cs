using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TinyRedPlanet.PrimusEngine
{
    class Input
    {
        public static MouseState _mouse;
        public static MouseState _prevMouse;
        public static KeyboardState _keys;
        public static KeyboardState _prevKeys;

        public static void Update()
        {
            _prevMouse = _mouse;
            _mouse = Mouse.GetState();

            _prevKeys = _keys;
            _keys = Keyboard.GetState();
            if (_prevKeys == null)
                _prevKeys = _keys;
            if (_prevMouse == null)
                _prevMouse = _mouse;
        }

        public static bool newKeyPress(Keys key)
        {
            return _keys.IsKeyDown(key) && _prevKeys.IsKeyUp(key);
        }

        public static bool isKeyDown(Keys key)
        {
            return _keys.IsKeyDown(key);               
        }

        public static bool isMouseLeftDown()
        {
            return _mouse.LeftButton == ButtonState.Pressed;
        }

        public static bool isMouseLeftNewPress()
        {
            return (_mouse.LeftButton == ButtonState.Pressed && _prevMouse.LeftButton == ButtonState.Released);
        }

        public static bool isMouseLeftNewRelease()
        {
            return (_mouse.LeftButton == ButtonState.Released && _prevMouse.LeftButton == ButtonState.Pressed);
        }

        public static Point GetMousePos()
        {
            return _mouse.Position;
        }

        public static Point GetMouseMove()
        {
            return _mouse.Position - _prevMouse.Position;
        }
    }
}

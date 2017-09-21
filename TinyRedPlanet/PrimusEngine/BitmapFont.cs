using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TinyRedPlanet.PrimusEngine
{
    class BitmapFont
    {
        public static BitmapFont defaultFont;

        private Texture2D _texture;
        private int _startingChar;
        private int tilewidth;
        private int tileheight;
        private Rectangle[] _rects;

        public  BitmapFont(Texture2D texture, int startingChar, int width, int height)
        {
            _texture = texture;
            _startingChar = startingChar;
            _rects = new Rectangle[width * height];
            tilewidth = _texture.Width / width;
            tileheight = texture.Height / height;
            for(int y=0; y<height; y++)
            {
                for(int x=0; x<width; x++)
                {
                    _rects[(y * width) + x] = new Rectangle(x * tilewidth, y * tileheight, tilewidth, tileheight);
                }
            }
        }

        public void Draw(String s, SpriteBatch sb, Vector2 location, Color colour)
        {
            int charToDraw = -1;
            foreach (char c in s)
            {
                charToDraw = c - _startingChar;
                if (charToDraw < 0 || charToDraw >= _rects.Length)
                    charToDraw = -1;
                if (charToDraw != -1)
                {
                    sb.Draw(_texture, location, _rects[charToDraw], colour);
                }
                location.X += tilewidth;
            }
           // sb.Draw(_texture, location, _source, _colour, 0f, _origin, 1.0f, effects, 1.0f);
        }
    }
}

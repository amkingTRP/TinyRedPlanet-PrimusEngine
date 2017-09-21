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
    class Tileset
    {
        private Texture2D _texture;
        private Rectangle[] _rectangles;

        private int _tileWidth, _tileHeight;

        public Tileset(string textureName, int width, int height)
        {
            Texture2D texture = GraphicsLoader.GetTextureOrLoad(textureName);
            Create(texture, width, height);
        }

        public Tileset(string textureName, int width, int height, Rectangle[] rects)
        {
            Texture2D texture = GraphicsLoader.GetTextureOrLoad(textureName);
            _texture = texture;
            _tileWidth = texture.Width / width;
            _tileHeight = texture.Height / height;

            _rectangles = rects;
        }

        public int GetTileWidth()
        {
            return _tileWidth;
        }

        public int GetTileHeight()
        {
            return _tileHeight;
        }

        public void Create(Texture2D texture, int width, int height)
        {
            _texture = texture;
            _tileWidth = texture.Width / width;
            _tileHeight = texture.Height / height;

            _rectangles = new Rectangle[width * height];
            int counter=0;
            for(int y=0; y< height; y++)
            {
                for(int x=0; x<width; x++)
                {
                    _rectangles[counter] = new Rectangle(x * _tileWidth, y * _tileHeight, _tileWidth, _tileHeight);
                    counter++;
                }
            }
        }
        public void Draw(SpriteBatch sb, Vector2 position, Color colour)
        {
            sb.Draw(_texture, position, colour);
        }

        public void Draw(SpriteBatch sb, Vector2 position, int image, Color colour)
        {
            sb.Draw(_texture, position, _rectangles[image], colour);
        }

        public static Rectangle[] GetBaseRectangles(int textureWidth, int textureHeight, int width, int height)
        {
            int _tileWidth = textureWidth / width;
            int _tileHeight = textureHeight / height;

            Rectangle[] _rectangles = new Rectangle[width * height];
            int counter = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    _rectangles[counter] = new Rectangle(x * _tileWidth, y * _tileHeight, _tileWidth, _tileHeight);
                    counter++;
                }
            }

            return _rectangles;

        }
    }
}

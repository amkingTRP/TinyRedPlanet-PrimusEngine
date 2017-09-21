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
    class Sprite
    {
        private Texture2D _texture;

        private Rectangle _source;
        private Vector2 _origin;
        private Color _colour;

        public Sprite(Texture2D txtr, Rectangle src, Vector2 htspt, Color col)
        {
            _texture = txtr;
            _source = src;
            _origin = htspt;
            _colour = col;
        }

        public void Draw(SpriteBatch sb, Vector2 location, SpriteEffects effects)
        {
            sb.Draw(_texture, location, _source, _colour,0f,_origin, 1.0f, effects, 1.0f);
        }
        public void Draw(SpriteBatch sb, Vector2 location, Color newCol)
        {
            sb.Draw(_texture, location, _source, newCol, 0f, _origin, 1.0f, SpriteEffects.None, 1.0f);
        }
        public void Draw(SpriteBatch sb, Rectangle destination, SpriteEffects effects)
        {
            sb.Draw(_texture, destination, _source, _colour);
        }
        public void Draw(SpriteBatch sb, Rectangle destination, Color newCol)
        {
            sb.Draw(_texture, destination, _source, newCol);
        }
    }

}

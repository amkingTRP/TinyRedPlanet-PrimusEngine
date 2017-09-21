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
    class AnimationCel
    {
        private Sprite _sprite;
        private int _milliseconds;

        public AnimationCel(Sprite spr, int millis)
        {
            _sprite = spr;
            _milliseconds = millis;
        }

        public Sprite GetSprite()
        {
            return _sprite;
        }

        public int GetMillis()
        {
            return _milliseconds;
        }
    }

    class AnimationPlayer
    {
        private int _currentFrame;
        private int _prevFrame;
        private int _millisecondsSinceStart;
        private Animation _curAnim;

        public AnimationPlayer()
        {

        }

        public void SetAnimation(Animation anim)
        {
            _curAnim = anim;
            _currentFrame = _prevFrame = 0;
            _millisecondsSinceStart = 0;
        }

        public void Update(GameTime time)
        {
            _millisecondsSinceStart += time.ElapsedGameTime.Milliseconds;
            _currentFrame = _curAnim.GetCurrentFrame(_millisecondsSinceStart, _currentFrame);
            if(_currentFrame!=_prevFrame)
            {
                _millisecondsSinceStart = 0;
                _prevFrame = _currentFrame;
            }
        }

        public void Draw(SpriteBatch sb, Vector2 position, SpriteEffects effects)
        {
            _curAnim.GetSprite(_currentFrame).Draw(sb, position, effects);
        }
    }

    class Animation
    {
        private AnimationCel[] _frames;

        public Animation(AnimationCel[] frames) // dependency injection
        {
            _frames = frames;
        }

        public int GetCurrentFrame(int milliseconds, int curFrame)
        {
            if(milliseconds>=_frames[curFrame].GetMillis())
            {
                curFrame = (++curFrame) % _frames.Length;
            }

            return curFrame;
        }

        public Sprite GetSprite(int curFrame)
        {
            return _frames[curFrame].GetSprite();
        }
    }
}

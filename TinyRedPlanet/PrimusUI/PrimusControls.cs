using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TinyRedPlanet.PrimusEngine;

namespace TinyRedPlanet.PrimusUI
{
    delegate void PrimusInvokeDelegate(PrimusUIControl c, PrimusUIEventArgs e);

    class PrimusUIControl : PrimusUIComponent
    {
        public event PrimusInvokeDelegate Trigger;
        protected int _ID;
        //        PrimusInvokeDelegate _eventHandler;

        public virtual void Invoke()
        {
            TriggerEvent();
        }

        protected virtual void TriggerEvent()
        {
            if (Trigger != null)
                Trigger(this, null);

        }

        protected virtual void TriggerEvent(PrimusUIEventArgs a)
        {
            if (Trigger != null)
                Trigger(this, a);
        }
    }

    class PrimusUIPanel : PrimusUIControl
    {
        protected Sprite _background;

        public PrimusUIPanel(Point location, Point size, Color col, Sprite background)
        {
            _localRectangle = new Rectangle(location, size);
            _background = background;
            colour = col;
        }

        public Color colour
        {
            get; set;
        }

        public override void Draw(SpriteBatch sb)
        {
            _background.Draw(sb, _globalRectangle, colour);

            base.Draw(sb);
        }

    }

    class PrimusUILabel : PrimusUIPanel
    {
        public String label
        {
            get; set;
        }

        public PrimusUILabel(Point location, Point size, Color c, Sprite background, String l) : base(location, size, c, background)
        {
            label = l;
        }
        public override void Draw(SpriteBatch sb)
        {
            _background.Draw(sb, _globalRectangle, colour);
            if (BitmapFont.defaultFont != null)
            {
                BitmapFont.defaultFont.Draw(label, sb, new Vector2(_globalRectangle.Left, _globalRectangle.Top), Color.White);
            }
            //base.Draw(sb); // can't have children?
        }
    }

    class PrimusUIEventArgs : EventArgs
    {
        public int ID { get; set; }
        public int UpdateType { get; set; }

        public PrimusUIEventArgs(int id)
        {
            ID = id;
        }
    }

    class PrimusUIButton : PrimusUIControl
    {
        protected Sprite _background;

        public PrimusUIButton(Point location, Point size, Color col, int ID, Sprite background)
        {
            _localRectangle = new Rectangle(location, size);
            _background = background;
            colour = col;
            _ID = ID;
        }

        public Color colour
        {
            get; set;
        }

        public override void Update()
        {
            foreach (PrimusUIEvent evt in _eventQueue)
            {
                if (evt.Type == 2) // 2 = mouse release
                {
                    Invoke();
                }
            }
            base.Update();
        }

        public override void Invoke()
        {
            PrimusUIEventArgs e = new PrimusUIEventArgs(_ID);
            e.UpdateType = 1; // 1= button press?
            TriggerEvent(e);
        }

        public override void Draw(SpriteBatch sb)
        {
            _background.Draw(sb, _globalRectangle, colour);

            base.Draw(sb);
        }

    }

    class PrimusUICheckBox : PrimusUIButton
    {
        public bool Checked { get; set; }
        Sprite _checkedSprite;

        public PrimusUICheckBox(Point location, Point size, Color col, int ID, Sprite uncheckedSprite, Sprite checkedSprite) : base(location, size, col, ID, uncheckedSprite)
        {
            _checkedSprite = checkedSprite;
        }

        public override void Invoke()
        {
            Checked = !Checked;
             PrimusUIEventArgs e = new PrimusUIEventArgs(_ID);
            e.UpdateType = 1; // 1= button press?
            TriggerEvent(e);
       }

        public override void Draw(SpriteBatch sb)
        {
            if(Checked)
                _checkedSprite.Draw(sb, _globalRectangle, colour);
            else 
                base.Draw(sb);
        }
    }
}
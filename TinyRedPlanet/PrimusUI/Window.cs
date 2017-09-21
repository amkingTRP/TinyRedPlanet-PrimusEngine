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
    class PrimusUIEvent
    {
        public int Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public PrimusUIEvent(int type)
        {
            Type = type;
        }
    }

    class PrimusUIComponent
    {
        protected Point _offset;
        protected PrimusUIComponent _parent;
        protected List<PrimusUIComponent> _children;
        protected Queue<PrimusUIEvent> _eventQueue;

        protected Rectangle _localRectangle;
        protected Rectangle _globalRectangle;

        protected bool _selectable;

        public PrimusUIComponent()
        {
            _children = new List<PrimusUIComponent>();
            _eventQueue = new Queue<PrimusUIEvent>();
        }

        public void AddEvent(PrimusUIEvent evt)
        {
            _eventQueue.Enqueue(evt);
        }

        public void SetLocalRectangle(Rectangle rect)
        {
            _localRectangle = rect;
        }

        public void SetSelectable(bool yesno)
        {
            _selectable = yesno;
        }

        public void UpdateGlobalRectangle()
        {
            _globalRectangle = new Rectangle(_localRectangle.Location, _localRectangle.Size);
            _globalRectangle.Offset(_offset);
            foreach (PrimusUIComponent child in _children)
            {
                child.UpdateGlobalRectangle();
            }
        }

        public virtual void AddChild(PrimusUIComponent child)
        {
            _children.Add(child);
            child.SetOffset(_offset);
            child.SetParent(this);
            child.UpdateGlobalRectangle();
        }

        public virtual void SetParent(PrimusUIComponent parent)
        {
            _parent = parent;
        }

        public virtual void SetOffset(Point offset)
        {
            _offset = offset;
            foreach (PrimusUIComponent child in _children)
            {
                child.SetOffset(offset);
            }
        }

        public virtual void UpdateOffset(int x, int y)
        {
            if(_offset!= null)
            {
                _offset.X = x;
                _offset.Y = y;
            }
            foreach(PrimusUIComponent child in _children)
            {
                child.UpdateOffset(x, y);
            }
        }

        public virtual void Draw(SpriteBatch sb)
        {
            foreach(PrimusUIComponent child in _children)
            {
                child.Draw(sb);
            }
        }

        public virtual void Update()
        {
            foreach(PrimusUIComponent child in _children)
            {
                child.Update();
            }

            _eventQueue.Clear(); // nothing to process here
        }

        
        public virtual Point GetOffset()
        {
            return _offset;
        }

        public Point LocalCoord(Point p)
        {
            Point temp = new Point();
            temp.X = p.X - _globalRectangle.Left;
            temp.Y = p.Y - _globalRectangle.Top;

            return temp;
        }

        public bool InRectGlobal(Point p)
        {
            return _globalRectangle.Contains(p);
        }

        public bool InRectLocal(Point p)
        {
            return _localRectangle.Contains(p);
        }

        public PrimusUIComponent GetFromGlobalPoint(Point p)
        {
            if(InRectGlobal(p))
            {
                foreach(PrimusUIComponent child in _children)
                {
                    PrimusUIComponent cmpnnt = child.GetFromGlobalPoint(p);
                    if(cmpnnt!=null)
                    {
                        return cmpnnt;
                    }
                }
                //if (_selectable)
                    return this;
            }

            return null;
        }
        public PrimusUIComponent GetFromLocalPoint(Point p)
        {
            if (InRectLocal(p))
            {
                foreach (PrimusUIComponent child in _children)
                {
                    PrimusUIComponent cmpnnt = child.GetFromLocalPoint(p);
                    if (cmpnnt != null)
                    {
                        return cmpnnt;
                    }
                }
                if (_selectable)
                    return this;
            }

            return null;
        }
    }

    class TitleBar:PrimusUIComponent
    {
        private string _title;
        private Sprite _background;

        public TitleBar(Point location, Point size, string title, Sprite background)
        {
            _title = title;
            _localRectangle = new Rectangle(location, size);
            _background = background;
        }

        public override void Draw(SpriteBatch sb)
        {
            _background.Draw(sb, _globalRectangle, Color.LightGray);
            if (BitmapFont.defaultFont != null)
            {
                BitmapFont.defaultFont.Draw(_title, sb, new Vector2(_globalRectangle.Left+4, _globalRectangle.Top+1), Color.White);
            }

            base.Draw(sb);
        }
    }

    class DisplayArea:PrimusUIComponent
    {
        Sprite _background;
        public DisplayArea(Point location, Point size, Sprite background)
        {
            _localRectangle = new Rectangle(location, size);
            _background = background;
        }
        public override void Draw(SpriteBatch sb)
        {
            _background.Draw(sb, _globalRectangle, Color.DarkGray);

            base.Draw(sb);
        }
        public override void AddChild(PrimusUIComponent child)
        {
            _children.Add(child);
            child.SetOffset(_offset + _localRectangle.Location);
            child.SetParent(this);
            child.UpdateGlobalRectangle();
        }

        public override void SetOffset(Point offset)
        {
            _offset = offset;
            foreach (PrimusUIComponent child in _children)
            {
                child.SetOffset(offset+_localRectangle.Location);
            }
            //base.SetOffset(offset);
        }
    }

    class Window : PrimusUIComponent
    {
        private TitleBar _headerRect;
        private DisplayArea _displayRect;
        private string _title;

        private Sprite _background;

        public Window(string title, Point initialPoint, Point displaySize, Sprite background)
        {
            SetOffset(initialPoint);

            _background = background;
            _title = title;

            int heightOff = 0;
            if (title!=null)
            {
                _headerRect = new TitleBar(new Point(0,0), new Point(displaySize.X, 10), title, background);
                AddChild(_headerRect);
                heightOff = 10;
            }
            _displayRect = new DisplayArea(new Point(0, heightOff), displaySize, background);
            AddChild(_displayRect);

            _localRectangle = new Rectangle(new Point(0, 0), new Point(displaySize.X, displaySize.Y + heightOff));
            UpdateGlobalRectangle();
        }

        public DisplayArea GetDisplayArea()
        {
            return _displayRect;
        }
    }

    class PrimusUIManager:PrimusUIComponent
    {
        public PrimusUIComponent GetFromPoint(Point p)
        {
            foreach(PrimusUIComponent child in _children)
            {
                PrimusUIComponent temp = child.GetFromGlobalPoint(p);
                if (temp != null)
                    return temp;
            }

            return null;
        }

        public PrimusUIComponent GetImmediateChildFromPoint(Point p)
        {
            foreach (PrimusUIComponent child in _children)
            {
                if (child.InRectGlobal(p))
                    return child;
            }
            return null;
        }
        public override void AddChild(PrimusUIComponent child)
        {
            _children.Add(child);
            child.SetParent(this);
        }
    }
}

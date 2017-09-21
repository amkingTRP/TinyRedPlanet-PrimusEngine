using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Collections;

namespace TinyRedPlanet.PrimusEngine
{
    

    class GraphicsLoader
    {
        private static ContentManager _content;
        private static GraphicsDevice _graphics;

        public static void SetContent(ContentManager content, GraphicsDevice gd)
        {
            _content = content;
            _graphics = gd;
        }

        private static Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
        private static Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();

        public static Texture2D GetTexture(string name)
        {
            if (_textures.ContainsKey(name))
            {
                return _textures[name];
            }
            else return null;
        }

        public static Texture2D GetTextureOrLoad(string name)
        {
            if(!_textures.ContainsKey(name))
            {
                LoadTexture(name);
            }
            if (_textures.ContainsKey(name))
            {
                return _textures[name];
            }
            else
                return null;
        }

        public static BitmapFont GetFont(string textureName, int width, int height, int startingChar)
        {
            Texture2D texture = GetTextureOrLoad(textureName);
            BitmapFont font = new BitmapFont(texture, startingChar, width, height);

            return font;
        }

        public static Sprite GetSprite(string name)
        {
            if (_sprites.ContainsKey(name))
            {
                return _sprites[name];
            }
            else return null;
        }

        public static void LoadTexture(string name)
        {
            if(_textures.ContainsKey(name))
            {
                return;
            }

            FileStream fileStream = new FileStream(name, FileMode.Open);
            Texture2D texture = Texture2D.FromStream(_graphics, fileStream);
            fileStream.Dispose();

            _textures.Add(name, texture);
        }

        public static void AddSprite(string name, string texName, Rectangle source, Vector2 origin)
        {
            if (_sprites.ContainsKey(name))
                return;

            if(!_textures.ContainsKey(texName)) // add in texture to be on safe side
            {
                LoadTexture(texName);
            }

            Sprite sprite = new Sprite(GetTexture(texName), source, origin, Color.White);
            _sprites.Add(name, sprite);
        }

        public static void AddSpriteFromString(string csv)
        {
            // format:  name, texture name, x1, y1, x2, y2, ox, oy
            string[] parts = csv.Split(new char[] { ',' });
            string spriteName = parts[0];
            string TextureName = parts[1];
            int x1 = int.Parse(parts[2]);
            int y1 = int.Parse(parts[3]);
            int x2 = int.Parse(parts[4]);
            int y2 = int.Parse(parts[5]);
            int ox = int.Parse(parts[6]);
            int oy = int.Parse(parts[7]);

            Rectangle rect = new Rectangle(x1, y1, x2 - x1, y2 - y1);
            Vector2 origin = new Vector2(ox, oy);

            AddSprite(spriteName, TextureName, rect, origin);
        }

        public static Animation GetAnimationFromString(string csv)
        {
            // format: number of frames, SpriteName, Duration, etc.
            string[] parts = csv.Split(new char[] { ',' });
            int index =1;
            int numFrames = int.Parse(parts[0]);
            AnimationCel[] anim = new AnimationCel[numFrames];
            for(int i=0; i<numFrames; i++)
            {
                string spriteName = parts[index];
                int duration = int.Parse(parts[index + 1]);
                anim[i] = new AnimationCel(GetSprite(spriteName), duration);

                index+=2;
            }
            return new Animation(anim);
        }

        public static Tileset[] LoadTilesets(string[] names, Point dimensions)
        {
            Tileset[] _tileSets = new PrimusEngine.Tileset[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                _tileSets[i] = new PrimusEngine.Tileset(names[i], dimensions.X, dimensions.Y);
            }

            return _tileSets;
        }

        public static Tileset[] LoadTilesetsFromFile(string filename)
        {
            List<string> strings = new List<string>();

            FileStream tilesetList = new FileStream(filename, FileMode.Open);
            StreamReader sr = new StreamReader(tilesetList);
            string line;

            while((line=sr.ReadLine())!=null)
            {
                strings.Add(line);
            }

            return LoadTilesets(strings.ToArray(), new Point(16, 16));
        }
    }
}

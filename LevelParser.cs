using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Globalization;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;

namespace AITW
{
    class LevelParser
    {
        private XDocument doc;
        private ContentManager ContentManager { get; set; }

        public LevelInfo Li;

        public LevelParser(ContentManager contentManager)
        {
            ContentManager = contentManager;
            Li = new LevelInfo();
        }

        public void Parse(int levelNumber)
        {
            doc = XDocument.Load("Content/Maps/Level" + levelNumber + ".xml");
            var levelInfo = doc.Root.Element("info");
            var levelIntro = levelInfo.Attribute("IntroText").Value;
            var music = levelInfo.Attribute("Music").Value;
            Li.IntroText = levelIntro;
            Li.LevelNumber = levelNumber;
            Li.Music = music;
        }

        public LevelInfo GetInfo()
        {
            return Li;
        }
    }
}

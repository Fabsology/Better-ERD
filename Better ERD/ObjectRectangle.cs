using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Better_ERD
{
    public class ObjectRectangle
    {
        private int x;
        private int y;
        private int width;
        private int height;
        private int type;
        private string title;
        private string content;
        private List<ObjectRectangle> connected = new List<ObjectRectangle>();

        public int X_Coord   // property
        {
            get { return this.x; }
            set { this.x = value; }
        }
        public int Y_Coord
        {
            get { return this.y; }
            set { this.y = value; }
        }
        public int Width
        {
            get { return this.width; }
            set { this.width = value; }
        }
        public int Height
        {
            get { return this.height; }
            set { this.height = value; }
        }
        public int Type
        {
            get { return this.type; }
            set { this.type = value; }
        }
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }
        public string Content
        {
            get { return this.content; }
            set { this.content = value; }
        }
    }
}

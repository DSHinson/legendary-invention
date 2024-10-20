using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeView.Scene
{
    public class RenderObjectDefinitions
    {
        public enum ObjectType
        {
            Cube,
            Sphere,
            Tetrahedron,
            Plane,
            Arrow,
            Compass
        }

        public enum Color
        {
            Red = 0xFF0000,
            Orange = 0xFFA500,
            Yellow = 0xFFFF00,
            Green = 0x00FF00,
            Blue = 0x0000FF,
            Indigo = 0x4B0082,
            Violet = 0x8A2BE2,
            DarkGray = 0xA9A9A9,
            Gray = 0x808080,
            White = 0xFFFFFF 
        }
    }
}

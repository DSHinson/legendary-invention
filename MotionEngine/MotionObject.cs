using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionEngine
{
    public class MotionObject
    {
        public Guid Id { get; set; }
        public float[] Position { get; set; }  // x, y, z coordinates
        public float[] Rotation { get; set; } // Rotation around x, y, z axes

        public string type { get; set; }
    }
}

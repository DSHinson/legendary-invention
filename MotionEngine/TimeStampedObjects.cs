using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionEngine
{
    public class TimeStampedObjects
    {
        public int Timestamp { get; set; }
        public List<MotionObject> Objects { get; set; }
    }
}

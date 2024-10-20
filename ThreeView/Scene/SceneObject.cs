using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static ThreeView.Scene.RenderObjectDefinitions;

namespace ThreeView.Scene
{
    public class SceneObject
    {
        public Guid Id { get; set; } 
        public ObjectType RenderObjectType { get; set; } // E.g., "cube", "sphere"
        public float[] Position { get; set; }  // x, y, z coordinates
        public float[] Rotation { get; set; }  // Rotation around x, y, z axes
        public float[] Scale { get; set; }     // Scale in x, y, z directions
        [JsonIgnore] // This prevents the integer Colour from being serialized in the JSON output
        public int Colour { get; set; } // Stored as an integer

        public bool Selected { get; set; } = false;

        // This property returns the hexadecimal string representation
        public string ColourHex => $"#{Colour:X6}"; // Converts to hex format

        public SceneObject(ObjectType renderObjectType, float[] position, float[] rotation, float[] scale, Color color, Guid id)
        {
            RenderObjectType = renderObjectType;
            Position = position;
            Rotation = rotation;
            Scale = scale;
            Colour = (int)color;
            Id = id;
        }
    }
}

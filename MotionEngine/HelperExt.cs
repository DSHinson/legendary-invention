using MotionEngine;
using static ThreeView.Scene.RenderObjectDefinitions;
using ThreeView.Scene;

namespace MotionEngine
{
    public static class HelperExt
    {
        private static readonly float[] DefaultScale = new float[] { 1, 1, 1 };

        private static readonly Dictionary<string, Color> DefaultColorMap = new Dictionary<string, Color>
        {
            { "cube", Color.Orange },
            { "sphere", Color.Red },
            { "pyramid", Color.Green },
            { "plane", Color.DarkGray },
            { "arrow", Color.Blue },
            { "compass", Color.White }
        };
        public static SceneObject ToSceneObject(this MotionObject motionObject, float[] scale = null)
        {
            // Map the object type string from MotionObject to ObjectType enum
            ObjectType renderObjectType = Enum.TryParse(motionObject.type, true, out ObjectType objectType)
                ? objectType
                : ObjectType.Cube; // Default to Cube if the type is unknown

            // Get the default color for the object type, or fall back to White
            Color defaultColor = DefaultColorMap.ContainsKey(motionObject.type.ToLower())
                ? DefaultColorMap[motionObject.type.ToLower()]
                : Color.White;

            // Use the provided scale or default to [1, 1, 1]
            float[] objectScale = scale ?? DefaultScale;

            // Return the new SceneObject based on the MotionObject properties
            return new SceneObject(
                renderObjectType,           // Map the object type from motionObject
                motionObject.Position,      // Use the position from motionObject
                motionObject.Rotation,      // Use the rotation from motionObject
                objectScale,                // Use the provided or default scale
                defaultColor,               // Use the mapped color as needed
                motionObject.Id             // Use the existing ID
            );
        }
    }
}

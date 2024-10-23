using MotionEngine;

namespace MotionEngine
{
    public class TestState
    {
        public event Action<List<ThreeView.Scene.SceneObject>> SceneObjectsUpdated;
        private List<ThreeView.Scene.SceneObject> SceneObjects = new();

        public void SetSceneObjects(List<MotionObject> motionObjects)
        {
            var newSceneObjects = motionObjects?.Select(x => x.ToSceneObject());
            SceneObjects = newSceneObjects?.ToList();
            SceneObjectsUpdated?.Invoke(SceneObjects);
        }
    }
}

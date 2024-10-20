using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeView.Scene
{
    public class Scene
    {
        public List<SceneObject> Objects { get; set; } = new List<SceneObject>();

        public Scene()
        {
        }

        // Method to add a new object to the scene
        public void AddObject(SceneObject sceneObject)
        {
            Objects.Add(sceneObject);
        }
    }
}

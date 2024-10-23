using MotionEngine;
using System.Drawing;
using ThreeView;
using ThreeView.Scene;

namespace Canva.Data
{
    public class MapService : IDisposable
    {
        private readonly ThreeViewService _threeViewService;
        private readonly TestState _testState;
        private SceneObject _selectedSceneObject;
        public event Action<List<ThreeView.Scene.SceneObject>> SceneObjectsUpdated;
        public List<SceneObject> _sceneObjects { get; private set;}

        public MapService(ThreeViewService threeViewService, TestState testState)
        {
            _threeViewService = threeViewService ?? throw new ArgumentNullException(nameof(threeViewService));
            _testState = testState ?? throw new ArgumentNullException(nameof(testState));

            _testState.SceneObjectsUpdated += UpdateThreeView;
            _threeViewService.selectedObjectChanged += setSelectedObject;

        }

        private async void UpdateThreeView(List<SceneObject> sceneObjects)
        {

            _sceneObjects = sceneObjects;
            if (_selectedSceneObject != null)
            {
                sceneObjects.ForEach(x =>
                {

                    if (x.Id == _selectedSceneObject.Id)
                    {
                        x.Colour = (int)RenderObjectDefinitions.Color.Blue;
                    }
                });
            }

            _threeViewService.SetSceneObjects(_sceneObjects);
            await _threeViewService.UpdateScene();
            SceneObjectsUpdated?.Invoke(sceneObjects);
        }

        public async void initScene() => await _threeViewService.initScene();

        public void setSelectedObject(Guid id)
        {
            _selectedSceneObject = _sceneObjects?.FirstOrDefault(x => x.Id == id);
        }

        public void clearSelectedObject()
        {
            _selectedSceneObject = null;
        }

        public void Dispose()
        {
            _testState.SceneObjectsUpdated -= UpdateThreeView;
            _threeViewService.selectedObjectChanged -= setSelectedObject;
        }
    }
}

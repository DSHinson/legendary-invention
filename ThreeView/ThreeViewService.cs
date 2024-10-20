using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ThreeView.Scene;
using static System.Formats.Asn1.AsnWriter;
using static ThreeView.Scene.RenderObjectDefinitions;

namespace ThreeView
{
    public class ThreeViewService
    {
        private readonly IJSRuntime _jsRuntime;
        private Scene.Scene scene;

        public List<SceneObject> Objects => scene.Objects;

        public ThreeViewService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            // Create a scene
            scene = new Scene.Scene();
            
            DotNetObjectReference<ThreeViewService> dotNetReference = DotNetObjectReference.Create(this);
            _jsRuntime.InvokeVoidAsync("setDotNetReference", dotNetReference);

            
        }

        public void SetSceneObjects(List<SceneObject> Objects)
        { 
            scene.Objects = Objects;
        }

        public string GetSceneJson()
        {
            // Serialize the scene to JSON
            return JsonSerializer.Serialize(scene) ?? "";
        }

        public async Task initScene()
        {
            await _jsRuntime.InvokeVoidAsync("initializeScene", GetSceneJson());
        }

        public async Task UpdateScene()
        {
            await _jsRuntime.InvokeVoidAsync("reRenderScene", GetSceneJson());
            
        }

        [JSInvokable("OnObjectClicked")]
        public void OnObjectClicked(string selectedID)
        {
            Console.WriteLine($"Clicked object: {selectedID}");
        }

    }
}


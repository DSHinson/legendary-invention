let scene, camera, renderer, DotNetReference;
let meshIdMap = new Map();
let raycaster = new THREE.Raycaster();
let pointer = new THREE.Vector2();
let objectTypeMap = {
    0: 'cube',
    1: 'sphere',
    2: 'pyramid',
    3: 'Plane',
    4: 'Arrow',
    5: 'Compass'
};

function setDotNetReference (dotNetReference) {
    DotNetReference = dotNetReference;
};
function initializeScene(sceneData) {

    window.addEventListener('pointermove', onPointerMove);

    window.requestAnimationFrame(render);
    // Parse the JSON scene data
    const sceneObject = JSON.parse(sceneData);

    // Step 1: Create a scene
    scene = new THREE.Scene();

    // Step 2: Set up a camera
    camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
    camera.position.z = 5; // Move the camera back

    // Step 3: Set up a renderer
    renderer = new THREE.WebGLRenderer();
    const canvasContainer = document.getElementById('threejs-canvas');
    const width = canvasContainer.clientWidth;
    const height = canvasContainer.clientHeight;
    renderer.setSize(width, height);
    canvasContainer.appendChild(renderer.domElement);

    // Step 4: Add objects from sceneData
    sceneObject?.Objects?.forEach(obj => {
        let geometry, material, mesh;

        // Check object type (cube, sphere, etc.)

        console.log(obj)
        // Later in the rendering logic
        const objectType = objectTypeMap[obj.RenderObjectType]; // Convert enum value to string
        switch (objectType) {
            case 'cube':
                geometry = new THREE.BoxGeometry(1, 1, 1);
                break;
            case 'sphere':
                geometry = new THREE.SphereGeometry(1, 32, 32);
                break;
            case 'pyramid':
                geometry = new THREE.ConeGeometry(1, 1, 4);
                break;
            case 'Plane':
                addFloorPlane()
                return;
            case 'Arrow':
                geometry = new THREE.CylinderGeometry(0, 0.1, 1, 5);
                break;
            case 'Compass': // Render compass
                addCompass();
                return; // Skip adding a mesh for compass, since it's rendered separately
            default:
                console.warn('Unknown object type:', obj.Type);
                return;
        }
        if (!material) {
            // Create the material
            material = new THREE.MeshBasicMaterial({ color: obj.ColourHex });
        }


        // Create the mesh
        mesh = new THREE.Mesh(geometry, material);
        meshIdMap.set(mesh.id, obj.Id);
        // Set position, rotation, and scale
        mesh.position.set(obj.Position[0], obj.Position[1], obj.Position[2]);
        mesh.rotation.set(obj.Rotation[0], obj.Rotation[1], obj.Rotation[2]);
        mesh.scale.set(obj.Scale[0], obj.Scale[1], obj.Scale[2]);

        // Add the mesh to the scene
        scene.add(mesh);
    });

    // Initialize OrbitControls
    const controls = new THREE.OrbitControls(camera, renderer.domElement);
    raycaster = new THREE.Raycaster();
    mouse = new THREE.Vector2();

    // Add an event listener for mouse clicks
    window.addEventListener('click', onMouseClick, false);
    function animate() {
        requestAnimationFrame(animate);
        controls.update(); // Required for OrbitControls
        render()

        if (camera.position.y < 1) {
            camera.position.y = 1;
        }
    }

    // Start the animation loop
    animate();
}
function reRenderScene(updatedSceneData) {
    // Clear the current scene
    while (scene.children.length > 0) {
        scene.remove(scene.children[0]);
    }

    // Parse the updated scene data
    const sceneObject = JSON.parse(updatedSceneData);

    // Add the updated objects to the scene
    sceneObject?.Objects?.forEach(obj => {
        let geometry, material, mesh;

        // Check object type (cube, sphere, etc.)

        console.log(obj)
        // Later in the rendering logic
        const objectType = objectTypeMap[obj.RenderObjectType]; // Convert enum value to string
        switch (objectType) {
            case 'cube':
                geometry = new THREE.BoxGeometry(1, 1, 1);
                break;
            case 'sphere':
                geometry = new THREE.SphereGeometry(1, 32, 32);
                break;
            case 'pyramid':
                geometry = new THREE.ConeGeometry(1, 1, 4);
                break;
            case 'Plane':
                addFloorPlane()
                return;
            case 'Arrow':
                geometry = new THREE.CylinderGeometry(0, 0.1, 1, 5);
                break;
            case 'Compass': // Render compass
                addCompass();
                return; // Skip adding a mesh for compass, since it's rendered separately
            default:
                console.warn('Unknown object type:', obj.Type);
                return;
        }
        if (!material) {
            // Create the material
            material = new THREE.MeshBasicMaterial({ color: obj.ColourHex });
        }


        // Create the mesh
        mesh = new THREE.Mesh(geometry, material);
        meshIdMap.set(mesh.id, obj.Id);
        // Set position, rotation, and scale
        mesh.position.set(obj.Position[0], obj.Position[1], obj.Position[2]);
        mesh.rotation.set(obj.Rotation[0], obj.Rotation[1], obj.Rotation[2]);
        mesh.scale.set(obj.Scale[0], obj.Scale[1], obj.Scale[2]);

        // Add the mesh to the scene
        scene.add(mesh);
    });
    renderer.render(scene, camera); // Render the updated scene
}
function addFloorPlane()
{
    // Add the floor plane
    const planeGeometry = new THREE.PlaneGeometry(100, 100);
    const planeMaterial = new THREE.MeshBasicMaterial({ color: 0x999999, side: THREE.DoubleSide });
    const plane = new THREE.Mesh(planeGeometry, planeMaterial);
    plane.rotation.x = -Math.PI / 2; // Rotate to make it horizontal
    plane.position.y = -0.5;
    scene.add(plane);
}
function createMesh(obj) {
    let geometry, material, mesh;

    const objectType = objectTypeMap[obj.RenderObjectType]; // Convert enum value to string
    switch (objectType) {
        case 'cube':
            geometry = new THREE.BoxGeometry(1, 1, 1);
            break;
        case 'sphere':
            geometry = new THREE.SphereGeometry(1, 32, 32);
            break;
        case 'pyramid':
            geometry = new THREE.ConeGeometry(1, 1, 4);
            break;
        case 'Plane':
            addFloorPlane();
            return;
        case 'Arrow':
            geometry = new THREE.CylinderGeometry(0, 0.1, 1, 5);
            break;
        case 'Compass':
            renderCompass();
            return;
        default:
            console.warn('Unknown object type:', obj.Type);
            return;
    }

    material = new THREE.MeshBasicMaterial({ color: obj.ColourHex });
    mesh = new THREE.Mesh(geometry, material);

    // Set position, rotation, and scale
    mesh.position.set(obj.Position[0], obj.Position[1], obj.Position[2]);
    mesh.rotation.set(obj.Rotation[0], obj.Rotation[1]);
    mesh.scale.set(obj.Scale[0], obj.Scale[1], obj.Scale[2]);

    // Attach custom ID
    mesh.userData.customId = obj.Id;

    // Add click event listener
    mesh.addEventListener('pointerdown', () => {
        DotNetReference.invokeMethodAsync('OnObjectClicked', mesh.userData.customId)
            .then(result => console.log('Object clicked:', result))
            .catch(err => console.error(err));
    });

    scene.add(mesh);
}
function addCompass() {
    // Add the directional compass
    const arrowLength = 5; // Length of the arrow
    const arrowColor = 0xff0000; // Red for the X direction
    const arrowHelperX = new THREE.ArrowHelper(new THREE.Vector3(1, 0, 0), new THREE.Vector3(0, 0.1, 0), arrowLength, arrowColor);
    const arrowHelperY = new THREE.ArrowHelper(new THREE.Vector3(0, 1, 0), new THREE.Vector3(0, 0, 0), arrowLength, 0x00ff00); // Green for the Y direction
    const arrowHelperZ = new THREE.ArrowHelper(new THREE.Vector3(0, 0, 1), new THREE.Vector3(0, 0, 0), arrowLength, 0x0000ff); // Blue for the Z direction
    scene.add(arrowHelperX);
    scene.add(arrowHelperY);
    scene.add(arrowHelperZ);
}
function render() {

    // update the picking ray with the camera and pointer position
    raycaster.setFromCamera(pointer, camera);

    // calculate objects intersecting the picking ray
    const intersects = raycaster.intersectObjects(scene.children);
    console.table(intersects)

    renderer.render(scene, camera);

}
function onPointerMove(event) {

    // calculate pointer position in normalized device coordinates
    // (-1 to +1) for both components

    pointer.x = (event.clientX / window.innerWidth) * 2 - 1;
    pointer.y = - (event.clientY / window.innerHeight) * 2 + 1;
    console.log(pointer)

}
function onMouseClick(event) {
    // Calculate mouse position in normalized device coordinates (-1 to +1)
    mouse.x = (event.clientX / window.innerWidth) * 2 - 1;
    mouse.y = -(event.clientY / window.innerHeight) * 2 + 1;

    // Update the raycaster with the camera and mouse position
    raycaster.setFromCamera(mouse, camera);

    // Calculate objects intersecting the ray
    const intersects = raycaster.intersectObjects(scene.children);

    if (intersects.length > 0) {
        // Get the first intersected object
        selectedObject = intersects[0].object;
        let serverId = meshIdMap.get(selectedObject.id);
        // Send the object details to C#
        DotNetReference.invokeMethodAsync('OnObjectClicked', serverId).then(result => {
            console.log('Object clicked:', result);
        }).catch(err => console.error(err));
    }
}




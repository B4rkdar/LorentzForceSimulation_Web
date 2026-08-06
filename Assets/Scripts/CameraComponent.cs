using UnityEngine;
using UnityEngine.UIElements;

public class CameraComponent : ISimComponent {
    public string DisplayName => "Camera";

    public float Fov = 60f;

    public void DrawInspector(VisualElement root) {
        var fovField = new FloatField("FOV") { value = Fov };
        fovField.RegisterValueChangedCallback(evt => Fov = evt.newValue);
        root.Add(fovField);
    }

    public void Update(float dt) { }
}

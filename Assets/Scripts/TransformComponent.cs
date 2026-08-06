using UnityEngine;
using UnityEngine.UIElements;

public class TransformComponent : ISimComponent {
    public Vector3 Position;
    public Vector3 Rotation;

    public string DisplayName => "Transform";

    public void DrawInspector(VisualElement root) {

        // ===== Position =====
        var posRow = new VisualElement();
        posRow.style.flexDirection = FlexDirection.Row;
        posRow.style.alignItems = Align.Center;

        var posLabel = new Label("Position");
        posLabel.style.minWidth = 70;
        posRow.Add(posLabel);

        var posX = new FloatField("X");
        posX.labelElement.style.minWidth = 15;
        posX.value = Position.x;
        posX.RegisterValueChangedCallback(evt => Position.x = evt.newValue);
        posRow.Add(posX);

        var posY = new FloatField("Y");
        posY.labelElement.style.minWidth = 15;
        posY.value = Position.y;
        posY.RegisterValueChangedCallback(evt => Position.y = evt.newValue);
        posRow.Add(posY);

        var posZ = new FloatField("Z");
        posZ.labelElement.style.minWidth = 15;
        posZ.value = Position.z;
        posZ.RegisterValueChangedCallback(evt => Position.z = evt.newValue);
        posRow.Add(posZ);

        root.Add(posRow);


        // ===== Rotation =====
        var rotRow = new VisualElement();
        rotRow.style.flexDirection = FlexDirection.Row;
        rotRow.style.alignItems = Align.Center;

        var rotLabel = new Label("Rotation");
        rotLabel.style.minWidth = 70;
        rotRow.Add(rotLabel);

        var rotX = new FloatField("X");
        rotX.labelElement.style.minWidth = 15;
        rotX.value = Rotation.x;
        rotX.RegisterValueChangedCallback(evt => Rotation.x = evt.newValue);
        rotRow.Add(rotX);

        var rotY = new FloatField("Y");
        rotY.labelElement.style.minWidth = 15;
        rotY.value = Rotation.y;
        rotY.RegisterValueChangedCallback(evt => Rotation.y = evt.newValue);
        rotRow.Add(rotY);

        var rotZ = new FloatField("Z");
        rotZ.labelElement.style.minWidth = 15;
        rotZ.value = Rotation.z;
        rotZ.RegisterValueChangedCallback(evt => Rotation.z = evt.newValue);
        rotRow.Add(rotZ);

        root.Add(rotRow);
    }


    public void Update(float dt) { }
}

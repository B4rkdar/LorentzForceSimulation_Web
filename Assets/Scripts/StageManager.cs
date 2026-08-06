using System.Collections.Generic;
using UnityEngine;

public class Stage {
    public string Name;
    public List<SimObject> Objects = new();

    public Stage(string name) {
        Name = name;
    }
}

public class StageManager {
    public List<Stage> Stages = new();
    public Stage CurrentStage;

    public StageManager() {
        var defaultStage = new Stage("Default Stage");

        // 導体オブジェクト
        var conductorObj = new SimObject("Conductor");
        conductorObj.Transform.Position = new Vector3(400, 300, 0); // ★ 画面中央に移動
        var conductor = new ConductorComponent();
        conductor.RebuildSegments();   // ★ これが必須
        conductorObj.AddComponent(conductor);

        defaultStage.Objects.Add(conductorObj);

        var cameraObj = new SimObject("Camera");
        cameraObj.Transform.Position = new Vector3(0, 0, -600);
        cameraObj.Transform.Rotation = Vector3.zero;
        cameraObj.AddComponent(new CameraComponent());

        defaultStage.Objects.Add(cameraObj);


        AddStage(defaultStage);
    }

    public void AddStage(Stage stage) {
        Stages.Add(stage);
        if (CurrentStage == null)
            CurrentStage = stage;
    }

    public void LoadStage(int index) {
        if (index < 0 || index >= Stages.Count) return;
        CurrentStage = Stages[index];
    }
}

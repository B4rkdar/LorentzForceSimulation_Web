using System.Collections.Generic;
using UnityEngine;

public class SimObject {
    public string Name;

    // Transform は必ず持つ
    public TransformComponent Transform;

    // 物理コンポーネントの一覧
    public List<ISimComponent> Components = new();

    public SimObject(string name) {
        Name = name;
        Transform = new TransformComponent();
        Components.Add(Transform);
    }

    public void AddComponent(ISimComponent comp) {
        Components.Add(comp);
    }

    public void Update(float dt) {
        foreach (var comp in Components)
            comp.Update(dt);
    }
}

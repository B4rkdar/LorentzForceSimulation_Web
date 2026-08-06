using UnityEngine;
using UnityEngine.UIElements;

public interface ISimComponent {
    string DisplayName { get; }

    // Inspector に UI を描画する
    void DrawInspector(VisualElement root);

    // 毎フレーム更新（物理計算・運動など）
    void Update(float dt);
}

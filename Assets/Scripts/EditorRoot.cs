using UnityEngine;
using UnityEngine.UIElements;

public class EditorRoot : VisualElement {
    StageManager stageManager;
    StageSelectorPanel stageSelector;
    HierarchyPanel hierarchyPanel;
    InspectorPanel inspectorPanel;
    SceneViewPanel sceneViewPanel;

    public EditorRoot() {

        // 親のサイズを確保
        style.flexDirection = FlexDirection.Column;
        style.flexGrow = 1;

        stageManager = new StageManager();

        // ───────────────────────────────
        // 上部バー（固定高さ）
        // ───────────────────────────────
        var topBar = new VisualElement();
        topBar.style.height = 40;
        topBar.style.flexShrink = 0;
        Add(topBar);

        stageSelector = new StageSelectorPanel(stageManager);
        topBar.Add(stageSelector);

        // ───────────────────────────────
        // メイン領域（横並び）
        // ───────────────────────────────
        var mainArea = new VisualElement();
        mainArea.style.flexDirection = FlexDirection.Row;
        mainArea.style.flexGrow = 1;   // ★ 高さを確保
        Add(mainArea);

        // 左：Hierarchy（20%）
        var leftArea = new VisualElement();
        leftArea.style.flexBasis = new Length(20, LengthUnit.Percent);
        leftArea.style.flexGrow = 0;
        leftArea.style.flexShrink = 0;
        mainArea.Add(leftArea);

        // 中央：SceneView（60%）
        var centerArea = new VisualElement();
        centerArea.style.flexBasis = new Length(60, LengthUnit.Percent);
        centerArea.style.flexGrow = 1;   // ★ 中央は伸びてもよい
        centerArea.style.flexShrink = 1;
        mainArea.Add(centerArea);

        // 右：Inspector（20%）
        var rightArea = new VisualElement();
        rightArea.style.flexBasis = new Length(20, LengthUnit.Percent);
        rightArea.style.flexGrow = 0;
        rightArea.style.flexShrink = 0;
        mainArea.Add(rightArea);


        // ───────────────────────────────
        // パネルを枠に入れる
        // ───────────────────────────────
        hierarchyPanel = new HierarchyPanel(stageManager);
        leftArea.Add(hierarchyPanel);

        sceneViewPanel = new SceneViewPanel();
        sceneViewPanel.style.flexGrow = 1;
        sceneViewPanel.style.backgroundColor = Color.black;
        centerArea.Add(sceneViewPanel);

        inspectorPanel = new InspectorPanel();
        rightArea.Add(inspectorPanel);

        // 初期表示
        hierarchyPanel.Refresh();

        // ステージ変更時
        stageSelector.OnStageChanged = stage => {
            hierarchyPanel.Refresh();

            // ★ SceneViewPanel にカメラをセット
            var cam = stage.Objects.Find(o => o.HasComponent<CameraComponent>());
            sceneViewPanel.SetCamera(cam);

            // ★ 導体をフレーム（初期位置にカメラを移動）
            var conductor = stage.Objects.Find(o => o.HasComponent<ConductorComponent>());
            if (conductor != null)
                sceneViewPanel.FrameObject(conductor);
        };


        // オブジェクト選択時
        hierarchyPanel.OnSelect = obj => {
            inspectorPanel.Show(obj);
            sceneViewPanel.Select(obj);
        };
    }
}

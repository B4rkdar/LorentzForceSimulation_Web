using UnityEngine.UIElements;

public class HierarchyPanel : VisualElement {
    public StageManager Manager;
    public System.Action<SimObject> OnSelect;

    public HierarchyPanel(StageManager manager) {
        Manager = manager;
    }

    public void Refresh() {
        Clear();

        if (Manager.CurrentStage == null) return;

        foreach (var obj in Manager.CurrentStage.Objects) {
            var btn = new Button(() => OnSelect?.Invoke(obj)) {
                text = obj.Name
            };
            Add(btn);
        }
    }
}

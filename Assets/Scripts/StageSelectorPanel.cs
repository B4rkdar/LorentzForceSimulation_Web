using UnityEngine.UIElements;

public class StageSelectorPanel : VisualElement {
    public StageManager Manager;

    // ★ Stage を渡すように修正
    public System.Action<Stage> OnStageChanged;

    public StageSelectorPanel(StageManager manager) {
        Manager = manager;
        Refresh();
    }

    public void Refresh() {
        Clear();

        for (int i = 0; i < Manager.Stages.Count; i++) {
            int index = i;

            var btn = new Button(() => {
                Manager.LoadStage(index);

                // ★ Stage を渡す
                OnStageChanged?.Invoke(Manager.CurrentStage);
            }) {
                text = Manager.Stages[i].Name
            };

            Add(btn);
        }
    }
}

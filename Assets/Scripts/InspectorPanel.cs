using UnityEngine.UIElements;

public class InspectorPanel : VisualElement {

    public void Show(SimObject obj) {
        Clear();

        if (obj == null) return;

        foreach (var comp in obj.Components) {
            var fold = new Foldout { text = comp.DisplayName };
            comp.DrawInspector(fold);
            Add(fold);
        }
    }
}

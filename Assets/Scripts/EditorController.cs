using UnityEngine;
using UnityEngine.UIElements;

public class EditorController : MonoBehaviour {
    [SerializeField] UIDocument uiDocument;

    void Start() {
        // UI Toolkit のルートを取得
        var root = uiDocument.rootVisualElement;

        // EditorRoot を追加
        var editor = new EditorRoot();
        root.Add(editor);
    }
}

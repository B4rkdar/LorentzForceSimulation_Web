using UnityEngine;
using UnityEngine.UIElements;

public class SceneViewPanel : VisualElement {
    SimObject selected;
    SimObject cameraObj;   // ★ カメラを SimObject として扱う

    Vector2 lastMousePos;

    public SceneViewPanel() {
        generateVisualContent += OnGenerateVisualContent;

        RegisterCallback<PointerDownEvent>(OnPointerDown);
        RegisterCallback<PointerMoveEvent>(OnPointerMove);
        RegisterCallback<WheelEvent>(OnWheel);
    }

    // ★ カメラをセットする
    public void SetCamera(SimObject cam) {
        cameraObj = cam;
        MarkDirtyRepaint();
    }

    public void Select(SimObject obj) {
        selected = obj;
        MarkDirtyRepaint();
    }

    void OnPointerDown(PointerDownEvent evt) {
        lastMousePos = evt.position;
    }

    void OnPointerMove(PointerMoveEvent evt) {
        if (cameraObj == null) return;

        Vector2 mousePos = (Vector2)evt.position;
        Vector2 delta = mousePos - lastMousePos;
        lastMousePos = mousePos;

        var camTrans = cameraObj.Transform;

        // 左ドラッグ → カメラ回転
        if (evt.pressedButtons == 1) {
            camTrans.Rotation.y += delta.x * 0.3f;
            camTrans.Rotation.x -= delta.y * 0.3f;
            camTrans.Rotation.x = Mathf.Clamp(camTrans.Rotation.x, -80f, 80f);

            MarkDirtyRepaint();
        }

        // 右ドラッグ → カメラパン
        if (evt.pressedButtons == 2) {
            Quaternion camRot = Quaternion.Euler(camTrans.Rotation);
            Vector3 right = camRot * Vector3.right;
            Vector3 up    = camRot * Vector3.up;

            camTrans.Position += -right * delta.x * 0.5f;
            camTrans.Position +=  up    * delta.y * 0.5f;

            MarkDirtyRepaint();
        }
    }

    void OnWheel(WheelEvent evt) {
        if (cameraObj == null) return;

        cameraObj.Transform.Position += new Vector3(0, 0, evt.delta.y * 20f);
        MarkDirtyRepaint();
    }

    // ★ 3D→2D 投影（Camera SimObject を使う）
    Vector2 Project(Vector3 worldPos) {
        if (cameraObj == null) return Vector2.zero;

        var camTrans = cameraObj.Transform;

        Vector3 cameraPos = camTrans.Position;
        Vector3 cameraRot = camTrans.Rotation;

        // ① カメラから見た相対座標
        Vector3 p = worldPos - cameraPos;

        // ② カメラの逆回転
        Quaternion camRotQ = Quaternion.Euler(cameraRot);
        Quaternion invRot = Quaternion.Inverse(camRotQ);
        Vector3 view = invRot * p;

        // ③ パースペクティブ投影
        float f = 400f;
        float x = (view.x / view.z) * f;
        float y = (view.y / view.z) * f;

        return new Vector2(
            x + layout.width / 2f,
            y + layout.height / 2f
        );
    }

    private void OnGenerateVisualContent(MeshGenerationContext ctx) {
        var painter = ctx.painter2D;
        painter.lineWidth = 2f;

        DrawGrid(painter);
        DrawAxes(painter);

        if (selected == null) return;

        foreach (var comp in selected.Components) {
            if (comp is ConductorComponent conductor) {

                painter.strokeColor = Color.yellow;

                foreach (var seg in conductor.Segments) {

                    Vector3 p0 = seg.start + selected.Transform.Position;
                    Vector3 p1 = seg.end   + selected.Transform.Position;

                    Vector2 s0 = Project(p0);
                    Vector2 s1 = Project(p1);

                    painter.BeginPath();
                    painter.MoveTo(s0);
                    painter.LineTo(s1);
                    painter.Stroke();
                }
            }
        }
    }

    void DrawGrid(Painter2D painter) {
        painter.strokeColor = new Color(0.2f, 0.2f, 0.2f, 1f);

        int gridSize = 50;
        int count = 40;

        for (int i = -count; i <= count; i++) {
            Vector3 p0 = new Vector3(i * gridSize, 0, -2000);
            Vector3 p1 = new Vector3(i * gridSize, 0,  2000);

            painter.BeginPath();
            painter.MoveTo(Project(p0));
            painter.LineTo(Project(p1));
            painter.Stroke();

            p0 = new Vector3(-2000, 0, i * gridSize);
            p1 = new Vector3( 2000, 0, i * gridSize);

            painter.BeginPath();
            painter.MoveTo(Project(p0));
            painter.LineTo(Project(p1));
            painter.Stroke();
        }
    }

    void DrawAxes(Painter2D painter) {
        float axisLength = 1000f;

        painter.strokeColor = Color.red;
        painter.BeginPath();
        painter.MoveTo(Project(Vector3.zero));
        painter.LineTo(Project(new Vector3(axisLength, 0, 0)));
        painter.Stroke();

        painter.strokeColor = Color.green;
        painter.BeginPath();
        painter.MoveTo(Project(Vector3.zero));
        painter.LineTo(Project(new Vector3(0, axisLength, 0)));
        painter.Stroke();

        painter.strokeColor = Color.blue;
        painter.BeginPath();
        painter.MoveTo(Project(Vector3.zero));
        painter.LineTo(Project(new Vector3(0, 0, axisLength)));
        painter.Stroke();
    }

    Vector3 ComputeObjectCenter(SimObject obj) {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (var comp in obj.Components) {
            if (comp is ConductorComponent conductor) {
                foreach (var seg in conductor.Segments) {
                    sum += seg.start + obj.Transform.Position;
                    sum += seg.end   + obj.Transform.Position;
                    count += 2;
                }
            }
        }

        if (count == 0) return obj.Transform.Position;
        return sum / count;
    }

    public void FrameObject(SimObject obj) {
        if (cameraObj == null || obj == null) return;

        Vector3 center = ComputeObjectCenter(obj);

        // カメラを導体の中心から少し離れた位置に置く
        cameraObj.Transform.Position = center + new Vector3(0, 0, -600);

        // カメラの向きを中心へ向ける
        Vector3 dir = center - cameraObj.Transform.Position;
        cameraObj.Transform.Rotation = Quaternion.LookRotation(dir).eulerAngles;

        MarkDirtyRepaint();
    }
}

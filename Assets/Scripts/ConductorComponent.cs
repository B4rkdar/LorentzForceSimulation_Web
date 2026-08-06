using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum ConductorType {
    Straight,
    Circle,
    Rectangle
}

public class ConductorComponent : ISimComponent {
    public string DisplayName => "Conductor";

    public ConductorType Type = ConductorType.Straight;

    // 導体を構成する線分（描画用）
    public List<(Vector3 start, Vector3 end)> Segments = new();

    // パラメータ
    public float Length = 200f;      // 直線導体の長さ
    public float Radius = 100f;      // 円形コイルの半径
    public float Width = 200f;       // 矩形コイルの幅
    public float Height = 150f;      // 矩形コイルの高さ
    public int Resolution = 32;      // 円形コイルの分割数

    public void Update(float dt) { }

    // Inspector UI
    public void DrawInspector(VisualElement root) {
        // 種類選択
        var typeField = new EnumField("Type", Type);
        typeField.RegisterValueChangedCallback(evt => {
            Type = (ConductorType)evt.newValue;
            RebuildSegments();
        });
        root.Add(typeField);

        // パラメータ編集
        switch (Type) {
            case ConductorType.Straight:
                var lenField = new FloatField("Length") { value = Length };
                lenField.RegisterValueChangedCallback(evt => {
                    Length = evt.newValue;
                    RebuildSegments();
                });
                root.Add(lenField);
                break;

            case ConductorType.Circle:
                var radField = new FloatField("Radius") { value = Radius };
                radField.RegisterValueChangedCallback(evt => {
                    Radius = evt.newValue;
                    RebuildSegments();
                });
                root.Add(radField);

                var resField = new IntegerField("Resolution") { value = Resolution };
                resField.RegisterValueChangedCallback(evt => {
                    Resolution = evt.newValue;
                    RebuildSegments();
                });
                root.Add(resField);
                break;

            case ConductorType.Rectangle:
                var wField = new FloatField("Width") { value = Width };
                wField.RegisterValueChangedCallback(evt => {
                    Width = evt.newValue;
                    RebuildSegments();
                });
                root.Add(wField);

                var hField = new FloatField("Height") { value = Height };
                hField.RegisterValueChangedCallback(evt => {
                    Height = evt.newValue;
                    RebuildSegments();
                });
                root.Add(hField);
                break;
        }
    }

    // 導体の形状を Segments に変換
    public void RebuildSegments() {
        Segments.Clear();

        switch (Type) {
            case ConductorType.Straight:
                BuildStraight();
                break;

            case ConductorType.Circle:
                BuildCircle();
                break;

            case ConductorType.Rectangle:
                BuildRectangle();
                break;
        }
    }

    private void BuildStraight() {
        Vector3 start = new Vector3(-Length / 2f, 0, 0);
        Vector3 end   = new Vector3( Length / 2f, 0, 0);
        Segments.Add((start, end));
    }

    private void BuildCircle() {
        for (int i = 0; i < Resolution; i++) {
            float a0 = Mathf.PI * 2f * i / Resolution;
            float a1 = Mathf.PI * 2f * (i + 1) / Resolution;

            Vector3 p0 = new Vector3(Mathf.Cos(a0) * Radius, Mathf.Sin(a0) * Radius, 0);
            Vector3 p1 = new Vector3(Mathf.Cos(a1) * Radius, Mathf.Sin(a1) * Radius, 0);

            Segments.Add((p0, p1));
        }
    }

    private void BuildRectangle() {
        Vector3 p0 = new Vector3(-Width/2, -Height/2, 0);
        Vector3 p1 = new Vector3( Width/2, -Height/2, 0);
        Vector3 p2 = new Vector3( Width/2,  Height/2, 0);
        Vector3 p3 = new Vector3(-Width/2,  Height/2, 0);

        Segments.Add((p0, p1));
        Segments.Add((p1, p2));
        Segments.Add((p2, p3));
        Segments.Add((p3, p0));
    }
}

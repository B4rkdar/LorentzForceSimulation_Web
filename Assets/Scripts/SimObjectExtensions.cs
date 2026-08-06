using System.Linq;

public static class SimObjectExtensions {

    public static bool HasComponent<T>(this SimObject obj) where T : ISimComponent {
        return obj.Components.Any(c => c is T);
    }

    public static T GetComponent<T>(this SimObject obj) where T : class, ISimComponent {
        return obj.Components.FirstOrDefault(c => c is T) as T;
    }
}

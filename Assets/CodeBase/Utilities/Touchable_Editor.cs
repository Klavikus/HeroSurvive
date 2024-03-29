#if UNITY_EDITOR
using UnityEditor;

namespace CodeBase.Utilities
{
    [CustomEditor(typeof(Touchable))]
    public class Touchable_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
        }
    }
}
#endif
using UnityEditor;
using UnityEngine;

// https://baba-s.hatenablog.com/entry/2018/12/19/085000
class RenameFieldAttribute : PropertyAttribute
{
    public string Name { get; }

    public RenameFieldAttribute(string name) => Name = name;

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(RenameFieldAttribute))]
    class FieldNameDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string[] path = property.propertyPath.Split('.');
            bool isArray = path.Length > 1 && path[1] == "Array";

            if (!isArray && attribute is RenameFieldAttribute fieldName)
                label.text = fieldName.Name;

            EditorGUI.PropertyField(position, property, label, true);
        }
    }
#endif
}
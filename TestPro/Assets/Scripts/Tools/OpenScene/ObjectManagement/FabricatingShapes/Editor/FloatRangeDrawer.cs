using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FloatRange))]
public class FloatRangeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //记录缩进级别和标签宽度
        int originalIndentLevel = EditorGUI.indentLevel;
        float originalLabelWidth = EditorGUIUtility.labelWidth;

        //base.OnGUI(position, property, label);
        EditorGUI.BeginProperty(position, label, property);
        // 得到添加label后的位置
        position = EditorGUI.PrefixLabel(position, EditorGUIUtility.GetControlID(FocusType.Passive), label);
        position.width = position.width / 2;
        //设置标签宽度
        EditorGUIUtility.labelWidth = position.width / 2;
        // 缩进一直保持1个字段
        EditorGUI.indentLevel = 1;
        // 填充属性min
        EditorGUI.PropertyField(position, property.FindPropertyRelative("min"));
        position.x += position.width;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("max"));
        EditorGUI.EndProperty();

        // 还原缩进级别和标签宽度
        EditorGUI.indentLevel = originalIndentLevel;
        EditorGUIUtility.labelWidth = originalLabelWidth;
    }
}

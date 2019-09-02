using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "RealFrameConfig", menuName = "CreateRealFrameConfig", order = 0)]
public class RealFrameConfig : ScriptableObject
{
    //打包时生成的ab包配置表的二进制路径
    public string m_ABBytePath;
    //xml文件夹路径
    public string m_XmlPath;
    //二进制文件夹路径
    public string m_BinaryPath;
    //脚本文件夹路径
    public string m_ScriptsPath;
}
[CustomEditor(typeof(RealFrameConfig))]
public class RealFramConfigInspector:Editor
{
    public SerializedProperty m_ABBytePath;
    public SerializedProperty m_XmlPath;
    public SerializedProperty m_BinaryPath;
    public SerializedProperty m_ScriptsPath;

    public void OnEnable()
    {
        m_ABBytePath = serializedObject.FindProperty("m_ABBytePath");
        m_XmlPath = serializedObject.FindProperty("m_XmlPath");
        m_BinaryPath = serializedObject.FindProperty("m_BinaryPath");
        m_ScriptsPath = serializedObject.FindProperty("m_ScriptsPath");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_ABBytePath, new GUIContent("ab包路径"));
        GUILayout.Space(5);
        EditorGUILayout.PropertyField(m_XmlPath, new GUIContent("xml路径"));
        GUILayout.Space(5);
        EditorGUILayout.PropertyField(m_BinaryPath, new GUIContent("二进制路径"));
        GUILayout.Space(5);
        EditorGUILayout.PropertyField(m_ScriptsPath, new GUIContent("脚本路径"));
        GUILayout.Space(5);
        serializedObject.ApplyModifiedProperties();
    }
}

public class ReadConfig
{
    private const string RealFramePath = "Assets/RealFrame/Editor/RealFrameConfig.asset";
    public static RealFrameConfig GetRealFrame()
    {
        RealFrameConfig realConfig = null;
#if UNITY_EDITOR
        realConfig = AssetDatabase.LoadAssetAtPath<RealFrameConfig>(RealFramePath);
#endif
        return realConfig;
    }
}
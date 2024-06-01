using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ExcelToScriptable : EditorWindow
{
    private string pathCSV = string.Empty;
    private string saveFullFolder = string.Empty;
    private string saveFolder = string.Empty;
    private ExportOptions options = ExportOptions.RowToList;
    private MonoScript script = null;
    private Type scriptDataType = null;
    private Type dataClassType = null;
    private int gridId = 0;
    private string collectionName = string.Empty;
    private bool isSerializable = true;
    private bool isScriptableObject = true;

    [MenuItem("我的工具/CSV转换ScriptableObject")]
    static void ShowWindows()
    {
        GetWindow(typeof(ExcelToScriptable));
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(20);

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Label("CSV转ScriptableObject", new GUIStyle(EditorStyles.whiteLargeLabel)
            {
                fontSize = 25,
                alignment = TextAnchor.MiddleCenter,
            }, GUILayout.ExpandWidth(true));
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(20);

        EditorGUILayout.BeginVertical(new GUIStyle()
        {
            padding = new RectOffset(10, 10, 0, 0),
        });
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("CSV文件路径: ", GUILayout.Width(80));
                pathCSV = GUILayout.TextField(pathCSV, GUILayout.ExpandWidth(true));
                if (GUILayout.Button("浏览选择", GUILayout.Width(100)))
                {
                    pathCSV = EditorUtility.OpenFilePanel("Import Excel Data", "", "csv");
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            if (!string.IsNullOrEmpty(pathCSV) && !pathCSV.EndsWith(".CSV") && !pathCSV.EndsWith(".csv"))
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.HelpBox("注意: 必须使用UTF-8编码的CSV格式!", MessageType.Warning, true);
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("SO保存路径: ", GUILayout.Width(80));
                saveFolder = GUILayout.TextField(saveFolder, GUILayout.ExpandWidth(true));
                if (GUILayout.Button("浏览选择", GUILayout.Width(100)))
                {
                    saveFullFolder = EditorUtility.OpenFolderPanel("", Application.dataPath, "folder");
                    saveFolder = "Assets" + saveFullFolder.Split("Assets")[1];
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(30);

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            {
                var rowToList = "将CSV所有行转换为单个SO里面的List";
                var SoWithSoCollection = "将CSV每一行都转换为单独的ScriptableObject并附在一个ScriptableObject下";
                var everyRowToSingleFile = "将CSV每一行都转换为单独的ScriptObject";
                var isRowToList = options == ExportOptions.RowToList;
                var isSoWithSoCollection = options == ExportOptions.SoWithSoCollection;
                var note = isRowToList ? rowToList : isSoWithSoCollection ? SoWithSoCollection : everyRowToSingleFile;
                EditorGUILayout.HelpBox(note, MessageType.Info, true);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("转换选项: ", GUILayout.Width(80));
                options = (ExportOptions)EditorGUILayout.EnumPopup(options);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("SO类脚本: ", GUILayout.Width(80));
                script = (MonoScript)EditorGUILayout.ObjectField(script, typeof(MonoScript), true);
                if (script)
                {
                    scriptDataType = script.GetClass();
                }
            }
            EditorGUILayout.EndHorizontal();

            if (scriptDataType != null && scriptDataType.BaseType != typeof(ScriptableObject))
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.HelpBox("注意: 这个脚本必须继承自ScriptableObject!", MessageType.Warning, true);
                }
                EditorGUILayout.EndHorizontal();
            }
            else if (scriptDataType != null && options == ExportOptions.SoWithSoCollection)
            {
                var members = scriptDataType.GetFields().Where(x => x.FieldType.GetInterfaces().FirstOrDefault(y => y == typeof(ICollection)) != null);

                if (members != null && members.Count() > 0)
                {
                    var membersName = members.Select(x => x.Name).ToArray();

                    EditorGUILayout.Separator();

                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("选择转换到那个集合： ");
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();

                    EditorGUILayout.BeginHorizontal();
                    {
                        gridId = GUILayout.SelectionGrid(gridId, membersName, 1);
                        collectionName = membersName[gridId];
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();

                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("选定集合： ", GUILayout.Width(80));
                        GUILayout.Label(collectionName);
                        var type = scriptDataType.GetField(collectionName).FieldType;
                        if (type.IsGenericType)
                        {
                            var args = type.GetGenericArguments();
                            dataClassType = args[args.Length - 1];

                            isScriptableObject = dataClassType.BaseType == typeof(ScriptableObject);
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    if (!isScriptableObject)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.HelpBox("注意: 集合里类必须同样是ScriptableObject!", MessageType.Warning, true);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            else if (scriptDataType != null && options == ExportOptions.RowToList)
            {
                var members = scriptDataType.GetFields().Where(x => x.FieldType.GetInterfaces().FirstOrDefault(y => y == typeof(ICollection)) != null);

                if (members != null && members.Count() > 0)
                {
                    var membersName = members.Select(x => x.Name).ToArray();

                    EditorGUILayout.Separator();

                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("选择转换到那个集合： ");
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();

                    EditorGUILayout.BeginHorizontal();
                    {
                        gridId = GUILayout.SelectionGrid(gridId, membersName, 1);
                        collectionName = membersName[gridId];
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();

                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("选定集合： ", GUILayout.Width(80));
                        GUILayout.Label(collectionName);
                        var type = scriptDataType.GetField(collectionName).FieldType;
                        if (type.IsGenericType)
                        {
                            var args = type.GetGenericArguments();
                            dataClassType = args[args.Length - 1];
                            isSerializable = dataClassType.GetAttribute(typeof(SerializableAttribute)) != null;
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    if (!isSerializable)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.HelpBox("注意: 集合里类必须有Serializable特性!", MessageType.Warning, true);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }

            GUILayout.Space(30);

            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            {
                GUILayout.Label(string.Empty, GUILayout.ExpandWidth(true));

                if (GUILayout.Button("开始转换", GUILayout.Width(100)))
                {
                    if (!string.IsNullOrEmpty(pathCSV)
                    && !string.IsNullOrEmpty(saveFolder)
                    && scriptDataType != null
                    && (options != ExportOptions.RowToList || isSerializable))
                    {
                        Execute();
                    }
                }

                GUILayout.Label(string.Empty, GUILayout.ExpandWidth(true));
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }

    private void Execute()
    {
        var text = File.ReadAllText(pathCSV);
        if (string.IsNullOrEmpty(text)) return;

        var rows = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < rows.Length; i++)
        {
            rows[i] = rows[i].TrimEnd('\r');
        }

        if (options == ExportOptions.RowToList)
        {
            var asset = CreateInstance(script.name);
            var assetPath = Path.Combine(saveFolder, scriptDataType.Name + ".asset");

            var fieldInfo = scriptDataType.GetField(collectionName);
            var genericArgs = fieldInfo.FieldType.GetGenericArguments();

            var newList = Activator.CreateInstance(typeof(List<>).MakeGenericType(genericArgs));
            fieldInfo.SetValue(asset, newList);
            var addMethod = newList.GetType().GetMethod("Add");

            for (int i = 1; i < rows.Length; i++)
            {
                if (string.IsNullOrEmpty(rows[i])) continue;
                var columns = rows[i].Split(',');

                var instance = Activator.CreateInstance(dataClassType);
                var fieldInfos = dataClassType.GetFields();

                for (int j = 0; j < fieldInfos.Length; j++)
                {
                    var value = StringConvert.ToValue(fieldInfos[j].FieldType, columns[j]);
                    fieldInfos[j].SetValue(instance, value);
                }

                addMethod?.Invoke(newList, new[] { instance });
            }

            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.ImportAsset(assetPath);
            AssetDatabase.Refresh();
        }
        else if (options == ExportOptions.SoWithSoCollection)
        {
            var asset = CreateInstance(script.name);
            var assetPath = Path.Combine(saveFolder, scriptDataType.Name + ".asset");

            var fieldInfo = scriptDataType.GetField(collectionName);
            var genericArgs = fieldInfo.FieldType.GetGenericArguments();

            var newList = Activator.CreateInstance(typeof(List<>).MakeGenericType(genericArgs));
            fieldInfo.SetValue(asset, newList);

            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.ImportAsset(assetPath);
            AssetDatabase.Refresh();

            var addMethod = newList.GetType().GetMethod("Add");

            for (int i = 1; i < rows.Length; i++)
            {
                if (string.IsNullOrEmpty(rows[i])) continue;
                var columns = rows[i].Split(',');

                var instance = CreateInstance(dataClassType.ToString());
                var fieldInfos = dataClassType.GetFields();

                for (int j = 0; j < fieldInfos.Length; j++)
                {
                    var value = StringConvert.ToValue(fieldInfos[j].FieldType, columns[j]);
                    fieldInfos[j].SetValue(instance, value);
                }

                AssetDatabase.AddObjectToAsset(instance, asset);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                addMethod?.Invoke(newList, new[] { instance });
            }
        }
        else if (options == ExportOptions.EveryRowToSingleFile)
        {
            var fieldInfos = scriptDataType.GetFields();

            for (int i = 1; i < rows.Length; i++)
            {
                if (string.IsNullOrEmpty(rows[i])) continue;

                var columns = rows[i].Split(',');
                var assetPath = Path.Combine(saveFolder, columns[0] + ".asset");
                var asset = CreateInstance(script.name);
                for (int j = 0; j < fieldInfos.Length; j++)
                {
                    var value = StringConvert.ToValue(fieldInfos[j].FieldType, columns[j]);
                    fieldInfos[j].SetValue(asset, value);
                }
                AssetDatabase.CreateAsset(asset, assetPath);
                AssetDatabase.ImportAsset(assetPath);
                AssetDatabase.Refresh();
            }
        }
    }
}
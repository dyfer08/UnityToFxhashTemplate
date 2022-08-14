using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;

public class Fxhash_Window : EditorWindow{

    Vector2 scrollPosFx;
    static Fxhash_Data fxhashSettings = null;

    static bool usecustomHash;
    static string customHash;
    static bool generatePreview;
    static int previewQuantity;
    static int previewSuperSize;
    static bool saveHistory;

    SerializedObject sO;
    public ReorderableList seedList = null;

    [MenuItem("Window/Fxhash Simulator")]
    public static void ShowWindow(){
        EditorWindow.GetWindow<Fxhash_Window>("Fxhash Simulator");
    }

    static void Initialize(){

        fxhashSettings = (Fxhash_Data)AssetDatabase.LoadAssetAtPath("Assets/Settings/FxhashSimulator.asset", typeof(Fxhash_Data));

        if(fxhashSettings == null){
            Fxhash_Data asset = ScriptableObject.CreateInstance<Fxhash_Data>();
            AssetDatabase.CreateAsset(asset, "Assets/Settings/FxhashSimulator.asset");
            AssetDatabase.SaveAssets();
            fxhashSettings = (Fxhash_Data)AssetDatabase.LoadAssetAtPath("Assets/Settings/FxhashSimulator.asset", typeof(Fxhash_Data));
        }

        usecustomHash = fxhashSettings.usecustomHash;
        customHash = fxhashSettings.customHash;
        generatePreview = fxhashSettings.generatePreview;
        previewQuantity = fxhashSettings.previewQuantity;
        previewSuperSize = fxhashSettings.previewSuperSize;
        saveHistory = fxhashSettings.seedsHistory;
    }

    void OnEnable(){

        Initialize();
        
        sO = new SerializedObject(fxhashSettings);
        seedList = new ReorderableList(sO, sO.FindProperty("seeds"),true, false, false, true);

        seedList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>{
            var element = seedList.serializedProperty.GetArrayElementAtIndex(index);

            rect.y += 2;

            GUI.enabled = true;

            GUIStyle seedStyle = new GUIStyle (GUI.skin.label);
            seedStyle.alignment = TextAnchor.MiddleLeft;
            seedStyle.margin.left = 0;
            seedStyle.fontSize = 12;

            GUI.Label( new Rect(rect.x, rect.y, 80, EditorGUIUtility.singleLineHeight), fxhashSettings.seeds[index].date, seedStyle );

            if (GUI.Button(new Rect(105, rect.y, EditorGUIUtility.currentViewWidth - 115, EditorGUIUtility.singleLineHeight), new GUIContent(fxhashSettings.seeds[index].hash, "copy hash"))) {
                GUIUtility.systemCopyBuffer = fxhashSettings.seeds[index].hash;
            }

        };

    }
    
    void OnGUI(){

        if(fxhashSettings == null){
            OnEnable();
        }
        
        scrollPosFx = EditorGUILayout.BeginScrollView(scrollPosFx);

        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        GUI.skin.label.margin.left = 10;

        GUIStyle titleStyle = new GUIStyle (GUI.skin.label);
        GUI.skin.label.fontSize = 15;

        GUI.skin.textField.margin.left = 20;
        GUI.skin.textField.margin.right = 20;

        GUI.skin.horizontalSlider.margin.left = 20;
        GUI.skin.horizontalSlider.margin.right = 20;

        EditorGUI.DrawRect(new Rect(0, 0, Screen.width, 1), new Color32(0,0,0,100));
        EditorGUI.DrawRect(new Rect(0, 1, Screen.width, 32), new Color32(0,0,0,25));
        EditorGUI.DrawRect(new Rect(0, 32, Screen.width, 1), new Color32(0,0,0,100));
        EditorGUI.DrawRect(new Rect(0, 75, Screen.width, 68), new Color32(255,255,255,20));
        EditorGUI.DrawRect(new Rect(0, 185, Screen.width, 98), new Color32(255,255,255,20));

        GUI.enabled = true;

        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal ();
            GUILayout.FlexibleSpace();
            GUILayout.Label ("Fxhash Simulator", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(20);

        GUILayout.Label ("Global Settings", titleStyle);

        GUILayout.Space(15);

        usecustomHash = fxhashSettings.usecustomHash = EditorGUILayout.Toggle("Use Custom Hash", usecustomHash );

        GUILayout.Space(10);
        
        GUI.enabled = fxhashSettings.usecustomHash;
        
        customHash = fxhashSettings.customHash = EditorGUILayout.TextField(customHash);
        
        GUILayout.Space(10);

        DrawUILine(new Color32(0, 0, 0, 100), 1, 0);

        GUILayout.Space(10);

        GUI.enabled = true;
        
        GUILayout.Label ("Preview Settings", titleStyle);
        
        GUILayout.Space(15);
        
        generatePreview = fxhashSettings.generatePreview = EditorGUILayout.Toggle("Generate Preview", generatePreview );

        GUI.enabled = fxhashSettings.generatePreview;
            
        GUILayout.Space(10);
        
        previewQuantity = fxhashSettings.previewQuantity = (int)EditorGUILayout.Slider("Preview Quantity", previewQuantity, 1, 1000 );
        
        GUILayout.Space(10);
    
        previewSuperSize = fxhashSettings.previewSuperSize = (int)EditorGUILayout.Slider("Surpersize Factor", previewSuperSize, 1, 4 );
        
        GUILayout.Space(10);

        GUI.enabled = true;

        DrawUILine(new Color32(0, 0, 0, 100), 1, 0);

        GUILayout.Space(10);

        GUILayout.Label ("History Settings", titleStyle);
        
        GUILayout.Space(15);
        
        saveHistory = fxhashSettings.seedsHistory = EditorGUILayout.Toggle("Save Seed History", saveHistory );

        if(fxhashSettings.seedsHistory){
            GUILayout.Space(11);
            sO.Update();
            seedList.DoLayoutList();
            sO.ApplyModifiedProperties();
        }
            
        GUILayout.Space(10);

        GUI.backgroundColor = Color.white;

        EditorGUILayout.EndScrollView();

        if (GUI.changed){
            EditorUtility.SetDirty(fxhashSettings);
            AssetDatabase.SaveAssets();
        }
    }

    void DrawUILine(Color color, int thickness = 2, int padding = 10){
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding+thickness));
        r.height = thickness;
        r.y += padding/2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }
}

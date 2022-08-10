using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fxhash_Window : EditorWindow{

    Vector2 scrollPosFx;
    static Fxhash_Data fxhashSettings = null;

    [MenuItem("Window/Fxhash Simulator")]
    public static void ShowWindow(){
        EditorWindow.GetWindow<Fxhash_Window>("Fxhash Simulator");
    }

    static bool usecustomHash;
    static string customHash;
    static bool generatePreview;
    static int previewQuantity;
    static int previewSuperSize;

    void Awake(){

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
    }
    
    void OnGUI(){

        if(fxhashSettings == null){
            Awake();
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

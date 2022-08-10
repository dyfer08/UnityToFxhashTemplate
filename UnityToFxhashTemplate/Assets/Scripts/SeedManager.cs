using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

#if UNITY_EDITOR_OSX
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
 
public class PreBuildProcessing : IPreprocessBuildWithReport{
    public int callbackOrder => 1;
    public void OnPreprocessBuild(BuildReport report){
        System.Environment.SetEnvironmentVariable("EMSDK_PYTHON", "/Library/Frameworks/Python.framework/Versions/2.7/bin/python");
    }
}
#endif

public class SeedManager : MonoBehaviour{
    [DllImport("__Internal")]
    private static extern string GetFxhash(); // Get a random float 0->1
    [DllImport("__Internal")]
    private static extern float GetFxrand(); // Get current hash
    [DllImport("__Internal")]
    private static extern bool GetIsFxpreview(); // True if preview mode active, false otherwise
    [DllImport("__Internal")]
    private static extern void TriggerFxpreview(); // Call this method to trigger the preview
    [DllImport("__Internal")]
    private static extern string GetFxfeature(string featureName); // Call this method to get a feature value !! String ONLY
    [DllImport("__Internal")]
    private static extern void TakeScreenshot(byte[] array, int byteLength, string fileName); // Call to take a screenshot and download it

    #if UNITY_EDITOR
    FxhashSimulator fxhashSimulator;
    #endif

    string currentHash;
    float randomValue;

    void Start() {

        #if UNITY_EDITOR
        try{ fxhashSimulator = FxhashSimulator.instance; }catch{ fxhashSimulator = null; }
        #endif

        switch(GetFeature("Sample color")){
            case "White":
                Camera.main.backgroundColor = Color.white;
            break;
            
            case "Red":
                Camera.main.backgroundColor = Color.red;
            break;
            
            case "Green":
                Camera.main.backgroundColor = Color.green;
            break;
            
            case "Blue":
                Camera.main.backgroundColor = Color.blue;
            break;
            
            case "Black":
                Camera.main.backgroundColor = Color.black;
            break;
        }

        currentHash = GetHash();
        Debug.Log("GetHash() : " + currentHash);

        randomValue = GetRandom();
        Debug.Log("GetRandom() : " + randomValue);

        TriggerPreview();
    }

    void OnGUI(){
        GUI.skin.box.alignment = TextAnchor.MiddleLeft;
        GUI.skin.box.fontSize = 20;
        GUI.skin.box.padding = new RectOffset(20, 20, 20, 20);
        GUI.skin.box.normal.background = null;
        GUI.contentColor = Color.white;
        GUI.backgroundColor = Color.black;
        GUI.Box(new Rect(10, 10, 850, 80), "GetHash() : " + currentHash + "\nGetRandom() : " + randomValue);
    }

    IEnumerator CaptureFrame(int superSize){
        yield return new WaitForEndOfFrame();
        var texture = ScreenCapture.CaptureScreenshotAsTexture(superSize);
        byte[] textureBytes = texture.EncodeToPNG();
        TakeScreenshot(textureBytes, textureBytes.Length, "Screenshot.png");
        Destroy(texture);
    }

    void LateUpdate(){
        if(Input.GetKeyDown(KeyCode.S)){
            StartCoroutine(CaptureFrame(1));
        }
    }

    public string GetHash(){
        #if UNITY_WEBGL && !UNITY_EDITOR
            return GetFxhash();
        #elif UNITY_EDITOR
            if(fxhashSimulator == null){
                return null;
            }else{
                return fxhashSimulator.fxhash;
            }
        #endif
    }

    public float GetRandom(){
        #if UNITY_WEBGL && !UNITY_EDITOR
            return GetFxrand();
        #elif UNITY_EDITOR
            if(fxhashSimulator == null){
                return Random.value;
            }else{
                return fxhashSimulator.fxrand();
            }
        #endif
    }

    public bool IsPreview(){
        #if UNITY_WEBGL && !UNITY_EDITOR
            return GetIsFxpreview();
        #elif UNITY_EDITOR
            if(fxhashSimulator == null){
                return false;
            }else{
                return fxhashSimulator.generatePreview;
            }
        #endif
    }

    public void TriggerPreview(){
        #if UNITY_WEBGL && !UNITY_EDITOR
            TriggerFxpreview();
        #elif UNITY_EDITOR
            if(fxhashSimulator == null){
                Debug.LogWarning("You are trying to render a preview in Editor but there is no PreviewRenderer prefab in the scene.");
            }else{
                fxhashSimulator.CreatePreview();
            }
        #endif
    }

    public string GetFeature(string featureName){
        #if UNITY_WEBGL && !UNITY_EDITOR
            return GetFxfeature(featureName);
        #elif UNITY_EDITOR

            // These are testing features that you need to fill manually to mimick your html file features in Unity Editor
            string featureValue = "";
            List<string> testFeatures = new List<string>();

            switch(featureName){

                case "Sample color":
                    testFeatures = new List<string>(){ "White", "Red", "Green", "Blue", "Black"};
                    featureValue = testFeatures[(int)Mathf.Floor(GetRandom() * testFeatures.Count)];
                break;

            }

            return featureValue;
        #endif
    }
}

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

    [SerializeField]
    UnityEngine.UI.Text DebugText;

    void Start() {
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

        DebugText.text += "GetHash() : " + GetHash();
        Debug.Log(GetHash());
        DebugText.text += "\nGetRandom() : " + GetRandom();
        Debug.Log(GetRandom());
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
            return FxhashSimulator.fxhash;
        #endif
    }

    public float GetRandom(){
        #if UNITY_WEBGL && !UNITY_EDITOR
            return GetFxrand();
        #elif UNITY_EDITOR
            return FxhashSimulator.fxrand();
        #endif
    }

    public bool IsPreview(){
        #if UNITY_WEBGL && !UNITY_EDITOR
            return GetIsFxpreview();
        #elif UNITY_EDITOR
            return false;
        #endif
    }

    public void TriggerPreview(){
        #if UNITY_WEBGL && !UNITY_EDITOR
            TriggerFxpreview();
        #elif UNITY_EDITOR
            Debug.LogWarning("TriggerPreview() is not working in Unity editor");
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

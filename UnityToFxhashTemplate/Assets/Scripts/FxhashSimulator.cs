using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine.SceneManagement;
#endif

public class FxhashSimulator : MonoBehaviour{

    public static FxhashSimulator instance;
    string alphabet = "123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
    [HideInInspector]
    public string fxhash;
    uint[] hashes;
    [HideInInspector]
    public bool generatePreview = true;
    int remaining;
    string folderPath;

#if UNITY_EDITOR

    Fxhash_Data fxhashSettings = null;

    void Awake(){

        fxhashSettings = (Fxhash_Data)AssetDatabase.LoadAssetAtPath("Assets/Settings/FxhashSimulator.asset", typeof(Fxhash_Data));

        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
            remaining = fxhashSettings.previewQuantity;
            CreateFolder();
        }else{
            Destroy(gameObject);
        }

        CreateHash();

        generatePreview = fxhashSettings.generatePreview;

    }

    void CreateHash(){
        if(fxhashSettings.usecustomHash){
            fxhash = fxhashSettings.customHash;
        }else{
            fxhash = "oo";
            for(int i=0; i<49; i++){
                fxhash += alphabet[(int)(UnityEngine.Random.value * alphabet.Length)|0];
            }
        }

        string fxhashTrunc = fxhash.Remove(0,2);
        Regex regex = new Regex(".{" + ((fxhash.Length/4)|0) + "}", RegexOptions.None);
        MatchCollection matches = regex.Matches(fxhashTrunc);
        string[] segments = new string[matches.Count];
        for(int i=0; i<matches.Count; i++){
            segments[i] = matches[i].Value;
        }
        hashes = segments.Select(s => b58dec(s)).ToArray();
    }

    public float fxrand(){
        return sfc32(hashes[0], hashes[1], hashes[2], hashes[3]);
    }

    uint b58dec(string segment){
        int hash = (int)segment.Aggregate(0, (p, c) =>p * alphabet.Length + System.Array.IndexOf(alphabet.ToCharArray(),c)|0);
        return (uint)hash;
    }

    float sfc32(uint a, uint b, uint c, uint d){
        a |= 0;
        b |= 0;
        c |= 0;
        d |= 0;
        uint t = (a + b | 0) + d | 0;
        hashes[3] = d = d + 1 | 0;
        hashes[0] = a = b ^ b >> 9;
        hashes[1] = b = c + (c << 3) | 0;
        c = c << 21 | c >> 11;
        hashes[2] = c = c + t | 0;
        return (t >> 0)/ 4294967296f;
    }

    void CreateFolder(){
        string path = "";
        switch(Application.platform){
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);
            break;
        }
        System.IO.Directory.CreateDirectory(path + "/Fxhash");
        System.IO.Directory.CreateDirectory(path + "/Fxhash/" + Application.productName);
        folderPath = path + "/Fxhash" + "/" + Application.productName + "/";
    }

    public void CreatePreview(){
        if(!fxhashSettings.generatePreview || remaining == 0) return;
        remaining --;
        Debug.Log( "Rendering preview " + (fxhashSettings.previewQuantity-remaining) + "/" + fxhashSettings.previewQuantity);
        StartCoroutine(RenderPreview());
    }

    IEnumerator RenderPreview(){
        yield return new WaitForEndOfFrame();
        ScreenCapture.CaptureScreenshot(folderPath + FxhashSimulator.instance.fxhash + ".png", fxhashSettings.previewSuperSize);
        yield return new WaitForSeconds(.5f);
        if(remaining == 0){
            Debug.Log("Rendering preview done !");
            OpenFolder();
        }else{
            CreateHash();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void OpenFolder(){
        if(Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor){
            bool openInsidesOfFolder = true;
            string macPath = "\"" + folderPath + "\"";
            string arguments = (openInsidesOfFolder ? "" : "-R ") + macPath;
            System.Diagnostics.Process.Start("open", arguments);
        }else if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor){
            bool openInsidesOfFolder = true;
            string myWinPath = folderPath;
            string winPath = myWinPath.Replace("/", "\\");
            System.Diagnostics.Process.Start("explorer.exe", (openInsidesOfFolder ? "/root," : "/select,") + winPath);
        }
    }
#endif
}
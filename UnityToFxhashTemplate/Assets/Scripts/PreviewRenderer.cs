using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class PreviewRenderer : MonoBehaviour{

    public static PreviewRenderer instance;
    public bool enabled = true;
    public int quantity = 10;
    [Range(1, 4)]
    public int superSize = 1;
    int remaining;
    string folderPath;

#if UNITY_EDITOR
    void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
            remaining = quantity;
            CreateFolder();
        }else{
            Destroy(gameObject);
        }
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
        if(!enabled || remaining == 0) return;
        remaining --;
        Debug.Log( "Rendering preview " + (quantity-remaining) + "/" + quantity);
        StartCoroutine(RenderPreview());
    }

    IEnumerator RenderPreview(){
        yield return new WaitForEndOfFrame();
        ScreenCapture.CaptureScreenshot(folderPath + FxhashSimulator.instance.fxhash + ".png", superSize);
        yield return new WaitForSeconds(.5f);
        if(remaining == 0){
            Debug.Log("Rendering preview done !");
            OpenFolder();
        }else{
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

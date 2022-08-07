# Fxhash Unity boilerplate

This is a simple Unity project that will help you get started on [fxhash](https://fxhash.xyz).
The version of Unity used is 2020.3.26f1, so I recommend downloading that one.

# Installation

Clone the repository to your machine, make sure you are in your desired folder.

```
$ git clone https://github.com/fgrart/fxhash-unity-boilerplate.git
```

# Description

In Assets/Scripts/ there is a RandomExample.cs script, the comments are explaining which lines are relevant to fxhash
![image](https://user-images.githubusercontent.com/98949354/152338865-2ae252d7-8e5a-43e7-98f3-1602dd672524.png)

This part references a function that is declared in the Assets/Plugins/plugin.jslib, which takes the random from the fxhash's html snippet
```
[DllImport("__Internal")]
    private static extern float GetRandomFromFxhash();
```

In order to test random behavior you can use Unity's native Random.value or Random.Range() because you cannot test with fxhash in editor, that's we have #if UNITY_EDITOR and #if UNITY_WEBGL to specify in which environment which part of the code should run.

Once you build for WebGL and you upload it to the site, the UNITY_WEBGL code will run.

# Building steps

1.
Go to Player Build Settings like below(or pressing Ctrl+Shift+B).

![image](https://user-images.githubusercontent.com/98949354/152337326-83a37539-547e-495f-8912-028cf4e8ce47.png)

2.
Select WebGL as your platform by selecting it and pressing Switch Platform in the lower right part of the ui, if it's already set to WebGL(Unity Icon next to it) ignore this step.

![image](https://user-images.githubusercontent.com/98949354/152337573-d89d2aab-af99-4115-a16c-75b488b81606.png)

3.
Select the Player Settings.

![image](https://user-images.githubusercontent.com/98949354/152337747-8412356a-87df-402d-9ad7-d736ab3ac931.png)

4.
Add your naming and select the WebGL tab.

![image](https://user-images.githubusercontent.com/98949354/152337845-00fcfcfe-2ea7-4c2e-9ffb-10c3c395a8d4.png)

5.
In resolution and presentation you set the canvas size, although you can edit that in the index.html after the build.
Select the minimal template.

![image](https://user-images.githubusercontent.com/98949354/152337973-4c94df0a-336f-49ae-afb3-d9aeaf3a2e22.png)

6.
In Other Settings uncheck Auto Graphics API.

![image](https://user-images.githubusercontent.com/98949354/152338140-d923f81b-e65b-45bb-91b8-80af0d974ca6.png)

7.
In Publishing Settings set Compression Format to Disabled.

![image](https://user-images.githubusercontent.com/98949354/152338238-f018b47e-0094-4ee6-bae9-5e915a0d43b1.png)

8.
Close Player Settings, and press Build.

![image](https://user-images.githubusercontent.com/98949354/152338441-65c76b79-76ca-4be9-9b76-32d5cb7c1a2d.png)

9.
After the build is complete you will get these 2 files:

![image](https://user-images.githubusercontent.com/98949354/152339616-c1d11ef9-9bf3-4a4c-8848-d006ed7dff97.png)

Open index.html and add the code found in the FxhashUnityBoilerplate/ root called "fxhashsnippet.txt" under the <title> tags
.
![image](https://user-images.githubusercontent.com/98949354/152339993-2d33bc6a-5a34-4d5b-a587-8731ed254180.png)

Fxhash snippet is lines 7-36 and 38-42 is the part that translates the snippets fxrand to GetRandomFromFxhash() in plugin.jslib which we then use in C# as our random generator.
  
10.
Once you made the edits in index.html, make a .zip from both files and it's ready to be uploaded to fxhash for testing.
Note: For some reason this won't work in the regular browser window, you have to go to incognito mode to test in sandbox, but it should work when you actually mint it.
  
11.
Upload the .zip to sandbox and test with same and different hashes to see how the value behaves.
It should look like this:
  
![image](https://user-images.githubusercontent.com/98949354/152340836-53f6a9ce-357b-4eff-bde8-c9f05b37675f.png)
  
# Conclusion
  
That's it! You can now use the value however you want to manipulate different aspects of your artworks

# Credits
  
By [GabrieleGenArt](https://www.fxhash.xyz/u/Gabriele%20GenArt), [nekropunk](https://www.fxhash.xyz/u/nekropunk), [lomz](https://www.fxhash.xyz/u/lomz), [ciphrd](https://www.fxhash.xyz/u/ciphrd) and [fgra](https://www.fxhash.xyz/u/fgra)

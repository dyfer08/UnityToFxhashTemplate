# UnityToFxhashTemplate
This is a basic Unity setup to export fxhash compatible WebGL projects

## Save Manager
An easy to use encrypted game save manager. Create, load, save game data on any platform. It's using a SHA256 encryption and warns you if the game save has been modified or the SHA key is missing so you can decide what to do. You can create as many game save as you want by passing a slot int.

**Download** :
- [SaveManager.unitypackage.zip](https://github.com/dyfer08/UnityTools/raw/master/Unity%20Tools/Assets/Unity%20Packages/SaveManager.unitypackage.zip)

**How to use** :
- Just drag and drop the SaveManager prefab in your scene. It is a DontDestroyOnLoad singleton object.
- Create or load an existing game save in a specific slot. If the slot is empty, it creates a new game save in this slot.
**You need to create or load a game save to be able to use most of the SaveManager functions( eg : GetData, SetData, SaveGame ).**
  ```csharp
  SaveManager.LoadOrCreateGameSave(int Slot);
  ```
  When loading a game save, the SaveManager checks it's integrity with a SHA256 encryption key. If the game save has been modified or the SHA256 key is missing, it triggers a Debug.LogWarning and you can decide to ignore it or react as you wish.

- Get the list of all game saves in the Application.persistentDataPath folder. It returns a list of int, each int being a slot containing a game save.
  ```csharp
  List<int> 

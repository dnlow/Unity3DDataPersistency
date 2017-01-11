using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

/// <summary>
///  Needs to persist in Every level. "Game Master" 
///  This sets to don't destroy on load, therefore it will follow the game scene transitions.
/// </summary>


public class GlobalGameControl : MonoBehaviour
{
    public static GlobalGameControl Instance;

    internal PlayerStatistics savedPlayerData = new PlayerStatistics();

    //LIST OF GAME NPCS
    internal List<SavedNPCList> GameNPCS;

    //LIST OF GAME ITEMS
    internal List<SavedLevelItemList> GameItems;

    //LIST OF GAME QUESTS
    internal List<SavedQuestList> GameQuests;

    //CURRENT SCENE INFO
    internal SceneInfo currentScene;

    internal SaveData currentGameState;
    internal SaveData currentSaveGame;

    public List<GameObject> gamePrefabs;

    UIManager UI;

    public GameObject Player;

    public delegate void SaveDelegate(object sender, EventArgs args);
    public static event SaveDelegate SaveEvent;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        UI = FindObjectOfType<UIManager>();

        ActiveStateOfChildren(false);

        SceneManager.LoadScene("MainMenu");
    }

    internal void ActiveStateOfChildren(bool state)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(!transform.GetChild(i).gameObject.name.Equals("EventSystem"))
                transform.GetChild(i).gameObject.SetActive(state);
        }

        if (state)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1.0f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 1.0f;
        }
    }

    public void StartPreviousGame()
    {
        // Check if there is a save game created (this is handy for when implementing a main menu "continue" button), if there is a save, set it to the current game state and current save game
        if (File.Exists(Application.persistentDataPath + "/gameSave.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameSave.dat", FileMode.Open);
            currentSaveGame = (SaveData)bf.Deserialize(file);
            file.Close();

            currentGameState = currentSaveGame;
            savedPlayerData = currentGameState.playerStats;
            LoadGame();
        }
        else
        {
            StartNewGame();
        }
        ActiveStateOfChildren(true);
    }

    public void StartNewGame()
    {
        currentGameState = new SaveData();
        TransitionScene(SceneManager.GetActiveScene().buildIndex, 1);
    }

    public void InitializeScene(int currentSceneIndex)
    {

        // Check if current scene has been initialized before
        // if not, we need to create a "SceneINfo" instance for the current scene, then add it to the current game state list of scene infos
        // if it has been initialized before, we need to set the current state to the gamestate save data scene info
        if(currentScene == null)
        {
            print("No Scene Info for this scene");
            currentScene = new SceneInfo(currentSceneIndex);
            currentGameState.sceneInfo.Add(currentScene);
        }
        else
        {
            currentScene = currentGameState.sceneInfo[currentSceneIndex];
        }
    }

    void TransitionScene(int currentSceneIndex, int nextSceneIndex)
    {
        // fire a save event to update currentScene info and then transition currentScene into the next scene's sceneInfo (currentGameState.sceneInfo[transitionSceneIndex])
        // LoadNewScene with transitionSceneIndex
        FireSaveEvent();

        currentGameState.activeScene = nextSceneIndex;    // set active scene for the "SaveData"

        SceneManager.LoadSceneAsync("LoadingScene"); //moves screen to a loading screen while the next scene is loaded
        IsSceneBeingLoaded = true;
        SceneManager.LoadSceneAsync(nextSceneIndex);    // beings loading next scene after the loading screen has been loaded, then removes loading screen
        IsSceneBeingTransitioned = true;
        ActiveStateOfChildren(true);
    }

    public void SaveGame()
    {
        // Fire a save event to save the current scene, all other scenes will have already been saved in their respective states.
        // take SaveData and save to file, taking note of the current active scene in SaveData.activeScene
        FireSaveEvent();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream saveFile = File.Create(Application.persistentDataPath + "/gameSave.dat");

        currentGameState.playerStats = savedPlayerData;
        currentGameState.activeScene = currentScene.sceneID;

        bf.Serialize(saveFile, currentGameState);
        saveFile.Close();

        currentSaveGame = currentGameState;

        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        // take File and load into SaveData, making sure to load the active scene (SaveData.activeScene)

        if (File.Exists(Application.persistentDataPath + "/gameSave.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameSave.dat", FileMode.Open);
            currentSaveGame = (SaveData)bf.Deserialize(file);
            currentGameState = currentSaveGame;
            file.Close();
        }

        UI.UnPause();
        UI.transform.GetChild(6).gameObject.SetActive(false);

        // Load the save games active scene

        SceneManager.LoadSceneAsync("LoadingScene");
        SceneManager.LoadSceneAsync(currentSaveGame.activeScene);
        ActiveStateOfChildren(true);
    }

    //public PlayerStatistics LocalCopyOfData;
    public bool IsSceneBeingLoaded = false;
    public bool IsSceneBeingTransitioned = false;

    void FireSaveEvent()
    {
        //If we have any functions in the event:
        if (SaveEvent != null)
            SaveEvent(null, null);
    }

    public void QuitToMM()
    {
        TransitionScene(currentScene.sceneID, 2);
        ActiveStateOfChildren(false);
    }
}

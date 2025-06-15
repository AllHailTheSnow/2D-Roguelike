using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;

    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        //creates a new data handler object with the path to the persistent data path and the file name
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);

        //Find all save managers in the scene
        saveManagers = FindAllSaveManagers();

        //Load the game data
        LoadGame();
    }

    [ContextMenu("Delete Saved Data")]
    public void DeleteSavedData()
    {
        //Delete the saved data file 
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHandler.DeleteFile();
    }

    public bool HasSavedData()
    {
        if(dataHandler.Load() != null)
        {
            return true;
        }

        return false;
    }

    public void NewGame()
    {
        //Create a new game data object
        gameData = new GameData();
    }
    public void SaveGame()
    {
        //loop through all save managers and save the data
        foreach (ISaveManager saveManager in saveManagers)
        {
            //call the save data method
            saveManager.SaveData(ref gameData);
        }

        //Save the game data to the file
        dataHandler.Save(gameData);
    }

    public void LoadGame()
    {
        //Load the game data from the file
        gameData = dataHandler.Load();

        //Check if the game data is null
        if (this.gameData == null)
        {
            //Calls the new game method
            Debug.Log("No game data found. Starting a new game.");
            NewGame();
        }

        //loop through all save managers and load the data
        foreach (ISaveManager saveManager in saveManagers)
        {
            //call the load data method
            saveManager.LoadData(gameData);
        }
    }

    private void OnApplicationQuit()
    {
        //Save the game data when the application quits
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        //Find all objects of type MonoBehaviour in the scene and filter them to get only the ISaveManager components
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();

        //Convert the IEnumerable to a List and return it
        return new List<ISaveManager>(saveManagers);
    }
}

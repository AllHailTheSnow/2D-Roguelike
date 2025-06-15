using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    private bool encryptData = false;
    private string password = "zvlf7gh7qjx74uj2rrg3t1vlmkj6hx5wdstihjgi08uvm4g9hc5dcwm6elewygx4uh1y6wdozln9gj1svf9";

    public FileDataHandler(string _dataDirPath, string _dataFileName, bool _encryptData)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
        encryptData = _encryptData;
    }

    public void Save(GameData _data)
    {
        //create a new string to store the modified data
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            //create the directory if it does not exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //create the data to store
            string dataToStore = JsonUtility.ToJson(_data, true);

            //encription to be added here
            if (encryptData)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            //create a new file stream to write the data to
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                //create a new stream writer to write the data to the file
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    //write the data to the file
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            //catch any exceptions and log them
            Debug.LogError("Failed to save data to " + fullPath + "\n" + e);
        }
    }

    public GameData Load()
    {
        //create a new string to store the modified data
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        //set the load data to null
        GameData loadData = null;

        //check if the file exists
        if (File.Exists(fullPath))
        {
            try
            {
                //create a new file stream to read the data from
                string dataToLoad = "";

                //create a new stream reader to read the data from the file
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    //create a new stream reader to read the data from the file
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        //read the data from the file
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //encription to be added here
                if (encryptData)
                {
                    //decrypt the data
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                //set the data to load to the data from the file
                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                //catch any exceptions and log them
                Debug.LogError("Failed to load data from " + fullPath + "\n" + e);
            }
        }

        return loadData;
    }

    public void DeleteFile()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    private string EncryptDecrypt(string _data)
    {
        //create a new string to store the modified data
        string modifiedData = "";

        //loop through the data and modify it using the password
        for (int i = 0; i < _data.Length; i++)
        {
            modifiedData += (char)(_data[i] ^ password[i % password.Length]);
        }

        //return the modified data
        return modifiedData;
    }
}

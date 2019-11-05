using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    static string savePath = Path.Combine(Application.persistentDataPath, "levels.bin"); //What our save file will be called
    static BinaryFormatter binFormatter = new BinaryFormatter(); //Serializes and deserializes the data for us <3 

    //Saves the necessary data for all of the levels
    public static void SaveLevels(LevelDatabase db)
    {
        FileStream stream = new FileStream(savePath, FileMode.Create); //Creates a new file for us to throw our save data in

        LevelData data = new LevelData(db); //Gets all the save data that is currently living in our LevelDatabase object

        binFormatter.Serialize(stream, data); //Serializes all our data

        stream.Close();//Close out our stream because we don't need it anymore
    }

    //Loads the necessary data for all of the levels
    public static LevelData LoadLevels()
    {
        if(File.Exists(savePath)) //If there's actually a save file that exists
        {
            FileStream stream = new FileStream(savePath, FileMode.Open); //Open it up

            LevelData data = binFormatter.Deserialize(stream) as LevelData; //Deserialize it as a LevelData object

            stream.Close();//Close out the stream

            return data; //return that good good level info
        }
        else
        {
            Debug.LogError("Save file not found in " + savePath); //Ruh-Roh
            return null;
        }
    }
}

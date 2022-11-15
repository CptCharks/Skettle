using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(PlayerController controller)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.skidaddle";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(controller);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.skidaddle";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = (PlayerData)formatter.Deserialize(stream);

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file no found in " + path);
            return null;
        }
    }

    public static void SaveProgress(ProgressContainer container)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/progress.skidaddle";
        FileStream stream = new FileStream(path, FileMode.Create);

        ProgressionData data = new ProgressionData(container);

        formatter.Serialize(stream, data);
        stream.Close();
    }


    public static ProgressionData LoadProgress()
    {
        Debug.Log(Application.persistentDataPath);
        string path = Application.persistentDataPath + "/progress.skidaddle";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ProgressionData data = (ProgressionData)formatter.Deserialize(stream);

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file no found in " + path);
            return null;
        }
    }
}

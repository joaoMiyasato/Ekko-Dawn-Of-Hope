using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(PlayerManager player, string _path)
    {
        string folder = Application.persistentDataPath + "/saves/saveSlot"+Save_Load.instance.saveSlot+"/";
        Directory.CreateDirectory(folder);
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + _path;
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer(string _path)
    {
        string path = Application.persistentDataPath + _path;
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
            Debug.Log("Este save não existe!!!" + path);
            return null;
        }
    }
}

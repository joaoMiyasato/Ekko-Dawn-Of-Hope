using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

[CreateAssetMenu(fileName = "New Interactable", menuName = "Interactable/Inter")]
public class InteractableObject : ScriptableObject
{
    public List<InteractableObj> Container = new List<InteractableObj>();

    #region Save/Load
    public void Save(string _path)
    {
        
        string folder = Application.persistentDataPath + "/saves/";
        Directory.CreateDirectory(folder);
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, _path));
        bf.Serialize(file, saveData);
        file.Close();
    }
    public void Load(string _path)
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, _path)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, _path), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    }

#endregion
}

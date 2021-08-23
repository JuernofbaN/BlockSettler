using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static void SaveData(Player veri)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/players.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        Player savedVeri = new Player(veri);
        binaryFormatter.Serialize(stream, savedVeri);
        stream.Close();
    }

    public static Player LoadData()
    {
        string path = Application.persistentDataPath + "/players.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Player loadVeri = formatter.Deserialize(stream) as Player;
            stream.Close();
            Debug.Log("Buldum saveledim");
            return loadVeri;

        }
        else
        {
            Debug.Log("Bulamadım veriyi");
            Player loadVeri = new Player(1, 1);
            return loadVeri;
        }
    }
}

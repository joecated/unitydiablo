// SaveSystem.cs (DOÐRU HALÝ)
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    // ARTIK GameManager DEÐÝL, DOÐRUDAN PlayerData ALIYOR
    public static void SavePlayer(PlayerData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        // PlayerData ZATEN HAZIR GELDÝÐÝ ÝÇÝN YENÝ BÝR TANE OLUÞTURMUYORUZ.
        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("Oyun kaydedildi: " + path);
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.sav";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            Debug.Log("Oyun yüklendi: " + path);
            return data;
        }
        else
        {
            Debug.LogError("Kayit dosyasi bulunamadi: " + path);
            return null;
        }
    }
}
// SaveSystem.cs (DO�RU HAL�)
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    // ARTIK GameManager DE��L, DO�RUDAN PlayerData ALIYOR
    public static void SavePlayer(PlayerData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        // PlayerData ZATEN HAZIR GELD��� ���N YEN� B�R TANE OLU�TURMUYORUZ.
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
            Debug.Log("Oyun y�klendi: " + path);
            return data;
        }
        else
        {
            Debug.LogError("Kayit dosyasi bulunamadi: " + path);
            return null;
        }
    }
}
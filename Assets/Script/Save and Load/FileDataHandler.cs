using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool isEncrypt;
    private readonly string codeWord = "JiaMiCodeWord";
    public FileDataHandler(string _dataDirPath, string _dataFileName, bool _isEncrypt)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
        isEncrypt = _isEncrypt;
    }

    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(_data, true);

            if (isEncrypt)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("FillDataHandler - Save" + e);
        }
    }
    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadData = new GameData();

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                if (isEncrypt)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }
                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("FillDataHandler - Load" + e);
            }
        }
        else
        {
            return null;
        }

        return loadData;

    }

    public void Delete()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if (File.Exists(fullPath))
        {
            try
            {
                File.Delete(fullPath);
            }
            catch (Exception e)
            {
                Debug.LogError("FillDataHandler - Delete" + e);
            }
        }
    }
    /// <summary>异或加/解密函数</summary>
    private string EncryptDecrypt(string _data)
    {
        string modifiedData = "";
        for (int i = 0; i < _data.Length; i++)
        {
            modifiedData += (char)(_data[i] ^ codeWord[i % codeWord.Length]);
        }
        return modifiedData;

    }
}

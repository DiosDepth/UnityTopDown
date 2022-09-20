using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    public bool isTestMode;

    private const string _baseFolderName = "/Save/";
    private const string _defaultFolderName = "SaveLoadManager";


    // Start is called before the first frame update
    public override void Start()
    {
        if(isTestMode)
        {
            Initialization();
        }
    }

    public override void Initialization()
    {
        base.Initialization();
    }

    protected string DetermineSavePath(string foldername = _defaultFolderName)
    {
        string savepath;

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            savepath = Application.persistentDataPath + _baseFolderName;
        }
        else
        {
            savepath = Application.persistentDataPath + _baseFolderName;
        }
#if UNITY_EDITOR
        savepath = Application.dataPath + _baseFolderName;
#endif

        savepath = savepath + foldername + "/";
        return savepath;

    }

    static string DetermineSaveFileName(string fileName)
    {
        return fileName + ".binary";
    }


    public void Save(object saveObject, string fileName, string foldername = _defaultFolderName)
    {
        string savePath = DetermineSavePath(foldername);
        string saveFileName = DetermineSaveFileName(fileName);
        // if the directory doesn't already exist, we create it
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        // we serialize and write our object into a file on disk
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create(savePath + saveFileName);
        formatter.Serialize(saveFile, saveObject);
        saveFile.Close();
    }

    public  object Load(string fileName, string foldername = _defaultFolderName)
    {
        string savePath = DetermineSavePath(foldername);
        string saveFileName = savePath + DetermineSaveFileName(fileName);

        object returnObject;

        // if the MMSaves directory or the save file doesn't exist, there's nothing to load, we do nothing and exit
        if (!Directory.Exists(savePath) || !File.Exists(saveFileName))
        {
            return null;
        }
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Open(saveFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        returnObject = formatter.Deserialize(saveFile);
        saveFile.Close();

        return returnObject;
    }

    public void DeleteSaveFolder(string folderName = _defaultFolderName)
    {
        string savePath = DetermineSavePath(folderName);
        if (Directory.Exists(savePath))
        {
            DeleteDirectory(savePath);
        }
    }

    public void DeleteDirectory(string target_dir)
    {
        string[] files = Directory.GetFiles(target_dir);
        string[] dirs = Directory.GetDirectories(target_dir);

        foreach (string file in files)
        {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }

        foreach (string dir in dirs)
        {
            DeleteDirectory(dir);
        }

        Directory.Delete(target_dir, false);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class GameStarter : Editor
{
    protected static string QuickScenePathF1 = "Assets/Scenes/MainMenu.unity";
    [MenuItem("Game/GameStart %F1")]//Ctrl+F1
    public static void GameStart()
    {
        if(EditorUtility.DisplayDialog("StartGame" + QuickScenePathF1, "Everything Not Saved Will Be Lost!", "Yes,I willing to start","No, Let'm save first") == false)
        {
            return;
        }
        EditorApplication.isPaused = false;
        EditorApplication.isPlaying = false;
        EditorSceneManager.OpenScene(QuickScenePathF1);
        EditorApplication.isPlaying = true;
    }

}

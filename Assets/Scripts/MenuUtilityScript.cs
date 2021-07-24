using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUtilityScript : MonoBehaviour
{
   public static void QuitGame(){
       #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
       Application.Quit();
   }
   public static void LoadLevel(int buildindex){
       UnityEngine.SceneManagement.SceneManager.LoadScene(buildindex);

   }
}

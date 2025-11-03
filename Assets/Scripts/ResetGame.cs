using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class ResetGame : MonoBehaviour
{
    [ContextMenu("Reset Game Now")]
    public void ResetGameNow()
    {
        PlayerPrefs.DeleteAll();

        // If in Play Mode → reload the scene
        if (Application.isPlaying)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        // If in Editor (not playing) → reopen the scene
        #if UNITY_EDITOR
        else
        {
            string scenePath = SceneManager.GetActiveScene().path;
            EditorSceneManager.OpenScene(scenePath);
            Debug.Log("Scene reloaded in Editor mode.");
        }
        #endif
    }
}

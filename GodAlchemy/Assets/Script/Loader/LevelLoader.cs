using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;

    [SerializeField]
    private SceneReference sceneAfterLoading;

    public void LoadLevel() {
        StartCoroutine(LoadAsync());
    }

    IEnumerator LoadAsync() {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneAfterLoading);
        loadingScreen.SetActive(true);

        while (!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
    }
}

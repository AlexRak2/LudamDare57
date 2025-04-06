using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LD57.UI;
using System;

namespace LD57
{
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private SettingsManager settingsManager;
    public SettingsManager SettingsManager => settingsManager;
    public CanvasGroup FadingCanvas;
    public Action<bool> OnFadeComplete;
    [SerializeField] private bool LoadMainMenuOnStart = false;
    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        #if !UNITY_EDITOR
            LoadMainMenuOnStart = true;
        #endif

        if(LoadMainMenuOnStart) {
            LoadMainMenu();
        } else{
            FadingCanvas.alpha = 0;
        }
    }
    public void LoadMainMenu()
    {
        StartCoroutine(UnloadSceneCoroutine(SceneIndexes.Level1));
        StartCoroutine(LoadSceneCoroutine(SceneIndexes.MainMenu));
    }

    public void LoadLevel1()
    {
        StartCoroutine(UnloadSceneCoroutine(SceneIndexes.MainMenu));
        StartCoroutine(LoadSceneCoroutine(SceneIndexes.Level1));
    }

    private IEnumerator LoadSceneCoroutine(SceneIndexes sceneIndex)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync((int)sceneIndex, LoadSceneMode.Additive);
        loadOperation.allowSceneActivation = false;

        while (loadOperation.progress < 0.9f) {
            // Debug.Log("Loading Progress for " + sceneIndex + ": " + (loadOperation.progress * 100) + "%");
            yield return null;
        }

        loadOperation.allowSceneActivation = true;

        while (!loadOperation.isDone) {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)sceneIndex));
        StartCoroutine(FadeCanvas(false));
    }
    private IEnumerator UnloadSceneCoroutine(SceneIndexes sceneIndex)
    {
        //check if scene is loaded
        if(SceneManager.GetSceneByBuildIndex((int)sceneIndex).isLoaded == false) {
            yield break;
        }
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync((int)sceneIndex);
        unloadOperation.allowSceneActivation = false;

        while (!unloadOperation.isDone) {
            yield return null;
        }
    }
    public IEnumerator FadeCanvas(bool _in)
    {
        // Debug.Log($"Fade canvas {_in}");

        float duration = 2f;
        float startAlpha = FadingCanvas.alpha; // Use current alpha as start
        float targetAlpha = _in ? 1f : 0f;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration;
            FadingCanvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            // Debug.Log($"t {t}, Alpha {FadingCanvas.alpha}");
            yield return null;
        }

        // Ensure final alpha is exact
        FadingCanvas.alpha = targetAlpha;
        // Debug.Log($"Done fading canvas {_in}");

        OnFadeComplete?.Invoke(_in);
    }
}
}

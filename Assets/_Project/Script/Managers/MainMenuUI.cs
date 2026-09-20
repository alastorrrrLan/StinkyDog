using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup canvasGroup;
    public GameObject creditsPanel;
    public Button startButton;
    public Button creditsButton;
    public Button exitButton;

    [Header("Settings")]
    public float fadeDuration = 5.0f;
    public string nextSceneName = "SampleScene";   // level scene name

    bool busy = false;

    void Start()
    {
        // Fade in
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            StartCoroutine(Fade(1f));
        }
    }

    public void OnStartClicked()
    {
        if (busy) return;
        busy = true;

        // Click sound effect
        if (UIAudio.Instance != null)
            UIAudio.Instance.PlayClick();

        // stop button interaction
        if (startButton) startButton.interactable = false;
        if (creditsButton) creditsButton.interactable = false;
        if (exitButton) exitButton.interactable = false;

        // fade out
        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeAndLoad()
    {
        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = false;
        yield return StartCoroutine(Fade(0f));

        // wait for sound
        yield return new WaitForSecondsRealtime(3.0f);

        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator Fade(float targetAlpha)
    {
        if (canvasGroup == null) yield break;

        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;   // unscaled
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }

    public void OnCreditsClicked()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(true);
    }

    public void OnBackClicked()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(false);
    }

    public void OnExitClicked()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
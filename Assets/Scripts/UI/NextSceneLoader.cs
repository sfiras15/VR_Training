using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextSceneLoader : MonoBehaviour
{
    // the logo that will fade in/out
    [SerializeField] private Image logo;
    [SerializeField] private GameObject splashScreen;
    [SerializeField] private float fadeInDuration = 3f;
    [SerializeField] private float fadeOutDuration = 3f;
    public void StartGame()
    {
        // Activate splash screen and start loading the "Test" scene asynchronously
        StartCoroutine(ShowSplashScreenAndLoadScene());
    }

    private IEnumerator ShowSplashScreenAndLoadScene()
    {
        splashScreen.gameObject.SetActive(true);
        logo.gameObject.SetActive(true);
        // Start loading the "Test" scene asynchronously
        // Make sure the mainLevel scene is after the tutorial scene in the settings
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        asyncOperation.allowSceneActivation = false;
        //Debug.Log("Scene progress : " + asyncOperation.progress);
        float currentTime = 0f;
        while (currentTime < fadeInDuration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 0.9f, currentTime / fadeInDuration);
            logo.color = new Color(1f, 1f, 1f, alpha);
            //Debug.Log(logo.color);
            yield return null;
        }
        //Debug.Log("Scene progress : " + asyncOperation.progress);
        // Fade out

        currentTime = 0f;
        while (currentTime < fadeOutDuration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0.9f, 0f, currentTime / fadeOutDuration);
            logo.color = new Color(1f, 1f, 1f, alpha);
            //Debug.Log(logo.color);
            yield return null;
        }

        // Ensure the logo is completely faded out
        logo.color = new Color(1f, 1f, 1f, 0f);

        // Allow scene activation
        asyncOperation.allowSceneActivation = true;
    }
    public void QuitGame()
    {
        // Log message for when the game is played in the editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}


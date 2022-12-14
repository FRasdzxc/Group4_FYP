using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject canvasAcrossScenes;
    [SerializeField] private GameObject canvasAcrossScenesMask;

    private string selectedSceneName;

    public void SetScene(string sceneName) // temporary
    {
        selectedSceneName = sceneName;
    }

    public async void ChangeScene(string sceneName) // not used for now?
    {
        canvasAcrossScenes.SetActive(true);
        await canvasAcrossScenesMask.GetComponent<RectTransform>().DOScale(new Vector2(0, 0), 0.5f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        DontDestroyOnLoad(canvasAcrossScenes);
        await WaitForSceneToLoad(sceneName);
        await canvasAcrossScenesMask.GetComponent<RectTransform>().DOScale(new Vector2(1, 1), 0.5f).SetEase(Ease.InQuart).AsyncWaitForCompletion();
        canvasAcrossScenes.SetActive(false);
    }

    public async void EnterScene() // temporary
    {
        if (SceneSelected())
        {
            canvasAcrossScenes.SetActive(true);
            await canvasAcrossScenesMask.GetComponent<RectTransform>().DOScale(new Vector2(0, 0), 0.5f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
            DontDestroyOnLoad(canvasAcrossScenes);
            await WaitForSceneToLoad(selectedSceneName);
            await canvasAcrossScenesMask.GetComponent<RectTransform>().DOScale(new Vector2(1, 1), 0.5f).SetEase(Ease.InQuart).AsyncWaitForCompletion();
            canvasAcrossScenes.SetActive(false);
        }
    }

    public bool SceneSelected() // temporary
    {
        return (selectedSceneName != null && selectedSceneName != "");
    }

    private async Task WaitForSceneToLoad(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            await Task.Yield();
        }
    }
}

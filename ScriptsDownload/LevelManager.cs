using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public GameObject nextLevelUI;
    public float delayBeforeNextScene = 5f;

    public void LoadNextLevel()
    {
        StartCoroutine(NextLevelRoutine());
    }

    private IEnumerator NextLevelRoutine()
    {
        nextLevelUI.SetActive(true);
        yield return new WaitForSeconds(delayBeforeNextScene);
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex + 1);
    }
}
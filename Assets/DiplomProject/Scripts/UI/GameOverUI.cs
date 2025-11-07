using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Button restartButton;
    public GameObject blackScreen;


    private void Awake()
    {
        // Панель скрыта
        gameOverPanel.SetActive(false);
        blackScreen.SetActive(false);
    }

    public void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true);
        blackScreen.SetActive(true);
       
        // Показываем курсор
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Останавливаем игру
        Time.timeScale = 0f;

        restartButton.onClick.AddListener(RestartGame);
    }

    public void RestartGame()
    {

        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Перезапуск текущей сцены
    }
}
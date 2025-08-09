using UnityEngine;

public class MainMenuButtonManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    void Start()
    {
        mainMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        mainMenu.SetActive(false);
    }
}

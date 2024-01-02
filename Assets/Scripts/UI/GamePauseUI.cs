using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() => 
        {
            GameManager.Instance.TogglePauseGame();
        });

        mainMenuButton.onClick.AddListener(() => 
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        optionsButton.onClick.AddListener(() =>
        {
            OptionsMenuUI.Instance.Show();
        });
    }
    private void Start()
    {
        Hide();
        GameManager.Instance.OnGamePaused += OnGamePaused_PauseMenuUI;
        GameManager.Instance.OnGameUnpaused += OnGameUnpaused_PauseMenuUI;
    }

    private void OnGamePaused_PauseMenuUI(object sender, System.EventArgs e)
    {
        Show();
    }

    private void OnGameUnpaused_PauseMenuUI(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    { 
        gameObject.SetActive(false); 
    }
}

using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Mirror;

public class UIManager : NetworkBehaviour
{
    public UIDocument uiDocument; // UI Document referansı

    private Button homeButton;
    private Button resumeButton;
    private Button restartButton;
    private Button pauseButton;
    private VisualElement pausedMenu;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        var root = uiDocument.rootVisualElement;

        // Butonları ve menüyü bul
        homeButton = root.Q<Button>("homeButton");
        resumeButton = root.Q<Button>("resumeButton");
        restartButton = root.Q<Button>("restartButton");
        pauseButton = root.Q<Button>("pauseButton");
        pausedMenu = root.Q<VisualElement>("pausedMenu");

        // Olay işleyicileri bağla
        homeButton.clicked += OnHomeButtonClicked;
        resumeButton.clicked += OnResumeButtonClicked;
        restartButton.clicked += OnRestartButtonClicked;
        pauseButton.clicked += OnPauseButtonClicked;

        // Başlangıçta pausedMenu'yu gizle
        pausedMenu.style.display = DisplayStyle.None;
    }

    private void OnDisable()
    {
        if (homeButton != null) homeButton.clicked -= OnHomeButtonClicked;
        if (resumeButton != null) resumeButton.clicked -= OnResumeButtonClicked;
        if (restartButton != null) restartButton.clicked -= OnRestartButtonClicked;
        if (pauseButton != null) pauseButton.clicked -= OnPauseButtonClicked;
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            OnPauseButtonClicked();
        }
    }

    [Command]
    void CmdTogglePause()
    {
        RpcTogglePause();
    }

    [ClientRpc]
    void RpcTogglePause()
    {
        if (pausedMenu.style.display == DisplayStyle.None)
        {
            pausedMenu.style.display = DisplayStyle.Flex;
            Time.timeScale = 0; // Oyunu durdur
        }
        else
        {
            pausedMenu.style.display = DisplayStyle.None;
            Time.timeScale = 1; // Oyunu devam ettir
        }
    }

    private void OnHomeButtonClicked()
    {
        CmdHome();
    }

    [Command]
    void CmdHome()
    {
        RpcHome();
    }

    [ClientRpc]
    void RpcHome()
    {
        Time.timeScale = 1; // Oyunu devam ettir
        SceneManager.LoadScene("MainMenu");
    }

    private void OnResumeButtonClicked()
    {
        CmdResume();
    }

    [Command]
    void CmdResume()
    {
        RpcResume();
    }

    [ClientRpc]
    void RpcResume()
    {
        pausedMenu.style.display = DisplayStyle.None;
        Time.timeScale = 1; // Oyunu devam ettir
    }

    private void OnRestartButtonClicked()
    {
        CmdRestart();
    }

    [Command]
    void CmdRestart()
    {
        RpcRestart();
    }

    [ClientRpc]
    void RpcRestart()
    {
        Time.timeScale = 1; // Oyunu devam ettir
        SceneManager.LoadScene("Level1");
    }

    private void OnPauseButtonClicked()
    {
        CmdTogglePause();
    }
}

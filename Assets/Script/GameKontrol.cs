using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Mirror;

public class GameKontrol : NetworkBehaviour
{
    [Header("Top Settings and Effects")]
    public GameObject TopYokOlmaEfekt; // Effect for ball disappearance
    public AudioSource YokOlmaSesi;  
    public AudioSource GameOverSes;   // Sound effect for game over
    public UIDocument uiDocument; // UI Toolkit for UIDocument reference

    [Header("Player Settings")]
    public GameObject Oyuncu_1;
    public GameObject Oyuncu_2;
    public GameObject DefeatEffectPrefab; // Prefab for the defeat effect
    public GameObject YaziEfekt; // The text effect prefab

    [SyncVar(hook = nameof(OnOyuncu1HealthChanged))]
    public float Oyuncu_1_saglik = 100;

    [SyncVar(hook = nameof(OnOyuncu2HealthChanged))]
    public float Oyuncu_2_saglik = 100;

    private VisualElement oyuncu1GreenBar;
    private VisualElement oyuncu1RedBar;
    private VisualElement oyuncu2GreenBar;
    private VisualElement oyuncu2RedBar;

    void Start()
    {
        var root = uiDocument.rootVisualElement;
        oyuncu1GreenBar = root.Q<VisualElement>("oyuncu1GreenBar");
        oyuncu1RedBar = root.Q<VisualElement>("oyuncu1RedBar");
        oyuncu2GreenBar = root.Q<VisualElement>("oyuncu2GreenBar");
        oyuncu2RedBar = root.Q<VisualElement>("oyuncu2RedBar");

        UpdateHealthUI(1, Oyuncu_1_saglik, 100);
        UpdateHealthUI(2, Oyuncu_2_saglik, 100);

        var gameOverPanel = root.Q<VisualElement>("gameOverPanel");
        var homeButton = gameOverPanel.Q<UnityEngine.UIElements.Button>("homeButton");
        var restartButton = gameOverPanel.Q<UnityEngine.UIElements.Button>("restartButton");

        if (homeButton != null)
        {
            homeButton.clicked += OnHomeButtonClicked;
        }
        else
        {
            Debug.LogError("Home button not found");
        }

        if (restartButton != null)
        {
            restartButton.clicked += OnRestartButtonClicked;
        }
        else
        {
            Debug.LogError("Restart button not found");
        }
    }

    private void OnHomeButtonClicked()
    {
        Time.timeScale = 1; // Reset time scale before loading the main menu
        SceneManager.LoadScene("MainMenu");
    }

    private void OnRestartButtonClicked()
    {
        Time.timeScale = 1; // Reset time scale before restarting the game
        SceneManager.LoadScene("Level1");
    }

    [ClientRpc]
    public void RpcSes_ve_Efekt_Olustur(int kriter, GameObject objeTransformu)
    {
        switch (kriter)
        {
            case 1:
                Instantiate(TopYokOlmaEfekt, objeTransformu.transform.position, objeTransformu.transform.rotation);
                Vector3 yaziPozisyonu = objeTransformu.transform.position + new Vector3(0, 1.5f, 0);
                Instantiate(YaziEfekt, yaziPozisyonu, objeTransformu.transform.rotation);
                YokOlmaSesi.Play();
                break;
        }
    }

    private void OnOyuncu1HealthChanged(float oldHealth, float newHealth)
    {
        UpdateHealthUI(1, newHealth, 100);
        if (isServer && newHealth <= 0)
        {
            HandleDefeat(1);
        }
    }

    private void OnOyuncu2HealthChanged(float oldHealth, float newHealth)
    {
        UpdateHealthUI(2, newHealth, 100);
        if (isServer && newHealth <= 0)
        {
            HandleDefeat(2);
        }
    }

    public void UpdateHealth(int playerIndex, float damage)
    {
        if (playerIndex == 1)
        {
            Oyuncu_1_saglik -= damage;
            UpdateHealthUI(1, Oyuncu_1_saglik, 100);
        }
        else if (playerIndex == 2)
        {
            Oyuncu_2_saglik -= damage;
            UpdateHealthUI(2, Oyuncu_2_saglik, 100);
        }
    }

    private void UpdateHealthUI(int playerIndex, float currentHealth, float maxHealth)
    {
        float healthPercentage = currentHealth / maxHealth;

        if (playerIndex == 1)
        {
            oyuncu1GreenBar.style.width = Length.Percent(healthPercentage * 100);
        }
        else if (playerIndex == 2)
        {
            oyuncu2GreenBar.style.width = Length.Percent(healthPercentage * 100);
            oyuncu2GreenBar.style.left = Length.Percent(100 - (healthPercentage * 100));
        }
    }

    [Server]
    public void HandleDefeat(int playerIndex)
    {
        GameObject defeatedPlayer = playerIndex == 1 ? Oyuncu_1 : Oyuncu_2;
        Vector3 defeatPosition = defeatedPlayer.transform.position;

        // Yenilgi efektini oluştur
        RpcCreateDefeatEffect(defeatPosition);

        // Oyuncuyu yok et
    }

    [ClientRpc]
    void RpcCreateDefeatEffect(Vector3 position)
    {
        Instantiate(DefeatEffectPrefab, position, Quaternion.identity);
        GameOverSes.Play();
        StartCoroutine(TransitionToGameOver());
    }

    private IEnumerator TransitionToGameOver()
    {
        yield return new WaitForSeconds(1);
        var gameOverPanel = uiDocument.rootVisualElement.Q<VisualElement>("gameOverPanel");
        if (gameOverPanel != null)
        {
            gameOverPanel.style.display = DisplayStyle.Flex;
        }
        Time.timeScale = 0; // Pause the game
    }
}

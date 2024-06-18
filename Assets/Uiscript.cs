using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameUIManager : MonoBehaviour
{
    public UIDocument uiDocument;
    public AudioSource audioSource; // Ses çalmak için AudioSource referansı

    private Button myButton, myButton2, cancelButton, level1Button, level2Button,exitButton;
    private SliderInt volumeSlider; // Ses seviyesi slider'ı
    private VisualElement secenekler, anamenu, levels;


void Awake()
{
    SceneManager.sceneLoaded += OnSceneLoaded;
}

void OnDestroy()
{
    SceneManager.sceneLoaded -= OnSceneLoaded;
}

void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    if (scene.name == "MainMenu")
    {
        InitializeUI();
    }
}

void InitializeUI()
{
    if (uiDocument == null)
    {
        Debug.LogError("UIDocument is not assigned to UIManager.");
        return;
    }
    else
    {
        Debug.Log("UIDocument is successfully assigned.");
    }

    var root = uiDocument.rootVisualElement;

    // UI elemanlarını yeniden bul
        myButton = root.Q<Button>("mybutton");
        myButton2 = root.Q<Button>("mybutton2");
        exitButton = root.Q<Button>("exitButton");
        cancelButton = root.Q<Button>("cancel");
        level1Button = root.Q<Button>("level1Button");
        level2Button = root.Q<Button>("level2Button");
        volumeSlider = root.Q<SliderInt>("volume");
        secenekler = root.Q<VisualElement>("secenekler");
        anamenu = root.Q<VisualElement>("anaMenu");
        levels = root.Q<VisualElement>("levels");

    // Elemanları kontrol et
    if (myButton == null || myButton2 == null || cancelButton == null || volumeSlider == null || secenekler == null || anamenu == null)
    {
        Debug.LogError("One or more UI elements could not be found.");
        return;
    }

    // Event handler'ları ata
        myButton.clicked += OnPlayButtonClicked;
        myButton2.clicked += OnOptionsButtonClicked;
        exitButton.clicked +=OnExitButtonClicked;
        cancelButton.clicked += OnCancelButtonClicked;
        level1Button.clicked += () => LoadLevel("Level1");
        level2Button.clicked += () => LoadLevel("Level2");
        volumeSlider.RegisterValueChangedCallback(evt => OnVolumeChanged(evt.newValue));

    // UI görünürlüğünü ayarla
    secenekler.style.display = DisplayStyle.None;
    anamenu.style.display = DisplayStyle.Flex;
}
    private void OnEnable()
    {
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument is not assigned to UIManager.");
            return;
        }

        var root = uiDocument.rootVisualElement;

        // Butonları ve diğer elementleri bul
        myButton = root.Q<Button>("mybutton");
        myButton2 = root.Q<Button>("mybutton2");
        cancelButton = root.Q<Button>("cancel");
        volumeSlider = root.Q<SliderInt>("volume");
        secenekler = root.Q<VisualElement>("secenekler");
        anamenu = root.Q<VisualElement>("anaMenu");

        // Gerekli kontroller
        if (myButton == null || myButton2 == null || cancelButton == null || volumeSlider == null || secenekler == null || anamenu == null)
        {
            Debug.LogError("One or more UI elements could not be found.");
            return;
        }

        // Buton olaylarını bağla
        myButton.clicked += OnPlayButtonClicked;
        myButton2.clicked += OnOptionsButtonClicked;
        cancelButton.clicked += OnCancelButtonClicked;
        
        volumeSlider.RegisterValueChangedCallback(evt => OnVolumeChanged(evt.newValue));
        volumeSlider.value = 50;
        OnVolumeChanged(volumeSlider.value);

        // UI görünürlüğünü ayarla
        secenekler.style.display = DisplayStyle.None;
        anamenu.style.display = DisplayStyle.Flex; // Ana menü başlangıçta görünür olacak
    }
    private void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
    private void OnExitButtonClicked()
    {
        // Application.Quit() does not work in the editor; thus, this method is only effective in builds.
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    private void OnDisable()
    {
        if (myButton != null)
            myButton.clicked -= OnPlayButtonClicked;
        if (myButton2 != null)
            myButton2.clicked -= OnOptionsButtonClicked;
        if (cancelButton != null)
            cancelButton.clicked -= OnCancelButtonClicked;
        if (volumeSlider != null)
            volumeSlider.UnregisterValueChangedCallback(evt => OnVolumeChanged(evt.newValue));
    }

     private void OnPlayButtonClicked()
    {
        anamenu.style.display = DisplayStyle.None;
        levels.style.display = DisplayStyle.Flex;
    }

    private void OnVolumeChanged(int newValue)
    {
        audioSource.volume = newValue / 100.0f;
    }

    private void OnOptionsButtonClicked()
    {
        // Seçenekler ve ana menü arasında geçiş yap
        secenekler.style.display = DisplayStyle.Flex;
        anamenu.style.display = DisplayStyle.None;
    }

    private void OnCancelButtonClicked()
    {
        // Cancel butonuna tıklandığında, seçenekler kapanır, ana menü açılır
        secenekler.style.display = DisplayStyle.None;
        anamenu.style.display = DisplayStyle.Flex;
    }
}

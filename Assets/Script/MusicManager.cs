using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips; // Audio clips array
    public UIDocument uiDocument;  // UIDocument reference

    private DropdownField dropdown; // UI Toolkit DropdownField
    public static MusicManager instance; // Singleton instance

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Objeyi sahneler arası yok etme
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Eğer daha önce bir instance varsa, yeni oluşturulanı yok et
        }
    }
    

    void Start()
{
    if (uiDocument == null)
    {
        Debug.LogError("UIDocument is not assigned.");
        return;
    }

    var root = uiDocument.rootVisualElement;

    dropdown = root.Q<DropdownField>("musicDropdown");
    if (dropdown == null)
    {
        Debug.LogError("DropdownField with ID 'musicDropdown' could not be found.");
        return;
    }

    // Dropdown options'a müzik isimlerini ekle
    List<string> options = new List<string>();
    if (audioClips != null)
    {
        foreach (var clip in audioClips)
        {
            options.Add(clip.name);
        }
        dropdown.choices = options;

        // Dropdown seçimini dinle
        dropdown.RegisterCallback<ChangeEvent<string>>(evt => {
            ChangeMusic(dropdown.index);
        });
    }
    else
    {
        Debug.LogError("AudioClips array is not assigned.");
    }
}
    public void ChangeMusic(int index)
    {
        if (index >= 0 && index < audioClips.Length)
        {
            audioSource.clip = audioClips[index];
            audioSource.Play();
        }
    }
}

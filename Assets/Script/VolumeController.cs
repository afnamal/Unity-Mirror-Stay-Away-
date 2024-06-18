using UnityEngine;
using UnityEngine.UI;  // UI namespace'ini eklemeyi unutmayın

public class VolumeController : MonoBehaviour
{
    public AudioSource audioSource;  // Kontrol etmek istediğiniz AudioSource
    public Slider volumeSlider;      // Ses seviyesini ayarlamak için kullanılan Slider

    void Start()
    {
        // Başlangıçta Slider'ın değerini AudioSource'un mevcut ses seviyesi ile eşleştirin
        volumeSlider.value = audioSource.volume;

        // Slider'ın değer değişikliği event'ini dinleyin ve sesi ayarlayacak fonksiyonu çağırın
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}

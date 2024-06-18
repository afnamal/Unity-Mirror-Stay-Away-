using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);  // Bu objenin sahne değişikliklerinde yok edilmemesini sağlar.
    }
}

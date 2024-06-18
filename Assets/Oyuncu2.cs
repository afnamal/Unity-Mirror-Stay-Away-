using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Mirror;

public class Oyuncu1 : NetworkBehaviour
{
    public GameObject topPrefab;
    public Transform TopCikisnoktasi;
    public ParticleSystem TopAtisEfekt;
    public AudioSource TopAtmaSesi;
    public float atisAci = 70f; // Sabit atış açısını 70 derece olarak ayarla
    public float hareketHizi = 5f; // Oyuncunun hareket hızı
    public int atis_gucu = 25;

    private Rigidbody2D rb; // Oyuncunun Rigidbody bileşeni
    private Coroutine moveCoroutine; // Hareket coroutine referansı
    private bool canMoveLeft = true; // Sola hareket edebilirlik durumu

    // UIDocument referansı
    public UIDocument uiDocument;

    private Button fireButton;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody bileşenini al

        // Rigidbody2D'nin dönmesini engelle
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (isLocalPlayer && uiDocument != null)
        {
            var root = uiDocument.rootVisualElement;
            fireButton = root.Q<Button>("fireButton");

            // FireButton'ı yalnızca yerel oyuncu için görünür yap
            fireButton.RegisterCallback<ClickEvent>(ev => CmdFireTop());
        }
        else if (uiDocument != null)
        {
            var root = uiDocument.rootVisualElement;
            fireButton = root.Q<Button>("fireButton");
            fireButton.style.display = DisplayStyle.None; // Yerel oyuncu değilse butonu gizle
        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        // Yerel oyuncunun rengini farklı yaparak ayırt edin
        GetComponent<SpriteRenderer>().color = Color.blue;
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        // Atış işlemi
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log("Ctrl tuşuna basıldı - Komut çalıştırılıyor");
            CmdFireTop();
        }

        // Fare ile hareket kontrolü
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePosition.x > 0)
            {
                StartMoveRight();
            }
            else
            {
                StartMoveLeft();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopMoving();
        }

        // Klavye ile hareket kontrolü
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartMoveRight();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartMoveLeft();
        }

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            StopMoving();
        }
    }

    private void StartMoveRight()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveRight());
    }

    private void StartMoveLeft()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveLeft());
    }

    private void StopMoving()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        CmdStopMoving();
    }

    private IEnumerator MoveRight()
    {
        while (true)
        {
            CmdMoveRight();
            yield return null;
        }
    }

    private IEnumerator MoveLeft()
    {
        while (true)
        {
            CmdMoveLeft();
            yield return null;
        }
    }

    [Command]
    void CmdMoveRight()
    {
        RpcMoveRight();
    }

    [Command]
    void CmdMoveLeft()
    {
        if (canMoveLeft)
        {
            RpcMoveLeft();
        }
    }

    [Command]
    void CmdStopMoving()
    {
        RpcStopMoving();
    }

    [ClientRpc]
    void RpcMoveRight()
    {
        rb.velocity = new Vector2(hareketHizi, rb.velocity.y);
        rb.angularVelocity = 0f; // Açısal hızı sıfırla
    }

    [ClientRpc]
    void RpcMoveLeft()
    {
        rb.velocity = new Vector2(-hareketHizi, rb.velocity.y);
        rb.angularVelocity = 0f; // Açısal hızı sıfırla
    }

    [ClientRpc]
    void RpcStopMoving()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        rb.angularVelocity = 0f; // Açısal hızı sıfırla
    }

    [Command]
    void CmdFireTop()
    {
        Debug.Log("CmdFireTop komutu çalıştırıldı");
        GameObject topobjem = Instantiate(topPrefab, TopCikisnoktasi.position, Quaternion.Euler(0, 0, 180 - atisAci));
        NetworkServer.Spawn(topobjem);
        RpcFireTop(topobjem, (180 - atisAci) * Mathf.Deg2Rad, atis_gucu);
    }

    [ClientRpc]
    void RpcFireTop(GameObject topobjem, float aciRadyan, int atisGucu)
    {
        Debug.Log("RpcFireTop RPC çalıştırıldı");
        Instantiate(TopAtisEfekt, TopCikisnoktasi.transform.position, Quaternion.Euler(0, 0, 180 - atisAci));
        TopAtmaSesi.Play();

        if (topobjem.TryGetComponent<Rigidbody2D>(out var rg))
        {
            Vector2 kuvvet = new Vector2(Mathf.Cos(aciRadyan), Mathf.Sin(aciRadyan)) * atisGucu;
            rg.AddForce(kuvvet, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ortadirek")
        {
            canMoveLeft = false; // Sola hareketi engelle
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ortadirek")
        {
            canMoveLeft = true; // Sola hareketi tekrar serbest bırak
        }
    }
}

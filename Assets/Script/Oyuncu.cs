using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Mirror;

public class Oyuncu : NetworkBehaviour
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
    private bool canMoveRight = true; // Sağa hareket edebilirlik durumu

    // UIDocument referansı
    public UIDocument uiDocument;

    private Button fireButton;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody bileşenini al
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Rigidbody2D'nin dönmesini engelle

        if (isLocalPlayer && uiDocument != null)
        {
            var root = uiDocument.rootVisualElement;
            fireButton = root.Q<Button>("fireButton");

            // FireButton'ı yalnızca yerel oyuncu için görünür yap
            fireButton.RegisterCallback<ClickEvent>(ev => FireTop());
        }
        else if (uiDocument != null)
        {
            var root = uiDocument.rootVisualElement;
            fireButton = root.Q<Button>("fireButton");
            fireButton.style.display = DisplayStyle.None; // Yerel oyuncu değilse butonu gizle
        }
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        // Atış işlemi
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            FireTop();
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

        // Touch input handling for mobile
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (touch.position.x > Screen.width / 2)
                {
                    StartMoveRight();
                }
                else
                {
                    StartMoveLeft();
                }
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                StopMoving();
            }
        }

        // Mouse input handling
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;

            if (mousePos.x > Screen.width / 2)
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
        if (canMoveRight)
        {
            RpcMoveRight();
        }
    }

    [Command]
    void CmdMoveLeft()
    {
        RpcMoveLeft();
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

    void FireTop()
    {
        if (!isLocalPlayer)
            return;

        CmdFireTop();
    }

    [Command]
    void CmdFireTop()
    {
        GameObject topobjem = Instantiate(topPrefab, TopCikisnoktasi.position, Quaternion.Euler(0, 0, atisAci));
        NetworkServer.Spawn(topobjem);
        RpcFireTop(topobjem, atisAci * Mathf.Deg2Rad, atis_gucu);
    }

    [ClientRpc]
    void RpcFireTop(GameObject topobjem, float aciRadyan, int atisGucu)
    {
        Instantiate(TopAtisEfekt, TopCikisnoktasi.position, Quaternion.Euler(0, 0, atisAci));
        TopAtmaSesi.Play();

        if (topobjem.TryGetComponent<Rigidbody2D>(out var rb))
        {
            Vector2 kuvvet = new Vector2(Mathf.Cos(aciRadyan), Mathf.Sin(aciRadyan)) * atisGucu;
            rb.AddForce(kuvvet, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ortadirek")
        {
            canMoveRight = false; // Sağa hareketi engelle
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ortadirek")
        {
            canMoveRight = true; // Sağa hareketi tekrar serbest bırak
        }
    }
}

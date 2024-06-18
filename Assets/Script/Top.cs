using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Top : NetworkBehaviour
{
    public float darbegucu = 20f;
    private GameKontrol gameKontrol;

    void Start()
    {
        gameKontrol = GameObject.FindWithTag("GameKontrol").GetComponent<GameKontrol>();
    }

    [Server]
    void HandleCollision(GameObject collisionObject, int playerIndex)
    {
        // Sağlık değerini güncelle
        gameKontrol.UpdateHealth(playerIndex, darbegucu);

        // Çarpma efekti oluştur
        RpcCreateHitEffect(collisionObject);

        // Eğer can sıfır veya altındaysa oyuncuyu yenilgiye uğrat
        if (playerIndex == 1 && gameKontrol.Oyuncu_1_saglik <= 0)
        {
            gameKontrol.HandleDefeat(1);
        }
        else if (playerIndex == 2 && gameKontrol.Oyuncu_2_saglik <= 0)
        {
            gameKontrol.HandleDefeat(2);
        }

        // Topu yok et
        NetworkServer.Destroy(gameObject);
    }

    [ClientRpc]
    void RpcCreateHitEffect(GameObject collisionObject)
    {
        Instantiate(gameKontrol.TopYokOlmaEfekt, collisionObject.transform.position, collisionObject.transform.rotation);
        Vector3 yaziPozisyonu = collisionObject.transform.position + new Vector3(0, 1.5f, 0);
        Instantiate(gameKontrol.YaziEfekt, yaziPozisyonu, collisionObject.transform.rotation);
        gameKontrol.YokOlmaSesi.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer) return; // Sadece server üzerinde çalışır

        if (collision.gameObject.CompareTag("Ortadaki_kutular"))
        {
            collision.gameObject.GetComponent<ortadaki_kutu>().darbeal(darbegucu);
            HandleCollision(collision.gameObject, 0); // 0 for obstacles
        }
        else if (collision.gameObject.CompareTag("Oyuncu_2_Kule"))
        {
            HandleCollision(collision.gameObject, 2);
        }
        else if (collision.gameObject.CompareTag("Oyuncu_1_Kule"))
        {
            HandleCollision(collision.gameObject, 1);
        }
        else if (collision.gameObject.tag == "Ortadirek")
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
        }
    }
}
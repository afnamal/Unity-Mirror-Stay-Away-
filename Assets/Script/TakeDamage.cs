using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class TakeDamage : NetworkBehaviour
{
    [SyncVar] float health = 100f;
    public GameObject healthCanvas;
    public Image healthBar;

    private GameKontrol gameKontrol;

    void Start()
    {
        GameObject gameKontrolObject = GameObject.FindWithTag("GameKontrol");
        if (gameKontrolObject != null)
        {
            gameKontrol = gameKontrolObject.GetComponent<GameKontrol>();
            if (gameKontrol == null)
            {
                Debug.LogError("GameKontrol script not found on 'GameKontrol' object!");
            }
        }
        else
        {
            Debug.LogError("'GameKontrol' object not found!");
        }
    }

    [ClientRpc]
    public void RpcTakeHit(float damagePower)
    {
        health -= damagePower;
        healthBar.fillAmount = health / 100f;

        if (health <= 0)
        {
            if (gameKontrol != null)
            {
                gameKontrol.RpcSes_ve_Efekt_Olustur(2, gameObject);
            }
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(ShowCanvas());
        }
    }

    IEnumerator ShowCanvas()
    {
        healthCanvas.SetActive(true);
        yield return new WaitForSeconds(2);
        healthCanvas.SetActive(false);
    }
}

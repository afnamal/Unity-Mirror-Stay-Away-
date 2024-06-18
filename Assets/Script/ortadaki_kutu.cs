using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA.Input;

public class ortadaki_kutu : MonoBehaviour
{
    float saglik = 100;
    public GameObject SaglikCanvası;
    public Image healtBar;    

    GameObject gameKontrol;

    private void Start()
    {
        gameKontrol = GameObject.FindWithTag("GameKontrol");
    }
    public void darbeal(float dargegucu)
    {
        saglik -= dargegucu;

        healtBar.fillAmount = saglik / 100; // 0.9

        if (saglik<=0)
        {

            gameKontrol.GetComponent<GameKontrol>().RpcSes_ve_Efekt_Olustur(2,gameObject);
            Destroy(gameObject);

        }else
        {
            StartCoroutine(CanvasCikar());

        }

        
    }


    IEnumerator CanvasCikar()
    {
        if (!SaglikCanvası.activeInHierarchy)
        {
            SaglikCanvası.SetActive(true);
            yield return new WaitForSeconds(2);
            SaglikCanvası.SetActive(false);
        }

    }
 
}

using UnityEngine;
using System.Collections;

public class CandleController : MonoBehaviour {

    public Light candleLight;
    public float lightIntensityMin;
    public float lightIntensityMax;
    public PlayerController player;
    public bool lit;
    public bool litCoroutineRunning;
    public Animator anim;
    public bool notInteractable;

    void Awake()
    {
        candleLight = this.GetComponent<Light>();
        StartCoroutine("CandleLit");
    }

     public void OnTriggerEnter2D(Collider2D other)
     {
         if(other.gameObject.GetComponent<PlayerController>() != null)
         {
             player = other.gameObject.GetComponent<PlayerController>();
             player.candle = this;
             player.canLightCandle = true;
         }
         if(other.gameObject.GetComponent<ResidentController>() != null)
         {
             Debug.Log("candle res col");
             if (SceneController.instance.gameHour < 9 || SceneController.instance.gameHour > 19)
             {
                 lit = true;
                 //anim.SetBool("lit", true);
                 if(litCoroutineRunning == false)
                 StartCoroutine("CandleLit");
             }
             else
             {
                 lit = false;
             }

         }
     }
     public void OnTriggerExit2D(Collider2D other)
     {
         if (other.gameObject.GetComponent<PlayerController>() != null)
         {
             player = other.gameObject.GetComponent<PlayerController>();
             player.candle = null;
             player.canLightCandle = false;
         }
     }



    public IEnumerator CandleLit()
    {
        if (litCoroutineRunning == false)
        {
            litCoroutineRunning = true;
            while (lit == true)
            {
                candleLight.intensity = Random.Range(lightIntensityMin, lightIntensityMax);
                yield return new WaitForSeconds(0.1f);
            }
        }
        if (lit == false)
        {
            //anim.SetBool("lit", false);
            litCoroutineRunning = false;
            candleLight.intensity = 0;
            yield break;
        }
    }
}

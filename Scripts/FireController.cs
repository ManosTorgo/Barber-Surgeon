using UnityEngine;
using System.Collections;

public class FireController : MonoBehaviour {

    public float maxAlpha;
    public float minAlpha;
    public bool lit;
    public SpriteRenderer sprite;
    public bool coroutineInProcess;
    public Light light;

    void Awake()
    {
        sprite = this.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(SceneController.instance.gameHour >= 7 && SceneController.instance.gameHour <= 22)
        {
            lit = true;
        }
        else
        {
            lit = false;
        }
        if(lit == true)
        {
            if(coroutineInProcess == false)
            StartCoroutine("AlphaRandomiser");
            coroutineInProcess = true;
            sprite.enabled = true;
            light.enabled = true;
        }
        else
        {
            sprite.enabled = false;
            light.enabled = false;
        }
    }

    public IEnumerator AlphaRandomiser()
    {
        while(lit == true)
        {
            sprite.color = new Color(sprite.color.a, sprite.color.g, sprite.color.b, Random.Range(minAlpha, maxAlpha));
            yield return new WaitForSeconds(0.1f);
        }
        if(lit == false)
        {
            coroutineInProcess = false;
        }
    }
}

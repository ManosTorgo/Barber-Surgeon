using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneController : MonoBehaviour {

    public enum SceneState
    {
        sceneActive,
        inText,
        examining,
    }
    public SceneState currentSceneState;
    public Image screenDarkening;
    public Image cinematicProp;
    public Canvas textBox;
    public Canvas descriptionCanvas;
    public static SceneController instance;
    public Canvas canvas;
    public AudioSource introAudio;
    public AudioSource dayTimeMusic;
    public AudioSource dayTimeAmbience1;
    public AudioSource dayTimeAmbience2;
    public AudioSource nightTimeMusic;
    private bool dayTimeMusicIsPlaying;
    private bool nightTimeMusicIsPlaying;
    public int gameHour;
    public Light sunLight;
    float sunriseTimePassed;
    public bool sunriseCoroutineStarted;
    public float realTime;
    public float lengthOfGameHour;
    public float gameTime;
    public bool timeProgressing;

    public SpriteGUIController activeGUIController;

	void Awake()
    {
        instance = this;
        currentSceneState = SceneState.sceneActive;
        gameTime = lengthOfGameHour;
        Debug.Log(GameObject.FindGameObjectsWithTag("Player Head").Length);
    }
    
    void Update()
    {
        if (currentSceneState == SceneState.sceneActive)
        {
            realTime += Time.deltaTime;
            if (timeProgressing == true)
            {
                if (realTime > gameTime)
                {
                    gameTime = lengthOfGameHour + realTime;
                    gameHour++;
                }
            }
            if (gameHour == 24)
            {
                gameHour = 0;
            }
            if (gameHour == 6)
            {
                sunriseTimePassed = 0;
               // StartCoroutine(Sunrise(true));
               // nightTimeMusic.Stop();
               // nightTimeMusicIsPlaying = false;
                
            }

            if (gameHour == 17)
            {
                sunriseTimePassed = 0;
               // StartCoroutine(Sunrise(false));
            }

            if (gameHour == 0)
            {
                timeProgressing = false;
              //  dayTimeMusic.Stop();
              //  dayTimeMusicIsPlaying = false;
              //  if(nightTimeMusicIsPlaying == false)
                {
              //      nightTimeMusicIsPlaying = true;
              //      nightTimeMusic.Play();
                }
            }
        }
    }	

	public void SetSceneState (SceneState newSceneState)
    {
        currentSceneState = newSceneState;
	}

    IEnumerator FadeAway()
    {

        for (float i = 4; i >= 0; i -= Time.deltaTime)
        {
            cinematicProp.color = new Color(1, 1, 1, i / 4);
            screenDarkening.color = new Color(0, 0, 0, i / 4);
            introAudio.volume = i / 4;

            if(i <= 0.1)
            {
                cinematicProp.enabled = false;
                screenDarkening.enabled = false;
                introAudio.Stop();
                descriptionCanvas.enabled = true;
                currentSceneState = SceneState.sceneActive;
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator Sunrise(bool sunrise)
    {
        sunriseCoroutineStarted = true;
        if (sunrise == true)
        {
            for(float f = 0; f < (6 * lengthOfGameHour); f += Time.deltaTime)
            {
                sunLight.intensity = (8 * f / (6 * lengthOfGameHour));
                if(f > lengthOfGameHour)
                {
                    if (f > (lengthOfGameHour * 1.5) && dayTimeMusicIsPlaying == false)
                    {
                        dayTimeMusicIsPlaying = true;
                        dayTimeMusic.Play();
                        //dayTimeAmbience1.Play();
                        //dayTimeAmbience2.Play();
                    }
                }
                yield return null;
            }

            if(sunriseTimePassed >= (6 * lengthOfGameHour))
            {
                sunriseCoroutineStarted = false;
                yield break;
            }

        }

        else if (sunrise == false)
        {
            for (float f = 0; f < (4 * lengthOfGameHour); f += Time.deltaTime)
            {
                sunLight.intensity = 8 - (8 * f / (4 * lengthOfGameHour));
                yield return null;
                    }

            if (sunriseTimePassed >= (4 * lengthOfGameHour))
            {
                sunriseCoroutineStarted = false;
                yield break;
            }

        }
    }
}

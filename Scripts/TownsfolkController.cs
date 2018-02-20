using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TownsfolkController : MonoBehaviour
{
    //public string[] 


    public PlayerController player;
    public TownsfolkLogisticController logistics;
    public string name;
    private float chanceOfNormalHealth;
    public float chanceOfFever;
    public bool inShop;
    public bool hasNotSpoken;
    public bool readyForExamination;

    public enum Speaker
    {
        thisTownsfolk,
        player
    }
    public List<Speaker> whoseSpeaking = new List<Speaker>();
    public int speakerIndex;
    public SpriteRenderer speechBubble;

    public enum SocialRank
    {
        pleb,
        beggar
    }
    public SocialRank townsfolkSocialRank;

    //the condition determines the humours, the symptoms and the statuses
    public enum Condition
    {
        //normal health will have one humour unbalanced, and no symptoms or statuses
        normalHealth,
        //fever will always have sanguine unbalanced. Depending on RnG, a fever may also have phlegmatic unbalanced. Fever will always have symptoms high temperature and fast pulse. Fever may also have symptoms head pain and joint pain, leading to status inPain.
        //If fever has phlegmatic unbalanced, RnG may include symptoms //chills and sweating. sanguine fever is revealed by dark blood that is quick to congeal - phlegmatic fever has the added properties of clear fluid at top of blood when 
        //left to rest. 
        fever,
        rheum
    }

    //the primary diagnosis determines the townsfolk's complexion, which in turn determines the humours.
    public Condition primaryDiagnosis;

    public enum ComplexionElements
    {
        hot,
        cold,
        wet,
        dry
    }
    public List<ComplexionElements> complexion = new List<ComplexionElements>();
    private int minComplexion = (int)ComplexionElements.hot;
    private int maxComplexion = (int)ComplexionElements.dry;

    public enum Humours
    {
        //one hot and one moist
        sanguine,
        //one cold and one moist
        phlegmatic,
        //one cold and one dry
        melancholy,
        //one hot and one dry
        choleric
    }
    private int minHumour = (int)Humours.sanguine;
    private int maxHumour = (int)Humours.choleric;

    public List<Humours> unbalancedHumours = new List<Humours>();

    public enum Symptoms
    {
        abnormalPulse,
        sweating,
        headPain,
        jointPain,
        stomachPain,
        highTemperature,
        chills
    }
    public List<Symptoms> currentSymptoms = new List<Symptoms>();
    public List<Symptoms> discoveredSymptoms = new List<Symptoms>();

    public enum Statuses
    {
        chronicPain,
    }

    public List<Statuses> currentStatuses = new List<Statuses>();

    void Awake()
    {
        for (int i = 0; i <= maxComplexion; i++)
        {
            complexion.Add((ComplexionElements)i);
            Debug.Log(i);
            Debug.Log((ComplexionElements)i);
        }
        CreateNewHealthState();
    }

    void Update()
    {
        if (SceneController.instance.currentSceneState == SceneController.SceneState.inText)
        {
            if (whoseSpeaking[speakerIndex] == Speaker.thisTownsfolk)
            {
                speechBubble.enabled = true;
            }
            else
            {
                speechBubble.enabled = false;
            }
            if (Input.GetButtonDown("Fire1"))
            {
                speakerIndex++;
            }
            if (speakerIndex > whoseSpeaking.Count)
            {
                whoseSpeaking.Clear();
                speakerIndex = 0;
            }
        }

        if (SceneController.instance.currentSceneState == SceneController.SceneState.sceneActive && logistics.movementOrdered == true)
        {
            logistics.playerCanBlock = true;
            logistics.MoveToNewDestination();
        }
    }

    void FixedUpdate()
    {

    }

    public void CreateNewHealthState()
    {
        //first we need to choose a condition for the Townsfolk. We'll do this through RnG, using the chanceOfCondition, e.g. chanceOfFever, floats at the top of the script. First, create a new float of value 100, which will be the unmodified chance of normal health.
        float chanceOfNormalHealth = 100;

        //next, we're going to minus every chanceOfCondition value. The remainder will be the chanceOfNormalHealth
        chanceOfNormalHealth -= chanceOfFever;

        //chanceOfNormalHealth now equals 100 - every condition. Now we need to create the minimum and maximum ranges for our RnG.
        float minRNG = 0;
        float maxRNG = 100;
        float rng = Random.Range(minRNG, maxRNG);
        Debug.Log(rng);

        if (rng <= chanceOfFever)
        {
            primaryDiagnosis = Condition.fever;
        }

        if (rng > chanceOfFever)
        {
            primaryDiagnosis = Condition.normalHealth;
        }
        Debug.Log(primaryDiagnosis);

        if (primaryDiagnosis == Condition.normalHealth)
        {
            int newHumourIndex = Random.Range(minHumour, maxHumour + 1);
            Debug.Log(minHumour + " " + maxHumour + " " + newHumourIndex);
            unbalancedHumours.Add((Humours)newHumourIndex);
            Debug.Log(unbalancedHumours[0]);
        }

        if (primaryDiagnosis == Condition.fever)
        {
            unbalancedHumours.Add(Humours.sanguine);

            rng = Random.Range(minRNG, maxRNG);
            if (rng < 50)
            {
                unbalancedHumours.Add(Humours.phlegmatic);
            }

            rng = Random.Range(minRNG, maxRNG);
            if (rng < 30)
            {
                currentStatuses.Add(Statuses.chronicPain);

                rng = Random.Range(minRNG, maxRNG);
                if (rng < 35)
                {
                    currentSymptoms.Add(Symptoms.headPain);
                }

                if (rng < 60)
                {
                    currentSymptoms.Add(Symptoms.jointPain);
                }

                rng = Random.Range(minRNG, maxRNG);
                if (rng < 40)
                {
                    currentSymptoms.Add(Symptoms.stomachPain);
                }

                if (currentStatuses.Count < 1)
                {
                    rng = Random.Range(3, 6);
                    currentSymptoms.Add((Symptoms)rng);
                }
            }

            for (int i = 0; i < unbalancedHumours.Count; i++)
            {
                if (unbalancedHumours[i] == Humours.sanguine)
                {
                    currentSymptoms.Add(Symptoms.abnormalPulse);
                    currentSymptoms.Add(Symptoms.highTemperature);
                    rng = Random.Range(minRNG, maxRNG);
                    if (rng < 50)
                    {
                        currentSymptoms.Add(Symptoms.abnormalPulse);
                    }
                }

                if (unbalancedHumours[i] == Humours.phlegmatic)
                {
                    currentSymptoms.Add(Symptoms.sweating);

                    rng = Random.Range(minRNG, maxRNG);
                    if (rng < 50)
                    {
                        currentSymptoms.Add(Symptoms.chills);
                    }
                }
            }
        }

        CreateComplexion();
    }

    public void CreateComplexion()
    {
        for (int i = 0; i < unbalancedHumours.Count; i++)
        {
            if (unbalancedHumours[i] == Humours.sanguine)
            {
                Debug.Log("sanguine complexions added");
                complexion.Add(ComplexionElements.hot);
                complexion.Add(ComplexionElements.wet);
            }
            if (unbalancedHumours[i] == Humours.phlegmatic)
            {
                Debug.Log("phlegmatic complexions added");
                complexion.Add(ComplexionElements.cold);
                complexion.Add(ComplexionElements.wet);
            }
            if (unbalancedHumours[i] == Humours.choleric)
            {
                Debug.Log("choleric complexions added");
                complexion.Add(ComplexionElements.hot);
                complexion.Add(ComplexionElements.dry);
            }
            if (unbalancedHumours[i] == Humours.melancholy)
            {
                Debug.Log("melancholy complexions added");
                complexion.Add(ComplexionElements.cold);
                complexion.Add(ComplexionElements.dry);
            }
        }
    }

    public void AssembleDialogue()
    {
        if (inShop == true)
        {
            List<string> dialogue = new List<string>();
            int index = 0;

            float minRNG = 0;
            float maxRNG = 100;
            float rng = Random.Range(minRNG, maxRNG);

            if (readyForExamination != true)
            {
                if (SceneController.instance.gameTime < 12)
                {
                    rng = Random.Range(minRNG, maxRNG);
                    Debug.Log(rng);

                    if (rng <= 33)
                    {
                        dialogue.Add("Your breeches and your very balls be blessed, sire. ");
                        whoseSpeaking.Add(Speaker.thisTownsfolk);
                        dialogue.Add("To mingle in this town and not get the clap is blessing enough. Come in, come in. ");
                        whoseSpeaking.Add(Speaker.player);
                    }

                    if (rng > 33 && rng <= 66)
                    {
                        dialogue.Add("L-Lord it's icy. The cold'll be the death of me. ");
                        whoseSpeaking.Add(Speaker.thisTownsfolk);
                        dialogue.Add("Aye, the sick may not weather this Winter. But I've known worse seasons... and worse deaths still. Best come in out the cold. ");
                        whoseSpeaking.Add(Speaker.player);
                    }

                    if (rng > 66 && rng <= 99)
                    {
                        dialogue.Add("Morn's blessings upon you sire. Praise be to Him on High. ");
                        whoseSpeaking.Add(Speaker.thisTownsfolk);
                        dialogue.Add("Sometimes more than prayer is needed. Take a seat inside. ");
                        whoseSpeaking.Add(Speaker.player);
                    }

                }

                else if (primaryDiagnosis == Condition.normalHealth)
                {
                    dialogue.Add("Just a quick bloodletting today. With God's grace may my good health continue. ");
                    whoseSpeaking.Add(Speaker.thisTownsfolk);
                }

                readyForExamination = true;
                logistics.movementOrdered = true;
            }

            else if (readyForExamination == true)
            {
                dialogue.Add("Now tell me, what brings you here today?");
                whoseSpeaking.Add(Speaker.player);

                rng = Random.Range(minRNG, maxRNG);

                if (primaryDiagnosis == Condition.fever)
                {
                    if(unbalancedHumours.Contains(Humours.sanguine) && unbalancedHumours.Contains(Humours.phlegmatic))
                    {
                        dialogue.Add("My body’s wrongly heated with a fickle burning. Either my bloods aflame or I shiver and sweat ‘til I’m sheened like a cod and just as cold. ");
                        whoseSpeaking.Add(Speaker.thisTownsfolk);
                        discoveredSymptoms.Add(Symptoms.chills);
                        discoveredSymptoms.Add(Symptoms.sweating);
                        discoveredSymptoms.Add(Symptoms.highTemperature);
                    }

                    else
                    {
                        if(rng <= 49)
                        {
                            dialogue.Add("Prayer does naught to settle my fever, so I'll pray instead on your skill. ");
                            whoseSpeaking.Add(Speaker.thisTownsfolk);
                            discoveredSymptoms.Add(Symptoms.highTemperature);
                        }
                        else if (rng > 49)
                        {
                            dialogue.Add("Each night I wheeze feverishly, restless as The Devil’s bellows. ");
                            whoseSpeaking.Add(Speaker.thisTownsfolk);
                            discoveredSymptoms.Add(Symptoms.highTemperature);
                        }
                    }

                    if (currentSymptoms.Contains(Symptoms.abnormalPulse))
                    {
                        rng = Random.Range(minRNG, maxRNG);
                        if (rng <= 49)
                        {
                            dialogue[dialogue.Count -1] += "My pulse feels unruly and sleep won’t take me. ";
                            discoveredSymptoms.Add(Symptoms.abnormalPulse);
                        }
                        else if (rng > 49)
                        {
                            dialogue[dialogue.Count - 1] += ("My heartbeat cannot decide upon it's rhythm and hammers in my neck. ");
                            discoveredSymptoms.Add(Symptoms.abnormalPulse);
                        }
                    }

                    if (currentStatuses.Contains(Statuses.chronicPain))
                    {
                        dialogue.Add("Please, tell me you have something that can ease my pain? ");
                        whoseSpeaking.Add(Speaker.thisTownsfolk);
                    }

                    dialogue.Add("Hmm...");
                    whoseSpeaking.Add(Speaker.player);
                }
            }

            TextBoxManager.instance.dialogue.InsertRange(0, dialogue);

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            player = other.gameObject.GetComponent<PlayerController>();
            player.activeTownsfolk = this;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            player = other.gameObject.GetComponent<PlayerController>();
            player.activeTownsfolk = null;
        }
    }
}

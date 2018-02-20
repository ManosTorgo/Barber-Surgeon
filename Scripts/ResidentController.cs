using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResidentController : MonoBehaviour {

    public SpriteRenderer sprite;
    public Animator anim;
    public bool atDest;
    private Transform finalDestTrans;
    public Transform destTrans;
    private Transform trans;
    private bool rerouteDestination;
    private Vector2 lerpStart;
    private float lerpTime;
    public float speed;
    private float lerpDistance;
    private bool executeLerpStartCalculations;
    private RaycastHit2D[] hits;
    public List<StairTrigger> stairs = new List<StairTrigger>();
    public int stairIndex;
    public bool destStairs;
    public bool usingStairs;
    public int[] itineraryTime;
    public Transform[] itineraryDestination;
    private bool readyForNewItineraryDestination;
    public enum Tasks
    {
        stokingFire,
        isSeated,
        seatedEating
    }
    public Tasks[] itineraryTask;
    public bool[] taskXFlip;
    public Tasks currentTask;
    
    void Awake()
    {
        trans = this.transform;
        sprite = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();
        readyForNewItineraryDestination = true;
    }
    void Update()
    {

        for (int i = 0; i < itineraryTime.Length; i++)
        {
            if (SceneController.instance.gameHour == itineraryTime[i] && readyForNewItineraryDestination == true)
            {
                destTrans = itineraryDestination[i];
                readyForNewItineraryDestination = false;
                ExecuteLerpStartCalculations();
            }
        }

        if (destTrans != null)
        {
            if (destTrans.transform.position != trans.position)
            {
                atDest = false;
                //if (executeLerpStartCalculations == true)
                //{
                 //   ExecuteLerpStartCalculations();
                //}
                lerpTime += Time.deltaTime;
                float lerpPercentage = lerpTime / (lerpDistance / speed);
                trans.position = Vector2.Lerp(lerpStart, destTrans.position, lerpPercentage);
                if (lerpStart.x > destTrans.position.x)
                    sprite.flipX = true;
                else
                    sprite.flipX = false;
            }

            else if (destTrans.transform.position == trans.position)
            {
                atDest = true;
                lerpTime = 0;
                if(rerouteDestination == false)
                {
                    readyForNewItineraryDestination = true;
                    BeginTask();
                }
                this.GetComponent<Rigidbody2D>().WakeUp();
                // isMoving anim bool here is false
            }
        }


        if (atDest == true)
        {
            if (rerouteDestination == true)
            {
                if (finalDestTrans.position == destTrans.position)
                {
                    rerouteDestination = false;
                }
            }
        }

        if (usingStairs == true)
        {
            if(stairs[stairIndex].bottomOfStaircase == false)
            {
                lerpTime += Time.deltaTime;
                lerpDistance = Vector2.Distance(stairs[stairIndex].stairUpstairsTransform.position, stairs[stairIndex].stairDownstairsTransform.position);
                float lerpPercentage = (lerpTime / (lerpDistance / speed)) * 0.5f;
                trans.position = Vector2.Lerp(stairs[stairIndex].stairUpstairsTransform.position, stairs[stairIndex].stairDownstairsTransform.position, lerpPercentage);
                if (stairs[stairIndex].stairUpstairsTransform.position.x > stairs[stairIndex].stairDownstairsTransform.position.x)
                    sprite.flipX = true;
                else
                    sprite.flipX = false;
                if (lerpPercentage >= 1)
                {
                    usingStairs = false;
                    destStairs = false;
                    NextDestination();
                }
            }
            else if(stairs[stairIndex].bottomOfStaircase == true)
            {
                lerpTime += Time.deltaTime;
                lerpDistance = Vector2.Distance(stairs[stairIndex].stairUpstairsTransform.position, stairs[stairIndex].stairDownstairsTransform.position);
                float lerpPercentage = (lerpTime / (lerpDistance / speed)) * 0.5f;
                trans.position = Vector2.Lerp(stairs[stairIndex].stairDownstairsTransform.position, stairs[stairIndex].stairUpstairsTransform.position, lerpPercentage);
                if (stairs[stairIndex].stairDownstairsTransform.position.x > stairs[stairIndex].stairUpstairsTransform.position.x)
                    sprite.flipX = true;
                else
                    sprite.flipX = false;
                if (lerpPercentage >= 1)
                {
                    usingStairs = false;
                    destStairs = false;
                    NextDestination();
                }
            }
        }
    }

    void NextDestination()
    {
        lerpTime = 0;
        lerpStart = trans.position;
        destTrans = finalDestTrans;
            if (Mathf.Abs(trans.position.y - destTrans.position.y) > 0.5)
            {
                if (rerouteDestination == false)
                {
                    rerouteDestination = true;
                    finalDestTrans = destTrans;
                }
                stairs.Clear();
                hits = Physics2D.RaycastAll(new Vector2(trans.position.x + 50, trans.position.y), Vector2.left);
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider != null)
                    {
                        if (hits[i].collider.gameObject.GetComponent<StairTrigger>() != null)
                        {
                            stairs.Add(hits[i].collider.gameObject.GetComponent<StairTrigger>());
                        }
                    }
                }
                for (int i = 0; i < stairs.Count; i++)
                {
                    if (destTrans.position.y < trans.position.y)
                    {
                        if (stairs[i].bottomOfStaircase == false)
                        {
                            destTrans = stairs[i].stairUpstairsTransform;
                            stairIndex = i;
                            destStairs = true;
                            break;
                        }
                    }
                    else if (destTrans.position.y > trans.position.y)
                    {
                        if (stairs[i].bottomOfStaircase == true)
                        {
                            destTrans = stairs[i].stairDownstairsTransform;
                            stairIndex = i;
                            destStairs = true;
                            break;
                        }
                    }
                }
            }
            lerpDistance = Mathf.Abs(destTrans.transform.position.x - trans.position.x);
            if (Mathf.Abs(trans.position.x - destTrans.transform.position.x) > 0)
            {
                sprite.flipX = true;
            }
            else if (Mathf.Abs(trans.position.x - destTrans.transform.position.x) < 0)
            {
                sprite.flipX = false;
            }
        }

    void ExecuteLerpStartCalculations()
    {
        Debug.Log("calculating target for " + gameObject.name);
        lerpTime = 0;
        executeLerpStartCalculations = false;
        lerpStart = trans.position;
        if(Mathf.Abs(trans.position.y - destTrans.position.y) > 0.2)
        {
            Debug.Log("finding new route");
            if(rerouteDestination != true)
            {
                rerouteDestination = true;
                finalDestTrans = destTrans;
            }
            stairs.Clear();
            hits = Physics2D.RaycastAll(new Vector2 (trans.position.x + 50, trans.position.y), Vector2.left);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider != null)
                {
                    if (hits[i].collider.gameObject.GetComponent<StairTrigger>() != null)
                    {
                        stairs.Add(hits[i].collider.gameObject.GetComponent<StairTrigger>());
                    }
                }
            }
            Debug.Log(gameObject.name + " can identify " + stairs.Count + "many stairs");
            for (int i = 0; i < stairs.Count; i++)
            {
                if(destTrans.position.y < trans.position.y)
                {
                    if(stairs[i].bottomOfStaircase == false)
                    {
                        destTrans = stairs[i].stairUpstairsTransform;
                        stairIndex = i;
                        destStairs = true;
                        break;
                    }
                }
                else if(destTrans.position.y > trans.position.y)
                {
                    if (stairs[i].bottomOfStaircase == true)
                    {
                        destTrans = stairs[i].stairDownstairsTransform;
                        stairIndex = i;
                        destStairs = true;
                        break;
                    }
                }
            }
        }
        else
        {
            rerouteDestination = false;
        }
        lerpDistance = Mathf.Abs(destTrans.transform.position.x - trans.position.x);
        if (Mathf.Abs(trans.position.x - destTrans.transform.position.x) > 0)
        {
            sprite.flipX = true;
        }
        else if (Mathf.Abs(trans.position.x - destTrans.transform.position.x) < 0)
        {
            sprite.flipX = false;
        }
        //reset animator in preparation for movement
        anim.SetBool("isSeated", false);
        //isMoving anim bool here is true
    }

    void BeginTask()
    {
        for(int i = 0; i < itineraryTime.Length; i++)
        {
            if(itineraryTime[i] == SceneController.instance.gameHour)
            {
                currentTask = itineraryTask[i];
                sprite.flipX = taskXFlip[i];
            }
        }

        if(currentTask == Tasks.isSeated)
        {
            anim.SetBool("isSeated", true);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<PlayerController>() != null)
        {
            other.gameObject.GetComponent<PlayerController>().resident = this;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            other.gameObject.GetComponent<PlayerController>().resident = null;
            if(other.gameObject.GetComponent<PlayerController>().dialogue == true)
            {
                other.gameObject.GetComponent<PlayerController>().dialogue = false;
            }
        }
    }
}

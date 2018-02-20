using UnityEngine;
using System.Collections;

public class TownsfolkLogisticController : MonoBehaviour {

    public Transform destination;
    public Transform trans;
    public bool movementOrdered;
    public bool playerCanBlock;
    private bool movementBlocked;
    private bool destinationIsLeft;
    private float lerpTime;
    private RaycastHit2D[] hits;
    public float speed;

    void Awake()
    {
        trans = this.transform;
        movementBlocked = true;
    }

    public void MoveToNewDestination()
    {
        if (movementOrdered == true)
        {
            if (playerCanBlock == true && movementBlocked == true)
            {
                lerpTime = 0;
                float distance = Mathf.Abs(trans.position.x - destination.position.x);

                if (trans.position.x - destination.position.x < 0)
                {
                    hits = Physics2D.RaycastAll(new Vector2(trans.position.x, trans.position.y + 1), Vector2.right, distance);
                    destinationIsLeft = false;
                }

                if (trans.position.x - destination.position.x > 0)
                {
                    hits = Physics2D.RaycastAll(new Vector2(trans.position.x, trans.position.y + 1), Vector2.left, distance);
                    destinationIsLeft = true;
                }

                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider != null)
                    {
                        if (hits[i].collider.gameObject.GetComponent<PlayerController>() != null)
                        {
                            movementBlocked = true;
                        }
                        else
                        {
                            movementBlocked = false;
                        }
                    }
                }
            }
            if (movementBlocked == false)
            {
                lerpTime += Time.deltaTime;
                Debug.Log(lerpTime);
                float lerpDistance = Vector2.Distance(trans.position, destination.position);
                float timeToTakeLerp = lerpDistance / speed;
                float lerpPercentage = (lerpTime / timeToTakeLerp);
                trans.position = Vector2.Lerp(trans.position, destination.position, lerpPercentage);
                if(lerpPercentage > 1)
                {
                    movementOrdered = false;
                }
            }
        }
    }
}

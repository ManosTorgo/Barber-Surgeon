using UnityEngine;
using System.Collections;

public class StairTrigger : MonoBehaviour {

    public Transform stairUpstairsTransform;
    public Transform stairDownstairsTransform;
    public bool bottomOfStaircase;
    public ResidentController character;
    public PlayerController player;

    //purely used for reorganising the stair-climber's sprting order to be below the staircase sprite
    public SpriteRenderer stairSprite;

    void Awake()
    {

    }
	public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("col");
        if(other.gameObject.GetComponent<PlayerController>() != null)
        {
            player = other.gameObject.GetComponent<PlayerController>();
        }

            if(player.stairCaseMovement != true) {
                {
                    Debug.Log("col player");
                    player.stairUpstairsTransform = stairUpstairsTransform;
                    player.stairDownstairsTransform = stairDownstairsTransform;

                    if (bottomOfStaircase == false && player.awaitingStaircaseInput == false)
                    {
                        player.atTopOfStairs = true;
                        player.awaitingStaircaseInput = true;
                        player.stairTrigger = this;
                    Debug.Log("top of stairs");
                    }

                    if (bottomOfStaircase == true && player.awaitingStaircaseInput == false)
                    {
                        player.atBottomOfStairs = true;
                        player.awaitingStaircaseInput = true;
                        player.stairTrigger = this;
                    Debug.Log("bottom of stairs");
                }
                }
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<ResidentController>() != null)
        {
            character = other.gameObject.GetComponent<ResidentController>();
            if (character.destStairs == true)
            {
                if (character.stairs[character.stairIndex] == this)
                {
                    //following if statement resets Lerptime in Character Controller
                    if (character.atDest == true)
                    {
                        character.usingStairs = true;
                    }
                }
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            player = other.gameObject.GetComponent<PlayerController>();
            player.awaitingStaircaseInput = false;
            if(player.stairCaseMovement == false)
            {
                player.stairTrigger = null;
                player.atBottomOfStairs = false;
                player.atTopOfStairs = false;
            }
        }
    }
}

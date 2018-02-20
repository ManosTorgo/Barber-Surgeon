using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public static PlayerController instance;
    public Transform trans;
    public SpriteRenderer shadow;
    public float speed;
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public Animator anim;
    public Animator shadowAnim;
    public bool isFacingLeft;
    public bool movementLocked;

    //for interacting with stairs
    public StairTrigger stairTrigger;
    public Transform stairUpstairsTransform;
    public Transform stairDownstairsTransform;
    public bool atTopOfStairs;
    public bool atBottomOfStairs;
    public bool awaitingStaircaseInput;
    private bool resetLerpTime;
    private float currentLerpTime;
    public float stairLerpPercentage;
    public bool stairCaseMovement;
    public float stairMovementSpeed;

    //for interacting with lightsources
    public bool canLightCandle;
    public CandleController candle;

    //for audio
    public AudioSource footsteps;

    //for interacting with other residents
    public ResidentController resident;
    public bool dialogue;

    //for interacting with townsfolk
    public TownsfolkController activeTownsfolk;
    public SpriteRenderer speechBubble;

    //for interacting with objects (e.g. surgical table)
    public bool upInteraction;
    public bool isExamining;

    //for Q menu
    public InventoryItem equippedInvItem;

    //for detecting if in a Canvas Trigger
    public bool canvasTriggerCol;

    //public InteractableCanvas interactionCanvas;
    //public InteractableObject activeInteractableObject;

    void Awake()
    {
        trans = this.transform;
        rb = this.GetComponent<Rigidbody2D>();
        sprite = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();
        instance = this;
    }
	void Update() {

        if (SceneController.instance.currentSceneState == SceneController.SceneState.sceneActive)
        {
            //this method checks whether the player is facing left or right, and then flips the sprite and the
            //shadow accordingly. See the method for more info (after fixed update)
            CheckDirectionFacing();

            //this method checks whether the attatched rigid body has a velocity of magnitude over 1.5. If the rigidbody
            //does, then the "isMoving" bool in the animator of the player and the player shadow is set to true, else it is false. 
            CheckIfMoving();
        }

        CheckIfAtStaircase();

        UpInput();

        DownInput();

        EInput();

        WhoseSpeakingCheck();

        CloseCanvasCheck();

        if(stairCaseMovement == true)
        {
            NavigateStairCase();
        }

        
        
        

        if (canLightCandle == true && candle != null && candle.notInteractable == false)
        {
            if (candle.lit == false)
            {
                DescriptionController.instance.text.text = "press up to light candle";
                if (Input.GetAxis("Vertical") == 1)
                {
                    candle.lit = true;
                    //candle.anim.SetBool("lit", true);
                    if (candle.litCoroutineRunning == false)
                    {
                        candle.StartCoroutine("CandleLit");
                    }
                }
            }
            if (candle.lit == true)
            {
                DescriptionController.instance.text.text = "press down to extinguish candle";
                if (Input.GetAxis("Vertical") == -1)
                {
                    candle.lit = false;
                }
            }
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (SceneController.instance.currentSceneState == SceneController.SceneState.sceneActive && movementLocked != true)
        {

            if (Input.GetAxis("Horizontal") == -1)
            {
                rb.velocity = new Vector2(-speed, 0);
            }
            if (Input.GetAxis("Horizontal") == 1)
            {
                rb.velocity = new Vector2(speed, 0);
            }
        }
	
	}

    void CheckDirectionFacing()
    {
        if (Input.GetAxis("Horizontal") == -1)
        {
            isFacingLeft = true;
        }

        if (Input.GetAxis("Horizontal") == 1)
        {
            isFacingLeft = false;
        }

        if (isFacingLeft != false)
        {
            sprite.flipX = true;
            shadow.flipX = true;
        }

        else
        {
            sprite.flipX = false;
            shadow.flipX = false;
        }
    }

    void CheckIfMoving()
    {

        if (rb.velocity.magnitude > 1.5)
        {
            anim.SetBool("isMoving", true);
            shadowAnim.SetBool("isMoving", true);
        }

        if (rb.velocity.magnitude < 1.5)
        {
            anim.SetBool("isMoving", false);
            shadowAnim.SetBool("isMoving", false);
        }
    }

    void CheckIfAtStaircase()
    {
        if (SceneController.instance.currentSceneState == SceneController.SceneState.sceneActive)
        {
            if (stairTrigger == null)
            {
                DescriptionController.instance.text.text = " ";
                return;
            }

            else if (stairTrigger != null)
            {
                if (awaitingStaircaseInput == true && atTopOfStairs == true)
                {
                    DescriptionController.instance.text.text = "press down to walk downstairs";
                }

                else if (awaitingStaircaseInput == true && atBottomOfStairs == true)
                {
                    DescriptionController.instance.text.text = "press up to walk upstairs";
                }
            }
        }

       
    }

    void UpInput()
    {
            if (canvasTriggerCol == true && CanvasManager.instance.activeCanvas.enabled != true)
            {
                DescriptionController.instance.text.text = "press up to examine " + CanvasManager.instance.activeCanvasName;
            }
        if(Input.GetAxis("Vertical") == 1)
        {
            //first, we're going to check if up has been pressed in the context of the player wanting to go upstairs
            //to do this, we check if the player is at the bottom of the stairs
            if (awaitingStaircaseInput == true && stairCaseMovement == false && atBottomOfStairs == true)
            {
                //we set the bool stairCaseMovement to true so that we can prevent certain functions from occuring
                //whilst the player is moving upstairs. We will set this bool to false once the player has navigated
                //the staircase.
                stairCaseMovement = true;

                //we set currentLerpTime to 0 as we will be starting a new lerp in the staircase movement function
                currentLerpTime = 0;

                //set the bools of the animator controllers to be isMoving
                anim.SetBool("isMoving", true);
                shadowAnim.SetBool("isMoving", true);

                //we set atBottomOfStairs to false so that this code is only executed once per press of the Up Button
                awaitingStaircaseInput = false;
            }
            if(canvasTriggerCol == true)
            {
                CanvasManager.instance.activeCanvas.enabled = true;
                isExamining = true;
                rb.velocity.Set(0, 0);
                if(CanvasManager.instance.activeCanvasName == "Surgical Tools")
                {
                    movementLocked = true;
                }
            }
        }
    }

    void DownInput()
    {
        if(Input.GetAxis("Vertical") == -1)
        {
            //first checks if the player is asking to go up a staircase
            if (awaitingStaircaseInput == true && stairCaseMovement == false && atTopOfStairs == true)
            {
                stairCaseMovement = true;
                currentLerpTime = 0;
                anim.SetBool("isMoving", true);
                shadowAnim.SetBool("isMoving", true);
                awaitingStaircaseInput = false;
            }
        }
    }

    void EInput()
    {
        if(activeTownsfolk != null)
        {
            if (activeTownsfolk.readyForExamination != true)
            {
                DescriptionController.instance.text.text = "Press e to talk to " + activeTownsfolk.name;
            }
            else if(activeTownsfolk.readyForExamination == true && activeTownsfolk.logistics.movementOrdered == false)
            {
                DescriptionController.instance.text.text = "Press e to diagnose " + activeTownsfolk.name;
            }
            if (TextBoxManager.instance.inDialogue == true)
            {
                DescriptionController.instance.text.text = "Press e to continue ";
            }

            if (Input.GetButtonDown("Fire1") && TextBoxManager.instance.inDialogue == false) 
            {
                TextBoxManager.instance.eLock = Time.deltaTime;
                activeTownsfolk.AssembleDialogue();
                TextBoxManager.instance.ActuatingDialogue();
            }
        }

        if(SceneController.instance.activeGUIController != null && SceneController.instance.currentSceneState == SceneController.SceneState.sceneActive)
        {
            DescriptionController.instance.text.text = "Press e to examine " + SceneController.instance.activeGUIController.name;
        }

        if (movementLocked == true && Input.GetButtonDown("Fire1"))
        {
            movementLocked = false;
            SceneController.instance.activeGUIController.DisableGUI();
            SceneController.instance.activeGUIController = null;
            SceneController.instance.SetSceneState(SceneController.SceneState.sceneActive);
        }

        if (Input.GetButtonDown("Fire1") && SceneController.instance.activeGUIController != null)
        {
            SceneController.instance.SetSceneState(SceneController.SceneState.examining);
            SceneController.instance.activeGUIController.EnableGUI();
            movementLocked = true;
        }
    }

    void WhoseSpeakingCheck()
    {
        if(activeTownsfolk != null && SceneController.instance.currentSceneState == SceneController.SceneState.inText)
        {
            if(activeTownsfolk.whoseSpeaking[activeTownsfolk.speakerIndex] == TownsfolkController.Speaker.player)
            {
                speechBubble.enabled = true;
            }
            else
            {
                speechBubble.enabled = false;
            }
        }
        else
        {
            speechBubble.enabled = false;
        }
    }

    void NavigateStairCase()
    {
        //first, we need to work out whether the player is at the top or bottom of the stairs or at the top.

        //stairTriggerEnabled = false;

        if (atBottomOfStairs == true)
        {
            //we work out what direction the player needs to face
            if (stairUpstairsTransform.position.x < stairDownstairsTransform.position.x)
            {
                sprite.flipX = true;
                shadow.flipX = true;
            }
            else
            {
                sprite.flipX = false;
                shadow.flipX = false;
            }

            //Lerp between two points performed here
            currentLerpTime += Time.deltaTime;
            stairLerpPercentage = currentLerpTime / stairMovementSpeed;
            
            trans.position = Vector2.Lerp(stairDownstairsTransform.position, stairUpstairsTransform.position, stairLerpPercentage);
        }

        if(atTopOfStairs == true)
        {
            //we work out what direction the player needs to face
            if (stairUpstairsTransform.position.x < stairDownstairsTransform.position.x)
            {
                sprite.flipX = false;
                shadow.flipX = false;
            }
            else
            {
                sprite.flipX = true;
                shadow.flipX = true;
            }

            //Lerp between two points performed here
            currentLerpTime += Time.deltaTime;
            stairLerpPercentage = currentLerpTime / stairMovementSpeed;
            trans.position = Vector2.Lerp(stairUpstairsTransform.position, stairDownstairsTransform.position, stairLerpPercentage);
        }


        //reorganise the sorting order of the player and their shadow so that they appear beneath the staircase sprite
        sprite.sortingOrder = stairTrigger.stairSprite.sortingOrder - 1;
        shadow.sortingOrder = stairTrigger.stairSprite.sortingOrder - 2;

        if (stairLerpPercentage >= 1)
        {
            if(atBottomOfStairs == true)
            {
                atBottomOfStairs = false;
                atTopOfStairs = true;
                awaitingStaircaseInput = true;
            }
            else if(atTopOfStairs == true)
            {
                atTopOfStairs = false;
                atBottomOfStairs = true;
                awaitingStaircaseInput = true;
            }
            stairCaseMovement = false;
            anim.SetBool("isMoving", false);
            shadowAnim.SetBool("isMoving", false);
            sprite.sortingOrder = 199;
            shadow.sortingOrder = 198;
        }
    }

    void CloseCanvasCheck()
    {
        if(rb.velocity.x > 0.1 || rb.velocity.x < -0.1)
        {
            if (isExamining == true)
            {
                CanvasManager.instance.activeCanvas.enabled = false;
                isExamining = false;
            }
        }
    }
}


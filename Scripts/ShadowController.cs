using UnityEngine;
using System.Collections;

public class ShadowController : MonoBehaviour
{

    public CandleController candle;
    private float baseScale;
    private float basePosition;
    private float baseScaleY;
    private float basePositionY;
    private Quaternion defaultRotation;
    public float litRotationZ;
    public float scaleModifier;
    public float positionModifier;
    public Transform trans;
    public Transform characterTrans;
    public SpriteRenderer sprite;
    public Color spriteColor;
    public bool yShadow;
    public float xAdjustment;
    public float playerShadowPosX;
    public float playerCandleDistance;

    void Awake()
    {
        baseScale = this.transform.localScale.x;
        basePosition = this.transform.localPosition.x;
        baseScaleY = this.transform.localScale.y;
        basePositionY = this.transform.localPosition.y;
        defaultRotation = this.transform.localRotation;
        trans = this.transform;
        sprite = this.GetComponent<SpriteRenderer>();
        spriteColor = sprite.color;
    }
    void Update()
    {
        if (yShadow == true)
        {
            if (candle.lit == true)
            {
                trans.localPosition = new Vector3(trans.localPosition.x * xAdjustment, (positionModifier * basePositionY * candle.candleLight.intensity), trans.localPosition.z);
                trans.localScale = new Vector3(trans.localScale.x, (scaleModifier * baseScaleY * candle.candleLight.intensity), trans.localScale.z);
            }
            if (candle.lit == false)
            {
                trans.localPosition = new Vector3(trans.localPosition.x, basePositionY, trans.localPosition.z);
                trans.localScale = new Vector3(trans.localScale.x, baseScaleY, trans.localScale.z);
            }
        }
        else
        {
            if (candle.lit == true)
            {
                if (this.gameObject.tag == "CharacterShadow")
                {
                    positionModifier = candle.transform.position.x - characterTrans.position.x;
                    positionModifier = Mathf.Clamp(positionModifier, -playerShadowPosX, playerShadowPosX);
                    float candleDist = characterTrans.position.x - candle.transform.position.x;
                    if (Mathf.Abs(candleDist) <= playerCandleDistance)
                    {
                        sprite.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, spriteColor.a * Mathf.Abs(candleDist / 4));
                    }
                    else
                        sprite.color = spriteColor;
                    if (characterTrans.position.x - candle.transform.position.x < 0)
                        litRotationZ = 4;
                    if (characterTrans.position.x - candle.transform.position.x > 0)
                        litRotationZ = -4;
                }
                trans.localPosition = new Vector3((positionModifier * basePosition * candle.candleLight.intensity), trans.localPosition.y, trans.localPosition.z);
                trans.localScale = new Vector3((scaleModifier * baseScale * candle.candleLight.intensity), trans.localScale.y, trans.localScale.z);
                trans.localRotation = Quaternion.Euler(defaultRotation.x, defaultRotation.y, litRotationZ);
            }
            if (candle.lit == false)
            {               
                trans.localPosition = new Vector3(basePosition, trans.localPosition.y, trans.localPosition.z);
                trans.localScale = new Vector3(baseScale, trans.localScale.y, trans.localScale.z);
                trans.localRotation = Quaternion.Euler(defaultRotation.x, defaultRotation.y, defaultRotation.z);
                if (this.gameObject.tag == "CharacterShadow")
                    sprite.color = spriteColor;
            }
        }
    }
}
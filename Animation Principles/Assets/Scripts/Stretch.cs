using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stretch : MonoBehaviour
{
    private Vector3 mOffset;
   
    private float mZCoord;

    public AudioSource hurt;
    public AudioSource stretch;

    public Sprite Blink;
    public Sprite Happy;
    public Sprite Charging;
    bool chargingSprite;
    public GameObject faceSprite;

    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        // Store offset = gameobject world pos - mouse world pos
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        hurt.Play();
        stretch.Play();
        chargingSprite = true;
        faceSprite.GetComponent<SpriteRenderer>().sprite = Charging;
    }

    void OnMouseUp()
    {
        stretch.Stop();
    }


    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;
        // z coordinate of game object on screen
        mousePoint.z = mZCoord;
        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        transform.position = new Vector3(GetMouseAsWorldPoint().z + mOffset.z, GetMouseAsWorldPoint().y + mOffset.y, transform.position.x);
       
        
    }

    void Start()
    {
        if (chargingSprite == false)
        {
            InvokeRepeating("Blinking", 0.5f, 2.0f);
            InvokeRepeating("BlinkingOpen", 1.0f, 1.0f);
        }

        //shakeGameObject(GameObjectToShake, 5, 3f, false);
    }

    void Blinking()
    {
        faceSprite.GetComponent<SpriteRenderer>().sprite = Blink;
    }
    void BlinkingOpen()
    {
        faceSprite.GetComponent<SpriteRenderer>().sprite = Happy;
    }
}
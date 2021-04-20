using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : MonoBehaviour
{

    public Animator anim;
    public ParticleSystem air;
    public ParticleSystem airPoof;
    public ParticleSystem airLine;
    public AudioSource jumpSigh;
    public AudioSource drop;

    public Sprite Blink;
    public Sprite Happy;
    public Sprite Charging;
    bool chargingSprite;
    public GameObject faceSprite;

    // Start is called before the first frame update

    private void Awake()
    {
        
    }
    void Start()
    {
        if (chargingSprite == false)
        {
            InvokeRepeating("Blinking", 0.5f, 2.0f);
            InvokeRepeating("BlinkingOpen", 1.0f, 1.0f);
        }
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            anim.Play("Jump");
            airPoof.Play();
            airLine.Play();
            jumpSigh.Play();
            StartCoroutine(GustAir());
            chargingSprite = true;
            faceSprite.GetComponent<SpriteRenderer>().sprite = Charging;



        }
    }

    IEnumerator GustAir()
    {
        yield return new WaitForSeconds(1.0f);
        air.Play();
        drop.Play();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public GameObject GameObjectToShake;
    bool shaking = false;
    private float mSize = 0.0f;
    public ParticleSystem sweat;
    public ParticleSystem boom;
    public AudioSource charge;
    public AudioSource kaboom;
    public Sprite Blink;
    public Sprite Happy;
    public Sprite Charging;
    bool chargingSprite ;
    public GameObject faceSprite;

    IEnumerator shakeGameObjectCOR(GameObject objectToShake, float totalShakeDuration, float decreasePoint, bool objectIs2D = false)
    {
        if (decreasePoint >= totalShakeDuration)
        {
            Debug.LogError("decreasePoint must be less than totalShakeDuration...Exiting");
            yield break; //Exit!
        }

        //Get Original Pos and rot
        Transform objTransform = objectToShake.transform;
        Vector3 defaultPos = objTransform.position;
        Quaternion defaultRot = objTransform.rotation;

        float counter = 0f;

        //Shake Speed
        const float speed = 0.1f;

        //Angle Rotation(Optional)
        const float angleRot = 4;

        //Do the actual shaking
        while (counter < totalShakeDuration)
        {
            counter += Time.deltaTime;
            float decreaseSpeed = speed;
            float decreaseAngle = angleRot;

            //Shake GameObject
            if (objectIs2D)
            {
                //Don't Translate the Z Axis if 2D Object
                Vector3 tempPos = defaultPos + UnityEngine.Random.insideUnitSphere * decreaseSpeed;
                tempPos.z = defaultPos.z;
                objTransform.position = tempPos;

                //Only Rotate the Z axis if 2D
                objTransform.rotation = defaultRot * Quaternion.AngleAxis(UnityEngine.Random.Range(-angleRot, angleRot), new Vector3(0f, 0f, 1f));
            }
            else
            {
                objTransform.position = defaultPos + UnityEngine.Random.insideUnitSphere * decreaseSpeed;
                objTransform.rotation = defaultRot * Quaternion.AngleAxis(UnityEngine.Random.Range(-angleRot, angleRot), new Vector3(1f, 1f, 1f));
            }
            yield return null;


            //Check if we have reached the decreasePoint then start decreasing  decreaseSpeed value
            if (counter >= decreasePoint)
            {
                Debug.Log("Decreasing shake");

                //Reset counter to 0 
                counter = 0f;
                while (counter <= decreasePoint)
                {
                    counter += Time.deltaTime;
                    decreaseSpeed = Mathf.Lerp(speed, 0, counter / decreasePoint);
                    decreaseAngle = Mathf.Lerp(angleRot, 0, counter / decreasePoint);

                    Debug.Log("Decrease Value: " + decreaseSpeed);

                    //Shake GameObject
                    if (objectIs2D)
                    {
                        //Don't Translate the Z Axis if 2D Object
                        Vector3 tempPos = defaultPos + UnityEngine.Random.insideUnitSphere * decreaseSpeed;
                        tempPos.z = defaultPos.z;
                        objTransform.position = tempPos;

                        //Only Rotate the Z axis if 2D
                        objTransform.rotation = defaultRot * Quaternion.AngleAxis(UnityEngine.Random.Range(-decreaseAngle, decreaseAngle), new Vector3(0f, 0f, 1f));
                    }
                    else
                    {
                        objTransform.position = defaultPos + UnityEngine.Random.insideUnitSphere * decreaseSpeed;
                        objTransform.rotation = defaultRot * Quaternion.AngleAxis(UnityEngine.Random.Range(-decreaseAngle, decreaseAngle), new Vector3(1f, 1f, 1f));
                    }
                    yield return null;
                }

                //Break from the outer loop
                break;
            }
        }
        objTransform.position = defaultPos; //Reset to original postion
        objTransform.rotation = defaultRot;//Reset to original rotation

        shaking = false; //So that we can call this function next time
        Debug.Log("Done!");
    }


    void shakeGameObject(GameObject objectToShake, float shakeDuration, float decreasePoint, bool objectIs2D = false)
    {
        if (shaking)
        {
            return;

        }
        shaking = true;
        StartCoroutine(shakeGameObjectCOR(objectToShake, shakeDuration, decreasePoint, objectIs2D));
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
    void Update()
    {
        



        if (Input.GetKeyDown(KeyCode.Space))
        {
            sweat.Play();
            charge.Play();
            chargingSprite = true;
            faceSprite.GetComponent<SpriteRenderer>().sprite = Charging;
            InvokeRepeating("Scale", 0.0f, 0.01f);
            shakeGameObject(GameObjectToShake, 2, 1f, false);
            StartCoroutine(ResetForm());
            
        }
    }

    void Scale()
    {
        if (mSize >= 100.0f)
        {
            CancelInvoke("Scale");
        }

        GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, mSize++);
        
        

    }
    IEnumerator ResetForm()
    {
        yield return new WaitForSeconds(2.0f);
        InvokeRepeating("NoScale", 0.0f, 0.001f);
        boom.Play();
        kaboom.Play();
        
    }

    void NoScale()
    {
        if (mSize <= 0.0f)
        {
            CancelInvoke("NoScale");
            
        }

        GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, mSize--);
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

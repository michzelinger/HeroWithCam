using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
   public bool isShaking = false;
   private Vector2 originPosition;
   private Quaternion originRotation;
   public float shake_decay = 0.002f;
   public float shake_intensity = 0.3f;

   private float temp_shake_intensity = 0;
   // public float shake_speed;
   //  public float shake_intensity;
   // public bool isShaking = true;
   private Vector3 mInitPosition = Vector3.zero;
   private int mHitCount = 0;
   private const int kHitLimit = 3;
   private const float kRepositionRange = 15f; // +- this value
   private Color mNormalColor = Color.white;

    public WaypointCam waypointCam;

   // Start is called before the first frame update
   void Start()
   {
      mInitPosition = transform.position;
      waypointCam = FindObjectOfType<WaypointCam>();
   }

   void Update()
   {

   }

   private void Reposition()
   {
       Debug.Log("Reposition");
      Vector3 p = mInitPosition;
      p += new Vector3(Random.Range(-kRepositionRange, kRepositionRange),
                       Random.Range(-kRepositionRange, kRepositionRange),
                       0f);
      transform.position = p;
      GetComponent<SpriteRenderer>().color = mNormalColor;
   }

    private IEnumerator shakeWaypoint(float totalShakeDuration, float magnitutde)
    {
        isShaking = true;
        Transform objTransform = gameObject.transform;
        Vector3 defaultPos = objTransform.position;
        Quaternion defaultRot = objTransform.rotation;

        float counter = 0f;

        const float speed = 0.1f;

        const float angleRot = 1.0f;

        if(!waypointCam.enabled) // not active
        {
            waypointCam.enabled = true;
            waypointCam.setTarget(transform.position);
        }

        while(counter < totalShakeDuration)
        {
            counter += Time.deltaTime;
            Vector3 tempPos = defaultPos + UnityEngine.Random.insideUnitSphere * magnitutde;
            tempPos.z = defaultPos.z;
            objTransform.position = tempPos;
            objTransform.rotation = defaultRot * Quaternion.AngleAxis(UnityEngine.Random.Range(-angleRot, angleRot), new Vector3(0f,0f,1f));

            yield return null;
        }
        isShaking = false;
        objTransform.position = defaultPos;
        objTransform.rotation = defaultRot;

        Debug.Log("Done");
    }
    private void shakeObject(float duration, float magnitutde)
    {
        StartCoroutine(shakeWaypoint(duration, magnitutde));
        return;
    }
   /*public void CallShake()
   {
      StartCoroutine(WaypointShake(5.0f, 5.0f));
   }*/
   

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.gameObject.name == "Egg(Clone)")
      {
         mHitCount++;
         Color c = mNormalColor * (float)(kHitLimit - mHitCount + 1) / (float)(kHitLimit + 1);
         GetComponent<SpriteRenderer>().color = c;
         if(mHitCount <= kHitLimit)
         {
             shakeObject(mHitCount, mHitCount);
         }
         else if (mHitCount > kHitLimit)
         {
            Debug.Log("goes here IF");
            mHitCount = 0;
            Reposition();
         }
         
      }
   }
}

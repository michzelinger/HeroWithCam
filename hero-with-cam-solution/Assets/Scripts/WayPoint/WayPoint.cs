using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
   public bool isShaking = false;
   private Vector3 mInitPosition = Vector3.zero;
   private int mHitCount = 0;
   private const int kHitLimit = 3;
   private const float kRepositionRange = 15f; // +- this value
   private Color mNormalColor = Color.white;

   private CameraManager cameraManager;

   // Start is called before the first frame update
   void Start()
   {
      mInitPosition = transform.position;
      cameraManager = FindObjectOfType<CameraManager>();
   }

   private void Reposition()
   {
      StopAllCoroutines();
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

      const float angleRot = 1.0f;

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
      cameraManager.activateWaypointCam(duration, transform.position);
      StartCoroutine(shakeWaypoint(duration, magnitutde));
   }
   
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

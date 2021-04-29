using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCamera : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    
    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        //transform.LookAt(target);
    }
    /*public void CallShake()
    {
        StartCoroutine(Shake(1.0f, 1.0f));
    }

    public IEnumerator Shake (float duration, float magnitutde)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while(elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitutde;
            float y = Random.Range(-1f, 1f) * magnitutde;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = originalPos;
    }*/
    private IEnumerator shakeWaypoint(float totalShakeDuration, float magnitutde)
    {
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
        objTransform.position = defaultPos;
        objTransform.rotation = defaultRot;

        Debug.Log("Done");
    }
    public void shakeObject(float duration, float magnitutde)
    {
        StartCoroutine(shakeWaypoint(duration, magnitutde));
        return;
    }
}
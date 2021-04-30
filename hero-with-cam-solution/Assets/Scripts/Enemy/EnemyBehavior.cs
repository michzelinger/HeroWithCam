using UnityEngine;
using System.Collections;

public partial class EnemyBehavior : MonoBehaviour {

    // All instances of Enemy shares this one WayPoint and EnemySystem
    static private WayPointSystem sWayPoints = null;
    static private EnemySpawnSystem sEnemySystem = null;
    static public void InitializeEnemySystem(EnemySpawnSystem s, WayPointSystem w) { sEnemySystem = s; sWayPoints = w; }

    private const float kSpeed = 5.0f;
    private int mWayPointIndex = 0;

    private const float kTurnRate = 0.03f/60f;

    CameraManager cameraManager;

    private float lerpDuration = 2.0f;
    private Vector3 pushBackLocation;
		
	// Use this for initialization
	void Start () {
        cameraManager = FindObjectOfType<CameraManager>();
        mWayPointIndex = sWayPoints.GetInitWayIndex();
	}
	
	// Update is called once per frame
	void Update () {
        if (mState == EnemyState.ePatrolState)
        {
            sWayPoints.CheckNextWayPoint(transform.position, ref mWayPointIndex);
            PointAtPosition(sWayPoints.WayPoint(mWayPointIndex), kTurnRate);
            transform.position += (kSpeed * Time.smoothDeltaTime) * transform.up;
        }
        UpdateFSM();
    }

    IEnumerator Lerp()
    {
        float timeElapsed = 0f;
        Vector3 originalPos = transform.position;

        while (timeElapsed < lerpDuration)
        {
            transform.position = Vector3.Lerp(originalPos, pushBackLocation, (timeElapsed / lerpDuration));
            timeElapsed += Time.smoothDeltaTime;

            yield return null;
        }

        transform.position = pushBackLocation;
    }

    private void PointAtPosition(Vector3 p, float r)
    {
        Vector3 v = p - transform.position;
        transform.up = Vector3.LerpUnclamped(transform.up, v, r);
    }

    #region Trigger into chase or die
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Hero")
        {
            if(mState == EnemyState.eChaseState)
            {
                ThisEnemyIsHit();
            }
            else if (mState == EnemyState.ePatrolState)
            {
                mState = EnemyState.eCCWRotation;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Egg(Clone)")
        {
            if(mState == EnemyState.eEggState) // Egg state
            {
                ThisEnemyIsHit();
            }
            else if(mState == EnemyState.eStunnedState) // Stunned state
            {
                PushBack(8.0f, collision.transform.up);
                mState = EnemyState.eEggState;
            }
            else // Patrol state
            {
                PushBack(4.0f, collision.transform.up);
                mState = EnemyState.eStunnedState;
            }
        }
    }

    private void PushBack(float distance, Vector3 direction)
    {
        StopAllCoroutines();
        pushBackLocation = transform.position + direction * distance;
        StartCoroutine(Lerp());
    }

    private void ThisEnemyIsHit()
    {
        sEnemySystem.OneEnemyDestroyed();
        Destroy(gameObject);
    }
    #endregion

    public bool isChasing()
    {
        return mState == EnemyState.eChaseState;
    }
}

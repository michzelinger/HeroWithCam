using UnityEngine;
using System.Collections;

public partial class EnemyBehavior : MonoBehaviour {

    // All instances of Enemy shares this one WayPoint and EnemySystem
    static private WayPointSystem sWayPoints = null;
    static private EnemySpawnSystem sEnemySystem = null;
    static public void InitializeEnemySystem(EnemySpawnSystem s, WayPointSystem w) { sEnemySystem = s; sWayPoints = w; }

    private const float kSpeed = 20f;
    private int mWayPointIndex = 0;

    private const float kTurnRate = 0.03f/60f;

    CameraManager cameraManager;
		
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

    private void PointAtPosition(Vector3 p, float r)
    {
        Vector3 v = p - transform.position;
        transform.up = Vector3.LerpUnclamped(transform.up, v, r);
    }

    #region Trigger into chase or die
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerCheck(collision.gameObject);
    }

    private void TriggerCheck(GameObject g)
    {
        if (g.name == "Hero")
        {
            if(mState == EnemyState.eChaseState)
            {
                ThisEnemyIsHit();
            }
            else if (mState == EnemyState.ePatrolState)
            {
                mState = EnemyState.eCCWRotation;
            }

        } else if (g.name == "Egg(Clone)")
        {
            if(mState == EnemyState.eEggState)
            {
                ThisEnemyIsHit();
            }
            else if(mState == EnemyState.eStunnedState)
            {
                mState = EnemyState.eEggState;
            }
            else
            {
                mState = EnemyState.eStunnedState;
            }
        }
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

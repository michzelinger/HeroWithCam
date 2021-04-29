using UnityEngine;
public partial class EnemyBehavior : MonoBehaviour {
    // partial class so at compile it will merge with EnemyBehavior.cs

    //FSM commands
    private enum EnemyState
    {
        ePatrolState,
        eCCWRotation,
        eCWRotation,
        eChaseState,
        eEnlargeState,
        eShrinkState,
        eStunnedState,
        eEggState
    };
    private const float kSizeChangeFrames = 120f; //To increase size of enemy
    private const float kRotateFrames = 60f; //For roation
    private const float kScaleRate = 0.5f / 60f; //around per second rate
    private const float kRotateRate = 45f / 60f; //in degrees, around per second rate
    
    private int mStateFrameTick = 0;
    private EnemyState mState = EnemyState.ePatrolState;

    private void UpdateFSM()
    {
        switch (mState)
        {
            case EnemyState.eCCWRotation:
                ServiceCCWRotation();
                break;
            case EnemyState.eCWRotation:
                ServiceCWRotation();
                break;
            case EnemyState.eChaseState:
                ServiceChaseState();
                break;   
            case EnemyState.eEnlargeState:
                ServiceEnlargeState();
                break;
            case EnemyState.eShrinkState:
                ServiceShrinkState();
                break;
            case EnemyState.eStunnedState:
                ServiceStunnedState();
                break;
            case EnemyState.eEggState:
                ServiceEggState();
                break;
            case EnemyState.ePatrolState:
                break;
        }
    }

    private void ServiceCCWRotation()
    {
        //Debug.Log("Entered CCW State");
        if (mStateFrameTick > kRotateFrames)
        {
            mState = EnemyState.eCWRotation;
            mStateFrameTick = 0;
        }
        else
        {
            mStateFrameTick++; // Increment the frame counter

            Vector3 angles = transform.rotation.eulerAngles;
            angles.z -= kRotateRate;
            transform.rotation = Quaternion.Euler(0, 0, angles.z);
        }
    }

    private void ServiceCWRotation()
    {
        //Debug.Log("Entered CW State");
        if (mStateFrameTick > kRotateFrames)
        {
            mState = EnemyState.eChaseState;
            mStateFrameTick = 0;
        }
        else
        {
            mStateFrameTick++; // Increment the frame counter

            Vector3 angles = transform.rotation.eulerAngles;
            angles.z += kRotateRate;
            transform.rotation = Quaternion.Euler(0, 0, angles.z);
        }
    }

    private void ServiceChaseState()
    {
        //Debug.Log("In Chase State");

        GetComponent<SpriteRenderer>().color = Color.red;

        HeroBehavior player = FindObjectOfType<HeroBehavior>();
        
        Vector2 currPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);

        cameraManager.activateEnemyCam(this, player);

        if(Vector2.Distance(playerPos, currPos) <= 40f)
        {
            currPos = Vector2.MoveTowards(currPos, playerPos, kSpeed * Time.deltaTime);
            transform.up = Vector3.Normalize(playerPos - currPos);
            
            transform.position = currPos;
        }
        else
        {
            Debug.Log("ChaseState -> PartolState");
            mState = EnemyState.eEnlargeState;
        }
    }

    private void ServiceEnlargeState()
    {
        //Debug.Log("Entered Enlarge State");
        if (mStateFrameTick > kSizeChangeFrames)
        {
            //Transition to next state
            mState = EnemyState.eShrinkState;
            mStateFrameTick = 0;
        }
        else
        {
            mStateFrameTick++;

            //assume scale in X/Y are the same
            float s = transform.localScale.x;
            s += kScaleRate;
            transform.localScale = new Vector3(s, s, 1);
        }
    }

    private void ServiceShrinkState()
    {
        //Debug.Log("Entered Shrink State");
        if (mStateFrameTick > kSizeChangeFrames)
        {
            // Resetting the color
            GetComponent<SpriteRenderer>().color = Color.white;

            //Transition to next state
            mState = EnemyState.ePatrolState;
            mStateFrameTick = 0;
        }
        else
        {
            mStateFrameTick++;

            //assume scale in X/Y are the same
            float s = transform.localScale.x;
            s -= kScaleRate;
            transform.localScale = new Vector3(s, s, 1);
        }
    }

    private void ServiceStunnedState()
    {

        //Debug.Log("Entered Stunned State");
        //mStateFrameTick++; // Increment the frame counter
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/shuriken") as Sprite;
        
        Vector3 angles = transform.rotation.eulerAngles;
        angles.z -= 720f / 90f;
        transform.rotation = Quaternion.Euler(0, 0, angles.z);


    }
    
    private void ServiceEggState()
    {
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Egg") as Sprite;
        Debug.Log("Entered Egg State");
    } 

}


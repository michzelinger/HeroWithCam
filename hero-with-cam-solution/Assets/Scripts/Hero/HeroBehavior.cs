using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeroBehavior : MonoBehaviour {
    
    public Restart restartButton;
    public Text mTimeSurvivedText = null;
    public float maxHealth = 100f;
    public float heroHealth;
    public HealthBarBehavior healthBar;
    public EggSpawnSystem mEggSystem = null;
    //public Text controlsHero = null;
    private const float kHeroRotateSpeed = 90f/2f; // 90-degrees in 2 seconds
    private const float kHeroSpeed = 20f;  // 20-units in a second
    private float mHeroSpeed = kHeroSpeed;
    
    public HeroCamera heroCamera;
    private bool mMouseDrive = true;
    //  Hero state
    private int mHeroTouchedEnemy = 0;
    private void TouchedEnemy() { mHeroTouchedEnemy++; }
    public string GetHeroState() { return "HERO: Drive(" + (mMouseDrive?"Mouse":"Key") + 
                                          ") TouchedEnemy(" + mHeroTouchedEnemy + ")   " 
                                            + mEggSystem.EggSystemStatus(); }

    private void Awake()
    {
        // Actually since Hero spwans eggs, this can be done in the Start() function, but, 
        // just to show this can also be done here.
        restartButton = FindObjectOfType<Restart>() as Restart;
        heroCamera = FindObjectOfType<HeroCamera>() as HeroCamera;
        Debug.Assert(mEggSystem != null);
        EggBehavior.InitializeEggSystem(mEggSystem);
        heroHealth = maxHealth;
        healthBar.SetHealth(heroHealth, maxHealth);
    }

    void Start ()
    { 
    }
	
	// Update is called once per frame
	void Update () {
        UpdateMotion();
        ProcessEggSpwan();
    }

    private int EggsOnScreen() { return mEggSystem.GetEggCount();  }

    private void UpdateMotion()
    {
        if (Input.GetKeyDown(KeyCode.M))
            mMouseDrive = !mMouseDrive;
            
        // Only support rotation
        transform.Rotate(Vector3.forward, -1f * Input.GetAxis("Horizontal") *
                                    (kHeroRotateSpeed * Time.smoothDeltaTime));
        if (mMouseDrive)
        {
            Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            p.z = 0f;
            transform.position = p;
            //controlsHero.text = "Controls: Mouse";

        } else
        {
            mHeroSpeed += Input.GetAxis("Vertical");
            transform.position += transform.up * (mHeroSpeed * Time.smoothDeltaTime);
            //controlsHero.text = "Controls: Keyboard";
        }
    }

    private void ProcessEggSpwan()
    {
        if (mEggSystem.CanSpawn())
        {
            if (Input.GetKey("space"))
            {
                mEggSystem.SpawnAnEgg(transform.position, transform.up);
                heroCamera.shakeObject(.5f, 1.0f);
            }
        }
    }

    public void DamageHero(float damage)
    {
        heroHealth -= damage;
        healthBar.SetHealth(heroHealth, maxHealth);
        Debug.Log("Hero Health: " + heroHealth);
        if(heroHealth == 0)
        {
            //Destroy(gameObject);
            var _time = (int)Time.time;
            GameObject.Find ("Hero").transform.localScale = new Vector3(0, 0, 0);
            GameObject.Find("RestartButton").transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            mTimeSurvivedText.text = "Time Survived: " + _time + " Seconds";
            Time.timeScale = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hero touched");
        if (collision.gameObject.name == "Enemy(Clone)")
            TouchedEnemy();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
   public float kBulletSpeed = 1f;
   public int bulletDamage = 10;
   // Start is called before the first frame update
   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {
      bool outside = GameManager.sTheGlobalBehavior.CollideWorldBound(GetComponent<Renderer>().bounds) == CameraSupport.WorldBoundStatus.Outside;
      if (outside)
      {
         Destroy(gameObject);
      }
      transform.position += transform.up * (kBulletSpeed * Time.smoothDeltaTime);
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.gameObject.tag == "Hero")
      {
         HeroBehavior hero = collision.gameObject.GetComponent<HeroBehavior>();
         hero.DamageHero(bulletDamage);
         Destroy(gameObject);
      }
   }
}

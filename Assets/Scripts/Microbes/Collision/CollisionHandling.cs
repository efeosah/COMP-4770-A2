using UnityEngine;

namespace Microbes.Collision
{
    // TODO for A2 (optional): Add some collision or trigger handling such as power-ups
    public class CollisionHandling : MonoBehaviour
    { 
        // If you need access to the associated microbe, you can use the following.

        #region Microbe Access

        //Microbe microbe;

        //public void Awake()
        //{
        //    microbe = gameObject.GetComponent<Microbe>();
        //}

        #endregion Microbe Access

        // If you want to handle collisions, you can do it here

        #region Collision Handling

        //public void OnCollisionEnter(Collision collision)
        //{
        //}

        //public void OnCollisionStay(Collision collision)
        //{
        //}

        //public void OnCollisionExit(Collision collision)
        //{
        //}

        #endregion Collision Handling

        // If you set microbe.collider.isTrigger = true; then the microbes will no longer collide and bounce off each other.
        // Instead, you can intercept the collision trigger below and do what you want when microbes touch.

        #region Trigger Handling

        //public void OnTriggerEnter(Collider other)
        //{
        //}

        //public void OnTriggerStay(Collider other)
        //{
        //}

        //public void OnTriggerExit(Collider other)
        //{
        //}

        #endregion Trigger Handling
    }
}
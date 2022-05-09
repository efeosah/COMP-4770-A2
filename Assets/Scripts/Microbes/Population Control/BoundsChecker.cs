using Microbes.Entities;
using UnityEngine;

namespace Microbes.Population_Control
{
    public delegate void OnOutOfBoundsDelegate(Microbe microbe);

    public delegate void OnAtBoundsDelegate(Microbe microbe);

    //[RequireComponent(typeof(Microbe))]

    // Deal with microbes at or past the specified bounds (or camera view).
    public sealed class BoundsChecker : MonoBehaviour
    {
        // The method to call on going out of bounds.
        public OnOutOfBoundsDelegate onOutOfBounds;
    
        // The method to call on reaching the bounds.
        public OnAtBoundsDelegate onAtBounds;
    
        // The minimum bounds offset from (0,0).
        public Vector2 minimumOffset;
    
        // The maximum bounds offset from (0,0).
        public Vector2 maximumOffset;

        static OnAtBoundsDelegate onAtBoundsKill;

        static OnOutOfBoundsDelegate onOutOfBoundsKill;

        Microbe microbe;
    
        // Gets a delegate to kill an out of bounds object.
        public static OnOutOfBoundsDelegate OnOutOfBoundsKill
        {
            get
            {
                return onOutOfBoundsKill ??= Kill;
            }
        }
    
        // Gets a delegate to kill an object at the bounds.
        public static OnAtBoundsDelegate OnAtBoundsKill
        {
            get
            {
                return onAtBoundsKill ??= Kill;
            }
        }

        public void Awake()
        {
            microbe = gameObject.GetComponent<Microbe>();
            if (onOutOfBounds == null)
            {
                onOutOfBounds = OnOutOfBoundsKill;
            }

            if (onAtBounds == null)
            {
                onAtBounds = OnAtBoundsKill;
            }
        }

        public void Update()
        {
            float radius = microbe.transform.localScale.x / 2.0f; // assume sphere where scale x = scale y = scale z
            float minX = minimumOffset.x;
            float minY = minimumOffset.y;
            float maxX = maximumOffset.x;
            float maxY = maximumOffset.y;

            if (microbe != null && onOutOfBounds != null &&
                (microbe.transform.position.x < minX - radius ||
                 microbe.transform.position.x > maxX + radius ||
                 microbe.transform.position.z < minY - radius ||
                 microbe.transform.position.z > maxY + radius))
            {
                onOutOfBounds(microbe);
            }

            if (microbe != null && onAtBounds != null &&
                (microbe.transform.position.x < minX + radius ||
                 microbe.transform.position.x > maxX - radius ||
                 microbe.transform.position.z < minY + radius ||
                 microbe.transform.position.z > maxY - radius))
            {
                onAtBounds(microbe);
            }
        }
    
        // Kill the microbe.
        static void Kill(Microbe microbe)
        {
            // TODO for A2 (optional): Play different sound for out of bounds death?
            // TODO for A2 (optional): special effects (poof)?
            microbe.Die();
        }
    }
}
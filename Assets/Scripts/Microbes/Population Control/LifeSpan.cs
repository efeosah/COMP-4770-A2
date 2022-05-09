using Microbes.Entities;
using UnityEngine;

namespace Microbes.Population_Control
{
    [RequireComponent(typeof(Microbe))]

    // A behaviour to kill off microbes at the end of their lifespan.
    public class LifeSpan : MonoBehaviour
    {
        public float lifeSpan = 500;
        public float lifeSpanVariation = 100;
        float age;
        public float Age => age;
        Microbe microbe;

        public void Start()
        {
            microbe = gameObject.GetComponent<Microbe>();

            // randomly adjust life span
            lifeSpan += (float)(Random.value - 0.5f) * lifeSpanVariation;
        }

        public void Update()
        {
            age += Time.deltaTime;
            if (age >= lifeSpan)
            {
                // TODO for A2 (optional): play different sound for life span death.
                // TODO for A2 (optional): special effects (dissolve)?
                //microbe.Die();
            }
        }
    }
}
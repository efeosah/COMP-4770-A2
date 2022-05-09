using System.Collections.Generic;
using System.Linq;
using GameBrains.Entities;
using GameBrains.Extensions.MonoBehaviours;
using Microbes.Entities;
using UnityEngine;

namespace Microbes.Movement
{
    public class Attractor : ExtendedMonoBehaviour
    {
        public float strength = 5;
        
        // The radius of the attraction.
        public float radius = 400;

        protected readonly List<Microbe> oldNearbyMicrobes = new List<Microbe>();

        protected readonly List<Microbe> nearbyMicrobes = new List<Microbe>();

        protected List<Microbe> noLongerPullingMicrobes;

        protected Microbe microbe;
        
        // Gets or sets the strength of the attraction.
        public float Strength
        {
            get => strength;

            set => strength = value;
        }
        
        // Which types of objects get attracted.
        public MicrobeTypes AttractTypes { get; set; }

        public override void Awake()
        {
            base.Awake();
            
            microbe = gameObject.GetComponent<Microbe>();
        }

        public override void Update()
        {
            base.Update();
            
            // Keep track of which microbes were nearby last tick.
            oldNearbyMicrobes.Clear();
            oldNearbyMicrobes.AddRange(nearbyMicrobes);

            nearbyMicrobes.Clear();

            // Find all microbes that match all of the attract types in a certain radius.
            foreach (Microbe existingMicrobe in EntityManager.FindAll<Microbe>())
            {
                if (microbe != existingMicrobe &&
                    (Vector3.Distance(existingMicrobe.transform.position, transform.position) <= radius) &&
                    ((existingMicrobe.microbeType & AttractTypes) != 0))
                {
                    nearbyMicrobes.Add(existingMicrobe);
                }
            }

            // Now tell the microbes that are within our attraction radius that we're attracting them.
            foreach (Microbe nearbyMicrobe in nearbyMicrobes)
            {
                // can't attract yourself!
                if (nearbyMicrobe.gameObject == gameObject)
                {
                    continue;
                }

                MicrobeMotor microbeMotor = nearbyMicrobe.MicrobeMotor;
                if (microbeMotor != null)
                {
                    // tell the object who we are, and how hard we're pulling.
                    microbeMotor.AddPull(microbe, Strength);
                }
            }

            // Remember how we kept track of which microbes we were pulling last update?
            // Well, if we're not going to pull them anymore, we need to tell them.
            noLongerPullingMicrobes = oldNearbyMicrobes.Where(a => !nearbyMicrobes.Contains(a)).ToList();
            foreach (Microbe oldMicrobe in noLongerPullingMicrobes)
            {
                if (oldMicrobe != null)
                {
                    MicrobeMotor microbeMotor = oldMicrobe.MicrobeMotor;
                    if (microbeMotor != null)
                    {
                        // it's a microbe we can affect...
                        microbeMotor.RemovePull(microbe);
                    }
                }
            }
        }
    }
}
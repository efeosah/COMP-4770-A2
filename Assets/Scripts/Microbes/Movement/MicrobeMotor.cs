using System.Collections.Generic;
using GameBrains.Actuators;
using GameBrains.Entities;
using Microbes.Entities;
using UnityEngine;

namespace Microbes.Movement
{
    [RequireComponent(typeof(Rigidbody))]

    // TODO for A2 (optional): Better integrate with the agent architecture.
    public class MicrobeMotor : Actuator
    {
        public float maximumSpeed = 50;

        public float maximumAcceleration = 10;

        // Who is pulling and by how much.
        readonly Dictionary<Microbe, float> pull = new Dictionary<Microbe, float>();

        // TODO for A2 (optional): Should using Kinematic Data
        Vector2 desiredDelta;
        Vector2 desiredVelocity = Vector2.zero;
        Vector2 netDesiredVelocity = Vector2.zero;

        public override void Update()
        {
            base.Update();
            
            // TODO: This should take elapsed time and time scale into account.

            #region Try commenting this out and try to spot the problem!!!

            var pullerList = new List<Microbe>(pull.Keys);
            foreach (Microbe puller in pullerList)
            {
                // TODO: When an entity is removed from the entity manager,
                // it should clean up after itself. ... but it doesn't so
                // we catch the problem here for now.
                if (EntityManager.Find<Microbe>(puller.ID) == null)
                {
                    pull.Remove(puller);
                }
            }

            #endregion Try commenting this out and try to spot the problem!!!

            netDesiredVelocity = Vector2.zero;

            foreach (Microbe puller in pull.Keys)
            {
                if (puller.transform != null)
                {
                    Vector3 distance3D = puller.transform.position - transform.position;
                    desiredVelocity = new Vector2(distance3D.x, distance3D.z);

                    if (desiredVelocity.sqrMagnitude > 0.001f)
                    {
                        desiredVelocity.Normalize();
                    }

                    // scale by the strength
                    desiredVelocity *= pull[puller];
                    netDesiredVelocity += desiredVelocity;
                }
            }

            // Cap the velocity with the max speed.
            if (netDesiredVelocity.magnitude > maximumSpeed)
            {
                netDesiredVelocity.Normalize();
                netDesiredVelocity *= maximumSpeed;
            }

            // calculate our delta
            Vector3 velocity3D = transform.GetComponent<Rigidbody>().velocity;
            desiredDelta = netDesiredVelocity - new Vector2(velocity3D.x, velocity3D.z);

            // Cap the acceleration with the max speed delta.
            if (desiredDelta.magnitude > maximumAcceleration)
            {
                desiredDelta.Normalize();
                desiredDelta *= maximumAcceleration;
            }

            // TODO: cache rigidbody
            transform.GetComponent<Rigidbody>().velocity += new Vector3(desiredDelta.x, 0, desiredDelta.y);
        }
    
        // Add a pull from the given source with the given strength.
        public void AddPull(Microbe source, float strength)
        {
            pull[source] = strength;
        }
    
        // Remove the pull of the given source.
        public void RemovePull(Microbe source)
        {
            pull.Remove(source);
        }
    }
}
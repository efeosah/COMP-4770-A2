using GameBrains.Entities.EntityData;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace Testing
{
    [AddComponentMenu("Scripts/Testing/W12 Test Kinematic Data")]
    public class W12TestKinematicData : W11TestStaticData
    {
        public VectorXZ velocity;
        public VectorXZ acceleration;

        public bool setVelocity;
        public bool setAcceleration;

        VectorXYZ lastPosition;

        public KinematicData KinematicData => (KinematicData)staticData;

        public override void Awake()
        {
            base.Awake();
            
            if (agentTransform == null) { return; }
            
            staticData
                = new KinematicData(agentTransform); // { Radius = 0.75f }; // weebles with arms are wider

            lastPosition = KinematicData.Position;
        }

        public override void Update()
        {
            base.Update();
            
            if (agentTransform == null) { return; }

            if (setVelocity)
            {
                setVelocity = false;
                KinematicData.Velocity = velocity;
            }

            if (setAcceleration)
            {
                setAcceleration = false;
                KinematicData.Acceleration = acceleration;
            }

            KinematicData.Update(Time.deltaTime);

            if (lastPosition != KinematicData.Position)
            {
                lastPosition = KinematicData.Position;
                Debug.Log("P:" + KinematicData.Position + " V: " + KinematicData.Velocity);
            }
        }
    }
}
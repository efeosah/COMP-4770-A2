using GameBrains.Entities.EntityData;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using GameBrains.Visualization;
using UnityEngine;

namespace Testing
{
    [AddComponentMenu("Scripts/Testing/W1 Test Static Data")]
    public class W11TestStaticData : ExtendedMonoBehaviour
    {
        public StaticData staticData;
        public Transform agentTransform;
        public VectorXYZ lookTargetPosition;
        public VectorXYZ moveTargetPosition;
        public bool checkHasLineOfSight;
        // public bool checkCanMoveTo;
        // public bool checkCanStepLeft;
        // public bool checkCanStepRight;
        public bool checkIsAtPosition;

        public RayCastVisualizer rayCastVisualizer;
        //public CapsuleCastVisualizer capsuleCastVisualizer;

        //public float castRadiusMultiplier = 1.0f;
        public float closeEnoughDistance = 1.0f;

        public override void Awake()
        {
            base.Awake();

            if (agentTransform == null) { return; }
            
            staticData
                //= new StaticData(agentTransform) { Radius = 0.75f }; // weebles with arms are wider
                = new StaticData(agentTransform);
            rayCastVisualizer = ScriptableObject.CreateInstance<RayCastVisualizer>();
            //capsuleCastVisualizer = ScriptableObject.CreateInstance<CapsuleCastVisualizer>();
        }

        public override void Update()
        {
            base.Update();
            
            if (agentTransform == null) { return; }
            
            if (checkHasLineOfSight)
            {
                checkHasLineOfSight = false;

                Debug.Log(
                    staticData.HasLineOfSight(
                        lookTargetPosition,
                        rayCastVisualizer,
                        true));
            }

            if (checkIsAtPosition)
            {
                checkIsAtPosition = false;

                Debug.Log(
                    staticData.IsAtPosition(
                        staticData.Position + VectorXYZ.forward, 
                        closeEnoughDistance));
            }
        }
    }
}
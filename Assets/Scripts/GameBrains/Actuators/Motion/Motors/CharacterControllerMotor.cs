using GameBrains.Entities.EntityData;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Motors
{
    public sealed class CharacterControllerMotor : Motor
    {
        public override void Start()
        {
            base.Start();
            SetupCharacterController();
        }

        public override void CalculatePhysics(KinematicData kinematicData, float deltaTime)
        {
            kinematicData.DoUpdate(deltaTime, false);
            Agent.CharacterController.SimpleMove((Vector3)kinematicData.Velocity);
        }
    }
}
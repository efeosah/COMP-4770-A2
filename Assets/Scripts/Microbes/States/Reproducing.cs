using GameBrains.FiniteStateMachine;
using GameBrains.EventSystem;
using Microbes.Entities;
using UnityEngine;
using System.Collections.Generic;
using GameBrains.Entities;

namespace Microbes.States
{
    // Reproducing state. Executing in the mating state results in "contributions" from parents.
    // This state should spawn zero, one, or more new microbes with characteristics determined by
    // the parents. Be creative.
    // TODO for A2: Fill in the details.
    [CreateAssetMenu(menuName = "Microbes/States/Reproducing")]
    public class Reproducing : State
    {
        // public override void OnEnable()
        // {
        //     base.OnEnable();
        // }

        // This will execute when the state is entered.
        public override void Enter(StateMachine stateMachine)
        {
            base.Enter(stateMachine);

            var microbe = stateMachine.Owner as Microbe;

            if (microbe == null) { return; }

            microbe.IsReproduce = true;

            EventManager.Instance.Fire(Events.Message, $"{microbe.name}: Time to reproduce...");

        }

        // This is the state's normal update function.
        public override void Execute(StateMachine stateMachine)
        {
            base.Execute(stateMachine);

            var microbe = stateMachine.Owner as Microbe;

            if (microbe == null) { return; }

            var radius = microbe.transform.localScale.x / 2.0f;

            var nearbyMicrobes = new List<Microbe>();

            // Find all microbes in a certain radius that match any of the types of possible mates.
            foreach (Microbe existingMicrobe in EntityManager.FindAll<Microbe>())
            {
                if (microbe != existingMicrobe && (existingMicrobe.microbeType & microbe.DatingTypes) != 0 && existingMicrobe.IsReproduce)
                {
                    if (Physics.Raycast(microbe.transform.position, existingMicrobe.transform.position - microbe.transform.position, out RaycastHit hit, radius))
                    {
                        if (hit.transform == existingMicrobe.transform)
                        {
                            nearbyMicrobes.Add(existingMicrobe);
                        }
                    }
                }
            }

            if (nearbyMicrobes.Count > 0)
            {
                foreach (Microbe nearbyMicrobe in nearbyMicrobes)
                {
                    //microbe.AttemptReproduction(nearbyMicrobe);

                    float rand = Random.value;

                    if (rand < 0.4) return;

                    //attempt reproduction here
                    MicrobeTypes childMicrobe = microbe.GetChildType(nearbyMicrobe.microbeType);

                    Vector2 spawn = microbe.spawner.GetEmptySpawnPoint();

                    //There is an available spawn point
                    if(spawn != Vector2.negativeInfinity)
                    {
                        //Microbe.Spawn(childMicrobe, spawn);
                        microbe.spawner.SpawnChild(childMicrobe, spawn);

                    }
                }
            }
        }


        // This will execute when the state is exited.
        public override void Exit(StateMachine stateMachine)
        {
            base.Exit(stateMachine);

            var microbe = stateMachine.Owner as Microbe;

            if (microbe == null) { return; }

            microbe.IsReproduce = false;

            EventManager.Instance.Fire(Events.Message, $"{microbe.name}: Finished reproducing....going to bed ^^");
            var sleepState = StateManager.Lookup(typeof(Sleeping));
            if (sleepState == null) { Debug.Log("Missing State"); }
            stateMachine.ChangeState(sleepState);
        }

        // This executes if the microbe receives a message from the message dispatcher.
        // public override bool HandleEvent<TEvent>(
        //     StateMachine stateMachine,
        //     Event<TEvent> eventArguments)
        // {
        //     // if (eventArguments.EventType == Events.MyEvent)
        //     // {
        //     //     var microbe = stateMachine.Owner as Microbe;
        //     //
        //     //     if (microbe != null && eventArguments.ReceiverId == microbe.ID)
        //     //     {
        //     //         if (VerbosityDebug)
        //     //         {
        //     //             Debug.Log($"Event {eventArguments.EventType} received by {microbe.name} at time: {Time.time}");
        //     //         }
        //     //         
        //     //         // TODO: Do stuff
        //     //
        //     //         return true;
        //     //     }
        //     // }
        //
        //     return base.HandleEvent(stateMachine, eventArguments);
        // }
    }
}
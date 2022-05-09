using System.Collections.Generic;
using GameBrains.Entities;
using GameBrains.EventSystem;
using GameBrains.FiniteStateMachine;
using Microbes.Entities;
using UnityEngine;

namespace Microbes.States
{
    // Seeking food state. This is just one possibility to get you started. Feel free to change it
    // to suit your design.
    // TODO for A2: Modify as desired.
    
    [CreateAssetMenu(menuName = "Microbes/States/SeekingFood")]
    public class SeekingFood : State
    {
        // This will execute when the state is entered.
        public override void Enter(StateMachine stateMachine)
        {
            base.Enter(stateMachine);
            
            var microbe = stateMachine.Owner as Microbe;

            if (microbe == null) { return; }
            
            EventManager.Instance.Fire(Events.Message, $"{microbe.name}: I've got the munchies. Any {microbe.FoodTypes} out there?");
        }
        
        // This is the state's normal update function.
        public override void Execute(StateMachine stateMachine)
        {
            base.Execute(stateMachine);

            var microbe = stateMachine.Owner as Microbe;
            
            if (microbe == null) { return; }
            
            // adjust for desired eating radius (assume sphere so scale x = scale y = scale z).
            var radius = microbe.transform.localScale.x / 2.0f;

            var nearbyMicrobes = new List<Microbe>();

            // Find all microbes in a certain radius that match any of the food types we eat.
            foreach (Microbe existingMicrobe in EntityManager.FindAll<Microbe>())
            {
                if (microbe != existingMicrobe && (existingMicrobe.microbeType & microbe.FoodTypes) != 0)
                {
                    if (Physics.Raycast(
                        microbe.transform.position + Vector3.up * radius,
                        existingMicrobe.transform.position - microbe.transform.position, 
                        out var hit, 
                        radius, 
                        microbe.raycastMask))
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
                    microbe.Hunger -= 500; // TODO: maybe use entity food value
                    if (microbe.Hunger < 0)
                    {
                        microbe.Hunger = 0;
                    }
                    
                    EventManager.Instance.Fire(
                        Events.YouJustGotSwallowed, 
                        microbe.ID, 
                        nearbyMicrobe.ID, 
                        string.Empty);
                    
                    EventManager.Instance.Fire(Events.Message, $"{microbe.name}: Yummy! I ate {nearbyMicrobe.name}");
                    //each time a microbe eats increase desire to mate
                    //microbe.Horny++;

                    if (!microbe.IsHungry)
                    {
                        var sleepingState = StateManager.Lookup(typeof(Sleeping));
                        if (sleepingState == null) { Debug.Log("Missing State"); }
                        stateMachine.ChangeState(sleepingState);
                        //if microbe is not hungry and microbe has exceeded horny threshold
                        //if (microbe.IsHorny)
                        //{
                        //    var matingState = StateManager.Lookup(typeof(Mating));
                        //    if (matingState == null) { Debug.Log("Missing State"); }
                        //    stateMachine.ChangeState(matingState);
                        //}
                        ////else sleep
                        //else
                        //{
                            
                        //}

                        return; // make sure we finish this as we change states
                    }
                }
            }

            // adjust attraction radius based one hunger
            if (microbe.IsHungry)
            {
                microbe.Attractor.Strength = 20000; // could also adjust strength
                microbe.Attractor.radius = 20 * microbe.Hunger; // as hunger increases so does radius
                microbe.Attractor.AttractTypes = microbe.FoodTypes; // could change food type
            }
        }
        
        // This will execute when the state is exited.
        // public override void Exit(StateMachine stateMachine)
        // {
        //     base.Exit(stateMachine);
        //     
        //     // var microbe = stateMachine.Owner as Microbe;
        //     //
        //     // if (microbe == null) { return; }
        // }
        
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
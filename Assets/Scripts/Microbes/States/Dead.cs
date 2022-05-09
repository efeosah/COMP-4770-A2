using GameBrains.FiniteStateMachine;
using Microbes.Entities;
using UnityEngine;

namespace Microbes.States
{
    // TODO for A2: Is this state needed?
    // Dead state. Stop doing stuff.
    [CreateAssetMenu(menuName = "Microbes/States/Dead")]
    public class Dead : State
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

            // TODO for A2: I don't think this stops repelling microbes already being repelled.
            // stop repelling.
            microbe.Repeller.Strength = 0;
            microbe.Repeller.radius = 0;

            // TODO for A2: I don't think this stops attracting microbes already being attracted.
            // stop attracting.
            microbe.Attractor.Strength = 0;
            microbe.Attractor.radius = 0;
        }
        
        // This is the state's normal update function.
        // public override void Execute(StateMachine stateMachine)
        // {
        //     base.Execute(stateMachine);
        //     
        //     // var microbe = stateMachine.Owner as Microbe;
        //     //
        //     // if (microbe == null) { return; }
        // }
        
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
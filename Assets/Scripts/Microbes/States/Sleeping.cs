using GameBrains.EventSystem;
using GameBrains.FiniteStateMachine;
using Microbes.Entities;
using UnityEngine;

namespace Microbes.States
{
    // Sleeping state.
    // TODO for A2: Modify to suit your needs.
    [CreateAssetMenu(menuName = "Microbes/States/Sleeping")]
    public class Sleeping : State
    {
        // Note states are share between entities. Do not cache entity specify data in states. Store it in the entity
        // that is the owner of the stateMachine.

        // This will execute when the state is entered.

        float TimeSinceLastMate = 20.0f;
        float curTime = 0;
        public override void Enter(StateMachine stateMachine)
        {
            base.Enter(stateMachine);
            
            var microbe = stateMachine.Owner as Microbe;

            if (microbe == null) { return; }
            
            EventManager.Instance.Fire(Events.Message, $"{microbe.name}: ZZZZ...");

            // stop repelling.
            //create reppeling radius for sleeping microbes
            //microbe.Repeller.Strength = 10;
            //microbe.Repeller.radius = 10;

            // TODO for A2: I don't think this stops attracting microbes already being attracted.
            // TODO for A2: This may result in microbes being attracted to a sleeping microbe
            // TODO for A2: which won't eat them until it gets hungry again. Bug or Feature?
            // stop attracting.
            microbe.Attractor.Strength = 0;
            microbe.Attractor.radius = 0;
        }
        
        // This is the state's normal update function.
        public override void Execute(StateMachine stateMachine)
        {
            base.Execute(stateMachine);
            
            var microbe = stateMachine.Owner as Microbe;

            if (microbe == null) { return; }


            if (microbe.IsHungry)
            {
                var seekingFoodState = StateManager.Lookup(typeof(SeekingFood));
                if (seekingFoodState == null) { Debug.Log("Missisng State"); }
                stateMachine.ChangeState(seekingFoodState);
                return;

            }

            curTime += Time.deltaTime;

            //when microbe is of "legal" age to mate
            //add conditions to make it less frequent hmm...
            if (microbe.LifeSpan.Age > 18 && Random.value < 0.2 && curTime >= TimeSinceLastMate)
            {
                var matingState = StateManager.Lookup(typeof(Mating));
                if (matingState == null) { Debug.Log("Missing State"); }
                stateMachine.ChangeState(matingState);
                curTime = 0;
                TimeSinceLastMate++;
                return;
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
        public override bool HandleEvent<TEvent>(
            StateMachine stateMachine,
            Event<TEvent> eventArguments)
        {
            //if (eventArguments.EventType == Events.LetsMakeABaby)
            //{
            //    var microbe = stateMachine.Owner as Microbe;

            //    if (microbe != null && eventArguments.ReceiverId == microbe.ID)
            //    {
            //        if (VerbosityDebug)
            //        {
            //            Debug.Log($"Event {eventArguments.EventType} received by {microbe.name} at time: {Time.time}");
            //        }

            //        // TODO: Do stuff
            //        //go into reproduction state
            //        var repoState = StateManager.Lookup(typeof(Reproducing));
            //        if (repoState == null) { Debug.Log("Missing State"); }
            //        stateMachine.ChangeState(repoState);
            //        return true;
            //    }
            //}

            return base.HandleEvent(stateMachine, eventArguments);
        }
    }
}
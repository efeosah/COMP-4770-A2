using GameBrains.EventSystem;
using GameBrains.FiniteStateMachine;
using Microbes.Entities;
using UnityEngine;

namespace Microbes.States
{
    // Microbe global state class.
    // TODO for A2: fill in any additional global state details needed.
    [CreateAssetMenu(menuName = "Microbes/States/MicrobeGlobalState")]
    public class MicrobeGlobalState : State
    {
        // This will execute when the state is entered.
        // public override void Enter(StateMachine stateMachine)
        // {
        //     base.Enter(stateMachine);
        //     
        //     // var microbe = stateMachine.Owner as Microbe;
        //     //
        //     // if (microbe == null) { return; }
        // }

        // This is the state's normal update function.
        public override void Execute(StateMachine stateMachine)
        {
            base.Execute(stateMachine);
            
            var microbe = stateMachine.Owner as Microbe;
            
            if (microbe == null) { return; }

            if (Random.value < 0.05f)
            {
                microbe.Hunger += 1; // TODO for A2 (optional): Add visual indicator (maybe shrink when hungry)
            }

            //if(Random.value >= 0.05f && !microbe.IsHungry)
            //{
            //    microbe.Horny += 1;
            //}

            //conditions for entering mating state
            //1. Not hungry
            //2. Microbe has eating 5 times
            //if(microbe.Horny > microbe.hornyThreshold && stateMachine.CurrentState == StateManager.Lookup(typeof(Sleeping)))
            //{
            //    var matingState = StateManager.Lookup(typeof(Mating));
            //    if (matingState == null) { Debug.Log("Missing State"); }
            //    stateMachine.ChangeState(matingState);
            //}
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
            if (eventArguments.EventType == Events.YouJustGotSwallowed)
            {
                var microbe = stateMachine.Owner as Microbe;
            
                if (microbe != null && eventArguments.ReceiverId == microbe.ID)
                {
                    if (VerbosityDebug)
                    {
                        Debug.Log($"Event {eventArguments.EventType} received by {microbe.name} at time: {Time.time}");
                    }
            
                    microbe.Die();
                    
                    // TODO for A2: Is the dead state needed?
                    var deadState = StateManager.Lookup(typeof(Dead));
                    if (deadState == null) { Debug.Log("Missing State"); }
                    stateMachine.ChangeState(deadState);
                    
                    return true;
                }
            }

            else if (eventArguments.EventType == Events.LetsMakeABaby)
            {
                var microbe = stateMachine.Owner as Microbe;

                if (microbe == null) { return base.HandleEvent(stateMachine, eventArguments); ; }

                if (eventArguments.ReceiverId == microbe.ID)
                {
                    if (VerbosityDebug)
                    {
                        Debug.Log($"Event {eventArguments.EventType} received by {microbe.name} at time: {Time.time}");
                    }

                    // TODO: Do stuff
                    ////change state to reproducing state
                    //var reproducingState = StateManager.Lookup(typeof(Reproducing));
                    //if (reproducingState == null) { Debug.Log("Missing State"); }
                    //stateMachine.ChangeState(reproducingState);
                    ////isReproduce = true;

                    var repoState = StateManager.Lookup(typeof(Reproducing));
                    if (repoState == null) { Debug.Log("Missing State"); }
                    stateMachine.ChangeState(repoState);
                    var rand = Random.value;
                    //if (rand < 0.7)
                    //{
                    //    var repoState = StateManager.Lookup(typeof(Reproducing));
                    //    if (repoState == null) { Debug.Log("Missing State"); }
                    //    stateMachine.ChangeState(repoState);

                    //}
                    ////send a "YoureNotMyType" event
                    //else if (rand >= 0.7)
                    //{
                    //    EventManager.Instance.Fire(
                    //    Events.YouAreNotMyType,
                    //    microbe.ID,
                    //    eventArguments.SenderId,
                    //    string.Empty);
                    //}


                    return true;
                }
            }

            //else if (eventArguments.EventType == Events.YouAreNotMyType)
            //{
            //    var microbe = stateMachine.Owner as Microbe;

            //    if (microbe == null) { return base.HandleEvent(stateMachine, eventArguments); ; }

            //    if (eventArguments.ReceiverId == microbe.ID)
            //    {
            //        if (VerbosityDebug)
            //        {
            //            Debug.Log($"Event {eventArguments.EventType} received by {microbe.name} at time: {Time.time}");
            //        }

            //        // TODO: Do stuff
            //        //change state to sleep or stay in mate
            //        int temp = Random.Range(0, 10);
            //        if (temp <= 5)
            //        {
            //            var sleepingState = StateManager.Lookup(typeof(Sleeping));
            //            if (sleepingState == null) { Debug.Log("Missing State"); }
            //            stateMachine.ChangeState(sleepingState);
            //        }
            //        else if (temp > 5)
            //        {
            //            //stay in mating and look for another mate
            //            return true;
            //        }



            //        return true;
            //    }
            //}


            return base.HandleEvent(stateMachine, eventArguments);
        }
    }
}
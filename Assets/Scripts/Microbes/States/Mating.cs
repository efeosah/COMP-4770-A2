using GameBrains.FiniteStateMachine;
using System.Collections.Generic;
using GameBrains.EventSystem;
using GameBrains.Entities;
using Microbes.Entities;
using UnityEngine;

namespace Microbes.States
{
    // When in this state, the microbe should try attracting a mate of a suitable type. When a
    // potential mate is close, we should send a LetsMakeABaby message. If a LetsMakeABaby message
    // is received, we should change to the Reproducing state. Or if a YouAreNotMyType message is
    // received, either change to the Sleep state (say 50% chance) or seek mate of a different type
    // (say 40%) or keep trying (say 10%). Note: the above is not a strict requirement, just one
    // possibility. You might make your microbe seek partners of several types before reproducing
    // (3 or 4 parents!). You might make the type of microbe that gets spawned depend on the type
    // of its parents. Use your imagination. Then take a cold shower :-)
    // TODO for A2: Fill in the details of this state.
    [CreateAssetMenu(menuName = "Microbes/States/Mating")]
    public class Mating : State
    {

        bool isMating;
        //add time to stay/leave mating state
        private float timeSpentInMating = 50.0f;
        private float curTime;
        // public override void OnEnable()
        // {
        //     base.OnEnable();
        // }

        // This will execute when the state is entered.
        //enter state after microbe eats 5 times
        public override void Enter(StateMachine stateMachine)
        {
            base.Enter(stateMachine);

            var microbe = stateMachine.Owner as Microbe;

            if (microbe == null) { return; }

            microbe.IsHorny = true; 

            EventManager.Instance.Fire(Events.Message, $"{microbe.name}: Hey microbes!! Lets make a baby....");

        }

        // This is the state's normal update function.
        public override void Execute(StateMachine stateMachine)
        {
            base.Execute(stateMachine);

            var microbe = stateMachine.Owner as Microbe;

            if (microbe == null) { return; }

            if (microbe.IsHungry)
            {
                var eatingState = StateManager.Lookup(typeof(SeekingFood));
                if (eatingState == null) { Debug.Log("Missing State"); }
                stateMachine.ChangeState(eatingState);
                return;
            }

            curTime += Time.deltaTime;

            //go to sleep if too much time spent in mating
            if (curTime >= timeSpentInMating) {

                var sleepState = StateManager.Lookup(typeof(Sleeping));
                if (sleepState == null) { Debug.Log("Missing State"); }
                stateMachine.ChangeState(sleepState);
                return;
            }

            var radius = microbe.transform.localScale.x / 2.0f;

            var nearbyMicrobes = new List<Microbe>();

            
            // Find all microbes in a certain radius that match any of the mates we prefer.
            //
            foreach (Microbe existingMicrobe in EntityManager.FindAll<Microbe>())
            {
                if (microbe != existingMicrobe && (existingMicrobe.microbeType & microbe.DatingTypes) != 0)
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

            //what to do with microbes found?
            //
            if(nearbyMicrobes.Count > 0)
            {
                //dont send to all, send to closest microbe
                foreach(Microbe nearbyMicrobe in nearbyMicrobes)
                {

                    EventManager.Instance.Fire(
                        Events.LetsMakeABaby,
                        microbe.ID,
                        nearbyMicrobe.ID,
                        string.Empty);


                }
                //microbe.Horny = 0;
                //isReproduce = true;
            }

            microbe.Attractor.Strength = 10000; // could also adjust strength
            microbe.Attractor.radius = 500 * microbe.LifeSpan.Age; // as age increases so does radius
            microbe.Attractor.AttractTypes = microbe.DatingTypes; // could change food type

        }

        // This will execute when the state is exited.
        public override void Exit(StateMachine stateMachine)
        {
            base.Exit(stateMachine);

            var microbe = stateMachine.Owner as Microbe;

            if (microbe == null) { return; }

            curTime = 0;

            microbe.IsHorny = false;


            
        }

        //This executes if the microbe receives a message from the message dispatcher.
        //if microbe is in the mating state and a lets make a baby message is recieved
        //or a you are not my type message 
        public override bool HandleEvent<TEvent>(
            StateMachine stateMachine,
            Event<TEvent> eventArguments)
        {
            //if a "Letsmakeababy" message is recieved 
            if (eventArguments.EventType == Events.LetsMakeABaby)
            {
                var microbe = stateMachine.Owner as Microbe;

                if (microbe == null) { return base.HandleEvent(stateMachine, eventArguments); ; }

                Microbe senderMicrobe = EntityManager.Find<Microbe>(eventArguments.SenderId);

                if (eventArguments.ReceiverId == microbe.ID)
                {
                    if (VerbosityDebug)
                    {
                        Debug.Log($"Event {eventArguments.EventType} received by {microbe.name} at time: {Time.time}");
                    }

                    // TODO: Do stuff

                    float rand = Random.value;
                    //randomly assign next action
                    //if microbe recieves "LetsMakeABaby" event
                    //enter reproduction state
                    if(rand < 0.7)
                    {
                        var repoState = StateManager.Lookup(typeof(Reproducing));
                        if (repoState == null) { Debug.Log("Missing State"); }
                        stateMachine.ChangeState(repoState);

                    }
                    //send a "YoureNotMyType" event
                    else //if(rand >=0.7 && rand < 1)
                    {
                        EventManager.Instance.Fire(
                        Events.YouAreNotMyType,
                        microbe.ID,
                        eventArguments.SenderId,
                        string.Empty);
                    }
                    

                    return true;
                }
            }

            //else if a "Youarenotmytype" message is recieved
            else if (eventArguments.EventType == Events.YouAreNotMyType)
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
                    //go to sleep
                    var sleepState = StateManager.Lookup(typeof(Sleeping));
                    if (sleepState == null) { Debug.Log("Missing State"); }
                    stateMachine.ChangeState(sleepState);


                    return true;
                }
            }

            return base.HandleEvent(stateMachine, eventArguments);
        }

        
    }
}
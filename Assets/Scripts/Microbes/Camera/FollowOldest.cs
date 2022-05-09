using GameBrains.Cameras;
using GameBrains.Entities;
using Microbes.Entities;

namespace Microbes.Camera
{
    // Follow oldest microbe
    // TODO for A2 (optional): Convert to follow oldest agent??
    public class FollowOldest : OverheadFollow
    {
        // TODO for A2 (optional): Be able to follow the oldest microbe (or a click selected one).
        // TODO for A2 (optional): Add zoom feature.
        public override void LateUpdate()
        {
            Microbe oldestMicrobe = null;
            foreach (Microbe microbe in EntityManager.FindAll<Microbe>())
            {
                if (oldestMicrobe == null)
                {
                    oldestMicrobe = microbe;
                }
                else if (microbe.ID < oldestMicrobe.ID)
                {
                    oldestMicrobe = microbe;
                }
            }
            
            if (oldestMicrobe != null)
            {
                Target = oldestMicrobe.transform;
            }
            else
            {
                Target = null; // base should assign a default target if no microbes present.
            }

            base.LateUpdate();
        }
    }
}
using UnityEngine;

namespace Microbes.Movement
{
    // Adds a push force to microbes in its effect radius.
    // Note: implemented as a negative attractor.
    public class Repeller : Attractor
    {
        // Gets or sets the strength of repulsion.
        public new float Strength
        {
            get { Debug.Log(base.Strength); return -1 * base.Strength; }
            set => base.Strength = -1 * value;
        }
    }
}
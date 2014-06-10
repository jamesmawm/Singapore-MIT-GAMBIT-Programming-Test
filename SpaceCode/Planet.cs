using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceCode
{
    // A planet is a boring sprite that just floats around and rotates by default.
    class Planet : Sprite
    {
        public Planet()
        {
            // Rotation speed... 
            this.RotationalVelocityInDegrees = 30;

            // Default image
            ImageFileName = "Images/Planet.gif";

            // Default "orbit"
            Velocity.X = 25;
        }
    }
}

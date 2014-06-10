using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;


using System.Windows.Forms;

namespace SpaceCode
{
    // Missiles fly through space and hit stuff.
    //  If they don't hit anything for a while, they vanish.
    class Missile : Sprite
    {
        // When should the missile vanish?
        DateTime _TimeOut = DateTime.Now + new TimeSpan(0, 0, 5);

        // Identify missiles fired from which ship.
        // firerID=0 -> My Ship.
        // firerID=1,2,3.. -> AI Ship.
        public int firerID = 0;

        public Missile(Ship firer)
        {
            firerID = 0;
            // Create a missile.
            RotationInDegrees = firer.RotationInDegrees;
            RotationalVelocityInDegrees = 1080;  // spin 3 times per second!
            ImageFileName = "Images/Missile.gif";  // default missile graphics

            float delta_v = 100;  // speed relative to firing ship

            Position = firer.Position;

            // Angle of zero has the sprite facing the top of the screen.
            Velocity = firer.Velocity + new Vector2F(delta_v * Math.Sin(RotationInRadians),
                                                     - delta_v * Math.Cos(RotationInRadians));
        }

        // For AI ship.
        public Missile(AIShip firer)
        {
            firerID = 1;

            // Create a missile.
            RotationInDegrees = firer.RotationInDegrees;
            RotationalVelocityInDegrees = 1080;  // spin 3 times per second!
            ImageFileName = "Images/Missile2.gif";  // default missile graphics

            float delta_v = 100;  // speed relative to firing ship

            Position = firer.Position;

            // Angle of zero has the sprite facing the top of the screen.
            Velocity = firer.Velocity + new Vector2F(delta_v * Math.Sin(RotationInRadians),
                                                     -delta_v * Math.Cos(RotationInRadians));
        }

        // Every so often, this gets called.
        public override void OnTick(TimeSpan delta)
        {
            base.OnTick(delta);

            // If is time to vanish?
            if (DateTime.Now > _TimeOut)
            {
                // Yes!  Remove us from space.
                Space = null;  
            }
        }

        // Gets called when we hit something
        internal override void Hit(Sprite s2)
        {
            // Do I already have a parent?
            if (null != Parent)
                // Yes!  I have a parent.  So I'm already stuck to something.
                return;

            // Am I hitting a ship?
            if (s2 is Ship)
                // Yes!  Ignore that, since I'll never really fire otherwise.
                return;

            // Otherwise, we hit something that we should stick to.
            // Never time out.
            _TimeOut = DateTime.MaxValue;

            // And stick to whatever we hit.
            Parent = s2;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SpaceCode
{
    // The ship is a user-controlled sprite that can shoot missiles.
    public class Ship : Sprite
    {
        public float Acceleration = 100.0f;  // Acceleration, is pixels/s/s
        public float RotationalAcceleration = 100.0f;  // Rotational acceleration, in degrees/s/s
        public TimeSpan MinTimeBetweenShots = new TimeSpan(0, 0, 0, 0, 300);  // How long must we wait to reload?
        protected DateTime LastShot = DateTime.Now;  // When was our last missile fired?

        public Ship()
        {
            ImageFileName = "Images/Ship.gif";  // default ship graphic
        }

        // Gets called often
        public override void OnTick(TimeSpan delta)
        {
            // Base class handles motion
            base.OnTick(delta);

            // Is the game over?
            if (Space.GameEndTime < DateTime.Now)
                // Yes!  Don't process user inputs any more.
                return;

            // Listen for keyboard input.

            // Are we applying thrust?
            if (IsKeyPressed(Keys.Up))
            {
                // Yes!  Increase the velocity.
                float delta_v = Acceleration * (float)delta.TotalSeconds;
                Velocity.X += delta_v * (float)Math.Sin(RotationInRadians);
                Velocity.Y -= delta_v * (float)Math.Cos(RotationInRadians);
            }

            // Are we applying left rotation?
            if (IsKeyPressed(Keys.Left))
            {
                // Yes!  Fire the counterclockwise thruster
                RotationalVelocityInDegrees -= RotationalAcceleration * (float)delta.TotalSeconds;
            }

            // Are we applying right rotation?
            if (IsKeyPressed(Keys.Right))
            {
                // Yes!  Fire the clockwise thruster
                RotationalVelocityInDegrees += RotationalAcceleration * (float)delta.TotalSeconds;
            }

            // Are we trying to fire a missile?
            if (IsKeyPressed(Keys.Space))
            {
                // Yes!  
                DateTime now = DateTime.Now;  // What time is it?

                // Have we reloaded?
                if (now - LastShot >= MinTimeBetweenShots)
                {
                    // Yes!  Fire.
                    Fire();
                    LastShot = now;   // and restart the reload clock
                }
            }

        }

        public void Fire()
        {
            // Create a missile.
            Missile missile = new Missile(this);
            missile.Space = this.Space;   // must add the missile to our Space, or it won't really get added
        }


        //  http://csharpfeeds.com/post/2313/Get_key_state_from_a_console_application.aspx
        [DllImport("user32.dll")]
        public static extern ushort GetKeyState(short nVirtKey);  // native to win32.  Might not work on a Mac under Mono!

        public const ushort keyDownBit = 0x80;

        // Returns true when the key in question is down.
        public static bool IsKeyPressed(Keys key)
        {
            return ((GetKeyState((short)key) & keyDownBit) == keyDownBit);
        }
    }
}

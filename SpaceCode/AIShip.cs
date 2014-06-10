using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SpaceCode
{
    // The ship is an AI sprite that can shoot missiles.
    // Cloned from Ship.
    public class AIShip : Sprite
    {
        public float Acceleration = 100.0f;  // Acceleration, is pixels/s/s
        public float RotationalAcceleration = 100.0f;  // Rotational acceleration, in degrees/s/s
        public TimeSpan MinTimeBetweenShots = new TimeSpan(0, 0, 0, 0, 300);  // How long must we wait to reload?
        protected DateTime LastShot = DateTime.Now;  // When was our last missile fired?
        
        // Custom AI Variables
        private int counter = 0; // To keep track how often to make a decision.
        public int randomState = 0; // Which state to perform, and press the keys accordingly.
        Random RandomClass = new Random(); // To generate random numbers.
        Boolean isKeyUp = false; // Simulate key presses...
        Boolean isKeyLeft = false;
        Boolean isKeyRight = false;
        Boolean isKeySpace = false;

        public AIShip()
        {
            ImageFileName = "Images/AIShip.gif";  // AI ship graphic
        }

        // Gets called often 
        public override void OnTick(TimeSpan delta)
        {
            counter++;

            // AI will think at every second interval
            if (counter == 60)
            {
                counter = 0;

                // AI is modelled after random variables.
                randomState = RandomClass.Next(100);
                
                // State 1: key pressed up
                // State 2: key pressed left
                // State 3: keypress right
                // State 4: key pressed space
                isKeyUp = false;
                isKeyLeft = false;
                isKeyRight = false;
                isKeySpace = false;

                // Assign possibilities to a random variable.
                // A forward movement has less possibility.
                // Assign more possibilities to turn left and right.
                // Even more possibilities for firing.
                // A possibility variables stored in config file.
                if (randomState < Properties.Settings.Default.doKeyUp ){
                    isKeyUp = true;
                }else if (randomState < Properties.Settings.Default.doKeyLeft){
                    isKeyLeft = true;
                }else if (randomState < Properties.Settings.Default.doKeyRight){
                    isKeyRight = true;
                }else{
                    isKeySpace = true;
                }

            }

            // Base class handles motion
            base.OnTick(delta);

            // Is the game over?
            if (Space.GameEndTime < DateTime.Now)
                // Yes!  Don't process user inputs any more.
                return;

            // Listen for keyboard input.

            // Are we applying thrust?
            if (isKeyUp)
            {
                // Yes!  Increase the velocity.
                float delta_v = Acceleration * (float)delta.TotalSeconds;
                Velocity.X += delta_v * (float)Math.Sin(RotationInRadians);
                Velocity.Y -= delta_v * (float)Math.Cos(RotationInRadians);
            }

            // Are we applying left rotation?
            if (isKeyLeft)
            {
                // Yes!  Fire the counterclockwise thruster
                RotationalVelocityInDegrees -= RotationalAcceleration * (float)delta.TotalSeconds;
            }

            // Are we applying right rotation?
            if (isKeyRight)
            {
                // Yes!  Fire the clockwise thruster
                RotationalVelocityInDegrees += RotationalAcceleration * (float)delta.TotalSeconds;
            }

            // Are we trying to fire a missile?
            if (isKeySpace)
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
            Missile aimissile = new Missile(this);
            aimissile.Space = this.Space;   // must add the missile to our Space, or it won't really get added
        }

    }
}

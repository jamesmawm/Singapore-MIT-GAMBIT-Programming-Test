using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SpaceCode
{
    // A control representing the game world.
    // Lists all sprites, and handles the game clock.
    public partial class SpaceControl : UserControl
    {
        protected List<Sprite> _Sprites = new List<Sprite>();   // list of all sprites
        public DateTime GameEndTime = DateTime.Now + new TimeSpan(0, 1, 0);  // When does the game end?
        int NumTargets = 8;  // How many targets are there?
        DateTime _lastTick = DateTime.Now;  // when did we last tell the sprites that time had passed?
        AIShip aiship;
        public SpaceControl()
        {
            DoubleBuffered = true;  // keeps redraws and screen refresh from happening at the same time

            InitializeComponent();  // standard Windows Forms control init function

            Ship ship = new Ship();  // create the player's ship
            ship.Position = new Vector2F(100, 100);

            aiship = new AIShip();
            aiship.Position = new Vector2F(800, 100);

            Planet planet = new Planet();  // create the big Planet
            planet.Position = new Vector2F(400, 300);

            float orbit = 200;  // how far from the planet's center are the targets?
            List<Planet> targets = new List<Planet>();  
            for (int i = 0; i < NumTargets; ++i)
            {
                // Create a target.  We're using "Planet", but we could use Sprite or a custom Target class.
                Planet target = new Planet();
                target.ImageFileName = "Images/FlatTarget.gif";  // standard target art
                target.Space = this;  // add the target to the simulation

                // Divide the targets uniformly around the planet.
                double angle = i * 2 * Math.PI / NumTargets;
                Vector2F offset = new Vector2F(Math.Cos(angle), Math.Sin(angle));
                target.Position = planet.Position + offset * orbit;
            }


            // Add planet, AIShip and ship to simulation.
            planet.Space = this;
            ship.Space = this;
            aiship.Space = this;
        }

        // Event handler, triggered fairly often by the timer
        private void timerTick_Tick(object sender, EventArgs e)
        {
            float cap_in_seconds = 0.1f;  // even if the computer runs slow, don't let the sim go too far

            // What time is it?
            DateTime now = DateTime.Now;

            // How long has it been since our last update?
            TimeSpan elapsed = now - _lastTick;

            // Is the elapsed time bigger than the cap?
            if (elapsed.TotalSeconds > cap_in_seconds)
                // Yes, it is.  Set elapsed time to the cap.
                elapsed = new TimeSpan(0, 0, 0, 0, (int)(1000 * cap_in_seconds));

            // copy the sprite list, just in case a sprite tries to change the list during the loop
            List<Sprite> list_copy = new List<Sprite>(_Sprites);

            // Look at all Sprites.  Give them a tick.
            foreach (Sprite sprite in list_copy)
            {
              sprite.OnTick(elapsed);
            }
            

            CheckForSpriteCollisions();
            Score();

            // Is the game over?
            TimeSpan remaining = (GameEndTime - DateTime.Now);
            if (remaining.TotalMilliseconds < 0)
                // Yes!  Say so.
                labelTime.Text = "GAME OVER";  
            else
                // No!  Say so.
                labelTime.Text = remaining.ToString();

            // Remember when we did this update.
            _lastTick = now;

            // Redraw the screen.
            Invalidate();
        }

        private void CheckForSpriteCollisions()
        {
            // copy the sprite list, just in case a sprite tries to change the list during the loop
            List<Sprite> l1 = new List<Sprite>(_Sprites);
            List<Sprite> l2 = new List<Sprite>(_Sprites);

            foreach (Sprite s1 in l1)
            {
                // If I have a parent, then I can't hit anything new.  Don't even ask the questions.
                if (null != s1.Parent)
                    continue;

                foreach (Sprite s2 in l2)
                {
                    // Am I overlapping with something?
                    if (s1.IsCollidingWith(s2))
                    {
                        // Yes!  Smack it.
                        s1.Hit(s2);
                    }
                }
            }
        }

        // Called when the OS tries to draw the control
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            foreach (Sprite sprite in _Sprites)
            {
                sprite.Paint(e.Graphics);
            }
        }

        public void AddSprite(Sprite sprite)
        {
            // Don't add null sprites or sprites that are already present.
            if (null == sprite || _Sprites.Contains(sprite))
                return;

            _Sprites.Add(sprite);
        }

        public void RemoveSprite(Sprite sprite)
        {
            _Sprites.Remove(sprite);
        }

        // Add up the score!
        protected void Score()
        {
            Dictionary<Planet, int> d_PlanetToNumMissiles = new Dictionary<Planet, int>();

            long score = 0;
            long aiscore = 0;
            foreach (Sprite sprite in this._Sprites)
            {
                Missile missile = sprite as Missile;
                if (null != missile)
                {
                    if (missile.Parent is Missile)
                    {
                        // Assign scores accordingly to fires by which ship.
                        if (missile.firerID == 0) score += 1;
                        else aiscore += 1;
                    }
                    else if (missile.Parent is Planet && missile.Parent.ImageFileName == "Images/FlatTarget.gif")
                    {
                        if (missile.firerID == 0) score += 50;
                        else aiscore += 1;

                        Planet target = (Planet)missile.Parent;
                        if (d_PlanetToNumMissiles.ContainsKey(target))
                            d_PlanetToNumMissiles[target]++;
                        else
                            d_PlanetToNumMissiles[target] = 1;
                    }
                    else if (missile.Parent is Planet)
                        if (missile.firerID == 0) score -= 100;
                        else aiscore -= 100;
                }
            }

            
            if (d_PlanetToNumMissiles.Count >= NumTargets)
            {
                int min = int.MaxValue;
                foreach( int missiles in d_PlanetToNumMissiles.Values)
                {
                    if (missiles < min)
                        min = missiles;
                }

                score += min * 1000;
            }

            labelScore.Text = score.ToString();
            labelAI.Text = aiscore.ToString();
        }

    }
}

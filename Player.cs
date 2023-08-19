using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FangamePhysicsSimulator
{
    internal class Player
    {
        public const double GRAVITY = 0.4, RELEASE_MULTIPLIER = 0.45, SINGLEJUMP = 8.5, DOUBLEJUMP = 7, MAXVSPEED = 9;
        public double Y = 407.4, VSpeed = 0;

        public Player(double Y, double VSpeed)
        {
            this.Y = Y;
            this.VSpeed = VSpeed;
        }

        // copy constructor
        public Player(Player p)
        {
            Y = p.Y;
            VSpeed = p.VSpeed;
        }

        public void Advance()
        {
            if (VSpeed > MAXVSPEED)
                VSpeed = MAXVSPEED;

            VSpeed += GRAVITY;
            Y += VSpeed;

            // ...
        }
    }
}

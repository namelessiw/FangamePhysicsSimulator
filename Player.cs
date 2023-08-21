using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FangamePhysicsSimulator
{
    internal class Player
    {
        [Flags]
        public enum Input
        {
            Press = 1,
            Release = 2,
        }

        // physics constants
        public static double GRAVITY = 0.4, RELEASE_MULTIPLIER = 0.45, SINGLEJUMP = 8.5, DOUBLEJUMP = 7, MAXVSPEED = 9;

        static double _Floor;
        public static double Floor
        {
            get { return _Floor; }
            set { _Floor = value - 8; }
        }

        static double _Ceiling;
        public static double Ceiling
        {
            get { return _Ceiling; }
            set { _Ceiling = value - 12; }
        }

        public double Y, VSpeed;

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

        public void Advance(bool Press, bool Release)
        {
            UpdateVSpeed(Press, Release);

            // collision

            UpdatePosition();

            // ...
        }

        void UpdateVSpeed(bool Press, bool Release)
        {
            CheckPress(Press);

            CheckRelease(Release);

            CapVSpeed();

            VSpeed += GRAVITY;
        }

        void CheckPress(bool Press)
        {
            if (Press)
            {
                VSpeed = SINGLEJUMP;
            }
        }

        void CheckRelease(bool Release)
        {
            if (Release)
            {
                // for the future
                if (VSpeed < 0)
                {
                    throw new Exception("negative vspeed release");
                }
                VSpeed *= RELEASE_MULTIPLIER;
            }
        }

        void CapVSpeed()
        {
            if (VSpeed > MAXVSPEED)
                VSpeed = MAXVSPEED;
        }

        void UpdatePosition()
        {
            Y += VSpeed;
        }
    }
}

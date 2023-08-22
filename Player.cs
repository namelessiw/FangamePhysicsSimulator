using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

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
            set { _Floor = value - 8; }
        }

        static double _Ceiling;
        public static double Ceiling
        {
            set { _Ceiling = value - 12; }
        }

        public bool Released;
        public double Y, VSpeed;
        public int Frame;
        List<Input> Inputs;

        public Player(double Y, double VSpeed)
        {
            Released = false;
            this.Y = Y;
            this.VSpeed = VSpeed;
            Frame = 0;
            Inputs = new();
        }

        // copy constructor
        public Player(Player p)
        {
            Released = p.Released;
            Y = p.Y;
            VSpeed = p.VSpeed;
            Frame = p.Frame;
            Inputs = new(p.Inputs);
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
            CheckInputs(Press, Release);

            CapVSpeed();

            VSpeed += GRAVITY;
        }

        void CheckInputs(bool Press, bool Release)
        {
            Input input = 0;

            // force release if there was none when reaching positive vspeed
            if (!Released && VSpeed > 0)
            {
                Release = true;
                Released = true;
            }

            if (Press)
            {
                VSpeed = -SINGLEJUMP;
                input = Input.Press;
                Released = false;
            }
            if (Release)
            {
                VSpeed *= RELEASE_MULTIPLIER;
                input |= Input.Release;
                Released = true;
            }

            Inputs.Add(input);
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

        [Pure]
        string GetStrat()
        {
            StringBuilder sb = new();
            int Frames = 0;
            bool Held = false;

            for (int i = 0; i < Inputs.Count; i++)
            {
                Input input = Inputs[i];
                bool Press = (input & Input.Press) == Input.Press, 
                    Release = (input & Input.Release) == Input.Release;

                if (Press)
                {
                    if (Frames > 0)
                    {
                        if (Held)
                        {
                            sb.Append($"{Frames}f 0p ");
                        }
                        else
                        {
                            sb.Append($"{Frames}p ");
                        }
                    }

                    Held = true;
                    Frames = 0;
                }
                if (Release)
                {
                    if (Held)
                    {
                        sb.Append($"{Frames}f ");
                    }
                    else
                    {
                        sb.Append($"+{Frames} ");
                    }

                    Held = false;
                    Frames = 0;
                }

                Frames++;
            }

            Frames--;

            if (Held)
            {
                sb.Append($"{Frames}f ");
            }
            else if (Frames > 0)
            {
                sb.Append($" {Frames}p");
            }

            return sb.ToString().Trim();
        }
    }
}

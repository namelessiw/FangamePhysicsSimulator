using System;
using System.Linq;
using System.Text;

namespace FangamePhysicsSimulator
{
    internal class GM8Player
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

        // make singlejump specific to frame 1 (of search)?
        public bool Released, SingleJump, DoubleJump;
        public double Y, VSpeed;
        public int Frame;
        readonly List<Input> Inputs;

        public GM8Player(double Y, double VSpeed)
        {
            Released = false;
            this.Y = Y;
            this.VSpeed = VSpeed;
            Frame = 0;
            Inputs = new();
            SingleJump = true;
            DoubleJump = true;
        }

        // copy constructor
        public GM8Player(GM8Player p)
        {
            Released = p.Released;
            Y = p.Y;
            VSpeed = p.VSpeed;
            Frame = p.Frame;
            Inputs = new(p.Inputs);
            SingleJump = p.SingleJump;
            DoubleJump = p.DoubleJump;
        }

        public void Advance(bool Press, bool Release)
        {
            UpdateVSpeed(Press, Release);

            Collision_2();

            // update position
            Y += VSpeed;

            Frame++;
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
            // intention is to force release on fulljump once done
            // !! might require && !Press
            if (!Released && VSpeed > 0)
            {
                if (Release && !Press)
                {
                    // temporary
                    throw new Exception("cannot release on positive vspeed");
                }
                input |= Input.Release;
                Released = true;
            }

            if (Press)
            {
                if (SingleJump)
                {
                    VSpeed = -SINGLEJUMP;
                    SingleJump = false;
                }
                else if (DoubleJump)
                {
                    VSpeed = -DOUBLEJUMP;
                    DoubleJump = false;
                }
                else
                {
                    // temporary
                    throw new Exception("no multiple doublejumps");
                }
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

        // collision checks against floor and ceiling during movement
        void Collision()
        {
            double RoundedPosition = Math.Round(Y);
            if (VSpeed > 0)
            {
                // above ground, moving downwards
                if (RoundedPosition < _Floor)
                {
                    // next intended position in or below ground
                    if (Math.Round(Y + VSpeed) >= _Floor)
                    {
                        double NextPosition = Y + 1;
                        // rerounding for vfpi behaviour
                        while (Math.Round(NextPosition) != _Floor)
                        {
                            Y++;
                            NextPosition++;
                        }

                        VSpeed = 0;
                    }
                }
            }
            else
            {
                // underneath ceiling, moving upwards
                if (RoundedPosition > _Ceiling)
                {
                    // next intended position in or above ceiling
                    if (Math.Round(Y + VSpeed) <= _Ceiling)
                    {
                        double NextPosition = Y - 1;
                        // rerounding for vfpi behaviour
                        while (Math.Round(NextPosition) != _Ceiling)
                        {
                            Y--;
                            NextPosition--;
                        }

                        VSpeed = 0;
                    }
                }
            }
        }

        // assumes floor > ceiling
        void Collision_2()
        {
            double NextPosition = Math.Round(Y + VSpeed);
            bool WillBeAboveFloor = NextPosition < _Floor;

            if (!WillBeAboveFloor)
            {
                bool AboveFloor = Math.Round(Y) < _Floor;
                if (AboveFloor)
                {
                    PerformCollision(1, _Floor);
                }
            }
            else if (!(NextPosition > _Ceiling))
            {
                bool BelowCeiling = Math.Round(Y) > _Ceiling;
                if (BelowCeiling)
                {
                    PerformCollision(-1, _Ceiling);
                }
            }
        }

        void PerformCollision(int Sign, double Solid)
        {
            double NextPosition = Y + Sign;
            // rerounding for vfpi behaviour
            while (Math.Round(NextPosition) != Solid)
            {
                Y += Sign;
                NextPosition += Sign;
            }

            VSpeed = 0;
        }

        public string GetStrat(bool OneFrameConvention)
        {
            StringBuilder sb = new();
            int Frames = 0, Convention = OneFrameConvention ? 1 : 0;
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
                            sb.Append($"{Frames + Convention}f 0p ");
                        }
                        else
                        {
                            sb.Append($" {Frames}p ");
                        }
                    }

                    Held = true;
                    Frames = 0;
                }
                if (Release)
                {
                    if (Held)
                    {
                        sb.Append($"{Frames + Convention}f");
                    }
                    else
                    {
                        sb.Append($"+{Frames}");
                    }

                    Held = false;
                    Frames = 0;
                }

                Frames++;
            }

            if (Held)
            {
                sb.Append($"{Frames + Convention}f");
            }
            else if (Frames > 0)
            {
                sb.Append($" {Frames}p");
            }

            return sb.ToString().Trim();
        }
    }
}

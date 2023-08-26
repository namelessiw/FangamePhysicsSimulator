using System.Diagnostics;

namespace FangamePhysicsSimulator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Search_Test();
        }

        // test if even ceiling/odd floor still work as expected
        // then try to get rid of rounding with modified ceiling/floor values
        void Search_Test()
        {
            Stopwatch sw = new();
            sw.Start();

            double start = 439.3;
            for (int i = 0; i < 0; i++)
            {
                start = Math.BitIncrement(start);
            }

            GM8Player p = new(start, 0);
            GM8Player.Ceiling = 391;
            GM8Player.CEILING_KILLER = false;
            GM8Player.Floor = 416;
            GM8Player.FLOOR_KILLER = false;
            GM8Player.Goal = 406.5;
            GM8Player.SetLowerBound();

            List<GM8Player> Results = new();

            GM8Player temp = new(p);
            temp.Advance(false, false);

            Search_Test(temp, Results);

            temp = new(p);
            temp.AdvanceSinglejump(true);

            Search_Test(temp, Results);

            p.AdvanceSinglejump(false);

            Search_Test(p, Results);

            string s = "";
            foreach (GM8Player r in Results)
            {
                s += r.GetStrat(false) + " (" + r.Frame + ")\n";
            }

            sw.Stop();

            MessageBox.Show($"{sw.Elapsed}\nResults: {Results.Count}\n{s}");
        }

        void Search_Test(GM8Player p, List<GM8Player> Results)
        {
            if (p.Y == GM8Player.Goal)
            {
                Results.Add(p);
            }

            if (p.VSpeed == 0 && p.IsStable())
                return;

            GM8Player temp;

            if (p.DoubleJump)
            {
                temp = new(p);
                if (temp.Advance(true, true))
                {
                    Search_Test(temp, Results);
                }

                temp = new(p);
                if (temp.Advance(true, false))
                {
                    Search_Test(temp, Results);
                }
            }
            if (p.VSpeed < 0)
            {
                temp = new(p);
                if (temp.Advance(false, true))
                {
                    Search_Test(temp, Results);
                }
            }

            temp = new(p);
            if (temp.Advance(false, false))
            {
                Search_Test(temp, Results);
            }
        }
    }
}
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
            GM8Player test = new(407.4, 0);
            GM8Player.Floor = 416;
            GM8Player.Ceiling = double.MinValue;
            GM8Player.Goal = 406.5;

            GM8Player.SetLowerBound();

            Random r = new();

            // goal
            // absolute lower bound
            // killer floor/ceil/lower bound
            // bruteforce search

            Search_Test();

            /*do
            {
                bool Press = test.Frame == 0 || test.DoubleJump && r.Next(16) < 1;
                bool Release = test.VSpeed < 0 && r.Next(4) < 1;

                test.Advance(Press, Release);
            }
            while (test.VSpeed != 0);

            MessageBox.Show($"Y: {test.Y}\nVSpeed: {test.VSpeed}\nInputs: {test.GetStrat(false)}\n");*/
        }

        void Search_Test()
        {
            GM8Player p = new(407.3999999999999, 0);
            GM8Player.Ceiling = double.MinValue;
            GM8Player.Floor = 416;
            GM8Player.FLOOR_KILLER = true;
            GM8Player.Goal = 406.5;
            GM8Player.SetLowerBound();

            List<GM8Player> Results = new();

            GM8Player temp = new(p);
            temp.Advance(true, false);

            Search_Test(temp, Results);

            p.Advance(true, true);

            Search_Test(p, Results);

            string s = "";
            foreach (GM8Player r in Results)
            {
                s += r.GetStrat(false) + " (" + r.Frame + ")\n";

                if (r.Frame > 66)
                {

                }
            }

            MessageBox.Show($"Results: {Results.Count}\n{s}");
        }

        void Search_Test(GM8Player p, List<GM8Player> Results)
        {
            if (p.Y == GM8Player.Goal)
            {
                Results.Add(p);
                return;
            }

            if (p.VSpeed == 0)
                return;

            if (p.DoubleJump)
            {
                GM8Player temp = new(p);
                if (temp.Advance(true, false))
                {
                    Search_Test(temp, Results);
                }

                temp = new(p);
                if (temp.Advance(true, true))
                {
                    Search_Test(temp, Results);
                }
            }
            if (p.VSpeed < 0)
            {
                GM8Player temp = new(p);
                if (temp.Advance(false, true))
                {
                    Search_Test(temp, Results);
                }
            }
            if (p.Advance(false, false))
            {
                Search_Test(p, Results);
            }
        }
    }
}
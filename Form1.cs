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

            GM8Player.GetLowerBound();

            Random r = new();

            // goal
            // absolute lower bound
            // killer floor/ceil/lower bound
            // bruteforce search

            do
            {
                bool Press = test.Frame == 0 || test.DoubleJump && r.Next(16) < 1;
                bool Release = test.VSpeed < 0 && r.Next(4) < 1;

                test.Advance(Press, Release);
            }
            while (test.VSpeed != 0);

            MessageBox.Show($"Y: {test.Y}\nVSpeed: {test.VSpeed}\nInputs: {test.GetStrat(false)}\n");
        }
    }
}
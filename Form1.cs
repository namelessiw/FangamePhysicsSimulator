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

            (bool, bool)[] Inputs = new (bool, bool)[]
            {
                (true, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, true),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (true, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, true),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
            };

            foreach ((bool Press, bool Release) in Inputs)
            {
                test.Advance(Press, Release);

                MessageBox.Show($"Y: {test.Y}\nVSpeed: {test.VSpeed}\nInputs: {test.GetStrat(false)}\n");
            }
        }
    }
}
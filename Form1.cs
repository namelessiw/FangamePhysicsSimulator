namespace FangamePhysicsSimulator
{
    public partial class Form1 : Form
    {
        Player test = new Player(407, 0);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Y: {test.Y}, VSpeed: {test.VSpeed}");
            test.Advance();
        }
    }
}
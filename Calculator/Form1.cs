using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static Calculator.CalculatorHelper;

namespace Calculator
{
    public partial class Body : Form
    {
        Operation operation = Operation.Empty;

        List<object> expression = new List<object>();

        public Body()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void output_Changed(object sender, EventArgs e)
        {
        }

        private void drop_Click(object sender, EventArgs e)
        {
            Output.Clear();
            Output.Text = "0";
        }

        private void button_click(object sender, EventArgs e)
        {
            if (Output.Text.StartsWith("0"))
            {
                Output.Clear();
            }

            Button button = (Button)sender;
            operation = Operation.Empty;
            Output.Text += button.Text;
        }

        private void operation_click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string[] outputArray = Output.Text.Trim().Split(' ');
            operation = GetOperation(outputArray.Last());

            Operation currentOperation = GetOperation(button.Text);

            if (operation == Operation.Empty)
            {
                Output.Text += " " + button.Text + " ";
            }

            if (!isValidToConcatOperation(currentOperation))
            {
                Output.Text = GetResult(ref outputArray, currentOperation).ToString();
            }
        }

        private void back_click(object sender, EventArgs e)
        {
            int lng = Output.Text.Trim().Length;

            if (lng > 1)
            {
                Output.Text = Output.Text.Trim().Remove(lng - 1);
            }
        }

        private void operation_ce_click(object sender, EventArgs e)
        {
            string[] outputArray = Output.Text.Trim().Split(' ');

            if (outputArray.Length > 1)
            {
                Output.Text = String.Join(" ", outputArray.Take(outputArray.Count() - 1).ToArray());
            }
        }
    }
}

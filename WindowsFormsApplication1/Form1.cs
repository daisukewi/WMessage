using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        CustomPaintTextBox customUnderlines;

        public Form1()
        {
            InitializeComponent();

            customUnderlines = new CustomPaintTextBox(textBox1);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //this.Refresh();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Tab)
            {
                MessageBox.Show("hola tabulador");
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestHarness
{
    using ShufflerLibrary;

    public partial class TestHarness : Form
    {
        public TestHarness()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int pe_pmd_id = int.Parse(textBox1.Text);

            Shuffler shuffler = new Shuffler();
            shuffler.ShuffleParagraph(pe_pmd_id);
        }
    }
}

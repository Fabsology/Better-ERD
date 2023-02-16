using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Better_ERD
{
    public partial class Form2 : MaterialForm
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Form2()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Green400, Primary.Green500, Primary.Red400, Accent.Red700, TextShade.WHITE);

        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {

            this.Title = materialSingleLineTextField1.Text;
            this.Content = richTextBox1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

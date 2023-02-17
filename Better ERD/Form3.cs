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
    public partial class Form3 : MaterialForm
    {
        public bool delete { get; set; }
        public Form3()
        {
            InitializeComponent();
            this.delete = false;
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Green400, Primary.Green500, Primary.Red400, Accent.Red700, TextShade.WHITE);

        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            this.delete = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Form3_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    this.Close();
                    break;
            }
        }
    }
}

using Centvrio.Emoji;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// See https://web.archive.org/web/20161130182929/http://web.simmons.edu/~benoit/lis458/CrowsFootNotation.pdf for understanding

namespace Better_ERD
{
    public partial class Form1 : MaterialForm
    {

        public int Pos_X = 0;
        public int Pos_Y = 0;
        public int thiccness = 3;
        public int factor = 16;

        public double Zoom = 1.00;

        public List<ObjectRectangle> ObjectRectangles = new List<ObjectRectangle>();
        public bool isDown = false;
        public Point relativeDragLocationCurrent = new Point(0, 0);
        public Point relativeDragLocationBeginning = new Point(0, 0);
        public Point relativeDragLocation = new Point(0, 0);
        public Point relativeMouseLocation = new Point(0, 0);
        public long clickTime = 0;

        public ObjectRectangle activeObjectRectangle = null;

        public Form1()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Green400, Primary.Green500, Primary.Red400, Accent.Red700, TextShade.WHITE);
        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

            int SHADOW = (Convert.ToInt32(10 * Zoom));
            int SHADOW_INTENSITY = 50;
            Font infoFont = new Font("Calibri", 13);
            Font drawFont = new Font("Calibri", Convert.ToInt32(13 * Zoom));
            Font contentDrawFont = new Font("Calibri", Convert.ToInt32(11 * Zoom));
            SolidBrush preDrawBrush = new SolidBrush(Color.FromArgb(190, 76, 175, 80));
            SolidBrush drawBrush = new SolidBrush(Color.FromArgb(255, 76, 175, 80));
            SolidBrush drawBrushTitle = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
            SolidBrush drawBrushShadow = new SolidBrush(Color.Black);

            SolidBrush BackgroundNormal = new SolidBrush(Color.FromArgb(255, 102, 187, 106));
            SolidBrush BackgroundActive = new SolidBrush(Color.FromArgb(255, 247, 86, 124));
            SolidBrush BackgroundNormal_Shadow = new SolidBrush(Color.FromArgb(255, 102 - SHADOW_INTENSITY, 187 - SHADOW_INTENSITY, 106 - SHADOW_INTENSITY));
            SolidBrush BackgroundActive_Shadow = new SolidBrush(Color.FromArgb(255, 247 - SHADOW_INTENSITY, 86 - SHADOW_INTENSITY, 124 - SHADOW_INTENSITY));
            StringFormat drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
            drawFormat.Alignment = StringAlignment.Center;


            e.Graphics.DrawEllipse(
                new Pen(Color.Red, 2f),
                0 - relativeDragLocation.X, 0 - relativeDragLocation.Y , pictureBox1.Size.Width, pictureBox1.Size.Height);

            foreach (ObjectRectangle OR in ObjectRectangles)
            {
                SizeF TitleBlockSize = e.Graphics.MeasureString(OR.Title.Replace("*", "»"), drawFont);
                SizeF TextBlockSize = e.Graphics.MeasureString(OR.Content.Replace("*", "»"), contentDrawFont);


                e.Graphics.FillRectangle(
                    new SolidBrush(Color.FromArgb(60, 0, 0, 0)),
                    new Rectangle(
                        new Point(Convert.ToInt32(((OR.X_Coord - relativeDragLocation.X) + SHADOW) * Zoom), Convert.ToInt32(((OR.Y_Coord - relativeDragLocation.Y) + SHADOW) * Zoom)),
                        new Size(Convert.ToInt32((TextBlockSize.Width > TitleBlockSize.Width ? TextBlockSize.Width : TitleBlockSize.Width)), Convert.ToInt32((TextBlockSize.Height + TitleBlockSize.Height)))
                        ));
            }

            foreach (ObjectRectangle OR in ObjectRectangles)
            {

                SizeF TitleBlockSize = e.Graphics.MeasureString(OR.Title, drawFont);
                SizeF TextBlockSize = e.Graphics.MeasureString(OR.Content, contentDrawFont);
                e.Graphics.FillRectangle(
                    new SolidBrush(Color.FromArgb(20, 0, 0, 0)),
                    new Rectangle(
                        new Point(Convert.ToInt32(((OR.X_Coord - relativeDragLocation.X) + SHADOW) * Zoom), Convert.ToInt32(((OR.Y_Coord - relativeDragLocation.Y) + SHADOW) * Zoom)),
                        new Size(Convert.ToInt32((TextBlockSize.Width > TitleBlockSize.Width ? TextBlockSize.Width : TitleBlockSize.Width)), Convert.ToInt32((TextBlockSize.Height + TitleBlockSize.Height)))
                        ));
                for (int i = 1; i < Convert.ToInt32(thiccness * Zoom); i++)
                {

                    e.Graphics.DrawRectangle(
                        new Pen((activeObjectRectangle == OR ? BackgroundActive_Shadow : BackgroundNormal_Shadow), 2f),
                        new Rectangle(
                            new Point(Convert.ToInt32(i + (OR.X_Coord - relativeDragLocation.X) * Zoom), Convert.ToInt32(i + (OR.Y_Coord - relativeDragLocation.Y) * Zoom)),
                            new Size(Convert.ToInt32((TextBlockSize.Width > TitleBlockSize.Width ? TextBlockSize.Width : TitleBlockSize.Width)), Convert.ToInt32((TextBlockSize.Height + TitleBlockSize.Height)))
                            ));
                }

                e.Graphics.FillRectangle(
                    new SolidBrush(Color.FromArgb(255, 51, 51, 51)),
                    new Rectangle(
                        new Point(Convert.ToInt32((OR.X_Coord - relativeDragLocation.X) * Zoom), Convert.ToInt32((OR.Y_Coord - relativeDragLocation.Y) * Zoom)),
                        new Size(Convert.ToInt32((TextBlockSize.Width > TitleBlockSize.Width ? TextBlockSize.Width : TitleBlockSize.Width)), Convert.ToInt32((TextBlockSize.Height + TitleBlockSize.Height)))
                        ));

                e.Graphics.FillRectangle(
                    (activeObjectRectangle == OR ? BackgroundActive : BackgroundNormal),
                    new Rectangle(
                        new Point(Convert.ToInt32((OR.X_Coord - relativeDragLocation.X) * Zoom), Convert.ToInt32((OR.Y_Coord - relativeDragLocation.Y) * Zoom)),
                        new Size(Convert.ToInt32((TextBlockSize.Width > TitleBlockSize.Width ? TextBlockSize.Width : TitleBlockSize.Width)), Convert.ToInt32((TitleBlockSize.Height)))
                        ));


                e.Graphics.DrawRectangle(
                    new Pen((activeObjectRectangle == OR ? BackgroundActive : BackgroundNormal), 2f),
                    new Rectangle(
                        new Point(Convert.ToInt32((OR.X_Coord - relativeDragLocation.X) * Zoom), Convert.ToInt32((OR.Y_Coord - relativeDragLocation.Y) * Zoom)),
                        new Size(Convert.ToInt32((TextBlockSize.Width > TitleBlockSize.Width ? TextBlockSize.Width : TitleBlockSize.Width)), Convert.ToInt32((TextBlockSize.Height + TitleBlockSize.Height) ))
                        ));

                e.Graphics.DrawString(OR.Title.Replace("*", "»"), drawFont, drawBrushTitle, Convert.ToInt32((OR.X_Coord - relativeDragLocation.X) * Zoom), Convert.ToInt32((OR.Y_Coord - relativeDragLocation.Y)*Zoom));
                e.Graphics.DrawString(OR.Content.Replace("*", "»"), contentDrawFont, drawBrush, Convert.ToInt32((OR.X_Coord - relativeDragLocation.X) * Zoom), Convert.ToInt32(((OR.Y_Coord - relativeDragLocation.Y) * Zoom) + TitleBlockSize.Height));
                
            }

            // Pre Brush
            if (activeObjectRectangle == null)
            {
                e.Graphics.DrawRectangle(
                    new Pen(preDrawBrush, 2f),
                    new Rectangle(
                            new Point(Convert.ToInt32(((relativeMouseLocation.X - relativeDragLocation.X)) * Zoom), Convert.ToInt32(((relativeMouseLocation.Y - relativeDragLocation.Y)) * Zoom)),
                        new Size(Convert.ToInt32(64 * Zoom), Convert.ToInt32(32 * Zoom))
                        ));
            }
            e.Graphics.DrawString(relativeDragLocation.X + ":" + relativeDragLocation.Y + "  Zoom: " + Zoom, infoFont, drawBrushShadow, 20, 21);
            e.Graphics.DrawString(relativeDragLocation.X + ":" + relativeDragLocation.Y + "  Zoom: " + Zoom, infoFont, drawBrush,20,20);

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("DOWN!");
            if (e.Button == MouseButtons.Right)
            {
                pictureBox1.Cursor = Cursors.Hand;
                isDown = true;
                relativeDragLocationBeginning = e.Location;
                relativeDragLocationCurrent = e.Location;
            } else if (e.Button == MouseButtons.Left)
            {
                foreach (ObjectRectangle obr in ObjectRectangles)
                {
                    if(clickedOn_Element(obr, e))
                    {
                        activeObjectRectangle = obr;
                        break;
                    }
                }
            }
            clickTime = CurrentMillis.Millis;
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                pictureBox1.Cursor = Cursors.Default;
            }
            activeObjectRectangle = null;
            isDown = false;
            Console.WriteLine("UP!");
            relativeDragLocationBeginning = e.Location;
            relativeDragLocationCurrent = relativeDragLocationBeginning;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            relativeMouseLocation = new Point(
                Convert.ToInt32((Math.Ceiling((double)((relativeDragLocation.X) + (e.Location.X / Zoom) - factor) / factor) * factor)),
                Convert.ToInt32((Math.Ceiling((double)((relativeDragLocation.Y) + (e.Location.Y / Zoom) - factor) / factor) * factor))
                );
            relativeDragLocationBeginning = e.Location;
            if (isDown)
            {
                Console.WriteLine("DRAG! {0} - {1} = " + (relativeDragLocationBeginning.X - relativeDragLocationCurrent.X), relativeDragLocationBeginning.X, relativeDragLocationCurrent.X);
                relativeDragLocation.X -= Convert.ToInt32((relativeDragLocationBeginning.X - relativeDragLocationCurrent.X) / Zoom);
                relativeDragLocation.Y -= Convert.ToInt32((relativeDragLocationBeginning.Y - relativeDragLocationCurrent.Y) / Zoom);
                Console.WriteLine(relativeDragLocation.X);
            }
            if (activeObjectRectangle != null)
            {
                Point location = new Point(Convert.ToInt32(((relativeMouseLocation.X - relativeDragLocation.X)) * Zoom), Convert.ToInt32(((relativeMouseLocation.Y - relativeDragLocation.Y)) * Zoom));
                ObjectRectangles.Where(obj => obj.Title == activeObjectRectangle.Title).First().X_Coord = relativeMouseLocation.X;
                ObjectRectangles.Where(obj => obj.Title == activeObjectRectangle.Title).First().Y_Coord = relativeMouseLocation.Y;

            }
            pictureBox1.Refresh();
            relativeDragLocationCurrent = e.Location;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (CurrentMillis.Millis - clickTime < 120 && e.Button == MouseButtons.Left) {
                foreach(ObjectRectangle obj in ObjectRectangles)
                {

                    if (clickedOn_Element(obj, e))
                    {
                        using (var form = new Form2())
                        {
                            form.Title = obj.Title;
                            form.Content = obj.Content;
                            form.Text = "Editing " + form.Title;

                            var result = form.ShowDialog();
                            if (result == DialogResult.OK)
                            {
                                string title = form.Title;
                                string content = form.Content;
                                ObjectRectangles.Where(ob => ob.Title == activeObjectRectangle.Title).First().Title = title;
                                ObjectRectangles.Where(ob => ob.Title == activeObjectRectangle.Title).First().Content = content;
                            }
                        }
                        return;
                    }
                }
                ObjectRectangle ObR = new ObjectRectangle();
                using (var form = new Form2())
                {
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        string title = form.Title;
                        string content = form.Content;

                        foreach(ObjectRectangle OBJR in ObjectRectangles)
                        {
                            if (OBJR.Title == title)
                            {
                                MessageBox.Show("Element Already Exists!");
                                return;
                            }
                        }

                        ObR.Width = 50;
                        ObR.Height = 50;
                        ObR.Title = title;
                        ObR.Content = content;
                        ObR.X_Coord = Convert.ToInt32((Math.Ceiling((double)((relativeDragLocation.X) + (e.Location.X / Zoom) - factor) / factor) * factor));
                        ObR.Y_Coord = Convert.ToInt32((Math.Ceiling((double)((relativeDragLocation.Y) + (e.Location.Y / Zoom) - factor) / factor) * factor));
                        ObjectRectangles.Add(ObR);
                        pictureBox1.Refresh();
                    }
                }


            }
        }

        private bool clickedOn_Element(ObjectRectangle OR, MouseEventArgs e)
        {
            Font drawFont = new Font("Consolas", Convert.ToInt32(13 * Zoom));
            Font contentDrawFont = new Font("Consolas", Convert.ToInt32(11 * Zoom));
            SizeF TitleBlockSize = new SizeF(0, 0);
            SizeF TextBlockSize = new SizeF(0, 0);
            SizeF GenerelSize = new SizeF(0, 0);
            using (Graphics g = Graphics.FromImage(new Bitmap(800, 600)))
            {
                TitleBlockSize = g.MeasureString(OR.Title, drawFont);
                TextBlockSize = g.MeasureString(OR.Content, contentDrawFont);
            }
            GenerelSize = new Size(Convert.ToInt32((TextBlockSize.Width > TitleBlockSize.Width ? TextBlockSize.Width : TitleBlockSize.Width)), Convert.ToInt32((TextBlockSize.Height + TitleBlockSize.Height)));
            Point relClickedLocation = e.Location;
            Point relObjectLocation = Relational_Location(new Point(OR.X_Coord, OR.Y_Coord));
            bool isInLocation =
                (relClickedLocation.X < relObjectLocation.X + GenerelSize.Width ? true : false) &
                (relClickedLocation.Y < relObjectLocation.Y + GenerelSize.Height ? true : false) &
                (relClickedLocation.X > relObjectLocation.X ? true : false) &
                (relClickedLocation.Y > relObjectLocation.Y ? true : false)
                ;
            return isInLocation;
        }


        private Point Relational_Location(Point Location)
        {
            Point p = new Point(Convert.ToInt32((Location.X - relativeDragLocation.X) * Zoom), Convert.ToInt32((Location.Y - relativeDragLocation.Y) * Zoom));
 
            Location = p;
            return Location;
        }
        static class CurrentMillis
        {
            private static readonly DateTime Jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            /// <summary>Get extra long current timestamp</summary>
            public static long Millis { get { return (long)((DateTime.UtcNow - Jan1St1970).TotalMilliseconds); } }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyData)
            {
                case Keys.Add:
                    if (this.Zoom < 2) { 
                        this.Zoom += 0.25;
                    }
                    Console.WriteLine("+" + this.Zoom);
                    break;
                case Keys.Subtract:
                    if (this.Zoom > 0.25)
                    {
                        this.Zoom -= 0.25;
                    }
                    Console.WriteLine("-" + this.Zoom);
                    break;
                case Keys.Delete:
                    isDown = false;
                    if (activeObjectRectangle != null)
                    {
                        using (var form = new Form3())
                        {
                            form.Text = "REMOVE " + activeObjectRectangle.Title + "?";
                            var result = form.ShowDialog();
                            if (result == DialogResult.OK)
                            {
                                if (form.delete) { 
                                    ObjectRectangles.Remove(activeObjectRectangle);
                                    activeObjectRectangle = null;
                                }
                            } else
                            {
                                activeObjectRectangle = null;
                            }
                        }
                    }
                    break;
            }
            pictureBox1.Refresh();
        }
    }
}

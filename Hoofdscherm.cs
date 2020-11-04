using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace SchetsEditor
{
    public class Hoofdscherm : Form
    {
        MenuStrip menuStrip;
        SchetsControl schetscontrol = new SchetsControl();
        public Hoofdscherm()
        {   this.ClientSize = new Size(800, 600);
            menuStrip = new MenuStrip();
            this.Controls.Add(menuStrip);
            this.maakFileMenu();
            this.maakHelpMenu();
            this.Text = "Schets editor";
            this.IsMdiContainer = true;
            this.MainMenuStrip = menuStrip;
        }
        private void maakFileMenu()
        {   ToolStripDropDownItem menu;
            menu = new ToolStripMenuItem("File");
            menu.DropDownItems.Add("Nieuw", null, this.nieuw);
            menu.DropDownItems.Add("Openen", null, this.openen);
            menu.DropDownItems.Add("Exit", null, this.afsluiten);
            menuStrip.Items.Add(menu);
        }
        private void maakHelpMenu()
        {   ToolStripDropDownItem menu;
            menu = new ToolStripMenuItem("Help");
            menu.DropDownItems.Add("Over \"Schets\"", null, this.about);
            menuStrip.Items.Add(menu);
        }
        private void about(object o, EventArgs ea)
        {   MessageBox.Show("Schets versie 1.0\n(c) UU Informatica 2010"
                           , "Over \"Schets\""
                           , MessageBoxButtons.OK
                           , MessageBoxIcon.Information
                           );
        }

        private void nieuw(object sender, EventArgs e)
        {
            Elementen elementen = new Elementen();
            SchetsWin s = new SchetsWin(elementen, null);
            s.MdiParent = this;
            s.Show();
        }
        private void openen(object o, EventArgs ea)
        {
            OpenFileDialog dialoog = new OpenFileDialog();
            dialoog.Filter = "Schetsbestand|*.xml|Alle bestanden|*.*";
            dialoog.Title = "Opnenen";
            if (dialoog.ShowDialog() == DialogResult.OK)
            {
                //XmlReader r = XmlReader.Create();
                SchetsWin s = new SchetsWin(null, dialoog.FileName);
                s.MdiParent = this;
                s.Show();
            }
        }
        private void afsluiten(object sender, EventArgs e)
        {   this.Close();
        }
    }
}

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SchetsEditor
{   public class SchetsControl : UserControl
    {   private Schets schets;
        private Color penkleur;
        public Elementen elementen;
        private int pendikte;

        public int PenDikte
        {
            get { return pendikte; }
            set { pendikte = value;  }
        }
        public Color PenKleur
        { 
            get { return penkleur; }
            set { penkleur = value; }
        }
        public Schets Schets
        { get { return schets;   }
        }
        public SchetsControl()
        {
            this.BorderStyle = BorderStyle.Fixed3D;
            this.schets = new Schets();
            this.Paint += this.teken;
            this.Resize += this.veranderAfmeting;
            this.veranderAfmeting(null, null);
            this.elementen = new Elementen();
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }
        private void teken(object o, PaintEventArgs pea)
        {   schets.Teken(pea.Graphics);
        }
        private void veranderAfmeting(object o, EventArgs ea)
        {   schets.VeranderAfmeting(this.ClientSize);
            this.Invalidate();
        }
        public Graphics MaakBitmapGraphics()
        {   Graphics g = schets.BitmapGraphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            return g;
        }
        public void Schoon(object o, EventArgs ea)
        {   schets.Schoon();
            this.Invalidate();
        }
        public void Roteer(object o, EventArgs ea)
        {   schets.VeranderAfmeting(new Size(this.ClientSize.Height, this.ClientSize.Width));
            schets.Roteer();
            this.Invalidate();
        }
        public void VeranderDikte(object obj, EventArgs ea)
        {
            string dikte = ((ComboBox)obj).Text;
            pendikte = Int32.Parse(dikte);
        }

        public void VeranderKleur(object obj, EventArgs ea)
        {   string kleurNaam = ((ComboBox)obj).Text;
            penkleur = Color.FromName(kleurNaam);
        }
        public void VeranderKleurViaMenu(object obj, EventArgs ea)
        {   string kleurNaam = ((ToolStripMenuItem)obj).Text;
            penkleur = Color.FromName(kleurNaam);
        }

        public void Undo(object o, EventArgs ea)
        {
            elementen.verwijderLaatste();
            tekenElementenLijst();
        }
        public void tekenElementenLijst()
        {
            Color huidigekleur = penkleur;
            int huidigedikte = pendikte;
            schets.Schoon();
            Graphics g = MaakBitmapGraphics();

            foreach(TekenElement tk in elementen.tekenElementen)
            {
                tk.teken(this);
            }
            this.Invalidate();
            penkleur = huidigekleur;
            pendikte = huidigedikte;
        }
    }
}

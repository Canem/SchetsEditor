﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SchetsEditor
{
    public interface ISchetsTool
    {
        void MuisVast(SchetsControl s, Point p, MouseButtons m);
        void MuisVirtueel(SchetsControl s, Point p);
        void MuisDrag(SchetsControl s, Point p);
        void MuisLos(SchetsControl s, Point p);
        void Letter(SchetsControl s, char c);
        void LetterVirtueel(SchetsControl s, char c);
    }

    public abstract class StartpuntTool : ISchetsTool
    {
        protected Point startpunt;
        protected Brush kwast;
        protected int penDikte;
        public virtual void MuisVast(SchetsControl s, Point p, MouseButtons m)
        {
            this.MuisVirtueel(s, p);
            s.elementen.Toevoegen(new TekenElement(this, startpunt, s.PenKleur, penDikte));
        }

        public void MuisVirtueel(SchetsControl s, Point p)
        {
            startpunt = p;
        }
        public virtual void MuisLos(SchetsControl s, Point p)
        {   kwast = new SolidBrush(s.PenKleur);
            penDikte = 3;
        }
        public abstract void MuisDrag(SchetsControl s, Point p);
        public abstract void Letter(SchetsControl s, char c);
        public virtual void LetterVirtueel(SchetsControl s, char c)
        {

        }
    }

    public class TekstTool : StartpuntTool
    {
        public override string ToString() { return "tekst"; }

        public override void MuisDrag(SchetsControl s, Point p) { }

        public override void Letter(SchetsControl s, char c)
        {
            this.LetterVirtueel(s, c);
            if (c != ' ')
                s.elementen.charToevoegen(c);
        }
        public override void LetterVirtueel(SchetsControl s, char c)
        {
            if (c >= 32)
            {
                Graphics gr = s.MaakBitmapGraphics();
                Font font = new Font("Tahoma", 40);
                string tekst = c.ToString();
                SizeF sz =
                gr.MeasureString(tekst, font, this.startpunt, StringFormat.GenericTypographic);
                gr.DrawString(tekst, font, kwast,
                                              this.startpunt, StringFormat.GenericTypographic);
                // gr.DrawRectangle(Pens.Black, startpunt.X, startpunt.Y, sz.Width, sz.Height);
                startpunt.X += (int)sz.Width;
                s.Invalidate();
            }
        }
    }

    public abstract class TweepuntTool : StartpuntTool
    {
        public static Rectangle Punten2Rechthoek(Point p1, Point p2)
        {   return new Rectangle( new Point(Math.Min(p1.X,p2.X), Math.Min(p1.Y,p2.Y))
                                , new Size (Math.Abs(p1.X-p2.X), Math.Abs(p1.Y-p2.Y))
                                );
        }
        public static Pen MaakPen(Brush b, int dikte)
        {   Pen pen = new Pen(b, dikte);
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;
            return pen;
        }
        public override void MuisVast(SchetsControl s, Point p, MouseButtons m)
        {   base.MuisVast(s, p, m);
            kwast = Brushes.Gray;
        }
        public override void MuisDrag(SchetsControl s, Point p)
        {   s.Refresh();
            this.Bezig(s.CreateGraphics(), this.startpunt, p);
        }
        public override void MuisLos(SchetsControl s, Point p)
        {   base.MuisLos(s, p);
            this.Compleet(s.MaakBitmapGraphics(), this.startpunt, p);
            s.Invalidate();

            s.elementen.eindpuntToevoegen(p);
        }
        public override void Letter(SchetsControl s, char c)
        {
        }
        public abstract void Bezig(Graphics g, Point p1, Point p2);
        
        public virtual void Compleet(Graphics g, Point p1, Point p2)
        {   this.Bezig(g, p1, p2);
        }
    }

    public class CirkelTool : TweepuntTool
    {
        public override string ToString() { return "cirkel"; }

        public override void Bezig(Graphics g, Point p1, Point p2)
        {
            g.DrawEllipse(MaakPen(kwast, penDikte), TweepuntTool.Punten2Rechthoek(p1, p2));
        }
    }

    public class VolCirkelTool : CirkelTool
    {
        public override string ToString() { return "cirkel vol"; }

        public override void Compleet(Graphics g, Point p1, Point p2)
        {
            g.DrawEllipse(MaakPen(kwast, penDikte), TweepuntTool.Punten2Rechthoek(p1, p2));
            g.FillEllipse(kwast, TweepuntTool.Punten2Rechthoek(p1, p2));
        }
    }

    public class RechthoekTool : TweepuntTool
    {
        public override string ToString() { return "kader"; }

        public override void Bezig(Graphics g, Point p1, Point p2)
        {   g.DrawRectangle(MaakPen(kwast, penDikte), TweepuntTool.Punten2Rechthoek(p1, p2));
        }
    }
    
    public class VolRechthoekTool : RechthoekTool
    {
        public override string ToString() { return "vlak"; }

        public override void Compleet(Graphics g, Point p1, Point p2)
        {   g.FillRectangle(kwast, TweepuntTool.Punten2Rechthoek(p1, p2));
        }
    }

    public class LijnTool : TweepuntTool
    {
        public override string ToString() { return "lijn"; }

        public override void Bezig(Graphics g, Point p1, Point p2)
        {   g.DrawLine(MaakPen(this.kwast, penDikte), p1, p2);
        }
    }

    public class PenTool : LijnTool
    {
        public override string ToString() { return "pen"; }

        public void Muis(SchetsControl s, Point p)
        {
            this.MuisVirtueel(s, p);
            s.elementen.beginPuntToevoegen(p);
        }

        public override void MuisDrag(SchetsControl s, Point p)
        {   this.MuisLos(s, p);
            this.Muis(s, p);
        }
    }
    
    public class GumTool : StartpuntTool
    {
        public override string ToString() { return "gum"; }
        public override void MuisVast(SchetsControl s, Point p, MouseButtons m)
        {
            s.elementen.verwijderElement(p);
            s.tekenElementenLijst();
        }

        public override void MuisLos(SchetsControl s, Point p) { }

        public override void MuisDrag(SchetsControl s, Point p) { }

        public override void Letter(SchetsControl s, char c) { }
    }

    public class LiftTool : StartpuntTool
    {
        public override string ToString() { return "lift"; }
        public override void MuisVast(SchetsControl s, Point p, MouseButtons m)
        {
            if(m == MouseButtons.Left)
            {
                s.elementen.elementOmhoog(p);
            }
            else
            {
                s.elementen.elementOmlaag(p);
            }
            
            s.tekenElementenLijst();
        }

        public override void MuisLos(SchetsControl s, Point p) { }

        public override void MuisDrag(SchetsControl s, Point p) { }

        public override void Letter(SchetsControl s, char c) { }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Drawing;
using System.Data.SqlTypes;
using System.Windows.Forms;

namespace SchetsEditor

{
	//Klasse voor het maken van een Tekenelement
	public class TekenElement
	{
		private string soort;
		private List<Point> beginpunt, eindpunt;
		private Color kleur;
		private string tekst;
		ISchetsTool tool;
		private int lijnDikte;
		public TekenElement(ISchetsTool t, Point p, Color c, int dikte)
		{
			tool = t;
			soort = tool.ToString();
			beginpunt = new List<Point>();
			eindpunt = new List<Point>();
			beginpunt.Add(p);
			kleur = c;
			tekst = "";
			lijnDikte = dikte;
		}

		public TekenElement(){ }

		public void zetEindpunt(Point p)
        {
			eindpunt.Add(p);
        }

		public void beginPuntToevoegen(Point p)
        {
			beginpunt.Add(p);
        }

		public void charToevoegen(char c)
        {
			tekst += c;
        }

		public bool raakKlik(Point p)
        {
			switch (this.soort)
			{
				case "tekst":
					Size s = TextRenderer.MeasureText(tekst, new Font("Tahoma", 40));
					int x = p.X - beginpunt[0].X;
					int y = p.Y - beginpunt[0].Y;

					if (x > 0 && y > 0 && x < s.Width && y < s.Height)
						return true;
					break;

				case "vlak":
					if (p.X > linksBoven().X && p.Y > linksBoven().Y && p.X < rechtsOnder().X && p.Y < rechtsOnder().Y)
						return true;
					break;

				case "kader":
					Point startKlein = new Point(linksBoven().X + lijnDikte, linksBoven().Y + lijnDikte);
					Point eindKlein = new Point(rechtsOnder().X - lijnDikte, rechtsOnder().Y - lijnDikte);
					if (p.X > linksBoven().X && p.Y > linksBoven().Y && p.X < rechtsOnder().X && p.Y < rechtsOnder().Y)
						if (p.X > startKlein.X && p.Y > startKlein.Y && p.X < eindKlein.X && p.Y < eindKlein.Y)
							return false;
						else
							return true;
					break;

				case "lijn":
					if (puntOpLijn(p, linksBoven(), rechtsOnder()))
						return true;
					break;

				case "pen":
					for (int i = 0; i < beginpunt.Count; i++)
						if (puntOpLijn(p, beginpunt[i], eindpunt[i]))
							return true;
					break;
				//kijkt of punt in ellipse zit en vervolgens of die niet in een iets kleinder ellipse zit om te checken of je alleen op de lijn klikt
				case "cirkel":
					if (inEllipse(p, linksBoven(), rechtsOnder(), lijnDikte))
						if (!inEllipse(p, linksBoven(), rechtsOnder(), -lijnDikte))
							return true;
					break;

				case "cirkel vol":
					if (inEllipse(p, linksBoven(), rechtsOnder(), lijnDikte))
						return true;
					break;
			}
			return false;
        }

		//Geeft de linker boven hoek
		Point linksBoven()
        {
			int x = Math.Min(beginpunt[0].X, eindpunt[0].X);
			int y = Math.Min(beginpunt[0].Y, eindpunt[0].Y);
			return new Point(x, y);
        }

		//Geeft rechts onder hoek
		Point rechtsOnder()
        {
			int x = Math.Max(beginpunt[0].X, eindpunt[0].X);
			int y = Math.Max(beginpunt[0].Y, eindpunt[0].Y);
			return new Point(x, y);
		}


		bool inEllipse(Point p, Point begin, Point eind, double dikte = 0)
        {
			double res;

			//Coord geklikt punt
			double x = p.X;
			double y = p.Y;

			//hoogte en breede ellipse
			double a = (eind.X + dikte) - (begin.X + dikte);
			double b = (eind.Y + dikte) - (begin.Y + dikte);

			//coord middelpunt
			double h = a / 2;
			double k = b / 2;

			x = x - linksBoven().X - h + dikte;
			y = y - linksBoven().Y - k + dikte;

			res = Math.Pow(x, 2) / Math.Pow(h, 2) + Math.Pow(y, 2) / Math.Pow(k, 2);

			if (res <= 1)
				return true;
			return false;
		}

		bool puntOpPen(Point p)
        {
			for(int i = 0; i < beginpunt.Count; i++)
            {
				if (beginpunt[i] == p)
                {
					return true;
                }
            }

			return false;
        }

		bool puntOpLijn(Point p, Point begin, Point eind)
        {
			PointF closest;

			float dx = eind.X - begin.X;
			float dy = eind.Y - begin.Y;

			//lijn is een punt
			if(dx == 0 && dy == 0)
            {
				dx = p.X - begin.X;
				dy = p.Y - begin.Y;
				if (Math.Sqrt(dx * dx + dy * dy) < lijnDikte + 2)
					return true;
			}

			float t = ((p.X - begin.X) * dx + (p.Y - begin.Y) * dy) / (dx * dx + dy * dy);

			if(t<0)
            {
				dx = p.X - begin.X;
				dy = p.Y - begin.Y;
			}
			else if (t > 1)
			{
				dx = p.X - eind.X;
				dy = p.Y - eind.Y;
			}
			else
			{
				closest = new PointF(linksBoven().X + t * dx, begin.Y + t * dy);
				dx = p.X - closest.X;
				dy = p.Y - closest.Y;
			}

			//+2 om raak klikken iets makkelijker te maken 
			if (Math.Sqrt(dx * dx + dy * dy) < lijnDikte + 2)
				return true;

			return false;
        }
		*/
		public void teken(SchetsControl s)
        {
			if(soort == "tekst")
            {
				tool.MuisVirtueel(s, beginpunt[0]);
				s.PenKleur = kleur;
				tool.MuisLos(s, beginpunt[0]);
				for (int i = 0; i < tekst.Length; i++)
				{
					tool.LetterVirtueel(s, tekst[i]);
				}
			}
			else
            {
				for (int i = 0; i < beginpunt.Count; i++)
				{
					tool.MuisVirtueel(s, beginpunt[i]);
					s.PenKleur = kleur;
					tool.MuisLos(s, eindpunt[i]);
				}
			}
        }
	}
}


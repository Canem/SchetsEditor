using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Drawing;

namespace SchetsEditor

{
	//Klasse voor het maken van een Tekenelement
	public class TekenElement
	{
		const float EPSILON = 0.001f;
		private string soort;
		private List<Point> beginpunt, eindpunt;
		private Color kleur;
		private string tekst;
		ISchetsTool tool;
		private int lijnDikte;
		public TekenElement(ISchetsTool t, Point p, Color c)
		{
			tool = t;
			soort = tool.ToString();
			beginpunt = new List<Point>();
			eindpunt = new List<Point>();
			beginpunt.Add(p);
			kleur = c;
			tekst = "";
			lijnDikte = 3;
		}

		public void zetEindpunt(Point p)
        {
			eindpunt.Add(p);
        }

		public bool raakKlik(Point p)
        {
			switch(this.soort)
            {
				case "vlak":
					if (p.X > linksBoven().X && p.Y > linksBoven().Y && p.X < rechtsOnder().X && p.Y < rechtsOnder().Y)
						return true;
					break;

				case "kader":
					if (p.X > linksBoven().X && p.Y > linksBoven().Y && p.X < rechtsOnder().X && p.Y < rechtsOnder().Y)
                    {
						if (p.X < linksBoven().X + lijnDikte)
							return true;
						if (p.Y < linksBoven().Y + lijnDikte)
							return true;
						if (p.X > linksBoven().X - lijnDikte)
							return true;
						if (p.Y > linksBoven().Y - lijnDikte)
							return true;
					}
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

		bool puntOpLijn(Point p)
        {

			Point beginPuntLijn = linksBoven();
			Point eindPuntLijn = rechtsOnder();

			float a = (eindPuntLijn.Y - beginPuntLijn.Y) / (eindPuntLijn.X - beginPuntLijn.X);
			float b = beginPuntLijn.Y - a * beginPuntLijn.X;
			if()


			return false;
        }

		public void teken(SchetsControl s)
        {
			for(int i = 0; i < beginpunt.Count; i++)
            {
				tool.MuisVirtueel(s, beginpunt[i]);
				s.PenKleur = kleur;
				tool.MuisLos(s, eindpunt[i]);
            }
        }
	}
}


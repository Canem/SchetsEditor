using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Drawing;

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
		public TekenElement(ISchetsTool t, Point p, Color c)
		{
			tool = t;
			soort = tool.ToString();
			beginpunt = new List<Point>();
			eindpunt = new List<Point>();
			beginpunt.Add(p);
			kleur = c;
			tekst = "";
		}

		public void zetEindpunt(Point p)
        {
			eindpunt.Add(p);
        }

		public bool raakKlik(Point p)
        {
			if (p.X > beginpunt[0].X && p.Y > beginpunt[0].Y && p.X < eindpunt[0].X && p.Y < eindpunt[0].Y)
				return true;

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


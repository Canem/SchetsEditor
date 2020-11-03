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

		public void teken(SchetsControl s)
        {
			for(int i = 0; i < beginpunt.Count; i++)
            {
				tool.MuisVirtueel(s, beginpunt[0]);
				s.PenKleur = kleur;
				tool.MuisLos(s, eindpunt[i]);
            }
        }
	}
}


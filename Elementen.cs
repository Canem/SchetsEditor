using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms.VisualStyles;

namespace SchetsEditor
{
	//Klasse voor het bijhouden van de lijst met getekende elementen
	public class Elementen
	{
		public List<TekenElement> tekenElementen = new List<TekenElement>();
		public Elementen()
		{
			Wissen();
		}

		//Maakt een nieuwe lege lijst
		public void Wissen()
		{
			tekenElementen = new List<TekenElement>();
		}

		//Add tekenelement to list
		public void Toevoegen(TekenElement t)
		{
			tekenElementen.Add(t);
		}

		//Add endpoint to last item in list
		public void eindpuntToevoegen(Point p)
        {
			tekenElementen[tekenElementen.Count - 1].zetEindpunt(p);
        }

		//Add beginpooint to last item in list
		public void beginPuntToevoegen(Point p)
        {
			tekenElementen[tekenElementen.Count - 1].beginPuntToevoegen(p);
		}

		public void charToevoegen(char c)
        {
			tekenElementen[tekenElementen.Count - 1].charToevoegen(c);

		}


		//Delete teken element that is clicked on in new gum
		public void verwijderElement(Point p)
        {
			for (int n = tekenElementen.Count - 1; n >= 0; n--)
			{
				if (tekenElementen[n].raakKlik(p))
                {
					tekenElementen.RemoveAt(n);
					break;
                }
            }
        }


		public void elementOmhoog(Point p)
		{
			for (int n = tekenElementen.Count - 1; n >= 0; n--)
			{
				if (tekenElementen[n].raakKlik(p))
				{
					TekenElement oud = tekenElementen[n];
					tekenElementen.RemoveAt(n);
					tekenElementen.Add(oud);
					break;
				}
			}
		}

		public void elementOmlaag(Point p)
		{
			for (int n = tekenElementen.Count - 1; n >= 0; n--)
			{
				if (tekenElementen[n].raakKlik(p))
				{
					TekenElement oud = tekenElementen[n];
					tekenElementen.RemoveAt(n);
					tekenElementen.Insert(0, oud);
					break;
				}
			}
		}

		public void verwijderLaatste()
        {
			if(tekenElementen.Count > 0)
				tekenElementen.RemoveAt(tekenElementen.Count - 1);
		}


		public void LeesVanFile(string naam, ISchetsTool[] deTools)
        {
			StreamReader sr = new StreamReader(naam);
			tekenElementen = new List<TekenElement>();
			while (sr.Peek() >= 0)
            {
				string temp = sr.ReadLine();
				string[] waarden = temp.Split('-');
				string[] begincoord = waarden[1].Split(',');
				string[] eindcoord = waarden[2].Split(',');

				string soort = waarden[0];
				Point beginPunt = new Point(int.Parse(begincoord[0]), int.Parse(begincoord[1]));
				Point eindpunt = new Point(int.Parse(eindcoord[0]), int.Parse(eindcoord[1]));
				Color kleur = Color.FromName(waarden[3]);
				string tekst = waarden[4];

				TekenElement t = new TekenElement(new CirkelTool(), beginPunt, kleur, 3);
				t.zetEindpunt(eindpunt);
				tekenElementen.Add(t);
			}
        }

		public void opslaan()
		{
			SaveFileDialog dialoog = new SaveFileDialog();
			dialoog.Filter = "Tekstfiles|*.txt|Alle files|*.*";
			dialoog.Title = "Schets opslaan als...";
			if (dialoog.ShowDialog() == DialogResult.OK)
			{
				TextWriter sw = new StreamWriter(dialoog.FileName);
				foreach(TekenElement tk in tekenElementen)
                {
					sw.WriteLine(tk.ToString());
				}
				sw.Close();
			}
		} 
	}
}


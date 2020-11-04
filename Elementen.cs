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


		public void LeesVanFile(string naam)
        {
			StreamReader sr = new StreamReader(naam);
			while(sr.Peek() >= 0)
            {
				tekenElementen = new List<TekenElement>();
				string temp = sr.ReadLine();
				string[] waarden = temp.Split('-');
				int i = 0;
				TekenElement t = new TekenElement(); 
				t.soort = waarden[i];
				//t.beginpunt[0] = 
				//t.eindpunt[0] = 
				//t.kleur = 
				//t.tekst =
				tekenElementen.Add(t);
				SchetsControl schetscontrol = new SchetsControl();
				t.teken(schetscontrol);
				i++;
			}
        }

		public void opslaan()
        {
			SaveFileDialog dialoog = new SaveFileDialog();
			dialoog.Filter = "Schetsbestand|*.xml|Alle bestanden|*.*";
			dialoog.Title = "Schets opslaan als...";
			if (dialoog.ShowDialog() == DialogResult.OK)
            {
				XmlSerializer s = new XmlSerializer(typeof(List<TekenElement>));
				StreamWriter sw = new StreamWriter(dialoog.FileName);
				s.Serialize(sw, tekenElementen);
				sw.Close();
			}
        } 
	}
}


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SchetsEditor
{
	//Klasse voor het bijhouden van de lijst met getekende elementen
	public class Elementen
	{
		public List<TekenElement> tekenElementen = new List<TekenElement>();
		public Elementen()
		{
			
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
	}
}


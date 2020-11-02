using System;
using System.Collections.Generic;

namespace SchetsEditor
{
	//Klasse voor het bijhouden van de lijst met getekende elementen
	public class Elementen
	{
		List<TekenElement> tekenElementen = new List<TekenElement>();
		public Elementen()
		{
			
		}

		//Maakt een nieuwe lege lijst
		public void Wissen()
		{
			List<TekenElement> tekenElementen = new List<TekenElement>();
		}

		public void Toevoegen(TekenElement t)
		{
			tekenElementen.Add(t);
		}
	}
}


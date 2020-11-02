using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Drawing;

namespace SchetsEditor

{
	//Klasse voor het maken van een Tekenelement
	public class TekenElement
	{
		public TekenElement()
		{
		}
		public string Kleur
		{ get; set; }
		public string Soort
		{ get; set; }
		public List<Point> Punten
		{ get; set; }
	}
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
	internal class Player
	{
		public Vector2 Position { get; set; }
		public double Rotate { get; set; }
		public double FOV { get; }
		public double Speed { get; }

		public Player(int fovAngle = 60, double speed = 2)
		{
			// Градусы в радианы
			FOV = fovAngle * (Math.PI / 180);
			Speed = speed;
		}
	}
}

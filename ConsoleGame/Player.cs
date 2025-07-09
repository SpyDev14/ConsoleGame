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
		public double StepSize { get; set; } = 0.1d;
		public Vector2 Position
		{
			get => _position;
			set => _position = value;
		}
		public double Rotation
		{
			get => _rotation;
			set => _rotation = value;
		}
		public double FOV
		{
			get => _fov * (Math.PI / 180);
			set => _fov = value;
		}
        private double _rotation;
		private Vector2 _position;
		private double _fov = 60;

		private const double _rotateStep = 0.05d;

        public void MoveStraight(int steps = 1)
        {
			// Почему-то оно именно так работает
			steps = steps - steps*2;
			var _perpenpedicularRotation = _rotation + Math.PI / 2;
			_position.X += (float)(StepSize * steps * Math.Cos(_perpenpedicularRotation));
			_position.Y += (float)(StepSize * steps * Math.Sin(_perpenpedicularRotation));
		}

		public void MoveSideway(int steps = 1)
		{
            _position.X += (float)(StepSize * steps * Math.Cos(_rotation));
			_position.Y += (float)(StepSize * steps * Math.Sin(_rotation));
		}

        public void Rotate(double steps = 1) => Rotation = Rotation + steps * _rotateStep;
    }
}

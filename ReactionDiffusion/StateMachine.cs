using System;

namespace ReactionDiffusion
{
	public class StateMachine
	{
		private readonly double _coeffDiff;
		private readonly int _iterations;
		private readonly double _maxVacancy;
		private double[,,] _dVacancy;

		public StateMachine(double coeffDiff, double maxVacancy, int iterations)
		{
			_coeffDiff = coeffDiff;
			_maxVacancy = maxVacancy;
			_iterations = iterations;
			_dVacancy = new double[_iterations, _iterations, _iterations];
			SetDefaultState();
		}

		public double[,,] GetNextState()
		{
			var newState = new double[_iterations, _iterations, _iterations];

			for (var i = 0; i < _iterations; i++)
			for (var j = 0; j < _iterations; j++)
			for (var k = 0; k < _iterations; k++)
			{
				var currentVacancy = _dVacancy[i, j, k];
				var tempVacancy = 0D;
				var numOfVacancy = 0;

				if (i != 0)
				{
					tempVacancy += _dVacancy[i - 1, j, k];
					numOfVacancy++;
				}
				if (i != _iterations - 1)
				{
					tempVacancy += _dVacancy[i + 1, j, k];
					numOfVacancy++;
				}
				if (j != 0)
				{
					tempVacancy += _dVacancy[i, j - 1, k];
					numOfVacancy++;
				}

				if (j != _iterations - 1)
				{
					tempVacancy += _dVacancy[i, j + 1, k];
					numOfVacancy++;
				}

				if (k != 0)
				{
					tempVacancy += _dVacancy[i, j, k - 1];
					numOfVacancy++;
				}

				if (k != _iterations - 1)
				{
					tempVacancy += _dVacancy[i, j, k + 1];
					numOfVacancy++;
				}

				newState[i, j, k] = currentVacancy - _coeffDiff * (tempVacancy - 6 * currentVacancy) / numOfVacancy;
			}
			_dVacancy = newState;
			return _dVacancy;
		}

		private void SetDefaultState()
		{
			FillRectangle(_iterations / 9D, _iterations / 3, _iterations / 3, _iterations / 2);
			FillRectangle(_iterations / 9D, _iterations / 3, _iterations / 3 * 2, _iterations / 2);
			FillRectangle(_iterations / 9D, _iterations / 3 * 2, _iterations / 3, _iterations / 2);
			FillRectangle(_iterations / 9D, _iterations / 3 * 2, _iterations / 3 * 2, _iterations / 2);
		}

		private void FillRectangle(double radius, int xCenter, int yCenter, int zCenter)
		{
			for (var i = 0; i < _iterations; i++)
			for (var j = 0; j < _iterations; j++)
			for (var k = 0; k < _iterations; k++)
			{
				var range = Math.Sqrt(Math.Pow(i - xCenter, 2) + Math.Pow(j - yCenter, 2) + Math.Pow(k - zCenter, 2));
				if (range <= radius)
					_dVacancy[i, j, k] = _maxVacancy;
			}
		}
	}
}
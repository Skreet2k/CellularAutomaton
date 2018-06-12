using System;

namespace ReactionDiffusion
{
	public class StateMachine3
	{
		private readonly int _iterations;
		private int[,] _dVacancy;

		public StateMachine3(int iterations)
		{
			_iterations = iterations;
			_dVacancy = new int[_iterations, _iterations];
			SetDefaultState();
		}

		public int[,] GetNextState()
		{
			var newState = new int[_iterations, _iterations];
			//var newState = _dVacancy;

			for (var i = 0; i < _iterations; i++)
			for (var j = 0; j < _iterations; j++)
			{
				var currentVacancy = _dVacancy[i, j];

				newState[i, j] = _dVacancy[GetCoordinate(i - 1), GetCoordinate(j)] +
				                 _dVacancy[GetCoordinate(i + 1), GetCoordinate(j)] +
				                 _dVacancy[GetCoordinate(i + 1), GetCoordinate(j - 1)] +
				                 _dVacancy[GetCoordinate(i + 1), GetCoordinate(j + 1)] +
				                 _dVacancy[GetCoordinate(i), GetCoordinate(j - 1)] +
				                 _dVacancy[GetCoordinate(i), GetCoordinate(j + 1)] +
				                 _dVacancy[GetCoordinate(i - 1), GetCoordinate(j - 1)] +
				                 _dVacancy[GetCoordinate(i - 1), GetCoordinate(j + 1)];
				if (currentVacancy > 0)
					newState[i, j] = newState[i, j] > 1 && newState[i, j] < 3 ? 1 : 0;
				else
					newState[i, j] = newState[i, j] == 3 ? 1 : 0;
			}
			_dVacancy = newState;
			return _dVacancy;
		}

		private int GetCoordinate(int coordinate)
		{
			if (coordinate < 0)
				return coordinate + _iterations;
			if (coordinate >= _iterations)
				return coordinate - _iterations;
			return coordinate;
		}

		private void SetDefaultState()
		{
			var radius = _iterations / 4D;
			var ijk0 = _iterations / 2;
			for (var i = 0; i < _iterations; i++)
				for (var j = 0; j < _iterations; j++)
				{
					var range = Math.Sqrt(Math.Pow(i - ijk0, 2) + Math.Pow(j - ijk0, 2));
					if (range <= radius)
						_dVacancy[i, j] = 1;
				}
			var minValue = _iterations / 4;
			var maxValue = _iterations * 3 / 4;
			for (var i = minValue; i < maxValue; i++)
				for (var j = minValue; j < maxValue; j++)
					for (var k = minValue; k < maxValue; k++)
						_dVacancy[i, j] = 1;
		}
	}
}
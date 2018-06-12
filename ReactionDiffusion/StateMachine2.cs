namespace ReactionDiffusion
{
	public class StateMachine2
	{
		private readonly double _coeffDiff;
		private double[,] _dVacancy;
		private readonly int _iterations;
		private readonly double _maxVacancy;

		public StateMachine2(double coeffDiff, double maxVacancy, int iterations)
		{
			_coeffDiff = coeffDiff;
			_maxVacancy = maxVacancy;
			_iterations = iterations;
			_dVacancy = new double[_iterations, _iterations];
			SetDefaultState();
		}

		public double[,] GetNextState()
		{
			var newState = new double[_iterations, _iterations];
			//var newState = _dVacancy;

			for (var i = 0; i < _iterations; i++)
			for (var j = 0; j < _iterations; j++)
			{
				var tempVacancy = 0D;
				var numOfVacancy = 0;
				var currentVacancy = _dVacancy[i, j];

				if (i != 0)
				{
					tempVacancy += _dVacancy[i - 1, j];
					numOfVacancy++;
				}

				if (i != _iterations - 1)
				{
					tempVacancy += _dVacancy[i + 1, j];
					numOfVacancy++;
				}

				if (j != 0)
				{
					tempVacancy += _dVacancy[i, j - 1];
					numOfVacancy++;
				}

				if (j != _iterations - 1)
				{
					tempVacancy += _dVacancy[i, j + 1];
					numOfVacancy++;
				}

				newState[i,j] = currentVacancy - _coeffDiff * (tempVacancy - 4 * currentVacancy) / numOfVacancy;

				//newState[i, j] = newState[i, j] > 0 ? newState[i, j] : 0;
				//newState[i, j] = newState[i, j] > _maxVacancy ? _maxVacancy : newState[i, j];

				}
			_dVacancy = newState;
			return _dVacancy;
		}

		private void SetDefaultState()
		{
			var minValue = _iterations / 4;
			var maxValue = _iterations * 3 / 4;
			for (var i = minValue; i < maxValue; i++)
			for (var j = minValue; j < maxValue; j++)
			for (var k = minValue; k < maxValue; k++)
				_dVacancy[i, j] = _maxVacancy;
		}
	}
}
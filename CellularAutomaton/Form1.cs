using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReactionDiffusion;

namespace CellularAutomaton
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			DrawArea = new Bitmap(Picture.Size.Width, Picture.Size.Height);
			Picture.Image = DrawArea;
		}

		private Bitmap DrawArea { get; }

		private void Form1_Load(object sender, EventArgs e)
		{


		}

		private void Picture_Click(object sender, EventArgs e)
		{
			GetImage().Wait();
		}

		private async Task GetImage()
		{
			var g = Graphics.FromImage(DrawArea);
			var n = 200;
			var k = n / 2;

			var state = new StateMachine(1, 255, n);
			for (var z = 0; z < 1000; z++)
			{
				var max = double.MinValue;
				var min = double.MaxValue;

				var nextState = state.GetNextState();
				g.Clear(Color.White);

				for (var i = 0; i < n; i++)
					for (var j = 0; j < n; j++)
					{
						var d = nextState[i, j, k] < 0 ? 0 : nextState[i, j, k] > 255 ? 255 : nextState[i, j, k];
						var size = Convert.ToInt32(d);
						var customColor = Color.FromArgb(255, 30, 0, size);
						var shadowBrush = new SolidBrush(customColor);
						g.FillRectangle(shadowBrush, i, j, 1, 1);

					}
				Picture.Image = DrawArea;

				await Task.Delay(1).ConfigureAwait(false);
			}
			g.Dispose();
		}
	}
}
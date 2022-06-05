namespace Kolory;

using static Settings;
public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
		var color = Load();
		if (!updateInterface) return;
		sliderR.Value = color.r;
		sliderG.Value = color.g;
		sliderB.Value = color.b;
	}

	private bool updateInterface = true;

	private void slider_ValueChanged(object s, EventArgs e)
    {
		Color color = Color.FromRgb(sliderR.Value, sliderG.Value, sliderB.Value);
		rectangle.Fill = new SolidColorBrush(color);
		labelR.Text = Math.Round(255*color.Red).ToString();
		labelG.Text = Math.Round(255 * color.Green).ToString();
		labelB.Text = Math.Round(255 * color.Blue).ToString();
		Save(sliderR.Value, sliderG.Value, sliderB.Value);
	}
}


using Microsoft.Maui.Layouts;

namespace MinuApp;

public class LiuguridPage : ContentPage
{
    Stepper stepper;
    Slider slider;
    Label silt;

    public LiuguridPage()
    {
        Title = "3.3 Stepper ja Slider";

        silt = new Label { Text = "Väärtus: 50", FontSize = 20, Padding = 10 };

        stepper = new Stepper { Minimum = 0, Maximum = 100, Increment = 5, Value = 50 };

        slider = new Slider
        {
            Minimum = 0,
            Maximum = 100,
            Value = 50,
            WidthRequest = 280,
            MinimumTrackColor = Colors.BlueViolet,   // täidetud osa
            MaximumTrackColor = Colors.LightGray,    // tühi osa
            ThumbColor = Colors.Orange               // liugur ise
        };

        // Mõlemal on sama sündmus. Kui üks muutub, liigutame ka teist.
        stepper.ValueChanged += Vaartus_Muutus;
        slider.ValueChanged += Vaartus_Muutus;

        AbsoluteLayout abs = new AbsoluteLayout();
        List<View> elemendid = new List<View> { silt, stepper, slider };
        for (int i = 0; i < elemendid.Count; i++)
        {
            double y = 0.15 + i * 0.35;
            AbsoluteLayout.SetLayoutBounds(elemendid[i], new Rect(0.5, y, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(elemendid[i], AbsoluteLayoutFlags.PositionProportional);
            abs.Add(elemendid[i]);
        }
        Content = abs;
    }

    private void Vaartus_Muutus(object? sender, ValueChangedEventArgs e)
    {
        double v = e.NewValue;   // 0 – 100

        // Sünkroniseerimine: ValueChanged käivitub ainult siis, kui väärtus TÕESTI muutub,
        // seega ei teki lõputut tsüklit
        if (sender == slider) stepper.Value = v;
        else slider.Value = v;

        silt.Text = $"Väärtus: {v:F0}";
        silt.FontSize = 20 + v / 4;                 // 20 … 45
        silt.Rotation = v * 3.6;                    // 0 … 360 kraadi
        int hall = (int)(v * 2.55);                 // 0 … 255
        silt.BackgroundColor = Color.FromRgb(hall, hall, 255);
    }
}
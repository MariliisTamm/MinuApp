using Microsoft.Maui.Layouts;

namespace MinuApp;

public class AbsoluutPage : ContentPage
{
    AbsoluteLayout abs;
    BoxView pall;
    Random rnd = new Random();

    public AbsoluutPage()
    {
        Title = "3.1 AbsoluteLayout";
        abs = new AbsoluteLayout { BackgroundColor = Color.FromArgb("#F3F0FF") };

        // Neli kasti nurkadesse: asukoht 0 = vasak/ülemine serv, 1 = parem/alumine serv
        LisaKast(Colors.Red, 0, 0);
        LisaKast(Colors.Green, 1, 0);
        LisaKast(Colors.Blue, 0, 1);
        LisaKast(Colors.Orange, 1, 1);

        // Pall keskel; puudutamisel hüppab juhuslikku kohta
        pall = new BoxView { Color = Colors.Purple, CornerRadius = 30 };
        AbsoluteLayout.SetLayoutBounds(pall, new Rect(0.5, 0.5, 60, 60));
        AbsoluteLayout.SetLayoutFlags(pall, AbsoluteLayoutFlags.PositionProportional);
        TapGestureRecognizer tap = new TapGestureRecognizer();
        tap.Tapped += (sender, e) =>
        {
            double x = rnd.NextDouble();   // 0.0 – 1.0
            double y = rnd.NextDouble();
            AbsoluteLayout.SetLayoutBounds(pall, new Rect(x, y, 60, 60));
        };
        pall.GestureRecognizers.Add(tap);
        abs.Add(pall);

        // Silt: laius 80% ekraanist, asukoht keskel all
        Label silt = new Label
        {
            Text = "Puuduta lillat palli!",
            FontSize = 18,
            HorizontalTextAlignment = TextAlignment.Center
        };
        AbsoluteLayout.SetLayoutBounds(silt, new Rect(0.5, 0.8, 0.8, AbsoluteLayout.AutoSize));
        AbsoluteLayout.SetLayoutFlags(silt, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);
        abs.Add(silt);

        Content = abs;
    }

    void LisaKast(Color varv, double x, double y)
    {
        BoxView kast = new BoxView { Color = varv, CornerRadius = 10 };
        // Rect(x, y, laius, kõrgus): x ja y on proportsionaalsed (0–1), suurus ühikutes
        AbsoluteLayout.SetLayoutBounds(kast, new Rect(x, y, 80, 80));
        AbsoluteLayout.SetLayoutFlags(kast, AbsoluteLayoutFlags.PositionProportional);
        abs.Add(kast);
    }
}
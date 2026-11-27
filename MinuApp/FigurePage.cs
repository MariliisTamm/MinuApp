using Microsoft.Maui.Controls.Shapes;

namespace MinuApp;

public class FigurePage : ContentPage
{
    BoxView kast;
    Ellipse pall;
    Polygon kolmnurk;
    Random rnd = new Random();

    public FigurePage()
    {
        Title = "Kujundid";

        kast = new BoxView
        {
            Color = JuhuslikVarv(),
            WidthRequest = 100,
            HeightRequest = 100,
            CornerRadius = 20,
            HorizontalOptions = LayoutOptions.Center
        };

        pall = new Ellipse
        {
            WidthRequest = 120,
            HeightRequest = 120,
            Fill = new SolidColorBrush(JuhuslikVarv()),   // täitevärv
            Stroke = Colors.BurlyWood,                      // äärise värv
            StrokeThickness = 5,                            // äärise paksus
            HorizontalOptions = LayoutOptions.Center
        };

        kolmnurk = new Polygon
        {
            Points = new PointCollection
            {
                new Point(0, 120),    // vasak all
                new Point(60, 0),     // ülal keskel
                new Point(120, 120)   // parem all
            },
            WidthRequest = 120,
            HeightRequest = 120,
            Fill = new SolidColorBrush(JuhuslikVarv()),
            Stroke = Colors.Aquamarine,
            StrokeThickness = 5,
            HorizontalOptions = LayoutOptions.Center
        };

        // Üks puudutus kastil või pallil -> kõik kolm muudavad värvi, kast kasvab
        TapGestureRecognizer tap = new TapGestureRecognizer();
        tap.Tapped += (sender, e) => MuudaKoiki();
        kast.GestureRecognizers.Add(tap);
        pall.GestureRecognizers.Add(tap);

        // Topeltpuudutus kolmnurgal -> kolmnurk pöörab end tagurpidi
        TapGestureRecognizer topelt = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
        topelt.Tapped += (sender, e) => kolmnurk.Rotation += 180;
        kolmnurk.GestureRecognizers.Add(topelt);

        Label juhend = new Label
        {
            Text = "Puuduta kasti või palli. Kolmnurka puuduta kaks korda.",
            FontSize = 14,
            TextColor = Colors.Gray,
            HorizontalTextAlignment = TextAlignment.Center
        };

        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Padding = 20,
                Spacing = 15,
                Children = { juhend, kast, pall, kolmnurk, StartPage.LooNavigatsioon(Navigation, 2) }
            }
        };
    }

    Color JuhuslikVarv()
    {
        return Color.FromRgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
    }

    void MuudaKoiki()
    {
        kast.Color = JuhuslikVarv();
        pall.Fill = new SolidColorBrush(JuhuslikVarv());
        kolmnurk.Fill = new SolidColorBrush(JuhuslikVarv());

        kast.WidthRequest += 20;
        kast.HeightRequest += 10;

        // Ekraani laius: Width on PIKSLITES, jagame tihedusega -> MAUI ühikud
        double ekraaniLaius = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        if (kast.WidthRequest > ekraaniLaius - 40)
        {
            kast.WidthRequest = 100;
            kast.HeightRequest = 100;
        }
    }
}
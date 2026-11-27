using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace MinuApp;

public class KuuskPage : ContentPage
{
    // KUUSE ELEMENDID
    AbsoluteLayout kuuseKonteiner; // Hoiab kõiki kuuse osi koos (saab korraga liigutada ja pöörata)
    Label tahtLabel;
    List<Ellipse> pallid = new List<Ellipse>(); // Nimekiri 6 jõulupallist
    List<Ellipse> tuled = new List<Ellipse>();  // Nimekiri 7 jõulutulest

    // JUHTPANEELI ELEMENDID
    Label statusLabel;
    Picker tegevusPicker;
    Label heledusLabel;
    Slider heledusSlider;
    Label kiirusLabel;
    Stepper kiirusStepper;
    DatePicker kuupaevPicker;
    TimePicker kellaaegPicker;

    // OLEKU MUUTUJAD (Järjekorra kontrollimiseks)
    bool kuuskPysti = false;
    bool kuuskEhitud = false;
    Random rnd = new Random();

    public KuuskPage()
    {
        Title = "Jõulukuusk";

        // Põhiline paigutus: ülal kuuse ala, all juhtpaneel
        ScrollView kerimisvaade = new ScrollView();
        VerticalStackLayout peaPaigutus = new VerticalStackLayout
        {
            Padding = 15,
            Spacing = 12
        };

        // OSA 1: KUUSE JOONISTAMINE (AbsoluteLayout)
        
        AbsoluteLayout lava = new AbsoluteLayout
        {
            HeightRequest = 310,
            WidthRequest = 300,
            HorizontalOptions = LayoutOptions.Center
        };

        // Kuuse konteiner: alguses külili (90°) ja paremal ekraani servas peidus (TranslationX = 250)
        kuuseKonteiner = new AbsoluteLayout
        {
            WidthRequest = 220,
            HeightRequest = 280,
            Rotation = 90,
            TranslationX = 250
        };
        AbsoluteLayout.SetLayoutBounds(kuuseKonteiner, new Rect(0.5, 0.0, 220, 280));
        AbsoluteLayout.SetLayoutFlags(kuuseKonteiner, AbsoluteLayoutFlags.XProportional);

        // Jalg (lai ja madal punane BoxView kuuse all)
        BoxView jalg = new BoxView { Color = Colors.DarkRed, CornerRadius = 4 };
        AbsoluteLayout.SetLayoutBounds(jalg, new Rect(0.5, 260, 80, 18));
        AbsoluteLayout.SetLayoutFlags(jalg, AbsoluteLayoutFlags.XProportional);

        // Tüvi (lühike pruun BoxView)
        BoxView tyvi = new BoxView { Color = Colors.SaddleBrown };
        AbsoluteLayout.SetLayoutBounds(tyvi, new Rect(0.5, 230, 26, 32));
        AbsoluteLayout.SetLayoutFlags(tyvi, AbsoluteLayoutFlags.XProportional);

        // Okstik (3 rohelist kolmnurka üksteise peal, alumine kõige laiem)
        Polygon alumineOks = LooKolmnurk(200, 100, Colors.DarkGreen);
        AbsoluteLayout.SetLayoutBounds(alumineOks, new Rect(0.5, 135, 200, 100));
        AbsoluteLayout.SetLayoutFlags(alumineOks, AbsoluteLayoutFlags.XProportional);

        Polygon keskmineOks = LooKolmnurk(160, 90, Colors.ForestGreen);
        AbsoluteLayout.SetLayoutBounds(keskmineOks, new Rect(0.5, 80, 160, 90));
        AbsoluteLayout.SetLayoutFlags(keskmineOks, AbsoluteLayoutFlags.XProportional);

        Polygon ylemineOks = LooKolmnurk(120, 80, Colors.Green);
        AbsoluteLayout.SetLayoutBounds(ylemineOks, new Rect(0.5, 30, 120, 80));
        AbsoluteLayout.SetLayoutFlags(ylemineOks, AbsoluteLayoutFlags.XProportional);

        // Täht tipus (alguses peidetud, süttib ehtimise lõpus)
        tahtLabel = new Label
        {
            Text = "⭐",
            FontSize = 34,
            IsVisible = false,
            HorizontalTextAlignment = TextAlignment.Center
        };
        AbsoluteLayout.SetLayoutBounds(tahtLabel, new Rect(0.5, 0, 45, 45));
        AbsoluteLayout.SetLayoutFlags(tahtLabel, AbsoluteLayoutFlags.XProportional);

        // Lisame põhiosad kuuse konteinerisse
        kuuseKonteiner.Children.Add(jalg);
        kuuseKonteiner.Children.Add(tyvi);
        kuuseKonteiner.Children.Add(alumineOks);
        kuuseKonteiner.Children.Add(keskmineOks);
        kuuseKonteiner.Children.Add(ylemineOks);
        kuuseKonteiner.Children.Add(tahtLabel);

        // Jõulupallid (6 tk, alguses peidetud)
        double[,] palliKoordinaadid = {
            { 95, 75 }, { 125, 85 },
            { 75, 130 }, { 140, 135 },
            { 60, 195 }, { 155, 200 }
        };
        for (int i = 0; i < 6; i++)
        {
            Ellipse pall = new Ellipse
            {
                Fill = new SolidColorBrush(Colors.Red),
                WidthRequest = 18,
                HeightRequest = 18,
                IsVisible = false
            };
            AbsoluteLayout.SetLayoutBounds(pall, new Rect(palliKoordinaadid[i, 0], palliKoordinaadid[i, 1], 18, 18));
            pallid.Add(pall);
            kuuseKonteiner.Children.Add(pall);
        }

        // 1.6 Tuled (7 väikest kollast ringi, alguses peidetud)
        double[,] tuleKoordinaadid = {
            { 108, 60 }, { 90, 105 }, { 130, 110 },
            { 70, 165 }, { 110, 160 }, { 145, 170 }, { 105, 215 }
        };
        for (int i = 0; i < 7; i++)
        {
            Ellipse tuli = new Ellipse
            {
                Fill = new SolidColorBrush(Colors.Yellow),
                Stroke = Colors.Orange,
                StrokeThickness = 1,
                WidthRequest = 11,
                HeightRequest = 11,
                IsVisible = false
            };
            AbsoluteLayout.SetLayoutBounds(tuli, new Rect(tuleKoordinaadid[i, 0], tuleKoordinaadid[i, 1], 11, 11));
            tuled.Add(tuli);
            kuuseKonteiner.Children.Add(tuli);
        }

        // 1.7 Loovuspunkt: Kingid kuuse all (TapGestureRecognizer)
        BoxView kink1 = LooKink(Colors.Crimson, "Legokomplekt 🧱", 45, 265);
        BoxView kink2 = LooKink(Colors.RoyalBlue, "Soe talvesall 🧣", 225, 265);
        lava.Children.Add(kink1);
        lava.Children.Add(kink2);
        lava.Children.Add(kuuseKonteiner);

        // ============================================================
        // OSA 2: JUHTPANEEL
        // ============================================================
        statusLabel = new Label
        {
            Text = "Kuusk ootab ukse taga! Vali 'Pane püsti'.",
            FontSize = 16,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        };

        tegevusPicker = new Picker
        {
            Title = "Vali tegevus...",
            ItemsSource = new List<string> { "Pane püsti", "Ehi", "Süüta tuled", "Vii ära" },
            WidthRequest = 175,
            TextColor = Colors.Black,
            BackgroundColor = Color.FromArgb("#F0F0F0")
        };

        Button kaivitaBtn = new Button
        {
            Text = "Käivita",
            BackgroundColor = Colors.ForestGreen,
            TextColor = Colors.White,
            FontAttributes = FontAttributes.Bold,
            CornerRadius = 10,
            WidthRequest = 100
        };
        kaivitaBtn.Clicked += OnActionClicked;

        HorizontalStackLayout tegevusRida = new HorizontalStackLayout
        {
            Spacing = 10,
            HorizontalOptions = LayoutOptions.Center,
            Children = { tegevusPicker, kaivitaBtn }
        };

        // Slider: tulede heledus (Opacity)
        heledusLabel = new Label { Text = "Tulede heledus: 100%", FontSize = 13, TextColor = Colors.Black };
        heledusSlider = new Slider { Minimum = 0.0, Maximum = 1.0, Value = 1.0 };
        heledusSlider.ValueChanged += (s, e) =>
        {
            foreach (Ellipse tuli in tuled)
                tuli.Opacity = e.NewValue;
            heledusLabel.Text = $"Tulede heledus: {e.NewValue * 100:F0}%";
        };

        // Stepper: animatsiooni kiirus (500 - 2000 ms)
        kiirusLabel = new Label { Text = "Kiirus: 800 ms", FontSize = 14, TextColor = Colors.Black, VerticalOptions = LayoutOptions.Center };
        kiirusStepper = new Stepper { Minimum = 500, Maximum = 2000, Increment = 250, Value = 800 };
        kiirusStepper.ValueChanged += (s, e) => kiirusLabel.Text = $"Kiirus: {e.NewValue:F0} ms";

        HorizontalStackLayout kiirusRida = new HorizontalStackLayout
        {
            Spacing = 15,
            HorizontalOptions = LayoutOptions.Center,
            Children = { kiirusLabel, kiirusStepper }
        };

        // DatePicker ja TimePicker
        kuupaevPicker = new DatePicker { Date = new DateTime(2026, 12, 15), Format = "dd.MM.yyyy", TextColor = Colors.Black };
        kellaaegPicker = new TimePicker { Time = new TimeSpan(18, 0, 0), Format = "HH:mm", TextColor = Colors.Black };
        kellaaegPicker.TimeSelected += (s, e) => UuendaTaustaOopaevast();

        HorizontalStackLayout aegRida = new HorizontalStackLayout
        {
            Spacing = 15,
            HorizontalOptions = LayoutOptions.Center,
            Children =
            {
                new VerticalStackLayout
                {
                    Children =
                    {
                        new Label { Text = "Kuupäev:", FontSize = 12, TextColor = Colors.Gray },
                        kuupaevPicker
                    }
                },
                new VerticalStackLayout
                {
                    Children =
                    {
                        new Label { Text = "Kellaaeg:", FontSize = 12, TextColor = Colors.Gray },
                        kellaaegPicker
                    }
                }
            }
        };

        Border juhtpaneel = new Border
        {
            BackgroundColor = Colors.White,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(15) },
            Padding = 15,
            Opacity = 0.95,
            Content = new VerticalStackLayout
            {
                Spacing = 10,
                Children = { tegevusRida, heledusLabel, heledusSlider, kiirusRida, aegRida }
            }
        };
        peaPaigutus.Children.Add(lava);
        peaPaigutus.Children.Add(statusLabel);
        peaPaigutus.Children.Add(juhtpaneel);

        kerimisvaade.Content = peaPaigutus;
        Content = kerimisvaade;

        UuendaTaustaOopaevast();
    }

    // ============================================================
    // OSA 3: TEGEVUSED JA TINGIMUSTE KONTROLL
    // ============================================================
    private async void OnActionClicked(object? sender, EventArgs e)
    {
        if (tegevusPicker.SelectedItem == null)
        {
            statusLabel.Text = "Vali esmalt rippmenüüst tegevus!";
            return;
        }

        if (kuupaevPicker.Date is not DateTime kuupaev || kellaaegPicker.Time is not TimeSpan kell)
            return;

        string valik = tegevusPicker.SelectedItem.ToString()!;
        uint kiirus = (uint)kiirusStepper.Value;

        switch (valik)
        {
            case "Pane püsti":
                // Tingimus: ainult detsembris (Month == 12)
                if (kuupaev.Month != 12)
                {
                    statusLabel.Text = "Kuusk tuuakse tuppa alles detsembris!";
                    return;
                }

                statusLabel.Text = "Toome kuuse tuppa ja paneme püsti...";
                await kuuseKonteiner.TranslateToAsync(0, 0, kiirus); // Toob keskele
                await kuuseKonteiner.RotateToAsync(0, kiirus);       // Tõstab püsti (0°)
                kuuskPysti = true;
                statusLabel.Text = "Kuusk on püsti! Nüüd saab ehtida.";
                break;

            case "Ehi":
                // Tingimus: kuusk peab olema enne püsti pandud
                if (!kuuskPysti)
                {
                    statusLabel.Text = "Enne ehtimist pane kuusk püsti!";
                    return;
                }

                statusLabel.Text = "Ehime kuuse ära...";
                // Loovuspunkt: juhuslikud värvid pallidele + ükshaaval ilmumine
                foreach (Ellipse pall in pallid)
                {
                    pall.Fill = new SolidColorBrush(Color.FromRgb(rnd.Next(50, 256), rnd.Next(50, 256), rnd.Next(50, 256)));
                    pall.Scale = 0;
                    pall.IsVisible = true;
                    await pall.ScaleToAsync(1.0, kiirus / 3);
                }

                // Lõpuks süttib täht tipus
                tahtLabel.Scale = 0;
                tahtLabel.IsVisible = true;
                await tahtLabel.ScaleToAsync(1.0, kiirus / 2);

                kuuskEhitud = true;
                statusLabel.Text = "Kuusk on ehitud!";
                break;

            case "Süüta tuled":
                // Tingimus: kuusk peab olema ehitud ja kell 16:00 - 23:59
                if (!kuuskEhitud)
                {
                    statusLabel.Text = "Ehi kuusk enne tulede süütamist!";
                    return;
                }
                if (kell.Hours < 16)
                {
                    statusLabel.Text = "Päeval tulesid ei süüdata! (Vali 16:00–23:59)";
                    return;
                }

                statusLabel.Text = "Jõulutuled säravad!";
                foreach (Ellipse tuli in tuled)
                {
                    tuli.Opacity = heledusSlider.Value;
                    tuli.IsVisible = true;
                }

                // Tuled vilguvad paar korda ja täht pöörleb tipus
                for (int k = 0; k < 2; k++)
                {
                    foreach (Ellipse tuli in tuled) _ = tuli.FadeToAsync(0.2, kiirus / 3);
                    await Task.Delay((int)(kiirus / 3));
                    foreach (Ellipse tuli in tuled) _ = tuli.FadeToAsync(heledusSlider.Value, kiirus / 3);
                    await Task.Delay((int)(kiirus / 3));
                }
                await tahtLabel.RotateToAsync(360, kiirus);
                tahtLabel.Rotation = 0;
                break;

            case "Vii ära":
                // Tingimus: 6. jaanuar või hiljem (Month == 1 && Day >= 6 või Month == 2)
                bool onOigeAeg = (kuupaev.Month == 1 && kuupaev.Day >= 6) || (kuupaev.Month == 2);
                if (!onOigeAeg)
                {
                    statusLabel.Text = "Kuusk jääb tuppa kolmekuningapäevani (6. jaanuar)!";
                    return;
                }

                statusLabel.Text = "Viime kuuse välja...";
                // Ehted ja tuled kaovad
                foreach (Ellipse pall in pallid) _ = pall.FadeToAsync(0, kiirus / 2);
                foreach (Ellipse tuli in tuled) _ = tuli.FadeToAsync(0, kiirus / 2);
                await tahtLabel.FadeToAsync(0, kiirus / 2);

                foreach (Ellipse pall in pallid) { pall.IsVisible = false; pall.Opacity = 1; }
                foreach (Ellipse tuli in tuled) { tuli.IsVisible = false; tuli.Opacity = 1; }
                tahtLabel.IsVisible = false;
                tahtLabel.Opacity = 1;

                // Kuusk kukub külili ja liigub ekraanilt välja
                await kuuseKonteiner.RotateToAsync(90, kiirus);
                await kuuseKonteiner.TranslateToAsync(250, 0, kiirus);

                kuuskPysti = false;
                kuuskEhitud = false;
                statusLabel.Text = "Kuusk on ära viidud! Kohtumiseni järgmistel jõuludel.";
                break;
        }
    }
    // ============================================================
    // OSA 4: ABIMEETODID JA LOOVUSPUNKTID
    // ============================================================

    // Kolmnurga joonistamine okste jaoks
    Polygon LooKolmnurk(double laius, double korgus, Color varv)
    {
        return new Polygon
        {
            Points = new PointCollection
            {
                new Point(0, korgus),          // vasak alumine nurk
                new Point(laius / 2, 0),       // ülemine tipp
                new Point(laius, korgus)       // parem alumine nurk
            },
            Fill = new SolidColorBrush(varv),
            Stroke = Colors.DarkGreen,
            StrokeThickness = 1
        };
    }

    // Loovuspunkt: interaktiivne kingipakk kuuse all
    BoxView LooKink(Color varv, string sisu, double x, double y)
    {
        BoxView kink = new BoxView
        {
            Color = varv,
            WidthRequest = 32,
            HeightRequest = 32,
            CornerRadius = 4
        };
        AbsoluteLayout.SetLayoutBounds(kink, new Rect(x, y, 32, 32));

        TapGestureRecognizer tap = new TapGestureRecognizer();
        tap.Tapped += async (s, e) =>
        {
            statusLabel.Text = $"Avasid kingi: {sisu}";
            await kink.ScaleToAsync(1.3, 150);
            await kink.ScaleToAsync(1.0, 150);
        };
        kink.GestureRecognizers.Add(tap);

        return kink;
    }

    // Loovuspunkt: päeva ja öö taustavärv vastavalt TimePickerile
    void UuendaTaustaOopaevast()
    {
        if (kellaaegPicker.Time is not TimeSpan kell) return;

        if (kell.Hours >= 16 || kell.Hours < 7)
        {
            BackgroundColor = Color.FromArgb("#1B263B"); // Õhtu/öö: tumesinine
            statusLabel.TextColor = Colors.White;
        }
        else
        {
            BackgroundColor = Color.FromArgb("#E0F2FE"); // Päev: hele talvine taevas
            statusLabel.TextColor = Colors.Black;
        }
    }
}
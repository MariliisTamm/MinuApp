
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MinuApp;

public class KuuskPage : ContentPage
{
    // 1. KUUSE ELEMENDID
    private AbsoluteLayout kuuseKonteiner = null!;
    private Label tahtLabel = null!;
    private List<Ellipse> pallid = new();
    private List<Ellipse> tuled = new();

    // 2. JUHTPANEELI ELEMENDID
    private Label statusLabel = null!;
    private Picker tegevusPicker = null!;
    private Label heledusLabel = null!;
    private Slider heledusSlider = null!;
    private Label kiirusLabel = null!;
    private Stepper kiirusStepper = null!;
    private DatePicker kuupaevPicker = null!;
    private TimePicker kellaaegPicker = null!;
    private Button kaivitaBtn = null!;

    // 3. KUUSE OLEKU MUUTUJAD
    private bool kuuskPysti = false;
    private bool kuuskEhitud = false;
    private bool tegevusKäib = false;
    private Random rnd = new Random();

    // 4. KONSTRUKTOR
    public KuuskPage()
    {
        Title = "Jõulukuusk";
        BackgroundColor = Color.FromArgb("#E0F2FE");
        LooLeht();
    }

    // 5. LEHE PAIGUTUSE LOOMINE
    private void LooLeht()
    {
        ScrollView kerimisvaade = new ScrollView();

        VerticalStackLayout peaPaigutus =
            new VerticalStackLayout
            {
                Padding = 15,
                Spacing = 12
            };

        AbsoluteLayout lava = new AbsoluteLayout
        {
            HeightRequest = 310,
            WidthRequest = 300,
            HorizontalOptions = LayoutOptions.Center
        };

        LooKuusk(lava);

        Border juhtpaneel = LooJuhtpaneel();

        peaPaigutus.Children.Add(lava);
        peaPaigutus.Children.Add(statusLabel);
        peaPaigutus.Children.Add(juhtpaneel);

        kerimisvaade.Content = peaPaigutus;
        Content = kerimisvaade;

        UuendaTaustaOopaevast();
    }

    // 6. KUUSE JOONISTAMINE
    private void LooKuusk(AbsoluteLayout lava)
    {
        kuuseKonteiner = new AbsoluteLayout
        {
            WidthRequest = 220,
            HeightRequest = 280,
            Rotation = 90,
            TranslationX = 250
        };

        AbsoluteLayout.SetLayoutBounds(
            kuuseKonteiner,
            new Rect(0.5, 0, 220, 280));

        AbsoluteLayout.SetLayoutFlags(
            kuuseKonteiner,
            AbsoluteLayoutFlags.XProportional);

        // KUUSE JALG
        BoxView jalg = new BoxView
        {
            Color = Colors.DarkRed,
            CornerRadius = 4
        };

        LisaElement(
            kuuseKonteiner, jalg,
            0.5, 260, 80, 18);

        // KUUSE TÜVI
        BoxView tyvi = new BoxView
        {
            Color = Colors.SaddleBrown
        };

        LisaElement(
            kuuseKonteiner, tyvi,
            0.5, 230, 26, 32);

        // KUUSE OKSAD
        Polygon alumineOks =
            LooKolmnurk(200, 100, Colors.DarkGreen);

        Polygon keskmineOks =
            LooKolmnurk(160, 90, Colors.ForestGreen);

        Polygon ylemineOks =
            LooKolmnurk(120, 80, Colors.Green);

        LisaElement(
            kuuseKonteiner, alumineOks,
            0.5, 135, 200, 100);

        LisaElement(
            kuuseKonteiner, keskmineOks,
            0.5, 80, 160, 90);

        LisaElement(
            kuuseKonteiner, ylemineOks,
            0.5, 30, 120, 80);

        // KUUSE TÄHT
        tahtLabel = new Label
        {
            Text = "⭐",
            FontSize = 34,
            IsVisible = false,
            HorizontalTextAlignment = TextAlignment.Center
        };

        LisaElement(
            kuuseKonteiner, tahtLabel,
            0.5, 0, 45, 45);

        // JÕULUPALLID
        double[,] palliKoordinaadid =
        {
            { 95, 75 },
            { 125, 85 },
            { 75, 130 },
            { 140, 135 },
            { 60, 195 },
            { 155, 200 }
        };

        for (int i = 0; i < 6; i++)
        {
            Ellipse pall = new Ellipse
            {
                Fill = Colors.Red,
                WidthRequest = 18,
                HeightRequest = 18,
                IsVisible = false
            };

            AbsoluteLayout.SetLayoutBounds(
                pall,
                new Rect(
                    palliKoordinaadid[i, 0],
                    palliKoordinaadid[i, 1],
                    18, 18));

            pallid.Add(pall);
            kuuseKonteiner.Children.Add(pall);
        }

        // JÕULUTULED
        double[,] tuleKoordinaadid =
        {
            { 108, 60 },
            { 90, 105 },
            { 130, 110 },
            { 70, 165 },
            { 110, 160 },
            { 145, 170 },
            { 105, 215 }
        };

        for (int i = 0; i < 7; i++)
        {
            Ellipse tuli = new Ellipse
            {
                Fill = Colors.Yellow,
                Stroke = Colors.Orange,
                StrokeThickness = 1,
                WidthRequest = 11,
                HeightRequest = 11,
                IsVisible = false,
                Opacity = 1
            };

            AbsoluteLayout.SetLayoutBounds(
                tuli,
                new Rect(
                    tuleKoordinaadid[i, 0],
                    tuleKoordinaadid[i, 1],
                    11, 11));

            tuled.Add(tuli);
            kuuseKonteiner.Children.Add(tuli);
        }

        // KINGITUSED
        BoxView kink1 = LooKink(
            Colors.Crimson,
            "Legokomplekt 🧱",
            45, 265);

        BoxView kink2 = LooKink(
            Colors.RoyalBlue,
            "Soe talvesall 🧣",
            225, 265);

        lava.Children.Add(kink1);
        lava.Children.Add(kink2);
        lava.Children.Add(kuuseKonteiner);
    }

    // 7. ELEMENTIDE LISAMISE ABIMEETOD
    private void LisaElement(
        AbsoluteLayout konteiner,
        View element,
        double x,
        double y,
        double laius,
        double korgus)
    {
        AbsoluteLayout.SetLayoutBounds(
            element,
            new Rect(x, y, laius, korgus));

        AbsoluteLayout.SetLayoutFlags(
            element,
            AbsoluteLayoutFlags.XProportional);

        konteiner.Children.Add(element);
    }


    // 8. JUHTPANEELI LOOMINE
    private Border LooJuhtpaneel()
    {
        statusLabel = new Label
        {
            Text = "Kuusk ootab ukse taga! Vali 'Pane püsti'.",
            FontSize = 16,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        };

        // TEGEVUSE VALIK
        tegevusPicker = new Picker
        {
            Title = "Vali tegevus...",
            ItemsSource = new List<string>
            {
                "Pane püsti",
                "Ehi",
                "Süüta tuled",
                "Vii ära"
            },
            WidthRequest = 175,
            TextColor = Colors.Black,
            BackgroundColor = Color.FromArgb("#F0F0F0")
        };

        // KÄIVITAMISE NUPP
        kaivitaBtn = new Button
        {
            Text = "Käivita",
            BackgroundColor = Colors.ForestGreen,
            TextColor = Colors.White,
            CornerRadius = 10,
            WidthRequest = 100
        };

        kaivitaBtn.Clicked += OnActionClicked;

        HorizontalStackLayout tegevusRida =
            new HorizontalStackLayout
            {
                Spacing = 10,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    tegevusPicker,
                    kaivitaBtn
                }
            };

        // TULEDE HELEDUS
        heledusLabel = new Label
        {
            Text = "Tulede heledus: 100%",
            FontSize = 13,
            TextColor = Colors.Black
        };

        heledusSlider = new Slider
        {
            Minimum = 0,
            Maximum = 1,
            Value = 1
        };

        heledusSlider.ValueChanged += (s, e) =>
        {
            foreach (Ellipse tuli in tuled)
            {
                if (tuli.IsVisible)
                    tuli.Opacity = e.NewValue;
            }

            heledusLabel.Text =
                $"Tulede heledus: {e.NewValue * 100:F0}%";
        };

        // ANIMATSIOONI KIIRUS
        kiirusLabel = new Label
        {
            Text = "Kiirus: 750 ms",
            FontSize = 14,
            TextColor = Colors.Black
        };

        kiirusStepper = new Stepper
        {
            Minimum = 500,
            Maximum = 2000,
            Increment = 250,
            Value = 750
        };

        kiirusStepper.ValueChanged += (s, e) =>
        {
            kiirusLabel.Text =
                $"Kiirus: {e.NewValue:F0} ms";
        };

        HorizontalStackLayout kiirusRida =
            new HorizontalStackLayout
            {
                Spacing = 15,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    kiirusLabel,
                    kiirusStepper
                }
            };

        // KUUPÄEV JA KELLAAEG
        kuupaevPicker = new DatePicker
        {
            Date = new DateTime(2026, 12, 15),
            Format = "dd.MM.yyyy",
            TextColor = Colors.Black
        };

        kellaaegPicker = new TimePicker
        {
            Time = new TimeSpan(18, 0, 0),
            Format = "HH:mm",
            TextColor = Colors.Black
        };

        kuupaevPicker.DateSelected += (s, e) =>
            UuendaTaustaOopaevast();

        kellaaegPicker.TimeSelected += (s, e) =>
            UuendaTaustaOopaevast();

        HorizontalStackLayout aegRida =
            new HorizontalStackLayout
            {
                Spacing = 15,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new VerticalStackLayout
                    {
                        Children =
                        {
                            new Label { Text = "Kuupäev:" },
                            kuupaevPicker
                        }
                    },

                    new VerticalStackLayout
                    {
                        Children =
                        {
                            new Label { Text = "Kellaaeg:" },
                            kellaaegPicker
                        }
                    }
                }
            };

        // JUHTPANEELI KOKKUPANEK
        return new Border
        {
            BackgroundColor = Colors.White,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(15)
            },
            Padding = 15,

            Content = new VerticalStackLayout
            {
                Spacing = 10,
                Children =
                {
                    tegevusRida,
                    heledusLabel,
                    heledusSlider,
                    kiirusRida,
                    aegRida
                }
            }
        };
    }

    // 9. TEGEVUSTE KÄIVITAMINE
    private async void OnActionClicked(
        object? sender,
        EventArgs e)
    {
        if (tegevusKäib)
            return;

        if (tegevusPicker.SelectedItem == null)
        {
            statusLabel.Text = "Vali esmalt tegevus!";
            return;
        }

        tegevusKäib = true;
        kaivitaBtn.IsEnabled = false;

        try
        {
            string valik =
                tegevusPicker.SelectedItem.ToString()!;

            switch (valik)
            {
                case "Pane püsti":
                    await PaneKuuskPysti();
                    break;

                case "Ehi":
                    await EhiKuusk();
                    break;

                case "Süüta tuled":
                    await SyutaTuled();
                    break;

                case "Vii ära":
                    await ViiKuuskAra();
                    break;
            }
        }
        finally
        {
            tegevusKäib = false;
            kaivitaBtn.IsEnabled = true;
        }
    }

    // 10. ANIMATSIOONI KIIRUS
    private uint AnimatsiooniKiirus =>
        (uint)kiirusStepper.Value;

    // 11. KUUSE PÜSTI PANEMINE
    private async Task PaneKuuskPysti()
    {
        DateTime kuupaev =
            kuupaevPicker.Date.GetValueOrDefault();

        if (kuupaev.Month != 12)
        {
            statusLabel.Text =
                "Kuusk tuuakse tuppa alles detsembris!";
            return;
        }

        if (kuuskPysti)
        {
            statusLabel.Text = "Kuusk on juba püsti!";
            return;
        }

        statusLabel.Text = "Toome kuuse tuppa...";

        await kuuseKonteiner.TranslateToAsync(
            0, 0, AnimatsiooniKiirus, Easing.CubicOut);

        await kuuseKonteiner.RotateToAsync(
            0, AnimatsiooniKiirus, Easing.CubicOut);

        kuuskPysti = true;

        statusLabel.Text =
            "Kuusk on püsti! Nüüd saab ehtida.";
    }

    // 12. KUUSE EHTIMINE
    private async Task EhiKuusk()
    {
        if (!kuuskPysti)
        {
            statusLabel.Text =
                "Enne ehtimist pane kuusk püsti!";
            return;
        }

        if (kuuskEhitud)
        {
            statusLabel.Text = "Kuusk on juba ehitud!";
            return;
        }

        statusLabel.Text = "Ehime kuuse ära...";

        foreach (Ellipse pall in pallid)
        {
            Color juhuslikVarv = Color.FromRgb(
                rnd.Next(50, 256),
                rnd.Next(50, 256),
                rnd.Next(50, 256));

            pall.Fill = juhuslikVarv;
            pall.Scale = 0;
            pall.IsVisible = true;

            await pall.ScaleToAsync(
                1,
                AnimatsiooniKiirus / 2,
                Easing.BounceOut);
        }

        // TÄHT ILMUB VIIMASENA
        tahtLabel.Scale = 0;
        tahtLabel.IsVisible = true;

        await tahtLabel.ScaleToAsync(
            1,
            AnimatsiooniKiirus,
            Easing.BounceOut);

        kuuskEhitud = true;

        statusLabel.Text = "Kuusk on ehitud! ⭐";
    }

    // 13. JÕULUTULEDE SÜÜTAMINE

    private async Task SyutaTuled()
    {
        TimeSpan kellaaeg =
            kellaaegPicker.Time.GetValueOrDefault();

        if (!kuuskEhitud)
        {
            statusLabel.Text =
                "Ehi kuusk enne tulede süütamist!";
            return;
        }

        if (kellaaeg.Hours < 16)
        {
            statusLabel.Text =
                "Päeval tulesid ei süüdata! Vali 16:00–23:59.";
            return;
        }

        statusLabel.Text = "Jõulutuled säravad! ✨";

        foreach (Ellipse tuli in tuled)
        {
            tuli.Opacity = 0;
            tuli.IsVisible = true;

            await tuli.FadeToAsync(
                heledusSlider.Value,
                AnimatsiooniKiirus / 2);
        }

        for (int i = 0; i < 2; i++)
        {
            foreach (Ellipse tuli in tuled)
            {
                await tuli.FadeToAsync(
                    0.2,
                    AnimatsiooniKiirus / 4);
            }

            foreach (Ellipse tuli in tuled)
            {
                await tuli.FadeToAsync(
                    heledusSlider.Value,
                    AnimatsiooniKiirus / 4);
            }
        }

        await tahtLabel.RotateToAsync(
            360, AnimatsiooniKiirus);

        tahtLabel.Rotation = 0;

        statusLabel.Text =
            "Kuusk särab jõuluvalguses! 🎄";
    }


    // 14. KUUSE ÄRA VIIMINE

    private async Task ViiKuuskAra()
    {
        DateTime kuupaev =
            kuupaevPicker.Date.GetValueOrDefault();

        bool oigeAeg =
            (kuupaev.Month == 1 && kuupaev.Day >= 6)
            || kuupaev.Month == 2;

        if (!oigeAeg)
        {
            statusLabel.Text =
                "Kuusk jääb tuppa kolmekuningapäevani!";
            return;
        }

        if (!kuuskPysti)
        {
            statusLabel.Text =
                "Kuusk on juba ära viidud!";
            return;
        }

        statusLabel.Text = "Viime kuuse välja...";

        // Ehted kaovad
        foreach (Ellipse pall in pallid)
        {
            await pall.FadeToAsync(
                0, AnimatsiooniKiirus / 2);

            pall.IsVisible = false;
            pall.Opacity = 1;
            pall.Scale = 1;
        }

        // Tuled kustuvad
        foreach (Ellipse tuli in tuled)
        {
            await tuli.FadeToAsync(
                0, AnimatsiooniKiirus / 2);

            tuli.IsVisible = false;
            tuli.Opacity = 1;
        }

        // Täht kaob
        await tahtLabel.FadeToAsync(
            0, AnimatsiooniKiirus / 2);

        tahtLabel.IsVisible = false;
        tahtLabel.Opacity = 1;
        tahtLabel.Scale = 1;
        tahtLabel.Rotation = 0;

        // Kuusk kukub külili
        await kuuseKonteiner.RotateToAsync(
            90,
            AnimatsiooniKiirus,
            Easing.CubicIn);

        // Kuusk liigub ekraanilt välja
        await kuuseKonteiner.TranslateToAsync(
            250,
            0,
            AnimatsiooniKiirus,
            Easing.CubicIn);

        kuuskPysti = false;
        kuuskEhitud = false;

        statusLabel.Text =
            "Kuusk on ära viidud! Kohtumiseni järgmistel jõuludel. 🎄";
    }


    // 15. KOLMNURKADE LOOMINE
    private Polygon LooKolmnurk(
        double laius,
        double korgus,
        Color varv)
    {
        return new Polygon
        {
            Points = new PointCollection
            {
                new Point(0, korgus),
                new Point(laius / 2, 0),
                new Point(laius, korgus)
            },

            Fill = new SolidColorBrush(varv),
            Stroke = Colors.DarkGreen,
            StrokeThickness = 1
        };
    }

    // 16. KINGITUSTE LOOMINE
    private BoxView LooKink(
        Color varv,
        string sisu,
        double x,
        double y)
    {
        BoxView kink = new BoxView
        {
            Color = varv,
            WidthRequest = 32,
            HeightRequest = 32,
            CornerRadius = 4
        };

        AbsoluteLayout.SetLayoutBounds(
            kink,
            new Rect(x, y, 32, 32));

        TapGestureRecognizer tap =
            new TapGestureRecognizer();

        tap.Tapped += async (s, e) =>
        {
            statusLabel.Text = $"Avasid kingi: {sisu}";

            await kink.ScaleToAsync(
                1.3, 150, Easing.BounceOut);

            await kink.ScaleToAsync(
                1.0, 150, Easing.BounceOut);
        };

        kink.GestureRecognizers.Add(tap);

        return kink;
    }

    // 17. PÄEVA JA ÖÖ TAUST
    private void UuendaTaustaOopaevast()
    {
        int tund =
            kellaaegPicker.Time.GetValueOrDefault().Hours;

        if (tund >= 16 || tund < 7)
        {
            // ÖÖTAUST
            BackgroundColor =
                Color.FromArgb("#1B263B");

            statusLabel.TextColor = Colors.White;
        }
        else
        {
            // PÄEVATAUST
            BackgroundColor =
                Color.FromArgb("#E0F2FE");

            statusLabel.TextColor = Colors.Black;
        }
    }
}


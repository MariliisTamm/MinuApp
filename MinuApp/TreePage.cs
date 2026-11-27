using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace MinuApp;

public class TreePage : ContentPage
{
    // Puu elemendid
    AbsoluteLayout treeContainer;
    BoxView trunkBox;
    Ellipse leavesEllipse;
    Ellipse apple1, apple2;

    // Juhtpaneeli elemendid
    Label statusLabel;
    Picker actionPicker;
    Label opacityLabel;
    Slider opacitySlider;
    Label speedLabel;
    Stepper speedStepper;
    DatePicker seasonDatePicker;
    TimePicker dayTimePicker;

    Random rnd = new Random();

    public TreePage()
    {
        Title = "Puu elutsükkel";
        BackgroundColor = Color.FromArgb("#87CEEB"); // Suvine helesinine taevas

        // 1. PUU JOONISTAMINE (AbsoluteLayout)
        treeContainer = new AbsoluteLayout
        {
            HeightRequest = 260,
            WidthRequest = 240,
            HorizontalOptions = LayoutOptions.Center
        };

        // Puutüvi: pruun piklik BoxView keskel all
        trunkBox = new BoxView
        {
            Color = Colors.SaddleBrown,
            CornerRadius = 5
        };
        AbsoluteLayout.SetLayoutBounds(trunkBox, new Rect(0.5, 1.0, 36, 130));
        AbsoluteLayout.SetLayoutFlags(trunkBox, AbsoluteLayoutFlags.PositionProportional);

        // Lehestik: suur roheline Ellipse tüve kohal
        leavesEllipse = new Ellipse
        {
            Fill = new SolidColorBrush(Colors.ForestGreen),
            Stroke = Colors.DarkGreen,
            StrokeThickness = 2
        };
        AbsoluteLayout.SetLayoutBounds(leavesEllipse, new Rect(0.5, 0.15, 160, 140));
        AbsoluteLayout.SetLayoutFlags(leavesEllipse, AbsoluteLayoutFlags.PositionProportional);

        // Loovuspunkt: kaks interaktiivset õuna lehestiku peal
        apple1 = LooOun(0.35, 0.30);
        apple2 = LooOun(0.65, 0.35);

        // Lisame osad puu konteinerisse
        treeContainer.Children.Add(trunkBox);
        treeContainer.Children.Add(leavesEllipse);
        treeContainer.Children.Add(apple1);
        treeContainer.Children.Add(apple2);

        // Info- ja veateadete silt
        statusLabel = new Label
        {
            Text = "Vali tegevus ja vajuta Käivita!",
            FontSize = 17,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.Black,
            HorizontalTextAlignment = TextAlignment.Center
        };

        // 2. JUHTPANEEL
        actionPicker = new Picker
        {
            Title = "Vali tegevus...",
            ItemsSource = new List<string> { "Kasva", "Õitse", "Värise", "Langeta" },
            WidthRequest = 170,
            TextColor = Colors.Black
        };

        Button kaivitaBtn = new Button
        {
            Text = "Käivita",
            BackgroundColor = Colors.ForestGreen,
            TextColor = Colors.White,
            FontAttributes = FontAttributes.Bold,
            CornerRadius = 10
        };
        kaivitaBtn.Clicked += OnActionClicked;

        HorizontalStackLayout tegevusRida = new HorizontalStackLayout
        {
            Spacing = 10,
            HorizontalOptions = LayoutOptions.Center,
            Children = { actionPicker, kaivitaBtn }
        };

        // Slider: lehestiku läbipaistvus (sügis)
        opacityLabel = new Label
        {
            Text = "Lehestiku tihedus (Opacity): 100%",
            FontSize = 13,
            TextColor = Colors.Black
        };

        opacitySlider = new Slider
        {
            Minimum = 0.0,
            Maximum = 1.0,
            Value = 1.0
        };
        opacitySlider.ValueChanged += (s, e) =>
        {
            leavesEllipse.Opacity = e.NewValue;
            apple1.Opacity = e.NewValue;
            apple2.Opacity = e.NewValue;
            opacityLabel.Text = $"Lehestiku tihedus (Opacity): {e.NewValue * 100:F0}%";
        };

        // Stepper: animatsiooni kiirus millisekundites
        speedLabel = new Label
        {
            Text = "Kiirus: 800 ms",
            FontSize = 14,
            TextColor = Colors.Black,
            VerticalOptions = LayoutOptions.Center
        };

        speedStepper = new Stepper
        {
            Minimum = 500,
            Maximum = 2000,
            Increment = 250,
            Value = 800
        };
        speedStepper.ValueChanged += (s, e) =>
        {
            speedLabel.Text = $"Kiirus: {e.NewValue:F0} ms";
        };

        HorizontalStackLayout kiirusRida = new HorizontalStackLayout
        {
            Spacing = 15,
            HorizontalOptions = LayoutOptions.Center,
            Children = { speedLabel, speedStepper }
        };

        // DatePicker (aastaaeg) ja TimePicker (ööpäevaaeg)
        seasonDatePicker = new DatePicker
        {
            Date = DateTime.Today,
            Format = "dd.MM.yyyy",
            TextColor = Colors.Black
        };
        seasonDatePicker.DateSelected += (s, e) =>
        {
            if (e.NewDate is DateTime uusKuupaev)
                UuendaTaevast(uusKuupaev.Month);
        };

        dayTimePicker = new TimePicker
        {
            Time = new TimeSpan(12, 0, 0), // Vaikimisi 12:00
            Format = "HH:mm",
            TextColor = Colors.Black
        };

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
                        new Label { Text = "Kuupäev (aastaaeg):", FontSize = 12, TextColor = Colors.Gray },
                        seasonDatePicker
                    }
                },
                new VerticalStackLayout
                {
                    Children =
                    {
                        new Label { Text = "Kellaaeg:", FontSize = 12, TextColor = Colors.Gray },
                        dayTimePicker
                    }
                }
            }
        };

        // Valge kaart juhtpaneeli ümber
        Border juhtpaneelKaart = new Border
        {
            BackgroundColor = Colors.White,
            StrokeShape = new RoundRectangle { CornerRadius = 15 },
            Padding = 15,
            Opacity = 0.92,
            Content = new VerticalStackLayout
            {
                Spacing = 12,
                Children = { tegevusRida, opacityLabel, opacitySlider, kiirusRida, aegRida }
            }
        };

        // Kogu lehe paigutus
        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Padding = 15,
                Spacing = 12,
                Children = { treeContainer, statusLabel, juhtpaneelKaart }
            }
        };

        // Määrame algse taeva värvi vastavalt praegusele kuule
        UuendaTaevast(DateTime.Today.Month);
    }

    // Abimeetod interaktiivse õuna loomiseks
    Ellipse LooOun(double x, double y)
    {
        Ellipse oun = new Ellipse
        {
            Fill = new SolidColorBrush(Colors.Red),
            WidthRequest = 20,
            HeightRequest = 20
        };
        AbsoluteLayout.SetLayoutBounds(oun, new Rect(x, y, 20, 20));
        AbsoluteLayout.SetLayoutFlags(oun, AbsoluteLayoutFlags.PositionProportional);

        TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += async (s, e) =>
            {
                statusLabel.Text = "Õun kukkus puu otsast alla!";
                await oun.TranslateToAsync(0, 160, 500);
                oun.IsVisible = false;
            };
            oun.GestureRecognizers.Add(tap);

        return oun;
    }

    // Käivitub "Käivita" nupule vajutamisel
    private async void OnActionClicked(object? sender, EventArgs e)
    {
        if (actionPicker.SelectedItem == null)
        {
            statusLabel.Text = "Palun vali esmalt rippmenüüst tegevus!";
            return;
        }

        string valik = actionPicker.SelectedItem.ToString()!;
        uint kiirus = (uint)speedStepper.Value; // Võtame Stepperist kiiruse millisekundites

        switch (valik)
        {
            case "Kasva":
                // Loovuspunkt: juhuslik kasvamine vahemikus 1.15 kuni 1.45
                double juhuslikSuurus = 1.15 + rnd.NextDouble() * 0.30;
                statusLabel.Text = $"Puu kasvab ({juhuslikSuurus:F2}x)!";
                await treeContainer.ScaleToAsync(juhuslikSuurus, kiirus);
                break;

            case "Õitse":
                // Lehestik muutub roosaks ja õunad tulevad nähtavale
                leavesEllipse.Fill = new SolidColorBrush(Colors.HotPink);
                apple1.IsVisible = true;
                apple1.TranslationY = 0;
                apple2.IsVisible = true;
                apple2.TranslationY = 0;
                statusLabel.Text = "Puu õitseb kaunilt!";
                break;

            case "Värise":
                // Puu väriseb tuules vasakule ja paremale
                statusLabel.Text = "Tuul sahistab puuokstes...";
                uint kiireSamm = kiirus / 4;
                await treeContainer.TranslateToAsync(-15, 0, kiireSamm);
                await treeContainer.TranslateToAsync(15, 0, kiireSamm);
                await treeContainer.TranslateToAsync(-10, 0, kiireSamm);
                await treeContainer.TranslateToAsync(0, 0, kiireSamm);
                statusLabel.Text = "Tuul vaibus.";
                break;

            case "Langeta":
                // Kontrollime ranget tingimust: ainult talvel (12, 1, 2) ja valgel ajal (08:00 - 17:00)
                if (seasonDatePicker.Date is not DateTime valitudKuupaev ||
                    dayTimePicker.Time is not TimeSpan valitudKellaaeg)
                {
                    statusLabel.Text = "Vali korrektne kuupäev ja kellaaeg!";
                    return;
                }

                int kuu = valitudKuupaev.Month;
                int tund = valitudKellaaeg.Hours;

                bool onTalv = (kuu == 12 || kuu == 1 || kuu == 2);
                bool onValgeAeg = (tund >= 8 && tund <= 17);

                if (!onTalv || !onValgeAeg)
                {
                    statusLabel.Text = "Pimedas ja suvel puid ei langetata! (Vali talv ja 08:00–17:00)";
                    return; // Katkestame tegevuse, puu ei kuku
                }

                // Kui tingimused on täidetud, kukub puu külili (90 kraadi)
                statusLabel.Text = "Puu langetatud!";
                await treeContainer.RotateToAsync(90, kiirus);
                break;
        }
    }

    // Muudab lehe taustavärvi vastavalt aastaajale
    private void UuendaTaevast(int kuu)
    {
        if (kuu == 12 || kuu == 1 || kuu == 2)
        {
            BackgroundColor = Colors.LightGray; // Talv: hall taevas
            leavesEllipse.Fill = new SolidColorBrush(Colors.WhiteSmoke);
        }
        else if (kuu >= 3 && kuu <= 5)
        {
            BackgroundColor = Color.FromArgb("#C8F7C5"); // Kevad: heleroheline
            leavesEllipse.Fill = new SolidColorBrush(Colors.LimeGreen);
        }
        else if (kuu >= 6 && kuu <= 8)
        {
            BackgroundColor = Color.FromArgb("#87CEEB"); // Suvi: helesinine
            leavesEllipse.Fill = new SolidColorBrush(Colors.ForestGreen);
        }
        else
        {
            BackgroundColor = Color.FromArgb("#FFD8A8"); // Sügis: oranžikas
            leavesEllipse.Fill = new SolidColorBrush(Colors.DarkOrange);
        }
    }
}

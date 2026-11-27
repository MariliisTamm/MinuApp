using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace MinuApp;

public class LumememmPage : ContentPage
{
    // Lumememme osad
    AbsoluteLayout lumememmKonteiner; // Hoiab kõiki lumememme osi üheskoos, et neid oleks lihtne korraga liigutada/peita
    BoxView amber;
    Ellipse pea;
    Ellipse keha;
    Ellipse vasakSilm, paremSilm, noopp1, noopp2;

    // Juhtelemendid
    Picker tegevusPicker;
    Label tegevusLabel;
    Slider labipaistvusSlider;
    Stepper kiirusStepper;
    Label kiirusLabel;

    Random rnd = new Random();

    public LumememmPage()
    {
        Title = "Lumememm";
        BackgroundColor = Color.FromArgb("#E6F2FF"); // Hele talvine taevasinine taust

        // Peamine paigutus kogu ekraanile
        AbsoluteLayout abs = new AbsoluteLayout();

        // 1. LUMEMEMME KONTEINER (AbsoluteLayout lumememme enda joonistamiseks)
        lumememmKonteiner = new AbsoluteLayout();
        AbsoluteLayout.SetLayoutBounds(lumememmKonteiner, new Rect(0.5, 0.05, 220, 310));
        AbsoluteLayout.SetLayoutFlags(lumememmKonteiner, AbsoluteLayoutFlags.PositionProportional);

        // Ämber (BoxView) - kõige ülemine osa
        amber = new BoxView
        {
            Color = Colors.DarkRed,
            CornerRadius = 5
        };
        AbsoluteLayout.SetLayoutBounds(amber, new Rect(0.5, 10, 60, 45));
        AbsoluteLayout.SetLayoutFlags(amber, AbsoluteLayoutFlags.XProportional);

        // Pea (Ellipse) - ämbri all
        pea = new Ellipse
        {
            Fill = new SolidColorBrush(Colors.White),
            Stroke = Colors.LightGray,
            StrokeThickness = 2
        };
        AbsoluteLayout.SetLayoutBounds(pea, new Rect(0.5, 50, 90, 90));
        AbsoluteLayout.SetLayoutFlags(pea, AbsoluteLayoutFlags.XProportional);

        // Keha (Ellipse) - pea all, suurem ring
        keha = new Ellipse
        {
            Fill = new SolidColorBrush(Colors.White),
            Stroke = Colors.LightGray,
            StrokeThickness = 2
        };
        AbsoluteLayout.SetLayoutBounds(keha, new Rect(0.5, 130, 150, 150));
        AbsoluteLayout.SetLayoutFlags(keha, AbsoluteLayoutFlags.XProportional);

        // Loovuse lisapunkt: Silmad ja nööbid
        vasakSilm = LooTapp(Colors.Black, 10);
        AbsoluteLayout.SetLayoutBounds(vasakSilm, new Rect(90, 80, 10, 10));

        paremSilm = LooTapp(Colors.Black, 10);
        AbsoluteLayout.SetLayoutBounds(paremSilm, new Rect(120, 80, 10, 10));

        noopp1 = LooTapp(Colors.Black, 12);
        AbsoluteLayout.SetLayoutBounds(noopp1, new Rect(0.5, 175, 12, 12));
        AbsoluteLayout.SetLayoutFlags(noopp1, AbsoluteLayoutFlags.XProportional);

        noopp2 = LooTapp(Colors.Black, 12);
        AbsoluteLayout.SetLayoutBounds(noopp2, new Rect(0.5, 210, 12, 12));
        AbsoluteLayout.SetLayoutFlags(noopp2, AbsoluteLayoutFlags.XProportional);

        // Lisame kõik osad lumememme konteinerisse (järjekord määrab, mis jääb peale)
        lumememmKonteiner.Children.Add(keha);
        lumememmKonteiner.Children.Add(pea);
        lumememmKonteiner.Children.Add(amber);
        lumememmKonteiner.Children.Add(vasakSilm);
        lumememmKonteiner.Children.Add(paremSilm);
        lumememmKonteiner.Children.Add(noopp1);
        lumememmKonteiner.Children.Add(noopp2);

        abs.Add(lumememmKonteiner);

        // 2. SILT (Label), mis näitab valitud tegevuse nime
        tegevusLabel = new Label
        {
            Text = "Vali tegevus ja vajuta nuppu",
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        };
        AbsoluteLayout.SetLayoutBounds(tegevusLabel, new Rect(0.5, 0.54, 0.9, AbsoluteLayout.AutoSize));
        AbsoluteLayout.SetLayoutFlags(tegevusLabel, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);
        abs.Add(tegevusLabel);

        // 3. PICKER tegevuse valimiseks
        tegevusPicker = new Picker
        {
            Title = "Vali tegevus...",
            ItemsSource = new List<string>
            {
                "Peida lumememm",
                "Näita lumememm",
                "Muuda värvi",
                "Sulata",
                "Tantsi"
            },
            WidthRequest = 250,
            BackgroundColor = Colors.White
        };
        // Kui Pickeris valikut muudetakse, uuendame kohe sildi teksti
        tegevusPicker.SelectedIndexChanged += (s, e) =>
        {
            if (tegevusPicker.SelectedItem != null)
                tegevusLabel.Text = $"Valitud: {tegevusPicker.SelectedItem}";
        };
        AbsoluteLayout.SetLayoutBounds(tegevusPicker, new Rect(0.5, 0.62, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        AbsoluteLayout.SetLayoutFlags(tegevusPicker, AbsoluteLayoutFlags.PositionProportional);
        abs.Add(tegevusPicker);

        // 4. NUPP (Button), mis käivitab valitud tegevuse
        Button kaivitaBtn = new Button
        {
            Text = "Käivita tegevus",
            FontSize = 18,
            BackgroundColor = Colors.DodgerBlue,
            TextColor = Colors.White,
            CornerRadius = 10,
            WidthRequest = 200
        };
        kaivitaBtn.Clicked += KaivitaBtn_Clicked;
        AbsoluteLayout.SetLayoutBounds(kaivitaBtn, new Rect(0.5, 0.71, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        AbsoluteLayout.SetLayoutFlags(kaivitaBtn, AbsoluteLayoutFlags.PositionProportional);
        abs.Add(kaivitaBtn);

        // 5. SLIDER (läbipaistvuse ehk Opacity muutmiseks 0.0 – 1.0)
        Label sliderPealkiri = new Label { Text = "Läbipaistvus:", FontSize = 14 };
        AbsoluteLayout.SetLayoutBounds(sliderPealkiri, new Rect(0.1, 0.80, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        AbsoluteLayout.SetLayoutFlags(sliderPealkiri, AbsoluteLayoutFlags.PositionProportional);
        abs.Add(sliderPealkiri);

        labipaistvusSlider = new Slider
        {
            Minimum = 0.0,
            Maximum = 1.0,
            Value = 1.0, // Alguses 100% nähtav
            WidthRequest = 200
        };
        labipaistvusSlider.ValueChanged += (s, e) =>
        {
            lumememmKonteiner.Opacity = e.NewValue;
        };
        AbsoluteLayout.SetLayoutBounds(labipaistvusSlider, new Rect(0.85, 0.80, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        AbsoluteLayout.SetLayoutFlags(labipaistvusSlider, AbsoluteLayoutFlags.PositionProportional);
        abs.Add(labipaistvusSlider);

        // 6. STEPPER (animatsiooni kiiruse muutmiseks millisekundites)
        kiirusLabel = new Label { Text = "Kiirus: 500 ms", FontSize = 14 };
        AbsoluteLayout.SetLayoutBounds(kiirusLabel, new Rect(0.1, 0.89, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        AbsoluteLayout.SetLayoutFlags(kiirusLabel, AbsoluteLayoutFlags.PositionProportional);
        abs.Add(kiirusLabel);

        kiirusStepper = new Stepper
        {
            Minimum = 100,    // Väga kiire (0.1 sek)
            Maximum = 2000,   // Aeglane (2 sek)
            Increment = 100,  // Samm 100 ms kaupa
            Value = 500       // Vaikimisi 500 ms
        };
        kiirusStepper.ValueChanged += (s, e) =>
        {
            kiirusLabel.Text = $"Kiirus: {e.NewValue:F0} ms";
        };
        AbsoluteLayout.SetLayoutBounds(kiirusStepper, new Rect(0.85, 0.89, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        AbsoluteLayout.SetLayoutFlags(kiirusStepper, AbsoluteLayoutFlags.PositionProportional);
        abs.Add(kiirusStepper);

        Content = abs;
    }

    // Abimeetod silmade ja nööpide loomiseks
    Ellipse LooTapp(Color varv, double suurus)
    {
        return new Ellipse
        {
            Fill = new SolidColorBrush(varv),
            WidthRequest = suurus,
            HeightRequest = suurus
        };
    }

    // Tegevuste käivitamine vastavalt Pickeri valikule
    private async void KaivitaBtn_Clicked(object? sender, EventArgs e)
    {
        if (tegevusPicker.SelectedItem == null)
        {
            await DisplayAlertAsync("Tähelepanu", "Vali esmalt rippmenüüst tegevus!", "OK");
            return;
        }

        string valik = tegevusPicker.SelectedItem.ToString()!;
        tegevusLabel.Text = $"Tegevus: {valik}";
        uint kiirus = (uint)kiirusStepper.Value; // Võtame Stepperist kiiruse millisekundites

        switch (valik)
        {
            case "Peida lumememm":
                lumememmKonteiner.IsVisible = false;
                break;

            case "Näita lumememm":
                lumememmKonteiner.IsVisible = true;
                lumememmKonteiner.Opacity = 1.0;
                lumememmKonteiner.Scale = 1.0; // Taastame suuruse, kui ta oli varem sulanud
                labipaistvusSlider.Value = 1.0;
                break;

            case "Muuda värvi":
                // Küsime kasutajalt kinnitust enne värvi muutmist
                bool kinnitus = await DisplayAlertAsync("Kinnitus", "Kas soovid lumememme värvi muuta?", "Jah", "Ei");
                if (kinnitus)
                {
                    Color juhuslikVarv = Color.FromRgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
                    pea.Fill = new SolidColorBrush(juhuslikVarv);
                    keha.Fill = new SolidColorBrush(juhuslikVarv);
                    amber.Color = Color.FromRgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
                }
                break;

            case "Sulata":
                // Lumememm hajub ja kahaneb järk-järgult vastavalt Stepperi kiirusele
                await Task.WhenAll(
                    lumememmKonteiner.FadeToAsync(0, kiirus * 2),
                    lumememmKonteiner.ScaleToAsync(0.1, kiirus * 2)
                );
                labipaistvusSlider.Value = 0;
                break;

            case "Tantsi":
                // Loovuse lisapunkt: TextToSpeech tervitus
                _ = TextToSpeech.Default.SpeakAsync("Jõulud tulevad!");

                // Lumememm liigub vasakule ja paremale (TranslateToAsync)
                await lumememmKonteiner.TranslateToAsync(-40, 0, kiirus);
                await lumememmKonteiner.TranslateToAsync(40, 0, kiirus);
                await lumememmKonteiner.TranslateToAsync(-20, 0, kiirus);
                await lumememmKonteiner.TranslateToAsync(0, 0, kiirus); // Tagasi keskele
                break;
        }
    }
}
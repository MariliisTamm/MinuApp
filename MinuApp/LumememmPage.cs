using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace MinuApp;

// 1. LUMEMEMME LEHE KLASS
public class LumememmPage : ContentPage
{
    // 2. LUMEMEMME OSAD
    AbsoluteLayout lumememmKonteiner; // Hoiab kõiki lumememme osi üheskoos, et neid oleks lihtne korraga liigutada/peita
    BoxView amber;
    Ellipse pea;
    Ellipse keha;
    Ellipse vasakSilm;
    Ellipse paremSilm;

    Ellipse nina;
    Ellipse suu;

    Ellipse noopp1;
    Ellipse noopp2;
    Ellipse noopp3;

    // 3. JUHTELEMENDID
    Picker tegevusPicker;
    Label tegevusLabel;
    Slider labipaistvusSlider;
    Stepper kiirusStepper;
    Label kiirusLabel;

    Random rnd = new Random();

    // 4. KONSTRUKTOR
    // Siin luuakse kogu ekraani sisu, nii lumememm kui ka juhtpaneel
    public LumememmPage()
    {
        Title = "Lumememm";
        BackgroundColor = Color.FromArgb("#E6F2FF"); // Hele talvine taevasinine taust

        // Peamine paigutus kogu ekraanile
        AbsoluteLayout abs = new AbsoluteLayout();

        // 5. LUMEMEMME KONTEINER
        // Kõik lumememme odan on ühes konteineris
        lumememmKonteiner = new AbsoluteLayout();
        AbsoluteLayout.SetLayoutBounds(
            lumememmKonteiner,
            new Rect(0.5, 0.05, 220, 310));

        AbsoluteLayout.SetLayoutFlags(
            lumememmKonteiner,
            AbsoluteLayoutFlags.PositionProportional);

        // 6. LUMEMEMME KEHA
        // Pea all, suurem ring
        keha = new Ellipse
        {
            Fill = new SolidColorBrush(Colors.White),
            Stroke = Colors.LightGray,
            StrokeThickness = 2
        };
        AbsoluteLayout.SetLayoutBounds(
            keha, new Rect(0.5, 130, 150, 150));
        AbsoluteLayout.SetLayoutFlags(
            keha, AbsoluteLayoutFlags.XProportional);

        // 7. LUMEMEMME PEA
        // Ämbri all
        pea = new Ellipse
        {
            Fill = new SolidColorBrush(Colors.White),
            Stroke = Colors.LightGray,
            StrokeThickness = 2
        };
        AbsoluteLayout.SetLayoutBounds(
            pea, new Rect(0.5, 50, 90, 90));
        AbsoluteLayout.SetLayoutFlags(
            pea, AbsoluteLayoutFlags.XProportional);

        // 8. LUMEMEMME ÄMBER
        // Ämber (BoxView) - kõige ülemine osa
        amber = new BoxView
        {
            Color = Colors.DarkRed,
            CornerRadius = 5
        };
        AbsoluteLayout.SetLayoutBounds(
            amber, new Rect(0.5, 10, 60, 45));
        AbsoluteLayout.SetLayoutFlags(
            amber, AbsoluteLayoutFlags.XProportional);

        // 9. SILMAD
        // Loovuse lisapunkt: Silmad ja nööbid
        vasakSilm = LooTapp(Colors.Black, 10);
        AbsoluteLayout.SetLayoutBounds(
            vasakSilm, new Rect(90, 80, 10, 10));

        paremSilm = LooTapp(Colors.Black, 10);
        AbsoluteLayout.SetLayoutBounds(
            paremSilm, new Rect(120, 80, 10, 10));

        // 10. NINA
        nina = LooTapp(Colors.Orange, 12);
        AbsoluteLayout.SetLayoutBounds(
            nina, new Rect(104, 95, 12, 12));

        // 11. SUU
        suu = LooTapp(Colors.Red, 8);
        AbsoluteLayout.SetLayoutBounds(
            suu, new Rect(110, 115, 8, 8));

        // 12. NÖÖBID
        noopp1 = LooTapp(Colors.Black, 12);

        AbsoluteLayout.SetLayoutBounds(
            noopp1, new Rect(0.5, 175, 12, 12));

        AbsoluteLayout.SetLayoutFlags(
            noopp1, AbsoluteLayoutFlags.XProportional);

        noopp2 = LooTapp(Colors.Black, 12);

        AbsoluteLayout.SetLayoutBounds(
            noopp2, new Rect(0.5, 210, 12, 12));

        AbsoluteLayout.SetLayoutFlags(
            noopp2, AbsoluteLayoutFlags.XProportional);

        noopp3 = LooTapp(Colors.Black, 12);

        AbsoluteLayout.SetLayoutBounds(
            noopp3, new Rect(0.5, 245, 12, 12));

        AbsoluteLayout.SetLayoutFlags(
            noopp3, AbsoluteLayoutFlags.XProportional);

        // 13. LISAME KÕIK OSAD LUMEMEMME KONTEINERISSE
        lumememmKonteiner.Children.Add(keha);
        lumememmKonteiner.Children.Add(pea);
        lumememmKonteiner.Children.Add(amber);
        lumememmKonteiner.Children.Add(vasakSilm);
        lumememmKonteiner.Children.Add(paremSilm);
        lumememmKonteiner.Children.Add(nina);
        lumememmKonteiner.Children.Add(suu);
        lumememmKonteiner.Children.Add(noopp1);
        lumememmKonteiner.Children.Add(noopp2);
        lumememmKonteiner.Children.Add(noopp3);

        abs.Add(lumememmKonteiner);

        // 14. TEGEVUSE TEKST (Label), mis näitab, milline tegevus on valitud
        tegevusLabel = new Label
        {
            Text = "Vali tegevus ja vajuta nuppu",
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        };
        AbsoluteLayout.SetLayoutBounds(
            tegevusLabel, new Rect(0.5, 0.54, 0.9, AbsoluteLayout.AutoSize));
        AbsoluteLayout.SetLayoutFlags(
            tegevusLabel, AbsoluteLayoutFlags.PositionProportional |
            AbsoluteLayoutFlags.WidthProportional);

        abs.Add(tegevusLabel);

        // 15. PICKER tegevuse valimiseks
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
            {
                tegevusLabel.Text =
                    $"Valitud: {tegevusPicker.SelectedItem}";
            }
        };
        AbsoluteLayout.SetLayoutBounds(
            tegevusPicker, 
            new Rect(0.5, 0.62, 
            AbsoluteLayout.AutoSize, 
            AbsoluteLayout.AutoSize));

        AbsoluteLayout.SetLayoutFlags(
            tegevusPicker, 
            AbsoluteLayoutFlags.PositionProportional);

        abs.Add(tegevusPicker);

        // 16. NUPP (Button), mis käivitab valitud tegevuse
        Button kaivitaBtn = new Button
        {
            Text = "Käivita tegevus",
            FontSize = 18,
            BackgroundColor = Colors.DodgerBlue,
            TextColor = Colors.White,
            CornerRadius = 12,
            WidthRequest = 200
        };
        kaivitaBtn.Clicked += KaivitaBtn_Clicked;

        AbsoluteLayout.SetLayoutBounds(
            kaivitaBtn, 
            new Rect(0.5, 0.71, 
            AbsoluteLayout.AutoSize, 
            AbsoluteLayout.AutoSize));
        AbsoluteLayout.SetLayoutFlags(
            kaivitaBtn, 
            AbsoluteLayoutFlags.PositionProportional);

        abs.Add(kaivitaBtn);

        // 17 SLIDER (läbipaistvuse ehk Opacity muutmiseks 0.0 – 1.0)
        Label sliderPealkiri = new Label 
        { 
            Text = "Läbipaistvus:", 
            FontSize = 14,
            TextColor = Colors.DarkBlue
        };
        AbsoluteLayout.SetLayoutBounds(
            sliderPealkiri, 
            new Rect(0.1, 0.80, 
            AbsoluteLayout.AutoSize, 
            AbsoluteLayout.AutoSize));

        AbsoluteLayout.SetLayoutFlags(
            sliderPealkiri, 
            AbsoluteLayoutFlags.PositionProportional);

        abs.Add(sliderPealkiri);

        labipaistvusSlider = new Slider
        {
            Minimum = 0,
            Maximum = 1,
            Value = 1, // Alguses 100% nähtav
            WidthRequest = 200
        };

        labipaistvusSlider.ValueChanged += (s, e) =>
        {
            lumememmKonteiner.Opacity = e.NewValue;
        };

        AbsoluteLayout.SetLayoutBounds(
            labipaistvusSlider, 
            new Rect(0.85, 0.80, 
            AbsoluteLayout.AutoSize, 
            AbsoluteLayout.AutoSize));

        AbsoluteLayout.SetLayoutFlags(
            labipaistvusSlider, 
            AbsoluteLayoutFlags.PositionProportional);

        abs.Add(labipaistvusSlider);

        // 18. STEPPER (animatsiooni kiiruse muutmiseks millisekundites)
        kiirusLabel = new Label 
        { 
            Text = "Kiirus: 500 ms", 
            FontSize = 14,
            TextColor = Colors.DarkBlue
        };
        AbsoluteLayout.SetLayoutBounds(
            kiirusLabel, 
            new Rect(0.1, 0.89, 
            AbsoluteLayout.AutoSize,
            AbsoluteLayout.AutoSize));

        AbsoluteLayout.SetLayoutFlags(
            kiirusLabel, 
            AbsoluteLayoutFlags.PositionProportional);

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
            kiirusLabel.Text = 
                $"Kiirus: {e.NewValue:F0} ms";
        };
        AbsoluteLayout.SetLayoutBounds(
            kiirusStepper, 
            new Rect(0.85, 0.89, 
            AbsoluteLayout.AutoSize, 
            AbsoluteLayout.AutoSize));

        AbsoluteLayout.SetLayoutFlags(
            kiirusStepper, 
            AbsoluteLayoutFlags.PositionProportional);

        abs.Add(kiirusStepper);

        // 19. LEHE SISU
        Content = abs;
    }

    // 20. ABIMEETOD - TÄPI LOOMINE
    // Kasutame seda silmade, nina ja nööpide tegemiseks
    Ellipse LooTapp(Color varv, double suurus)
    {
        return new Ellipse
        {
            Fill = new SolidColorBrush(varv),
            WidthRequest = suurus,
            HeightRequest = suurus
        };
    }

    // 21. TEGEVUSTE KÄIVITAMINE
    // Siin määrame, mida iga valik teeb
    private async void KaivitaBtn_Clicked(
        object? sender, EventArgs e)
    {
        // Kontrollime, kas kasutaja valis tegevuse
        if (tegevusPicker.SelectedItem == null)
        {
            await DisplayAlertAsync(
                "Tähelepanu", 
                "Vali esmalt rippmenüüst tegevus!",
                "OK");
            return;
        }

        string valik = tegevusPicker.SelectedItem.ToString()!;

        tegevusLabel.Text = $"Tegevus: {valik}";

        uint kiirus = (uint)kiirusStepper.Value; // Võtame Stepperist kiiruse millisekundites

        // 22. SWITCH - TEGEVUSTE VALIK

        switch (valik)
        {
            // PEIDA LUMEMEMM
            case "Peida lumememm":
                lumememmKonteiner.IsVisible = false;
                break;

            // NÄITA LUMEMEMM
            case "Näita lumememm":
                lumememmKonteiner.IsVisible = true;
                lumememmKonteiner.Opacity = 1;
                lumememmKonteiner.Scale = 1; // Taastame suuruse, kui ta oli varem sulanud
                lumememmKonteiner.TranslationX = 0;
                lumememmKonteiner.TranslationY = 0;
                labipaistvusSlider.Value = 1;
                break;

            // MUUDA VÄRVI
            case "Muuda värvi":
                // Küsime kasutajalt kinnitust enne värvi muutmist
                bool kinnitus = await DisplayAlertAsync(
                    "Kinnitus", 
                    "Kas soovid lumememme värvi muuta?", 
                    "Jah", 
                    "Ei");
                if (kinnitus)
                {
                    Color uusVarv = Color.FromRgb(
                        rnd.Next(0, 256),
                        rnd.Next(0, 256),
                        rnd.Next(0, 256));

                    pea.Fill = new SolidColorBrush(uusVarv);
                    keha.Fill = new SolidColorBrush(uusVarv);

                    amber.Color = Color.FromRgb(
                        rnd.Next(0, 256),
                        rnd.Next(0, 256),
                        rnd.Next(0, 256));
                }
                break;

            // SULATA
            case "Sulata":
                // Lumememm hajub ja kahaneb järk-järgult vastavalt Stepperi kiirusele
                await Task.WhenAll(
                    lumememmKonteiner.FadeToAsync(0, kiirus * 2),
                    lumememmKonteiner.ScaleToAsync(0.1, kiirus * 2)
                );
                labipaistvusSlider.Value = 0;
                break;

            // TANTSI 
            case "Tantsi":
                //Lumememm tervitus
                _ = TextToSpeech.Default.SpeakAsync(
                    "Jõulud tulevad!");

                // Lumememm liigub vasakule ja paremale
                await lumememmKonteiner.TranslateToAsync(
                    -40, 0, kiirus);
                await lumememmKonteiner.TranslateToAsync(
                    40, 0, kiirus);
                await lumememmKonteiner.TranslateToAsync(
                    -20, 0, kiirus);
                await lumememmKonteiner.TranslateToAsync(
                    0, 0, kiirus); // Tagasi keskele
                break;
        }
    }
}
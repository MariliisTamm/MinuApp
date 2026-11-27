namespace MinuApp;

public class ValgusfoorPage : ContentPage
{
    // Klassi muutujad, mida kasutame üle terve lehe
    Label statusLabel;
    BoxView redBox, yellowBox, greenBox;
    bool onSees = false; // Jätab meeles, kas foor on käsitsi sisse lülitatud

    // Taimer ja loendur, mis peab järge (0 = punane, 1 = kollane, 2 = roheline)
    IDispatcherTimer autoTaimer;
    int samm = 0;

    public ValgusfoorPage()
    {
        Title = "Valgusfoor"; // Lehe pealkiri ülemisel ribal

        // Pealkiri / olekusilt, mis annab kasutajale juhiseid
        statusLabel = new Label
        {
            Text = "Vali valgus",
            FontSize = 26,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center
        };

        // Loome kolm valgusfoori tuld abimeetodiga LooTuli()
        redBox = LooTuli();
        yellowBox = LooTuli();
        greenBox = LooTuli();

        // Lisame igale tulele puudutuse kuulaja ja vastava teksti
        LisaKlops(redBox, "STOP");
        LisaKlops(yellowBox, "Ettevaatust");
        LisaKlops(greenBox, "Sõida");

        // Foori tume korpus: paigutab kolm tuld üksteise alla musta kasti sisse
        VerticalStackLayout fooriKorpus = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 15,
            BackgroundColor = Colors.Black,
            HorizontalOptions = LayoutOptions.Center,
            Children = { redBox, yellowBox, greenBox }
        };

        // Seadistame taimeri, mis tiksub iga 2 sekundi järel
        autoTaimer = Dispatcher.CreateTimer();
        autoTaimer.Interval = TimeSpan.FromSeconds(2);
        autoTaimer.Tick += (s, e) => VahetaTuldAutomaatselt(); // Iga 2 sekundi tagant käivitab meetodi

        // Nupp "Sisse" – lülitab foori käsitsi tööle
        Button sisseBtn = new Button
        {
            Text = "Sisse",
            FontSize = 18,
            BackgroundColor = Colors.Green,
            TextColor = Colors.White,
            CornerRadius = 10,
            WidthRequest = 105
        };
        sisseBtn.Clicked += (s, e) =>
        {
            autoTaimer.Stop();                // Peatame automaatrežiimi, kui see peaks töötama
            onSees = true;                    // Märgime oleku sisselülitatuks
            redBox.Color = Colors.Red;        // Kõik tuled põlema, et kasutaja saaks valida
            yellowBox.Color = Colors.Yellow;
            greenBox.Color = Colors.Green;
            statusLabel.Text = "Vali valgus";
        };

        // Nupp "Välja" – lülitab foori välja ja kustutab kõik tuled
        Button valjaBtn = new Button
        {
            Text = "Välja",
            FontSize = 18,
            BackgroundColor = Colors.DarkGray,
            TextColor = Colors.White,
            CornerRadius = 10,
            WidthRequest = 105
        };
        valjaBtn.Clicked += (s, e) =>
        {
            autoTaimer.Stop();                               // Peatame automaatrežiimi
            onSees = false;                                  // Märgime oleku väljalülitatuks
            redBox.Color = Colors.Gray;                      // Muudame kõik tuled halliks
            yellowBox.Color = Colors.Gray;
            greenBox.Color = Colors.Gray;
            statusLabel.Text = "Lülita esmalt foor sisse";
        };

        // Nupp, mis paneb tuled ise tsükliga vahetuma
        Button autoBtn = new Button
        {
            Text = "Auto",
            FontSize = 18,
            BackgroundColor = Colors.RoyalBlue,
            TextColor = Colors.White,
            CornerRadius = 10,
            WidthRequest = 105
        };
        autoBtn.Clicked += (s, e) =>
        {
            onSees = true;              // Lülitame foori sisse
            samm = 0;                   // Alustame tsüklit algusest (punasest tulest)
            VahetaTuldAutomaatselt();   // Näitame esimest tuld kohe (ilma 2 sek ootamata)
            autoTaimer.Start();         // Käivitame taimeri
        };

        // Paigutame kõik kolm nuppu ("Sisse", "Välja", "Auto") horisontaalselt kõrvuti
        HorizontalStackLayout nupudRida = new HorizontalStackLayout
        {
            Spacing = 10,
            HorizontalOptions = LayoutOptions.Center,
            Children = { sisseBtn, valjaBtn, autoBtn }
        };

        // Kogu lehe sisu
        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Padding = 20,
                Spacing = 20,
                Children =
                {
                    statusLabel,
                    fooriKorpus,
                    nupudRida,
                    StartPage.LooNavigatsioon(Navigation, -1) // Tagasi ja Avaleht nupud
                }
            }
        };
    }

    // Meetod, mida taimer kutsub välja iga 2 sekundi tagant
    void VahetaTuldAutomaatselt()
    {
        // Kustutame enne kõik tuled halliks, et korraga põleks ainult üks tuli
        redBox.Color = Colors.Gray;
        yellowBox.Color = Colors.Gray;
        greenBox.Color = Colors.Gray;

        // Kontrollime, mitmes samm on, ja paneme vastava tule põlema
        if (samm == 0)
        {
            redBox.Color = Colors.Red;
            statusLabel.Text = "STOP";
            samm = 1; // Järgmine kord tuleb kollane
        }
        else if (samm == 1)
        {
            yellowBox.Color = Colors.Yellow;
            statusLabel.Text = "Ettevaatust";
            samm = 2; // Järgmine kord tuleb roheline
        }
        else
        {
            greenBox.Color = Colors.Green;
            statusLabel.Text = "Sõida";
            samm = 0; // Järgmine kord algab tsükkel jälle punasest
        }
    }

    // Abimeetod ühe halli ringi loomiseks
    BoxView LooTuli()
    {
        return new BoxView
        {
            Color = Colors.Gray,
            WidthRequest = 100,
            HeightRequest = 100,
            CornerRadius = 50, // Pool laiusest/kõrgusest muudab ruudu ringiks
            HorizontalOptions = LayoutOptions.Center
        };
    }

    // Abimeetod tulele puudutuse ja animatsiooni lisamiseks
    void LisaKlops(BoxView tuli, string sonum)
    {
        TapGestureRecognizer tap = new TapGestureRecognizer();
        tap.Tapped += async (s, e) =>
        {
            if (!onSees)
            {
                statusLabel.Text = "Lülita esmalt foor sisse";
                return;
            }

            // Kui kasutaja vajutab käsitsi tuld, peatame automaatrežiimi
            autoTaimer.Stop();
            statusLabel.Text = sonum;

            // Suurendamise ja tuhmistamise animatsioon
            await Task.WhenAll(
                tuli.ScaleToAsync(1.2, 150),
                tuli.FadeToAsync(0.5, 150)
            );
            await Task.WhenAll(
                tuli.ScaleToAsync(1.0, 150),
                tuli.FadeToAsync(1.0, 150)
            );
        };
        tuli.GestureRecognizers.Add(tap);
    }

    // Kui kasutaja lahkub lehelt, peatame taimeri, et see taustal ei töötaks
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        autoTaimer.Stop();
    }
}
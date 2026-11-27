namespace MinuApp;

public class StartPage : ContentPage
{
    List<string> lehenimed = new List<string> 
    { 
        "Tekst", 
        "Kujundid", 
        "Kell", 
        "Valgusfoor", 
        "AbsoluteLayout",
        "Valikud",
        "Stepper ja Slider",
        "Lumememm",
        "Puu elutsükkel",
        "Jõulukuusk"
    };

    public StartPage()
    {
        Title = "Avaleht";

        VerticalStackLayout vsl = new VerticalStackLayout { Padding = 20, Spacing = 15 };

        for (int i = 0; i < lehenimed.Count; i++)
        {
            int nr = i;   // NB! iga nupp saab oma koopia i väärtusest (vt teooria)

            Button nupp = new Button
            {
                Text = lehenimed[i],
                FontSize = 28,
                BackgroundColor = Colors.LightGray,
                TextColor = Colors.Black,
                CornerRadius = 10,
                HeightRequest = 60
            };
            nupp.Clicked += async (sender, e) => await Navigation.PushAsync(LooLeht(nr));
            vsl.Add(nupp);
        }

        Content = new ScrollView { Content = vsl };
    }

    // Leht luuakse alles siis, kui nuppu vajutatakse (mitte kõik korraga äpi käivitamisel)
    public static ContentPage LooLeht(int nr)
    {
        switch (nr)
        {
            case 0: return new TextPage();
            case 1: return new FigurePage();
            case 2: return new TimerPage();
            case 3: return new ValgusfoorPage();
            case 4: return new AbsoluutPage();
            case 5: return new ValikudPage();
            case 6: return new LiuguridPage();
            case 7: return new LumememmPage();
            case 8: return new TreePage();
            default: return new KuuskPage();
        }
    }

    // Kolm nuppu, mida kasutavad kõik lehed: Tagasi, Avaleht, Edasi.
    // jargmine = järgmise lehe number; -1 tähendab, et Edasi-nuppu pole.
    public static HorizontalStackLayout LooNavigatsioon(INavigation nav, int jargmine)
    {
        HorizontalStackLayout hsl = new HorizontalStackLayout
        {
            Spacing = 10,
            HorizontalOptions = LayoutOptions.Center
        };

        List<string> nupud = new List<string> { "Tagasi", "Avaleht" };
        if (jargmine >= 0) nupud.Add("Edasi");

        foreach (string tekst in nupud)
        {
            Button nupp = new Button
            {
                Text = tekst,
                FontSize = 18,
                BackgroundColor = Colors.LightGray,
                TextColor = Colors.Black,
                CornerRadius = 10
            };

            // Üks sündmus kõigile nuppudele: sender ütleb, millist nuppu vajutati
            nupp.Clicked += async (sender, e) =>
            {
                Button vajutatud = (Button)sender!;
                switch (vajutatud.Text)
                {
                    case "Tagasi":
                        await nav.PopAsync();                       // eelmisele lehele
                        break;
                    case "Avaleht":
                        await nav.PopToRootAsync();                 // esimesele lehele
                        break;
                    case "Edasi":
                        await nav.PushAsync(LooLeht(jargmine));     // uus leht peale
                        break;
                }
            };
            hsl.Add(nupp);
        }
        return hsl;
    }
}
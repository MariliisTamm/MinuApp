namespace MinuApp;

public class TextPage : ContentPage
{
    Label pealkiri;
    Editor editor;
    Label info;

    public TextPage()
    {
        Title = "Tekst";

        pealkiri = new Label
        {
            Text = "Pealkiri",
            FontFamily = "RobotoRegular",
            FontSize = 32,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center
        };

        editor = new Editor
        {
            Placeholder = "Sisesta tekst...",
            PlaceholderColor = Colors.Red,
            FontSize = 18,
            AutoSize = EditorAutoSizeOption.TextChanges,   // kasvab koos tekstiga
            Keyboard = Keyboard.Text
        };
        // TextChanged käivitub iga tähe järel. e.NewTextValue = uus tekst.
        editor.TextChanged += (sender, e) =>
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
                pealkiri.Text = "Pealkiri";
            else
                pealkiri.Text = e.NewTextValue;
        };

        Button raagiNupp = new Button
        {
            Text = "🔊 Räägi",
            FontSize = 20,
            BackgroundColor = Colors.BlueViolet,
            TextColor = Colors.White,
            CornerRadius = 10
        };
        raagiNupp.Clicked += Raagi_Clicked;

        info = new Label { FontSize = 13, TextColor = Colors.Gray, HorizontalOptions = LayoutOptions.Center };

        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Padding = 20,
                Spacing = 15,
                Children = { pealkiri, editor, raagiNupp, info, StartPage.LooNavigatsioon(Navigation, 1) }
            }
        };
    }

    private async void Raagi_Clicked(object? sender, EventArgs e)
    {
        string tekst = editor.Text;
        if (string.IsNullOrWhiteSpace(tekst))
        {
            await DisplayAlertAsync("Hoiatus", "Kirjuta enne midagi!", "OK");
            return;
        }

        // Otsime eesti keele häält. Language on "et" (mitte "et-EE").
        IEnumerable<Locale> keeled = await TextToSpeech.Default.GetLocalesAsync();
        Locale? eesti = keeled.FirstOrDefault(l => l.Language.StartsWith("et"));

        if (eesti == null)
            info.Text = "Eesti häält ei leitud – loen vaikimisi häälega.";
        else
            info.Text = "Loen eesti keeles.";

        SpeechOptions seaded = new SpeechOptions
        {
            Volume = 1.0f,   // 0.0 – 1.0
            Pitch = 1.0f,    // 0.0 – 2.0
            Locale = eesti   // null = telefoni vaikimisi hääl
        };

        try
        {
            await TextToSpeech.Default.SpeakAsync(tekst, seaded);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Viga", "Ettelugemine ebaõnnestus: " + ex.Message, "OK");
        }
    }
}
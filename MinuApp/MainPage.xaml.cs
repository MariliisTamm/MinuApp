namespace MinuApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;                       // muutuja: mitu korda on vajutatud
        Random rnd = new Random();      // UUS: üks juhuslike arvude generaator kogu lehele
        public MainPage()
        {
            InitializeComponent();           // loeb MainPage.xaml ja loob elemendid
        }

        private async void OnCounterClicked(object? sender, EventArgs e)
        {
            count++;
            // Juhuslik värv: kolm arvu 0–255 (punane, roheline, sinine)
            NulliBtn.BackgroundColor = Color.FromRgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
            
            if (count == 1)
                CounterBtn.Text = $"Vajutatud {count} kord";
            else
                CounterBtn.Text = $"Vajutatud {count} korda";

            // UUS: suurendame teksti suurust ja muudame värvi
            CounterLabel.FontSize += 2;
            CounterLabel.TextColor = Colors.Green;
            CounterLabel.FontAttributes = FontAttributes.Bold;

            if (count >= 5)
            {
                CounterBtn.BackgroundColor = Colors.Red;
                CounterBtn.TextColor = Colors.White;
            }

            if (count >= 10)
            {
                BotImage.IsVisible = false;
                CounterLabel.Text = "Pilt kadus ära! Vajuta „Nulli“.";
            }
            else
            {
                CounterLabel.Text = $"Nuppu on vajutatud kokku: {count}";
            }
            SemanticScreenReader.Announce(CounterBtn.Text);

            double uusLabipaistvus = BotImage.Opacity > 0.2 ? BotImage.Opacity - 0.1 : BotImage.Opacity;
            await Task.WhenAll(
                BotImage.RotateToAsync(BotImage.Rotation + 15, 250),
                BotImage.FadeToAsync(uusLabipaistvus, 250)
            );
        }

        // UUS meetod Nulli-nupu jaoks
        private void OnNulliClicked(object? sender, EventArgs e)
        {
            count = 0;
            CounterBtn.Text = "Vajuta mind";
            CounterLabel.Text = "Alustame uuesti!";
            
            BotImage.Rotation = 0;                                     // pilt jälle otse
            BotImage.IsVisible = true;                                  // pilt nähtavaks
            BotImage.Opacity = 1;                                     // pilt täielikult läbipaistmatu

            // taastame teksti suuruse ja värvi
            CounterLabel.ClearValue(Label.FontSizeProperty);
            CounterLabel.ClearValue(Label.TextColorProperty);
            CounterLabel.FontAttributes = FontAttributes.None;

            // taastame btn tausta suuruse ja värvi
            CounterBtn.ClearValue(Button.BackgroundColorProperty);
            CounterBtn.ClearValue(Button.TextColorProperty);
            NulliBtn.BackgroundColor = Colors.Red;
            // UUS: pilt liigub vasakult paremale
            if (BotImage.HorizontalOptions.Alignment == LayoutAlignment.Start)
            {
                BotImage.HorizontalOptions = LayoutOptions.End;
            }
            else
            {
                BotImage.HorizontalOptions = LayoutOptions.Start;
            }

        }
    }
}

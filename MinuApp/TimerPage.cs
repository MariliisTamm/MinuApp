using System.Globalization;

namespace MinuApp;

public class TimerPage : ContentPage
{
    Label kell;
    Label kuupaev;
    Button nupp;
    IDispatcherTimer taimer;
    CultureInfo eesti = new CultureInfo("et-EE");

    public TimerPage()
    {
        Title = "Kell";

        kell = new Label
        {
            FontSize = 56,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center
        };
        kuupaev = new Label
        {
            FontSize = 18,
            TextColor = Colors.Gray,
            HorizontalOptions = LayoutOptions.Center
        };

        // Taimer käivitab sündmuse Tick iga sekundi järel
        taimer = Dispatcher.CreateTimer();
        taimer.Interval = TimeSpan.FromSeconds(1);
        taimer.Tick += (sender, e) => NaitaAega();

        nupp = new Button
        {
            Text = "⏸ Peata",
            FontSize = 20,
            CornerRadius = 10,
            HorizontalOptions = LayoutOptions.Center
        };
        nupp.Clicked += (sender, e) =>
        {
            if (taimer.IsRunning)
            {
                taimer.Stop();
                nupp.Text = "▶ Käivita";
            }
            else
            {
                taimer.Start();
                nupp.Text = "⏸ Peata";
            }
        };

        NaitaAega();

        Content = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 20,
            Children = { kell, kuupaev, nupp, StartPage.LooNavigatsioon(Navigation, 3) }
        };
    }

    void NaitaAega()
    {
        DateTime nyyd = DateTime.Now;
        kell.Text = nyyd.ToString("HH:mm:ss");
        kuupaev.Text = nyyd.ToString("dddd, d. MMMM yyyy", eesti);   // nt "teisipäev, 24. november 2026"
    }

    // Leht tuleb ekraanile -> kell käima
    protected override void OnAppearing()
    {
        base.OnAppearing();
        taimer.Start();
        nupp.Text = "⏸ Peata";
    }

    // Leht kaob ekraanilt -> kell seisma (muidu töötab taustal edasi)
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        taimer.Stop();
    }
}
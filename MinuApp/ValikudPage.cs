using Microsoft.Maui.Layouts;

namespace MinuApp;

public class ValikudPage : ContentPage
{
    Picker picker;
    DatePicker datePicker;
    TimePicker timePicker;
    Label tulemus;

    public ValikudPage()
    {
        Title = "3.2 Valikud";

        picker = new Picker
        {
            Title = "Vali taustavärv",
            ItemsSource = new List<string> { "Punane", "Roheline", "Sinine", "Valge" },
            WidthRequest = 250
        };
        // SelectedIndex: 0 = esimene rida, -1 = midagi pole valitud
        picker.SelectedIndexChanged += (sender, e) =>
        {
            switch (picker.SelectedIndex)
            {
                case 0: BackgroundColor = Colors.IndianRed; break;
                case 1: BackgroundColor = Colors.LightGreen; break;
                case 2: BackgroundColor = Colors.LightBlue; break;
                case 3: BackgroundColor = Colors.White; break;
            }
        };

        datePicker = new DatePicker
        {
            MinimumDate = DateTime.Today,
            MaximumDate = DateTime.Today.AddYears(1),
            Date = DateTime.Today,
            Format = "D"
        };
        // .NET 10: Date ja NewDate on DateTime? (võib olla tühi)
        datePicker.DateSelected += (sender, e) =>
        {
            if (e.NewDate is not DateTime valitud) return;   // tühi kuupäev -> ei tee midagi
            int paevi = (valitud - DateTime.Today).Days;
            tulemus.Text = $"Valitud päevani on {paevi} päeva";
        };

        timePicker = new TimePicker
        {
            Time = DateTime.Now.TimeOfDay,
            Format = "HH:mm"
        };
        // .NET 10: sündmus TimeSelected; Time ja NewTime on TimeSpan?
        timePicker.TimeSelected += (sender, e) =>
        {
            if (e.NewTime is not TimeSpan aeg) return;
            tulemus.Text = $"Valitud kellaaeg: {aeg:hh\\:mm}";
        };

        tulemus = new Label { Text = "Tee oma valik", FontSize = 20, FontAttributes = FontAttributes.Bold };

        // Kõik elemendid ühe tsükliga: x = keskel (0.5), y kasvab iga elemendiga
        AbsoluteLayout abs = new AbsoluteLayout();
        List<View> elemendid = new List<View> { picker, datePicker, timePicker, tulemus };
        for (int i = 0; i < elemendid.Count; i++)
        {
            double y = 0.1 + i * 0.2;
            AbsoluteLayout.SetLayoutBounds(elemendid[i], new Rect(0.5, y, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(elemendid[i], AbsoluteLayoutFlags.PositionProportional);
            abs.Add(elemendid[i]);
        }
        Content = abs;
    }
}
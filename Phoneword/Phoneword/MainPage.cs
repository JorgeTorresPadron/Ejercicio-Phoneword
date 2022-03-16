using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

public class MainPage : ContentPage
{
    Entry phoneNumberText;
    Button translateButton;
    Button callButton;
    String translatedNumber;

    public MainPage()
    {
        this.Padding = new Thickness(20, 20, 20, 20);
        BackgroundColor = Color.Aquamarine;

        StackLayout panel = new StackLayout
        {
            Spacing = 15,
        };

        panel.Children.Add(new Label
        {
            Text = "Escribe un telefóno:", TextColor = Color.Red,
            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
        });

        panel.Children.Add(phoneNumberText = new Entry
        {
            Text = "1-883-XAMARIN", TextColor = Color.Black,
        });

        panel.Children.Add(translateButton = new Button
        {
            Text = "Transformar", 
            BackgroundColor = Color.Gold,
        });

        panel.Children.Add(callButton = new Button
        {
            Text = "Llamar",
            BackgroundColor = Color.Gold,
            IsEnabled = false,
        });

        translateButton.Clicked += OnTranslate;
        callButton.Clicked += OnCall;
        this.Content = panel;
    }

    private void OnTranslate (object sender, EventArgs e)
    {
        string enteredNumber = phoneNumberText.Text;
        translatedNumber = Phoneword.PhonewordTranslator.ToNumber(enteredNumber);

        if (!string.IsNullOrEmpty(translatedNumber))
        {
            callButton.IsEnabled = true;
            callButton.Text = "Llamar " + translatedNumber;
        } else
        {
            callButton.IsEnabled= false;
            callButton.Text = "Llamar";
        }
    }

    private async void OnCall(object sender, System.EventArgs e)
    {
        if (await this.DisplayAlert(
            "Marcar a número",
            "¿Te gustaría llamar a " + translatedNumber + "?",
            "Sí",
            "No"))
        {
            try
            {
                PhoneDialer.Open(translatedNumber);
            }
            catch (ArgumentNullException)
            {
                await DisplayAlert("No se puede llamar", "Número de teléfono inválido", "Vale");
            }
            catch (FeatureNotSupportedException)
            {
                await DisplayAlert("No se puede llamar", "Número de teléfono inválido", "Vale");
            }
            catch (Exception)
            {
                // Other error has occurred.
                await DisplayAlert("No se puede llamar", "Número de teléfono inválido", "Vale");
            }
        }
    }
}
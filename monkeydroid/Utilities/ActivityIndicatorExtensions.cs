
using Microsoft.Maui.Layouts;

namespace monkeydroid.Utilities;

internal static class ActivityIndicatorExtensions
{
    // AbsoluteLayout and ability to overlay ActivityIndicator is
    // buggy as hell: https://github.com/dotnet/maui/issues/14120

    public static Layout AddActivityIndicator(this Layout layout)
    {
        // The Grid layout is cleaner but a little more work in the
        // XAML to assign individual rows to each child control.

        var absoluteLayout = new AbsoluteLayout
        {
            IsVisible = false,
            Margin = 0,
            Padding = 0,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
        };
        absoluteLayout.GestureRecognizers.Add(new TapGestureRecognizer());

        var stackLayout = new StackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Fill,
            Opacity = 0.5,
            Margin = 0,
            Padding = 0,
        };
        AbsoluteLayout.SetLayoutFlags(stackLayout, AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(stackLayout, new Rect(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

        var activityIndicator = new ActivityIndicator
        {
            IsRunning = true,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
        };

        stackLayout.Children.Add(activityIndicator);
        absoluteLayout.Children.Add(stackLayout);
        layout.Children.Add(absoluteLayout);
        return absoluteLayout;
    }

    public static Layout AddActivityIndicator(this Grid grid, int gridRow, int gridRowSpan)
    {
        /* Equivalent to this XAML:

        <AbsoluteLayout x:Name="activityIndicator"
                        Grid.Row="0"
                        Grid.RowSpan="4"
                        IsVisible="false"
                        ZIndex="444">
            <BoxView HorizontalOptions="Center"
                     VerticalOptions="Center"
                     Color="Black"
                     Opacity="0.85"
                     WidthRequest="9999"
                     HeightRequest="9999"
                     ZIndex="555"/>
            <ActivityIndicator IsVisible="true"
                               IsRunning="true"
                               AbsoluteLayout.LayoutFlags="PositionProportional"
                               AbsoluteLayout.LayoutBounds="0.5,0.5,AutoSize,AutoSize"
                               HorizontalOptions="Center"
                               VerticalOptions="Center"
                               Color="Red"
                               Opacity="1.0"
                               ZIndex="999"/>
        </AbsoluteLayout>

        */

        var absoluteLayout = new AbsoluteLayout
        {
            IsVisible = false,
            ZIndex = 997,
        };
        grid.SetRow(absoluteLayout, gridRow);
        grid.SetRowSpan(absoluteLayout, gridRowSpan);

        var box = new BoxView
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Color = Colors.Black,
            Opacity = 0.85,
            WidthRequest = 9999,
            HeightRequest = 999,
            ZIndex = 998,
        };

        var activityIndicator = new ActivityIndicator
        {
            IsRunning = true,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Color = Colors.Red,
            Opacity = 1.0,
            ZIndex = 999
        };
        AbsoluteLayout.SetLayoutFlags(activityIndicator, AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(activityIndicator, new Rect(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

        absoluteLayout.Children.Add(box);
        absoluteLayout.Children.Add(activityIndicator);
        grid.Children.Add(absoluteLayout);

        return absoluteLayout;
    }
}

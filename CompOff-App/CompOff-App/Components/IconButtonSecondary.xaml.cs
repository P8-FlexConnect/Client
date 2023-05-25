using Microsoft.Maui.Controls.Shapes;
using System.Windows.Input;

namespace CompOff_App.Components;

public partial class IconButtonSecondary : ContentView
{
    public event EventHandler? Clicked;

    /// <summary>
    /// Backing BindableProperty for the <see cref="Text"/> property.
    /// </summary>
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(IconButtonSecondary), string.Empty, propertyChanged: OnTextPropertyChanged);

    /// <summary>
    /// Backing BindableProperty for the <see cref="BorderBackgroundColor"/> property.
    /// </summary>
    public static readonly BindableProperty BorderBackgroundColorProperty = BindableProperty.Create(nameof(BorderBackgroundColor), typeof(Color), typeof(IconButtonSecondary), Colors.White, propertyChanged: OnColorPropertyChanged);

    /// <summary>
    /// Backing BindableProperty for the <see cref="IconColor"/> property.
    /// </summary>
    public static readonly BindableProperty IconColorProperty = BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(IconButtonSecondary), Colors.White, propertyChanged: OnColorPropertyChanged);

    /// <summary>
    /// Backing BindableProperty for the <see cref="BorderRadius"/> property.
    /// </summary>
    public static readonly BindableProperty BorderRadiusProperty = BindableProperty.Create(nameof(BorderRadius), typeof(int), typeof(IconButtonSecondary), 12, propertyChanged: OnColorPropertyChanged);

    /// <summary>
    /// Backing BindableProperty for the <see cref="IconSize"/> property.
    /// </summary>
    public static readonly BindableProperty IconSizeProperty = BindableProperty.Create(nameof(IconSize), typeof(int), typeof(IconButtonSecondary), 20);

    /// <summary>
    /// Backing BindableProperty for the <see cref="Glyph"/> property.
    /// </summary>
    public static readonly BindableProperty GlyphProperty = BindableProperty.Create(nameof(Glyph), typeof(ImageSource), typeof(IconButtonSecondary), null);

    /// <summary>
    /// Backing BindableProperty for the <see cref="Command"/> property.
    /// </summary>
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(IconButtonSecondary));

    /// <summary>
    /// Backing BindableProperty for the <see cref="CommandParameter"/> property.
    /// </summary>
    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(IconButtonSecondary));

    /// <summary>
    /// Backing BindableProperty for the <see cref="ShowTrailingIcon"/> property.
    /// </summary>
    public static readonly BindableProperty ShowTrailingIconProperty = BindableProperty.Create(nameof(ShowTrailingIcon), typeof(bool), typeof(IconButtonSecondary), false);

    /// <summary>
    /// The text to be displayed in the button
    /// </summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// Whether the trailing icon should be shown
    /// </summary>
    public bool ShowTrailingIcon
    {
        get => (bool)GetValue(ShowTrailingIconProperty);
        set => SetValue(ShowTrailingIconProperty, value);
    }

    /// <summary>
    /// The background color of the button
    /// </summary>
    public Color BorderBackgroundColor
    {
        get => (Color)GetValue(BorderBackgroundColorProperty);
        set => SetValue(BorderBackgroundColorProperty, value);
    }

    /// <summary>
    /// The icon color of the button
    /// </summary>
    public Color IconColor
    {
        get => (Color)GetValue(IconColorProperty);
        set => SetValue(IconColorProperty, value);
    }

    /// <summary>
    /// The border radius
    /// </summary>
    public int BorderRadius
    {
        get => (int)GetValue(BorderRadiusProperty);
        set => SetValue(BorderRadiusProperty, value);
    }

    /// <summary>
    /// The glyph symbol which will be used to generate the icon
    /// </summary>
    public ImageSource Glyph
    {
        get => (ImageSource)GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }

    /// <summary>
    /// The size of the icons
    /// </summary>
    public int IconSize
    {
        get => (int)GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    /// <summary>
    /// The command which is executed when the button is tapped.
    /// </summary>
    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>
    /// Parameters that will be passed to the <see cref="Command"/>/>
    /// </summary>
    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public IconButtonSecondary()
    {
        InitializeComponent();

        OnColorChange();
        OnTextChange();

        GestureContainer.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() =>
            {
                if (Command?.CanExecute(CommandParameter) == true)
                {
                    Clicked?.Invoke(this, EventArgs.Empty);
                    Command.Execute(CommandParameter);
                }
            })
        });
    }

    private static void OnColorPropertyChanged(BindableObject bindable, object oldValue, object newValue) => ((IconButtonSecondary)bindable).OnColorChange();

    private void OnColorChange()
    {
        BatchBegin();

        Border.BackgroundColor = BorderBackgroundColor;
        Border.Background = BorderBackgroundColor;
        Border.StrokeShape = new RoundRectangle
        {
            CornerRadius = BorderRadius,
            StrokeThickness = 2,
        };

        TrailingIcon.Color = IconColor;

        BatchCommit();
    }

    private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue) => ((IconButtonSecondary)bindable).OnTextChange();

    private void OnTextChange()
    {
        BatchBegin();
        if (string.IsNullOrEmpty(Text))
        {
            var thickness = new Thickness(0);
            GestureContainer.Padding = new Thickness(10);
            Trailing.Padding = thickness;
            Label.IsVisible = false;
        }
        else
        {
            GestureContainer.Padding = new Thickness(10);
            Label.IsVisible = true;

            if (ShowTrailingIcon)
            {
                Trailing.Margin = new Thickness(6, 0, 0, 0);
            }
        }

        BatchCommit();
    }
}
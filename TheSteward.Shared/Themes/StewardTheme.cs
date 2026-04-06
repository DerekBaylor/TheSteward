using MudBlazor;

namespace TheSteward.Shared.Themes;

public static class StewardTheme
{
    // -------------------------------------------------------
    // Blue palette derived from --base-blue: hsl(224.74 91% 45%)
    // -------------------------------------------------------
    private const string Blue1 = "#EEF1FC";
    private const string Blue2 = "#C9D2F7";
    private const string Blue3 = "#A3B3F2";
    private const string Blue4 = "#7D94ED";
    private const string Blue5 = "#5775E8";
    private const string Blue6 = "#1535D6"; // base-blue / Primary
    private const string Blue7 = "#1029A6";
    private const string Blue8 = "#0B1D76"; // dark-blue
    private const string Blue9 = "#061146";
    private const string Blue10 = "#020616";

    // Supporting colors
    private const string BaseGray = "#808080";
    private const string Gray1 = "#F2F2F2";
    private const string Gray8 = "#404040";
    private const string BaseGreen = "#16B200";
    private const string BaseRed = "#E5001A";
    private const string BaseOrange = "#E64400";
    private const string OffWhite = "#FAFAFA";

    public static MudTheme Theme => new MudTheme
    {
        // ---------------------------------------------------
        // Palette — Light Mode
        // ---------------------------------------------------
        PaletteLight = new PaletteLight
        {
            Primary = Blue6,
            PrimaryLighten = Blue4,
            PrimaryDarken = Blue8,
            PrimaryContrastText = OffWhite,

            Secondary = Blue4,
            SecondaryLighten = Blue2,
            SecondaryDarken = Blue7,
            SecondaryContrastText = OffWhite,

            Tertiary = Blue2,
            TertiaryLighten = Blue1,
            TertiaryDarken = Blue3,
            TertiaryContrastText = Blue8,

            Success = BaseGreen,
            SuccessContrastText = OffWhite,

            Warning = BaseOrange,
            WarningContrastText = OffWhite,

            Error = BaseRed,
            ErrorContrastText = OffWhite,

            Info = Blue5,
            InfoContrastText = OffWhite,

            Background = OffWhite,
            BackgroundGray = Gray1,
            Surface = "#FFFFFF",

            DrawerBackground = Blue8,
            DrawerText = OffWhite,
            DrawerIcon = Blue2,

            AppbarBackground = Blue6,
            AppbarText = OffWhite,

            TextPrimary = Blue10,
            TextSecondary = Gray8,
            TextDisabled = BaseGray,

            Divider = Gray1,
            DividerLight = "#E8E8E8",

            TableLines = Gray1,
            TableStriped = Blue1,
            TableHover = Blue1,

            ActionDefault = Blue6,
            ActionDisabled = BaseGray,
            ActionDisabledBackground = Gray1,

            LinesDefault = Gray1,
            LinesInputs = Blue3,
            HoverOpacity = 0.08,

            OverlayLight = "rgba(255,255,255,0.5)",
            OverlayDark = "rgba(11,29,118,0.5)",
        },

        // ---------------------------------------------------
        // Palette — Dark Mode
        // ---------------------------------------------------
        PaletteDark = new PaletteDark
        {
            Primary = Blue4,
            PrimaryLighten = Blue2,
            PrimaryDarken = Blue6,
            PrimaryContrastText = Blue10,

            Secondary = Blue3,
            SecondaryLighten = Blue1,
            SecondaryDarken = Blue5,
            SecondaryContrastText = Blue10,

            Tertiary = Blue7,
            TertiaryLighten = Blue5,
            TertiaryDarken = Blue9,
            TertiaryContrastText = OffWhite,

            Success = "#4ADE80",
            Warning = "#FB923C",
            Error = "#F87171",
            Info = Blue3,

            Background = Blue10,
            BackgroundGray = "#0D0D0D",
            Surface = Blue9,

            DrawerBackground = "#020616",
            DrawerText = Blue2,
            DrawerIcon = Blue3,

            AppbarBackground = Blue9,
            AppbarText = Blue1,

            TextPrimary = Blue1,
            TextSecondary = Blue3,
            TextDisabled = Blue7,

            Divider = Blue8,
            DividerLight = Blue9,

            TableLines = Blue8,
            TableStriped = Blue9,
            TableHover = Blue8,

            ActionDefault = Blue3,
            ActionDisabled = Blue7,
            ActionDisabledBackground = Blue9,

            LinesDefault = Blue8,
            LinesInputs = Blue5,
            HoverOpacity = 0.1,

            OverlayLight = "rgba(255,255,255,0.05)",
            OverlayDark = "rgba(2,6,22,0.8)",
        },

        // ---------------------------------------------------
        // Typography
        // ---------------------------------------------------
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = new[] { "Poppins", "sans-serif" },
                FontSize = "0.875rem",
                FontWeight = "400",
                LineHeight = "1.5",
            },
            H1 = new H1Typography
            {
                FontFamily = new[] { "FiraCode", "monospace" },
                FontSize = "2.5rem",
                FontWeight = "600",
            },
            H2 = new H2Typography
            {
                FontFamily = new[] { "FiraCode", "monospace" },
                FontSize = "2rem",
                FontWeight = "600",
            },
            H3 = new H3Typography
            {
                FontFamily = new[] { "FiraCode", "monospace" },
                FontSize = "1.75rem",
                FontWeight = "600",
            },
            H4 = new H4Typography
            {
                FontFamily = new[] { "FiraCode", "monospace" },
                FontSize = "1.5rem",
                FontWeight = "600",
            },
            H5 = new H5Typography
            {
                FontFamily = new[] { "FiraCode", "monospace" },
                FontSize = "1.25rem",
                FontWeight = "600",
            },
            H6 = new H6Typography
            {
                FontFamily = new[] { "FiraCode", "monospace" },
                FontSize = "1rem",
                FontWeight = "600",
            },
            Button = new ButtonTypography
            {
                FontFamily = new[] { "FiraCode", "monospace" },
                FontSize = "0.875rem",
                FontWeight = "600",
                TextTransform = "none",
            },
            Subtitle1 = new Subtitle1Typography
            {
                FontFamily = new[] { "Poppins", "sans-serif" },
                FontSize = "1rem",
                FontWeight = "600",
            },
            Subtitle2 = new Subtitle2Typography
            {
                FontFamily = new[] { "Poppins", "sans-serif" },
                FontSize = "0.875rem",
                FontWeight = "600",
            },
            Body1 = new Body1Typography
            {
                FontFamily = new[] { "Poppins", "sans-serif" },
                FontSize = "0.875rem",
                FontWeight = "400",
            },
            Body2 = new Body2Typography
            {
                FontFamily = new[] { "Poppins", "sans-serif" },
                FontSize = "0.75rem",
                FontWeight = "400",
            },
            Caption = new CaptionTypography
            {
                FontFamily = new[] { "Poppins", "sans-serif" },
                FontSize = "0.75rem",
                FontWeight = "400",
            },
            Overline = new OverlineTypography
            {
                FontFamily = new[] { "FiraCode", "monospace" },
                FontSize = "0.625rem",
                FontWeight = "400",
            },
        },

        // ---------------------------------------------------
        // Layout
        // ---------------------------------------------------
        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "8px",
            DrawerWidthLeft = "200px",
            DrawerWidthRight = "240px",
            DrawerMiniWidthLeft = "56px",
            AppbarHeight = "64px",
        },

        // ---------------------------------------------------
        // Z-Index
        // ---------------------------------------------------
        ZIndex = new ZIndex
        {
            Drawer = 1100,
            AppBar = 1200,
            Dialog = 1300,
            Snackbar = 1400,
            Tooltip = 1500,
        }
    };
}



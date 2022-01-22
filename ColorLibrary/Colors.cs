using System;
using System.Drawing;

public enum ColorType
{
    Red,
    Green,
    Blue,
    Black,
    White,
    Gray,
    Yellow,
    Purple,
    YellowGreen,
    Cyan,
    Unknown,
    Orange,
    WineRed,
    DarkBlue,
    LightBlue,
    PurpleBlue,
    DarkGreen,
    LightGreen
}
public struct HSVColor
{
    public double H { get; set; }
    public double S { get; set; }
    public double V { get; set; }
    public HSVColor(double h, double s, double v)
    {
        H = h;
        S = s;
        V = v;
    }
}
public class Colors
{
    public int R { get; set; }
    public int G { get; set; }
    public int B { get; set; }
    public Colors(byte r, byte g, byte b)
    {
        R = r;
        G = g;
        B = b;
    }
    public Colors(Color color)
    {
        R = color.R;
        G = color.G;
        B = color.B;
    }


    public ColorType GetSimpleColorType()
    {
        if (IsWhite())
            return ColorType.White;
        else if (IsBlack())
            return ColorType.Black;
        else if (IsGray())
            return ColorType.Gray;

        if (B >= Math.Max(R, G))
        {
            return ColorType.Blue;
        }
        if (R >= Math.Max(B, G))
        {
            return ColorType.Red;
        }
        if (G >= Math.Max(R, B))
        {
            return ColorType.Green;
        }
        return ColorType.Unknown;
    }
    public ColorType GetColorType()
    {
        if (IsRed())
        {
            return ColorType.Red;
        }
        if (IsLightBlue())
        {
            return ColorType.LightBlue;
        }
        else if (IsDarkBlue())
        {
            return ColorType.DarkBlue;
        }
        else if (IsYellowGreen())
        {
            return ColorType.YellowGreen;
        }
        else if (IsGreen())
        {
            return ColorType.Green;
        }
        else if (IsLightGreen())
        {
            return ColorType.LightGreen;
        }
        else if (IsDarkGreen())
        {
            return ColorType.DarkGreen;
        }
        else if (IsCyan())
        {
            return ColorType.Cyan;
        }
        else if (IsWineRed())
        {
            return ColorType.WineRed;
        }
        else if (IsPurple())
        {
            return ColorType.Purple;
        }
        else if (IsOrange())
        {
            return ColorType.Orange;
        }
        else if (IsYellow())
        {
            return ColorType.Yellow;
        }
        else if (IsBlack())
        {
            return ColorType.Black;
        }
        else if (IsWhite())
        {
            return ColorType.White;
        }
        else if (IsGray())
        {
            return ColorType.Gray;
        }
        else
            return ColorType.Unknown;
    }

    public bool IsBlack()
    {
        return (R <= 50 && G <= 50 && B <= 50);

    }

    private bool IsWhite()
    {
        return R >= 250 && G >= 250 && B >= 250;
    }

    private bool IsGray()
    {
        int rg = Math.Abs(R - G);
        int rb = Math.Abs(R - B);
        int gb = Math.Abs(G - B);
        return rg <= 10 && rb <= 10 && gb <= 10;
    }

    private bool IsWhiteORBlackORGray()
    {
        return IsBlack() || IsWhite() || IsGray();
    }

    public bool IsGreen()
    {
        if (IsWhiteORBlackORGray())
        {
            return false;
        }
        if (G - R <= 255 && G - R > 127 && B < Math.Min(R, G))
        {
            return true;
        }
        else
            return false;
    }

    public bool IsLightGreen()
    {
        if (IsWhiteORBlackORGray())
        {
            return false;
        }
        if (G - B <= 255 && G - B > 127 && R < Math.Min(G, B))
        {
            return true;
        }
        else
            return false;
    }
    public bool IsDarkGreen()
    {
        if (IsWhiteORBlackORGray())
        {
            return false;
        }
        if (G - B <= 127 && G - B >= 0 && R < Math.Min(G, B))
        {
            return true;
        }
        else
            return false;
    }
    public bool IsYellowGreen()
    {
        if (IsWhiteORBlackORGray())
        {
            return false;
        }
        if (G - R <= 127 && G - R >= 0 && B < Math.Min(R, G))
        {
            return true;
        }
        else
            return false;
    }

    public bool IsYellow()
    {
        if (IsWhiteORBlackORGray())
        {
            return false;
        }
        if (R - G <= 127 && R - G >= 0 && B < Math.Min(R, G))
        {
            return true;
        }
        else
            return false;
    }

    public bool IsOrange()
    {
        if (IsWhiteORBlackORGray())
        {
            return false;
        }
        if (R - G <= 255 && R - G > 127 && B < Math.Min(R, G))
        {
            return true;
        }
        else
            return false;
    }

    public bool IsPurple()
    {
        if (IsWhiteORBlackORGray())
        {
            return false;
        }
        if (B - R >= 0 && B - R <= 127 && G < Math.Min(R, B))
        {
            return true;
        }
        else
            return false;
    }

    public bool IsCyan()
    {
        if (IsWhiteORBlackORGray())
        {
            return false;
        }
        if (B - G >= 0 && B - G < 128 && R < Math.Min(G, B))
        {
            return true;
        }
        else
            return false;
    }


    public bool IsDarkBlue()
    {
        if (IsWhiteORBlackORGray())
        {
            return false;
        }
        if (B - R <= 255 && B - R > 127 && G < Math.Min(R, B))
        {
            return true;
        }
        else
            return false;
    }
    public bool IsLightBlue()
    {
        if (IsWhiteORBlackORGray())
        {
            return false;
        }
        if (B - G <= 255 && B - G >= 128 && R < Math.Min(G, B))
        {
            return true;
        }
        else
            return false;
    }

    public bool IsRed()
    {
        if (IsWhiteORBlackORGray())
        {
            return false;
        }
        if (R - B <= 255 && R - B >= 128 && G < Math.Min(R, B))
        {
            return true;
        }
        else
            return false;
    }

    public bool IsWineRed()
    {
        if (IsWhiteORBlackORGray())
        {
            return false;
        }
        if (R - B >= 0 && R - B < 128 && G < Math.Min(R, B))
        {
            return true;
        }
        else
            return false;
    }

}
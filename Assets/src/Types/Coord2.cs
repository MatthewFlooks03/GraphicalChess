using System;
using System.Numerics;

public class Coord2 : IEquatable<Coord2>
{
    public int x;

    public int y;

    public Coord2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Coord2(Vector2 vect)
    {
        this.x = (int)Math.Round(vect.X);
        this.y = (int)Math.Round(vect.Y);
    }


    //Equalities
    public bool Equals(Coord2 other)
    {
        if (other.x == this.x && other.y == this.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public override bool Equals(object obj)
    {
        return this == (Coord2)obj;
    }

    public override int GetHashCode()
    {
        return this.x ^ this.y;
    }

    public static bool operator ==(Coord2 a, Coord2 b)
    {
        if (a.x == b.x && a.y == b.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool operator !=(Coord2 a, Coord2 b)
    {
        if (a.x == b.x && a.y == b.y)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static bool operator <(Coord2 a, Coord2 b)
    {
        if (a.x < b.x && a.y < b.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool operator >(Coord2 a, Coord2 b)
    {
        if (a.x > b.x && a.y > b.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Addition
    public static Coord2 operator +(Coord2 a, Coord2 b)
    {
        return new Coord2(a.x + b.x, a.y + b.y);
    }

    //Subtraction
    public static Coord2 operator -(Coord2 a, Coord2 b)
    {
        return new Coord2(a.x - b.x, a.y - b.y);
    }

    //Multiplication
    public static Coord2 operator *(Coord2 a, Coord2 b)
    {
        return new Coord2(a.x * b.x, a.y * b.y);
    }

    public static Coord2 operator *(Coord2 a, int b)
    {
        return new Coord2(a.x * b, a.y * b);
    }

    public static Coord2 operator *(int a, Coord2 b)
    {
        return new Coord2(a * b.x, a * b.y);
    }

    //Division
    public static Coord2 operator /(Coord2 a, Coord2 b)
    {
        return new Coord2(a.x / b.x, a.y / b.y);
    }

    public static Coord2 operator /(Coord2 a, int b)
    {
        return new Coord2(a.x / b, a.y / b);
    }

    public static Coord2 operator /(int a, Coord2 b)
    {
        return new Coord2(a / b.x, a / b.y);
    }

    //Modulus
    public static Coord2 operator %(Coord2 a, Coord2 b)
    {
        return new Coord2(a.x % b.x, a.y % b.y);
    }

    public static Coord2 operator %(Coord2 a, int b)
    {
        return new Coord2(a.x % b, a.y % b);
    }

    public static Coord2 operator %(int a, Coord2 b)
    {
        return new Coord2(a % b.x, a % b.y);
    }

    /// <summary>
    /// Coverts the Coord2 to a string
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return "(" + x + ", " + y + ")";
    }

    /// <summary>
    /// Converts the Coord2 to a Vector2
    /// </summary>
    /// <returns></returns>
    public UnityEngine.Vector2 ToVector2()
    {
        return new UnityEngine.Vector2(x, y);
    }

    /// <summary>
    /// Checks if the Coord2 is on the board
    /// </summary>
    /// <returns></returns>
    public bool IsOnBoard()
    {
        if (x >= 0 && x <= 7 && y >= 0 && y <= 7)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
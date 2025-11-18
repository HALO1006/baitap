using System;

class PTB1
{
    protected double a;
    protected double b;

    public PTB1(double a, double b)
    {
        this.a = a;
        this.b = b;
    }

    public virtual string Giai()
    {
        if (a == 0)
        {
            return (b == 0) ?
                "Phuong trinh vo so nghiem" :
                "Phuong trinh vo nghiem";
        }

        double x = -b / a;
        return $"Nghiem x = {x:F2}";
    }
}

class PTB2 : PTB1
{
    private double c;

    public PTB2(double a, double b, double c) : base(a, b)
    {
        this.c = c;
    }

    public override string Giai()
    {
        if (a == 0)
        {
            PTB1 pt1 = new PTB1(b, c);
            return "Phuong trinh tro thanh bac 1 -> " + pt1.Giai();
        }

        double delta = b * b - 4 * a * c;

        if (delta < 0)
            return "Phuong trinh vo nghiem";

        if (delta == 0)
        {
            double x = -b / (2 * a);
            return $"Phuong trinh co nghiem kep x = {x:F2}";
        }
        else
        {
            double sqrtDelta = Math.Sqrt(delta);
            double x1 = (-b + sqrtDelta) / (2 * a);
            double x2 = (-b - sqrtDelta) / (2 * a);
            return $"Nghiem x1 = {x1:F2}, x2 = {x2:F2}";
        }
    }
}

class Program
{
    static double InputDouble(string msg)
    {
        double value;
        while (true)
        {
            Console.Write(msg);
            if (double.TryParse(Console.ReadLine(), out value))
                return value;
            else
                Console.WriteLine("Gia tri khong hop le! Nhap lai.");
        }
    }

    static void Main()
    {
        double a = InputDouble("Nhap a: ");
        double b = InputDouble("Nhap b: ");
        double c = InputDouble("Nhap c: ");

        PTB2 pt2 = new PTB2(a, b, c);

        Console.WriteLine("\nKet qua: " + pt2.Giai());
    }
}


namespace InterfaceBad;

public class Circle : IShape
{
    public double _radius;

    public Circle(double radius)
    {
        _radius = radius;
    }

    public double GetArea()
    {
        return Math.PI * _radius * _radius;
    }

    public double GetPerimeter()
    {
        return 2 * Math.PI * _radius;
    }

}
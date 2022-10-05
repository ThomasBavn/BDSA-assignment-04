namespace InterfaceBad;

public class Cube : IShape
{
    public double _sideLength;

    public Cube(double sideLength)
    {
        _sideLength = radius;
    }

    public double GetArea()
    {
        return (_sideLength * _sideLength) * 6;
    }

    public double GetPerimeter()
    {
        return 12 * _sideLength;
    }

    public double GetVolume()
    {
        return _sideLength * _sideLength * _sideLength;
    }
}
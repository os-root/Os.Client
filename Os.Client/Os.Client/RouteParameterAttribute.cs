namespace OrlemSoftware.Client;

public class RouteParameterAttribute : Attribute
{
    public string Name { get; }

    public RouteParameterAttribute(string name)
    {
        Name = name;
    }
}
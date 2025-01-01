namespace OrlemSoftware.Client;

public class QueryParameterAttribute : Attribute
{
    public string Name { get; }

    public QueryParameterAttribute(string name)
    {
        Name = name;
    }
}
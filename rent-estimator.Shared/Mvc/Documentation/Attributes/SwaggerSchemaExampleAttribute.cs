namespace rent_estimator.Shared.Mvc.Documentation.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Enum, AllowMultiple = false)]
public class SwaggerSchemaExampleAttribute : Attribute
{
    public SwaggerSchemaExampleAttribute(string value)
    {
        Value = value;
    }

    public string Value { get; set; }
}

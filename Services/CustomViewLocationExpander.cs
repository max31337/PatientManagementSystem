using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;

public class CustomViewLocationExpander : IViewLocationExpander
{
    public void PopulateValues(ViewLocationExpanderContext context) { }

    public IEnumerable<string> ExpandViewLocations(
        ViewLocationExpanderContext context,
        IEnumerable<string> viewLocations)
    {
        // Add your custom view locations here
        var locations = new List<string>
        {
            "/Views/Shared/{0}.cshtml",
            "/Views/MedicalRecord/{0}.cshtml",
            "/Views/Shared/{1}.cshtml"
        };

        // Return the combined list of view locations
        return locations;
    }
}

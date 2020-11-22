using System.Runtime.InteropServices;
using Rhino.PlugIns;

// Plug-in Description Attributes - all of these are optional
// These will show in Rhino's option dialog, in the tab Plug-ins
[assembly: PlugInDescription(DescriptionType.Address, "Barcelona")]
[assembly: PlugInDescription(DescriptionType.Country, "Spain")]
[assembly: PlugInDescription(DescriptionType.Email, "info@paramdigma.com")]
[assembly: PlugInDescription(DescriptionType.Phone, "")]
[assembly: PlugInDescription(DescriptionType.Organization, "Paramdigma")]
[assembly: PlugInDescription(DescriptionType.UpdateUrl, "")]
[assembly: PlugInDescription(DescriptionType.WebSite, "https://paramdigma.com")]

// Rhino requires a Guid assigned to the assembly.
[assembly: Guid("2DC94584-44C2-487A-8F95-52E63E8170D2")]
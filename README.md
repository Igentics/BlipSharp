## Prerequisits
* Visual Studio 2010
* Visual Studio 2012

The Blipsharp C# wrapper is built using **PCL** ( _Portable Class Libraries_ ) in order to take advantage of the cross platform nature of the .net framework. You will need to install the latest service pack for **Visual Studio 2010** in order to load this feature. **Visual Studio 2012** is fully supported.

* Microsoft.Bcl.*
* Newtonsoft.JSON

The libraries above are included in the packages.config file and are available on nuget.

***

## ApiContext Context Class
The BlipSharp ApiContext Context class is the communication layer of the wrapper library and it is recommended you use the Singleton pattern for instantiating your context

`
private ApiContext _context;
public ApiContext Context
{
    get { return _context ?? (_context = new ApiContext {
                ApiKey = ConfigurationManager.AppSettings["BlipSharp.ApiKey"],
                ApiSecret = ConfigurationManager.AppSettings["BlipSharp.ApiSecret"],
                PermissionsURL = ConfigurationManager.AppSettings["BlipSharp.AuthenticationUrl"]
            });
        }
}
`  

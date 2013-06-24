namespace BlipSharp.Interfaces
{
    public interface IApiContext
    {
        string ApiKey { get; set; }
        string ApiSecret { get; set; }
        string PermissionsURL { get; set; }
        string AuthenticationToken { get; set; }
        string UserSecret { get; set; }
    }
}
namespace HubbleSpace_Final.Services
{
    public interface IUserService
    {
        string GetUserId();
        bool IsAuthenticated();
    }
    
}
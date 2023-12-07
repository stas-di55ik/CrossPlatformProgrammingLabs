using CPP_Lab13API.Models;

namespace CPP_Lab13API.Services
{
    public interface IMessageService
    {
        Message GetPublicMessage();
        Message GetProtectedMessage();
    }
}

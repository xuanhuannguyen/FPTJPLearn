namespace JPLearn.Core.Common.Services;

public interface ICurrentUserContext
{
    Guid UserId { get; }
    bool IsAuthenticated { get; }
}

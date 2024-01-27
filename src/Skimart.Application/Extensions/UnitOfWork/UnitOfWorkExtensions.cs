namespace Skimart.Application.Extensions.UnitOfWork;

public static class UnitOfWorkExtensions
{
    public static bool TransactionSuccess(this int result) => result > 0;

    public static bool TransactionFailed(this int result) => !result.TransactionSuccess();
}
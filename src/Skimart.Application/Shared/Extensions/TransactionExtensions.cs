namespace Skimart.Application.Shared.Extensions;

public static class TransactionExtensions
{
    public static bool TransactionSuccess(this int result) => result > 0;

    public static bool TransactionFailed(this int result) => !result.TransactionSuccess();
}
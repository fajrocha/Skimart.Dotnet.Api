namespace Skimart.Domain;

public class AppConstants
{
    public static class Pwd
    {
        public const string Regex = @"(?=^.{6,10}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}
                                              {&quot;:;'?/&gt;.&lt;,])(?!.*\s).*$";
    }
}
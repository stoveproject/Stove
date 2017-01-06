using System.Globalization;
using System.Threading;

namespace Stove.Threading
{
    /// <summary>
    ///     This class is copied from here:
    ///     http://www.zpqrtbnk.net/posts/appdomains-threads-cultureinfos-and-paracetamol
    ///     It's a workaround for application startup problem.
    /// </summary>
    public static class ThreadCultureSanitizer
    {
        public static void Sanitize()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;

            // at the top of any culture should be the invariant culture,
            // find it doing an .Equals comparison ensure that we will
            // find it and not loop endlessly
            CultureInfo invariantCulture = currentCulture;
            while (invariantCulture.Equals(CultureInfo.InvariantCulture) == false)
            {
                invariantCulture = invariantCulture.Parent;
            }

            if (ReferenceEquals(invariantCulture, CultureInfo.InvariantCulture))
            {
                return;
            }

            Thread thread = Thread.CurrentThread;
            thread.CurrentCulture = CultureInfo.GetCultureInfo(thread.CurrentCulture.Name);
            thread.CurrentUICulture = CultureInfo.GetCultureInfo(thread.CurrentUICulture.Name);
        }
    }
}

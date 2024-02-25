using Microsoft.Playwright;

namespace AcceptanceTests.Drivers;

public abstract class PageBase
{
    protected readonly ILocator Base;

    // ReSharper disable once MemberCanBePrivate.Global
    protected PageBase(ILocator baseLocator)
    {
        Base = baseLocator;
    }

    protected PageBase(IPage page): this(page.Locator("*"))
    {
    }
}

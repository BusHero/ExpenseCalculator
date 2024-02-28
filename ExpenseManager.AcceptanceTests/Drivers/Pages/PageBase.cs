using Microsoft.Playwright;

namespace AcceptanceTests.Drivers.Pages;

public abstract class PageBase
{
    protected readonly ILocator Base;

    protected PageBase(ILocator baseLocator)
    {
        Base = baseLocator;
    }

    protected PageBase(IPage page) : this(page.Locator("*"))
    {}
}

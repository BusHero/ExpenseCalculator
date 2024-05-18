using Microsoft.Playwright;

namespace AcceptanceTests.Drivers.Pages;

public abstract class PageBase(ILocator baseLocator)
{
    protected readonly ILocator Base = baseLocator;

    protected PageBase(IPage page) : this(page.Locator("*"))
    {}
}

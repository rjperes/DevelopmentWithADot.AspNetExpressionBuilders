DevelopmentWithADot.AspNetExpressionBuilders
======================================

ASP.NET Expression Builders
First discussed on http://weblogs.asp.net/ricardoperes/archive/2009/03/04/useful-expression-builders.aspx, http://weblogs.asp.net/ricardoperes/archive/2012/12/31/asp-net-web-forms-extensibility-expression-builders.aspx and http://weblogs.asp.net/ricardoperes/archive/2012/04/25/url-related-expression-builders.aspx.

Included expression builders:
- Application: returns a value from the current HttpApplication
- Cache: returns a value from the ASP.NET cache
- Code: returns the result of a fragment of .NET code
- Concat: concatenates multiple strings, either literal or coming from resource files (local or global)
- Context: returns a property value from the current HttpContext
- Cookie: returns a cookie value
- Format: formats a string, either from a literal or from a resource file (local or global)
- InRole: checks if the current user belongs to a role
- MemoryCache: returns a value from the MemoryCache
- Profile: returns a property value from the current user profile
- QueryString: returns a value from the current query string
- ServiceLocator: returns a value from the Common Service Locator
- Session: returns a value from the current session
- ThemeFileUrl: returns the true URL for a file inside a theme directory for the current theme
- ViewState: returns a value from the view state of a control
- WebResourceUrl: returns the true URL for a file stored as an embedded resource

Copyright Ricardo Peres 2013

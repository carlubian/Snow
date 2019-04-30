# Snow
<strong>Experimental .NET Standard IOC container.</strong>

## Overview
Snow allows .NET apps to use a dependency injection mechanism. Both component classes and dependency requests are indicated via attributes, like this:

```cs
[Component]
class TextService
{
    public string GetText() => "Hello from Component";
}

[Component]
class Execution
{
    [Autowired]
    private TextService textService;

    public void Execute()
    {
        Console.WriteLine(textService.GetText());
    }
}
```

## Requirements
Snow is compatible with .NET Standard 2.0, which includes a myriad of destination platforms like .NET Core, ASP.NET Core, Xamarin or UWP.
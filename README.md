[![Build Status](https://carlubian.visualstudio.com/GitHub%20Interop/_apis/build/status/Snow%20Build?branchName=master)](https://carlubian.visualstudio.com/GitHub%20Interop/_build/latest?definitionId=24&branchName=master)
# Snow
<strong>Experimental .NET Standard IOC container.</strong>

## Overview
Snow allows .NET apps to use a dependency injection mechanism without having to manually
register and inject dependencies or initialize and keep track of containers. 
Both component classes and dependency requests are indicated via attributes, like this:

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

## Documentation
See [the documentation site](https://carlubian.github.io/Snow) for more details on how to use Snow.

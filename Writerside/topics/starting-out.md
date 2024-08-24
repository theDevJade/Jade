# Starting Out

Welcome to the JadeLib developer documentation, in *starting out* you will learn how to reference and initialize JadeLib

## Before you start

List the prerequisites that are required or recommended.

Make sure that:
- You have an IDE capable of using C# code
- An EXILED plugin project set up.
- `JadeLib.dll` downloaded
- The aforementioned file added as a reference to your project.

## Using JadeLib

1. Open up your main plugin file, this could be `ExamplePlugin` or any similar file, make sure it looks something like this.
```c#
 using Exiled.API.Enums;
 using Exiled.API.Features;

 /// <summary>
 /// The example plugin.
 /// </summary>
 public class Example : Plugin<Config>
 {
     /// <inheritdoc/>
     public override PluginPriority Priority { get; } = PluginPriority.Last;

     /// <inheritdoc/>
     public override void OnEnabled()
     {
         // Plugin logic here
         base.OnEnabled();
     }
 }
```

2. Now, all you need to do is put this simple line of code in your `OnEnabled` function:
```c#
Jade.Initialize();
```


### Settings
JadeLib also allows use of settings when initializing, here is the default settings:
```c#
public static JadeSettings Default = new()
    {
        UseHintSystem = true,
        JadeCredit = true,
        InitializeFFmpeg = false
    };
```

In order to customize this, just pass your own `JadeSettings` into `Jade::Initialize`

<seealso>
<!--Give some related links to how-to articles-->
</seealso>

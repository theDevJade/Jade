# Hints

### The hint system expands upon the functionality of RueI's parser to allow compatible and easy-use hints.

The hint system is composed of a few elements:

- Elements: the actual hints that are displayed to a player, every element has a delegate method that is either static or dynamic, a position, formatting options, and a ZIndex.
- PlayerDisplay: the display for a player handling updating, screens, and other logic
- Screen: a hot-swappable list of elements for a player that allow you to hot-swap between UI. Each screen has an identifier, the default one being `default`


## Basic Usage

In order to retrieve a player's display, you can simply use the extension function for their `ReferenceHub`, such as follows:

```c#
var display = referenceHub.GetDisplay(); // <-- Never null!
```

In order to retrieve the active screen in a display simply:

```c#
...
var screen = display.ActiveScreen
```

For more information on how to actually add a hint to a player, read the following:


<a href="Easy-Hints.md">Easy Hints</a>
<br></br>
<a href="Elements.md"/>

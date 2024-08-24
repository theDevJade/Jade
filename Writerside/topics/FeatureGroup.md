# FeatureGroup's

Feature groups allow easily registering your events. Here's how to use them.

## Step 1

Create a new class and add a few functions with their respective EventArgs, for example:

```c#
public class Events
{   
    public void Candy(InteractingScp330EventArgs args)
    {
    }

    public void OnDamage(HurtEventArgs args)
    {
    }

    public void SCP096(AddingTargetEventArgs args)
    {
    }
}
```

Then for each function containing an event you want to listen for add the attribute

```c#
[Listener] <--
public void Candy(InteractingScp330EventArgs args)
{
}
```

## Step 2: Register the FeatureGroup

Now, all you need to do is register the feature group, it's as simple as this

```c#
var id = "example" // A unique identifier for the group.
object events = new Events() // An object instance of the listeners.
var featureGroup = new FeatureGroup(id).Supply(events);
// Now, just register it!
featureGroup.Register();
```

That's it! Everything else is done for you!

**Remember that this ONLY supports EventArgs in `Exiled.Events`**
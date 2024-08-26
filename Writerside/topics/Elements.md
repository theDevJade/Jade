# Elements

Elements are the root of how hints function

Each element is composed of:
- A text getter
- Element options
- Position
- ZIndex

There are two different types of elements that you commonly use:
- Dynamic Element
- Fixed Element

Here's how to create one of both:

**Dynamic**
```c#
var element = new DynamicElement(pos, HintContent);
```

**Fixed**
```c#
var element = new FixedElement("hint here", pos);
```

For Dynamic elements you need a hint content, a hint content is any function that looks like this:
```c#
string Content(HintCtx ctx)
{
    return "example";
}
```

In order to add an element to a screen, it's simple as
```c#
yourElement.AddTo(yourScreen);
```
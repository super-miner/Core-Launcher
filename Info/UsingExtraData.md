# Using Extra Data

## What is Extra Data?

Extra Data is the name for a system that allows you to attach extra information to your mod using the mod description. This includes:
- Donation Links (which will be displayed beside your mod)
- Dependency info (this saves one API key use per mod so please implement this)

This can be set up by putting certain words into the description of your mod

## How do I set up extra data?

Extra data can be set up by typing words into the description of your mod with a specific syntax.

This is an example of the extra data included in CK Hud:
`CL_Data(Depend:3456859,3177992|Donate:___)`

All of your extra data should be contained between `CL_Data(` and a `)` (closed bracket). From there you can put in any data that is supported separated by a `|` (pipe).

The format for each piece of data is `<the name of the data>:<the information>`. For example above I write `Depend:3456859,3177992` which represents dependencies on the mods 3456859 (CoreLib.Localization) and 3177992 (CoreLib). The numbers are separated by a `,` (comma) which represents a list of dependencies.

Now, this is all good but it might look a bit ugly to have this somewhere in your description. That is what we'll talk about in the next section.

## Hiding the text
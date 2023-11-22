# Using Extra Data

Extra Data is the name for a system that allows you to attach extra information to your mod using the mod description. This includes:
- Donation Links (which will be displayed beside your mod)
- Dependency info (this saves one API key use per mod so please implement this)
- Whether the mod is server-side or client-side
- Whether your mod is a library (this will also work if your mod has the library tag)

This can be set up by putting certain words into the description of your mod.

## How do I set up extra data?

Extra data can be set up by typing words into the description of your mod with a specific syntax.

This is an example of the extra data included in CK Hud:
`CL_Data(Depend:3456859,3177992|Donate:https://www.buymeacoffee.com/flown|ClientSide:true|ServerSide:false)`

All of your extra data should be contained between `CL_Data(` and a `)` (closed bracket). From there you can put pieces of data separated by a `|` (pipe).

The format for each piece of data is `<variable name>:<data>`. For example above I write `Donate:https://www.buymeacoffee.com/flown` which represents my donation link to be linked to from the launcher.

Now, this is all good but it might look a bit ugly to have this somewhere in your description. That is what we'll talk about in the next section.

## Hiding the text
Luckily for us mod.io provides access to the HTML data for your description which we can use to hide the text. Although some methods for hiding the text dont work the one I've found that works is surrounding your text with a `<p><span style="font-size: 0px;">` at the start, and a `</span></p>` at the end. this will display a bit of a blank line but it's the best option I've found.

## List of data
Here is a list of all the data variables that you can set

- `Depend` A list of your mods dependencies, **this saves the user of Core Launcher one api key use per mod that incorperates this so please use it**
  - You can enter the IDs of your dependencies as a comma separated list
- `Donate` Adds a button that can link to your donation page beside your mod
- `ClientSide` Specifies that your mod is client side
  - Both ClientSide and ServerSide default to true so you will only have to specify what your mod is not
- `ServerSide` Specifies that your mod is server side
  - See ClientSide for more info
- `Library` Specifies whether your mod is a library, this means that it will be hidden in the Show Library Mods button is toggled off
  - It will also be detected if your mod has the Library tag on mod.io

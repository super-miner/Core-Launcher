# CL_Data

The CL_Data system allows you (the mod creator) to add data to your mod that will be used by Core Launcher by putting it in your mod's description. This data can be hidden from regular users using the techniques shown in the **Hiding the text** section below. 

Here are some examples of data you can add:
- A link to your donation page
- Whether or not your mod is server-side or client-side
- Whether or not your mod is a library

## How do I set up CL_Data for my mod?

CL_Data can be implemented by including a `CL_Data()` section in your mod's description on mod.io.

Here is an example of what your CL_Data section might look like:
```
CL_Data(Donate:https://www.buymeacoffee.com/flown|ClientSide:true|ServerSide:false)
```

All of your data should be contained between `CL_Data(` and `)`. From there you can separate individual fields using a `|`.

The format for each piece of data is `<variable name>:<data>`. For example above I write `Donate:https://www.buymeacoffee.com/flown` which represents a donation link to be linked to in the launcher.

![An example of a donation page being linked to in the launcher](https://github.com/user-attachments/assets/67fbe4cd-79b7-4763-8ceb-abc5d0169252)
*An example of a donation page being linked to in the launcher*

## Hiding the text

Having this data in your mod's description is useful for Core Launcher users but having it visible on mod.io can be a little odd. Luckily for us mod.io provides access to the HTML data for your description which we can use to hide the text. 

The html data can be accessed by clicking on the `<>` icon when editing your mod's description on mod.io
![HTML toggle in mod.io](https://github.com/user-attachments/assets/42067315-9082-4422-8262-7fb1f3de3d57)

Surrounding your CL_Data text with `<p><span style="font-size: 0px;">` ... (your CL_Data here) `</span></p>` makes it invisible. This method will display a bit of a blank line so I'd recommend putting it at the end of your mod's description.

## List of CL_Data fields
Here is a list of all the data variables that you can set

- `Donate` Adds a button that can link to your donation page beside your mod
- `ClientSide` Specifies that your mod is client side
  - Both ClientSide and ServerSide default to true so you will only have to specify what your mod is not
- `ServerSide` Specifies that your mod is server side
  - See ClientSide for more info
- `Library` Specifies whether your mod is a library, this means that it will be hidden in the Show Library Mods button is toggled off
  - This will be auto detected if your mod has the Library tag on mod.io
- ~~`Depend` A list of your mods dependencies~~ **(Depricated)**
  - ~~You can enter the IDs of your dependencies as a comma separated list~~

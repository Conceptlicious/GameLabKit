Dialogue Writing Standards for GameLabKit
By Christian Rietbergen

All Dialogue must be written in Ink!
The Dialogue system is not to be used to display strings specified in code. 
If this is your intent, make that string as a separate knot in Ink and call the knot through the dialogue system.
Grammar and spelling should be checked by peers to ensure that it is up to professional standards. 

Ink Tutorial:
https://github.com/inkle/ink/blob/master/Documentation/WritingWithInk.md


Use PascalCasing when writing knot names and encase it with 3 equals signs (as shown in Ink Tutorial)

Example:
===KnotName===

The first Knot per dialogue should be called Intro:

Example: 
===Intro===

Aside from these rules knot names are variable and can be used as you please, as long as your script calls the relevant knots any name will work


Lines should start with a dash and whenever Dr Gamelab talks it should be surrounded by quotation marks

Example:
-"Lorem ipsum dolor sit amet."


Single-choices are not allowed, because the user is to answer questions honestly rather than reading their script. 
So make multiple choices and encase them in square brackets to ensure they do not show up in the line after. 
Empty lines are ignored by the DialogueMenu

Good:
-"Are you happy with your choice?"
* [Yes]
->DONE
* [No]
->END

Bad:

-"Are you happy with your choice?"
* Yes //This will return in a text box as a new line of dialogue after selecting
->DONE
* No //This will return in a text box as a new line of dialogue after selecting
->END


Double bad: 
-"Is this even a choice?"
* Continue
-> END


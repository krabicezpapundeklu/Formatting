# Krabicezpapundeklu.Formatting #

## What is it? ##

`String.Format` on steroids:

* Supports everything what `String.Format` does (http://msdn.microsoft.com/en-us/library/txafckwd.aspx)
* Supports expressions in format strings - useful for plural forms
* Supports named arguments

## Why should I use it? ##

You want to display number of messages. How?

	Display(string.Format("You have {0} messages.", messageCount));

It will display "You have 0 messages.", "You have 2 messages.", "You have 1 messages.".
Wait... "1 messages"? Is it plural???

So you change your code to:

	if(messageCount == 1)
	{
		Display("You have 1 message.");
	}
	else
	{
		Display(string.Format("You have {0} messages.", messageCount));
	}

If you have to support only one language you are done.
However, if you have to support multiple languages then you may have something like this:

	if(messageCount == 1)
	{
		Display(Resources.YouHave1Message);
	}
	else
	{
		Display(string.Format(Resources.YouHaveMessages, messageCount));
	}

Where resources are resolved based on current culture and for our example they are defined as follows:

<table>
	<thead>
		<tr><td>Resource</td><td>English translation</td><td>Czech translation</td></tr>
	</thead>
	<tbody>
		<tr><td>YouHave1Message</td><td>You have 1 message.</td><td>Máte 1 zprávu.</td></tr>
		<tr><td>YouHaveMessages</td><td>You have {0} messages.</td><td>Máte {0} zpráv.</td></tr>
	</tbody>
</table>

English version works, but for Czech, you now get "Máte 2 zpráv."
This is not correct - it should be "Máte 2 zprávy." (same applies for 3 and 4 messages).

So you add new resource:

<table>
	<thead>
		<tr><td>Resource</td><td>English translation</td><td>Czech translation</td></tr>
	</thead>
	<tbody>
		<tr><td>YouHave1Message</td><td>You have 1 message.</td><td>Máte 1 zprávu.</td></tr>
		<tr><td>YouHaveMessages</td><td>You have {0} messages.</td><td>Máte {0} zpráv.</td></tr>
		<tr><td>YouHave2To4Messages</td><td>You have {0} messages.</td><td>Máte {0} zprávy.</td></tr>
	</tbody>
</table>

And change code to:

	if(messageCount == 1)
	{
		Display(Resources.YouHave1Message);
	}
	else if(messageCount > 1 && messageCount < 5)
	{
		Display(string.Format(Resources.YouHave2To4Messages, messageCount));
	}
	else
	{
		Display(string.Format(Resources.YouHaveMessages, messageCount));
	}

It works, but imagine adding more languages, each having it's own rules... crazy, heh?

And what about this?

	Display(Format.Evaluate(Resources.YouHaveMessages, messageCount));

Where resources are defined as:

<table>
	<thead>
		<tr><td>Resource</td><td>English translation</td><td>Czech translation</td></tr>
	</thead>
	<tbody>
		<tr>
			<td>YouHaveMessages</td>
			<td>You have {0 {=0:no message}{=1:1 message}{else:{0} messages}}.</td>
			<td>{0 {=0:Nemáte žádnou zprávu}{else:Máte {0} {0 {=1:zprávu}{>1&lt;5:zprávy}{else:zpráv}}}}.</td>
		</tr>
	</tbody>
</table>

Now you get "You have no message.", "You have 1 message.", "You have 5 messages.", "Máte 2 zprávy.", "Nemáte žádnou zprávu." etc. Mission accomplished!

## What do these expressions mean? ##

	You have {0 {=0:no message}{=1:1 message}{else:{0} messages}}.

* Display "You have "
* Take value of first argument (message count in our case)
	* if its value is 0
		* display "no message"
	* if its value is 1
		* display "1 message"
	* else
		* evaluate `{0} messages` (yes, you can nest expressions!)
* Display "."

***
	{0} messages

* Display value of first argument (message count)
* Display " messages"

***
	{0 {=0:Nemáte žádnou zprávu}{else:Máte {0} {0 {=1:zprávu}{>1<5:zprávy}{else:zpráv}}}}.

* Take value of first argument
	* if its value is 0
		* display "Nemáte žádnou zprávu"
	* else
		* evaluate `Máte {0} {0 {=1:zprávu}{>1<5:zprávy}{else:zpráv}}`
* Display "."

***
	Máte {0} {0 {=1:zprávu}{>1<5:zprávy}{else:zpráv}}

* Display "Máte "
* Display value of first argument
* Display " "
* Take value of first argument
	* if its value is 1
		* display "zpráv"
	* if its value is greater than 1 and less than 5
		* display "zprávy"
	* else
		* display "zpráv"

### Other examples ###

	{0 {=1:...}}

If value of first argument is 1 then...

***
	{0 {=-1:...}}

If value of first argument is -1 then...

***
	{0 {={1}:...}}

If value of first argument is equal to value of second argument then...

***
	{0 {>=1<5:...}}

If value of first argument is greater or equal to 1 and less than 5 then...

***
	{0 {=1,=5:...}}

If value of first argument is 1 or 5 then...

## Comparison with "String.Format" ##

### Compatibility ###

Most ``String.Format`` format strings are compatible with ``Format``.
Padding, format providers etc. are supported.

There is howevever one difference - escapes:

* In ``String.Format`` you have to double ``{`` and ``}`` to escape them - ``{{escaped}}`` gives "{escaped}".
* In ``Format`` you have to add backslash - ``\{escaped\}`` gives "{escaped}".

This change was necessary to support nesting.

### Usage ###

Method calls are pretty same as of ``String.Format``, so it is possible to do simple find & replace:

	String.Format(IFormatProvider provider, string format, params object[] arguments)
	String.Format(string format, params object[] arguments)

vs.

	Format.Evaluate(IFormatProvider provider, string format, params object[] arguments)
	Format.Evaluate(string format, params object[] arguments)

### Performance ###

``Format`` does more than ``String.Format``, so yes, it is slower.

If you plan to reuse format expression you can "pre-compile" it, so it doesn't need to be parsed again:

	var format = Format.Parse("...");

	...
	format.Evaluate(...);
	...

### Named Arguments ###

You do not have to remember what `{0}` or `{1}` mean.
Just use overload taking `ArgumentCollection` as its input:

	var arguments = new ArgumentCollection();

	arguments.Add(1); // argument without name - accessible as {0}
	arguments.Add("MESSAGE_COUNT", 10); // argument with name, accessible both as {1} and {MESSAGE_COUNT}

	Format.Evaluate("You have {MESSAGE_COUNT {=0:no message}...", arguments);

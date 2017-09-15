## TimeRange - a TimeSpan alternative.

Today I wrote a class to build a report.  It has a method to retrieve data over a given range of dates. 
Sometimes it will one day, other times, three months or a year. I wanted to paramterize that, but that
wasn't as easy as it seems.  Now, I could give start and end dates, but that seemed a bit shaky.  The function
would be called from several places, so the range calculation would have to be repeated in several times.
I would much rather just given a start date and indicate the desired range.  My first thought was to use a `TimeSpan`.  This seems to be what it was designed for:

I would call it like this:

    rep.GenerateReport(DateTime.Today, TimeSpan.FromDays(90));

But there's a problem.  Three months isn't 90 days.  It's somewhere between 90 and 92 days, depending on the months.  And a year is only sometimes 365 days.  And because of this, there's no way to create a `TimeSpan` for "one year" or "three months" -- only a fixed number of days.  This would not work for us.

Next I considered creating an `enum`.
    
	enum TimeRange {OneDay, OneMonth, ThreeMonths, SixMonths, OneYear};

I could call it like this:

    rep.GenerateReport(DateTime.Today, TimeRange.ThreeMonths);

Which is clean and self-descriptive at the call site -- but in the method, it would require a messy
`switch/case` or worse:

<script src="https://gist.github.com/jamescurran/a5bc154106de28f3e210d23552faa5df.js"> </script>

But, how many elements do I define in the enum? OneDay, TwoDays, FiveDays, TwelveDays?  What do I
do if the mthod is called is called with a enum not defined in the switch? If I want to use that enum somewhere else do I have to copy'n'paste that switch block around?  If I want to refactor that into a function, where do I put it?



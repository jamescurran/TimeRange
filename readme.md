## TimeRange - a TimeSpan alternative.

Today I wrote a class to build a report.  It has a method to retrieve data over a given range of dates. 
Sometimes it will one day, other times, three months or a year. I wanted to paramterize that, but that
wasn't as easy as it seems.  Now, I could give start and end dates, but that seemed a bit shaky.  The function
would be called from several places, so the range calculation would have to be repeated several times.
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

(Gist of ugly code this would cause [here](https://gist.github.com/jamescurran/a5bc154106de28f3e210d23552faa5df))

And... how many elements do I define in the enum? OneDay, TwoDays, FiveDays, TwelveDays?  What do I
do if the method is called with a enum not defined in the switch? If I want to use that enum somewhere else do I have to copy'n'paste that switch block around?  If I want to refactor that into a function, where do I put it?

So, who 'bout I create a class with static properties, so it looks just like an enum, and I can slide some real code in there, to do the work.  So, we can still write the call as 

    rep.GenerateReport(DateTime.Today, TimeRange.OneMonth);

Then inside the method, it would just be:

     public void GenerateReport(DateTime endDate, TimeRange range)
      {
            DateTime startDate = endDate.Add(range);


(*"Wait a minute -- wasn't that `ThreeMonths` last time ??  And didn't you Subtract the days? "* -- Shh, we'll get to that in a minute.)

That gives us the basic version of this:
<script src="https://gist.github.com/jamescurran/24e4e1cfb4207feb406d7b46f593a272.js"> </script>
(link to gist [here](https://gist.github.com/jamescurran/24e4e1cfb4207feb406d7b46f593a272))

Each object holds a reference to a function which does the actual conversion we want, which is set in it's it's constructor, which is private so the only instances of `TimeRange` are the properties. So, this solves all the problems we had with the `enum`: Using it is just one line, and each one has the needed function.   

Well, almost all the problems with the enums.  We still have the problem of which properties to define.  OneMonth/TwoMonth/ThreeMonth ?  And Subtration.

Fortunately, both these problems lead to the same solution: Stop hard-coding the "1" in the AddDays/AddMonths/AddYears, and make `count` a field.

        private readonly int _count;
        private TimeRange(Func<DateTime, int, DateTime> delta, int count = 1)
        {
            _delta = delta;
            _count = count;
        }
		// :
		// :
	    public static readonly TimeRange OneDay = new TimeRange((dt, count) => dt.AddDays(count));
        public static readonly TimeRange OneYear = new TimeRange((dt, count) => dt.AddYears(count));
        public static readonly TimeRange OneMonth = new TimeRange((dt, count) => dt.AddMonths(count));
   
For Subtraction, we just need to be able to negate a `TimeRange` object -- which means create a new one with a negative count -- with a unary minus operator:

        public static TimeRange operator -(TimeRange tr)
        {
            return new TimeRange(tr._delta, -tr._count);
        }

OK, That let's use add a negative `TimeRange` but we really want to subtract a positive one.  And while we are at it, you may have noticed that we've been adding a date to a `TimeRange` (`timeRange.Add(date)`) when it would be more natural to add the TimeRange to the DateTime. So, next we rename our Add() method to Transform(), and put in a couple extension methods on `DateTime`:

    public static class TimeRangeExt
    {
        public static DateTime Add(this DateTime date, TimeRange range)
        {
            return range.Transform(date);
        }

        public static DateTime Subtract(this DateTime date, TimeRange range)
        {
            return Add(date, -range);
        }
    }

Finally, we need a way to change the `count` value of a `TimeRange`.  Cue the multiplication operators:

        public static TimeRange operator *(TimeRange tr, int multiplier)
        {
            return new TimeRange(tr._delta, tr._count * multiplier);
        }
        public static TimeRange operator *(int multiplicand, TimeRange tr)
        {
            return new TimeRange(tr._delta, multiplicand * tr._count);
        }

I defined two so you can write `date.Add(3*timeRange)` or `date.Subtract(timeRange * 3)`. 

Now, can we get rid of those `Add` and `Subtract` methods and use plus & minus, like normal people ?


A word about comparibility: I tried to make a `TimeRange` `IComaprable`.  Couldn't do it. Any two lambdas are equal if their signatures match. I'd have to add another field which would state the type (Day/month/year), but then, we'd either have to make 24*TimeRange.OneMonth less than TimeRange.OneYear or add a whole bunch more logic to work out which is more --- which was the original problem this set out to avoid.




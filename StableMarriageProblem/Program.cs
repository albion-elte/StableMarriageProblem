Console.WriteLine("========================================================");
Console.WriteLine("================== Solution Usin List ==================");
Console.WriteLine("========================================================");

SolutionUsingList();

Console.WriteLine("=======================================================");
Console.WriteLine("================= Solution Usin Queue =================");
Console.WriteLine("=======================================================");

SolutionUsingQueue();

void SolutionUsingList()
{
    var boysPreferences = new Dictionary<int, List<int>>()
{
    {1, new List<int> { 3, 2, 5, 1, 4} },
    {2, new List<int> { 1, 2, 5, 4, 3} },
    {3, new List<int> { 4, 3, 2, 1 ,5} },
    {4, new List<int> { 1, 3, 4, 2, 5} },
    {5, new List<int> { 1, 2, 4, 5 ,3} }
};

    var girlsPreferences = new Dictionary<int, List<int>>()
{
    {1, new List<int> { 3, 5, 2, 1, 4} },
    {2, new List<int> { 5, 2, 1, 4, 3} },
    {3, new List<int> { 4, 3, 5, 1, 2} },
    {4, new List<int> { 1, 2, 3, 4, 5} },
    {5, new List<int> { 2, 3, 4, 1, 5} }
};

    var currentStateInGirlsBalcony = new Dictionary<int, List<int>>()
{
    {1, new List<int>() },
    {2, new List<int>() },
    {3, new List<int>() },
    {4, new List<int>() },
    {5, new List<int>() }
};

    var dayCount = 1;

    List<int> unmatchedBoys = new() { 1, 2, 3, 4, 5 };
    do
    {
        if (dayCount > 1)
        {
            var girlsWithMultipleOptions = currentStateInGirlsBalcony.Where(x => x.Value.Count() > 1).ToDictionary();


            foreach (var item in girlsWithMultipleOptions)
            {
                int boyToKeep = 0;
                foreach (var preference in girlsPreferences[item.Key])
                {
                    if (item.Value.Contains(preference))
                    {
                        boyToKeep = preference;
                        break;
                    }
                }

                unmatchedBoys = new List<int>();
                unmatchedBoys.AddRange(item.Value.Where(x => x != boyToKeep).ToList());

                currentStateInGirlsBalcony[item.Key] = new List<int> { boyToKeep };

            }
        }

        foreach (var item in boysPreferences.Where(x => unmatchedBoys.Contains(x.Key)))
        {
            currentStateInGirlsBalcony[item.Value[0]].Add(item.Key);
            item.Value.RemoveAt(0);
        }
        dayCount++;
    }
    while (currentStateInGirlsBalcony.Values.Any(x => x.Count() < 1));

    foreach (var item in currentStateInGirlsBalcony)
    {
        Console.WriteLine($"g{item.Key} matched with b{item.Value.First()}");
    }
}

void SolutionUsingQueue()
{
    var boysPreferences = new Dictionary<int, Queue<int>>()
{
    {1, new Queue<int>(new[] { 3, 2, 5, 1, 4 })},
    {2, new Queue<int>(new[] { 1, 2, 5, 4, 3 })},
    {3, new Queue<int>(new[] { 4, 3, 2, 1, 5 })},
    {4, new Queue<int>(new[] { 1, 3, 4, 2, 5 })},
    {5, new Queue<int>(new[] { 1, 2, 4, 5, 3 })}
};

    var girlsPreferences = new Dictionary<int, List<int>>()
{
    {1, new List<int> { 3, 5, 2, 1, 4 }},
    {2, new List<int> { 5, 2, 1, 4, 3 }},
    {3, new List<int> { 4, 3, 5, 1, 2 }},
    {4, new List<int> { 1, 2, 3, 4, 5 }},
    {5, new List<int> { 2, 3, 4, 1, 5 }}
};

    var girlProposals = new Dictionary<int, int>();
    var boyProposals = new Dictionary<int, int>();
    var freeBoys = new Queue<int>(new[] { 1, 2, 3, 4, 5 });

    while (freeBoys.Count > 0)
    {
        int boy = freeBoys.Dequeue();
        int girl = boysPreferences[boy].Dequeue();

        if (!girlProposals.ContainsKey(girl))
        {
            girlProposals[girl] = boy;
            boyProposals[boy] = girl;
        }
        else
        {
            int currentBoy = girlProposals[girl];
            if (girlsPreferences[girl].IndexOf(boy) < girlsPreferences[girl].IndexOf(currentBoy))
            {
                freeBoys.Enqueue(currentBoy);
                girlProposals[girl] = boy;
                boyProposals[boy] = girl;
            }
            else
            {
                freeBoys.Enqueue(boy);
            }
        }
    }

    foreach (var proposal in girlProposals.OrderBy(x => x.Key))
    {
        Console.WriteLine($"Girl {proposal.Key} is matched with Boy {proposal.Value}");
    }
}
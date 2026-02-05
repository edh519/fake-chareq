// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using System.Collections.Generic;
using System;

public class Review
{
    public User user { get; set; }
    public string body { get; set; }
    public string state { get; set; }
    public DateTime submitted_at { get; set; }
}

public class Root
{
    public string PR_Body { get; set; }
    public List<Review> Reviews { get; set; }
    public long RepositoryId { get; set; }
}

public class User
{
    public string login { get; set; }
}


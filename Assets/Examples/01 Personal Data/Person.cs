public class Person
{
    public int id;
    public string firstName;
    public string lastName;
    public int age;
    public bool hadCovid;
    public CovidRelationLevel covidRelationLevel;
    public int postNumber;
    public bool hasPet;
    public int cohabitantsCount;
    public int steamGamesCount;
    public int siblingsCount;
    

    public enum CovidRelationLevel
    {
        Family, FamilyOrFriend, Anyone, None
    }

    public Person(int _id)
    {
        id = _id;
    }
}

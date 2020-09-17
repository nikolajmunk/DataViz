using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class PersonalDataExample : MonoBehaviour
{
    public string dataCsvFileName = "";
    public List<Person> _people = new List<Person>();
    public int _ageMin, _ageMax;


    void Awake()
    {
        string csvFilePath = Application.streamingAssetsPath + "/" + dataCsvFileName;
        //Debug.Log(csvFilePath);

        string csvContent = File.ReadAllText(csvFilePath);
        //Debug.Log(csvContent);

        Parse(csvContent);

        //Debug.Log("_people.Count: " + _people.Count);
        Filter();

        Mine();

        Represent();
    }

   bool StringToBool(string input, out bool output)
    {
        input = input.ToLower();
        output = true;
        if (input == "yes" || input == "true" || input == "ja" || input == "1") { output = true; return true; }
        if (input == "no" || input == "false" || input == "nej" || input == "0") { output = false; return true; }
        return false;
    }

    void Parse(string csvText)
    {
        string[] rowContents = csvText.Split('\n');

        for (int i = 1; i < rowContents.Length; i++)
        {
            string rowContent = rowContents[i];
            string[] fieldContents = rowContent.Split(',');
            Person person = new Person(i);
            for (int f = 0; f < fieldContents.Length; f++)
            {
                string fieldContent = fieldContents[f];

                switch(f)
                {
                    case 0:
                        // First name
                        person.firstName = fieldContent;
                        break;
                    case 1:
                        person.lastName = fieldContent;
                        // Last name
                        break;
                    case 2:
                        // Age
                        int age;
                        if (int.TryParse(fieldContent, out age)) { person.age = age; }
                        break;
                    case 3:

                        break;
                    case 7:
                        int postNumber;

                        if (int.TryParse(fieldContent, out postNumber)) { person.postNumber = postNumber; }
                        break;
                    case 8:
                        bool hasPet;
                        //if (bool.TryParse(fieldContent, out hasPet)) { person.hasPet = hasPet; Debug.Log(hasPet); }
                        if (StringToBool(fieldContent, out hasPet)) { person.hasPet = hasPet; Debug.Log("has pet: " + person.hasPet); }
                        break;
                    case 9:
                        int cohabitantsCount;

                        if (int.TryParse(fieldContent, out cohabitantsCount)) { person.cohabitantsCount = cohabitantsCount; }
                        break;
                    case 10:
                        int steamGamesCount;

                        if (int.TryParse(fieldContent, out steamGamesCount)) { person.steamGamesCount = steamGamesCount; }
                        break;
                    case 11:
                        int siblingsCount;

                        if (int.TryParse(fieldContent, out siblingsCount)) { person.siblingsCount = siblingsCount; }
                        break;

                        // To do: fix covid relation level fra CECs.
                }
            }

            // Parse covid relation level.
            Person.CovidRelationLevel covidRelationLevel = Person.CovidRelationLevel.None;
            if (fieldContents.Length > 6)
            {
                bool familyHadCovid = fieldContents[4].ToLower() == "yes";
                bool familyOrFriendsHadCovid = fieldContents[5].ToLower() == "yes";
                bool anyoneHadCovid = fieldContents[6].ToLower() == "yes";
                if (anyoneHadCovid) covidRelationLevel = Person.CovidRelationLevel.Anyone;
                else if (familyOrFriendsHadCovid) covidRelationLevel = Person.CovidRelationLevel.FamilyOrFriend;
                else if (familyOrFriendsHadCovid) covidRelationLevel = Person.CovidRelationLevel.Family;
            }
            person.covidRelationLevel = covidRelationLevel;

            _people.Add(person);
        }

    }

    void Filter()
    {
        for (int p = _people.Count-1; p >= 0; p--)
        {
            Person person = _people[p];
            
            if (person.age < 18 || person.age > 127)
            {
                _people.RemoveAt(p);
                Debug.Log("Invalid: " + person.firstName);
            }

        }
    }

    void Mine()
    {
        _ageMin = int.MaxValue;
        _ageMax = int.MinValue;
        foreach (Person person in _people)
        {
            if (person.age > _ageMax)
            {
                _ageMax = person.age;
            }
            if (person.age < _ageMin)
            {
                _ageMin = person.age;
            }
        }
        Debug.Log("Min: " + _ageMin + ", Max: " + _ageMax);
    }

    void Represent()
    {
        for (int p = 0; p < _people.Count; p++)
        {
            Person person = _people[p];

            float x = p;
            float height = Mathf.InverseLerp(_ageMin, _ageMax, person.age) * 10;
            float y = height * 0.5f;
            float width = .95f;

            GameObject mainObject = new GameObject(string.Join(" ", person.id, person.firstName));
            mainObject.transform.SetParent(transform);
            mainObject.transform.localPosition = new Vector3(x, y, 0);


            GameObject barObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            barObject.transform.SetParent(mainObject.transform);
            barObject.transform.localPosition = Vector3.zero;
            barObject.transform.localScale = new Vector3(1, height, 1);
        }
    }
}

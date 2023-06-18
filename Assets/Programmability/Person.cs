using System;
using System.Collections.Generic;
using System.Linq;
#nullable enable
public class Person : IEquatable<Person>
{
    public string Name;
    public static List<Person> people => GameController.Instance.people;
    public static Person player = "Brigid";

    private Person(string name)
    {
        Name = name;
        people.Add(this);
    }

    public static Person GetPerson(string name)
    {
        return people.SingleOrDefault(p => p.Name == name) ?? new(name);
    }

    public static implicit operator Person(string name) => GetPerson(name);
    public static implicit operator string(Person person) =>  person is null ? string.Empty : person.Name;
    public override string ToString() => Name;
    public override bool Equals(object obj)
    {
        return Equals(obj as Person);
    }

    public bool Equals(Person? other)
    {
        if (other != null && Name == other.Name) 
            return true;
        return false;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}

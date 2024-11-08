using System.Reflection;

namespace HomeExercise.Tasks.ObjectComparison;

public class Person : IEquatable<Person>
{
    public static int IdCounter = 0;
    public int Age, Height, Weight;
    public string Name;
    public Person Parent;
    public int Id;
    
    private static readonly FieldInfo[] Fields;

    static Person()
    {
        Fields = typeof(Person).GetFields();
    }

    public Person(string name, int age, int height, int weight, Person parent)
    {
        Id = IdCounter++;
        Name = name;
        Age = age;
        Height = height;
        Weight = weight;
        Parent = parent;
    }

    public bool Equals(Person? other)
        => Fields
            .Where(field => field.Name != nameof(Id))
            .Where(field => field.GetValue(this) != null || field.GetValue(other) != null)
            .All(field => field.GetValue(this) != null && field.GetValue(this)!.Equals(field.GetValue(other)));

    public override bool Equals(object obj) => obj is Person person && Equals(person);
}
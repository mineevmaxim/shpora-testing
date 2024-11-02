using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.ObjectComparison;
public class ObjectComparison
{
    private Person actualTsar;
    private Person expectedTsar;

    [SetUp]
    public void Setup()
    {
        actualTsar = TsarRegistry.GetCurrentTsar();
        expectedTsar = CreateDefaultTsar();
    }
    
    [Test]
    [Description("Проверка текущего царя")]
    [Category("ToRefactor")]
    public void CheckCurrentTsar()
    {
        actualTsar.Name.Should().Be(expectedTsar.Name);
        actualTsar.Age.Should().Be(expectedTsar.Age);
        actualTsar.Height.Should().Be(expectedTsar.Height);
        actualTsar.Weight.Should().Be(expectedTsar.Weight);

        actualTsar.Parent!.Name.Should().Be(expectedTsar.Parent!.Name);
        actualTsar.Parent.Age.Should().Be(expectedTsar.Parent.Age);
        actualTsar.Parent.Height.Should().Be(expectedTsar.Parent.Height);
        actualTsar.Parent.Parent.Should().Be(expectedTsar.Parent.Parent);
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        // Какие недостатки у такого подхода? 
        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));
    }

    private bool AreEqual(Person? actual, Person? expected)
    {
        if (actual == expected) return true;
        if (actual == null || expected == null) return false;
        return
            actual.Name == expected.Name
            && actual.Age == expected.Age
            && actual.Height == expected.Height
            && actual.Weight == expected.Weight
            && AreEqual(actual.Parent, expected.Parent);
    }
    
    private static Person CreateDefaultTsar()
        => new ("Ivan IV The Terrible", 54, 170, 70, CreateDefaultParent());

    private static Person CreateDefaultParent()
        => new ("Vasili III of Russia", 28, 170, 60, null);
}

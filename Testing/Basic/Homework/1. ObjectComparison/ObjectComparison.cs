using FluentAssertions;
using NUnit.Framework;

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

    // Плюсы моего подхода:
    // При расширении класса Person, программа не скомпилируется, если не поменять код (предотвращает ошибки).
    // Код приходится менять только в генераторах объектов
    // При расширении класса Person, нам не нужно изменять тест (первый тест придется, компилятор не подскажет)
    // Засчет рефлексии нам не нужно менять код Equals в Person даже при расширении
    // Тест в одну строчку
    // P.S. создание объектов вынесено в Setup. Мы не сравниваем Id, создавать каждый раз новые данные для теста нет необходимости
    
    [Test]
    public void GetCurrentTsar_ShouldReturn_ValidTsar() => actualTsar.Should().BeEquivalentTo(expectedTsar);

    [Test]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        // Какие недостатки у такого подхода? 
        // AreEqual не содержит в себе Assert, при падении теста мы не знаем, что произошло (неинформативный вывод)
        // При расширении класса Person, нам необходимо менять код уже написанного метода AreEqual
        // и компилятор нам не подскажет, что это нужно сделать
        // В тесте только вызывается метод AreEqual, приходится писать инфраструктурный код для его создания
        AreEqual(actualTsar, expectedTsar).Should().BeTrue();
    }

    private static bool AreEqual(Person? actual, Person? expected)
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
        => new("Ivan IV The Terrible", 54, 170, 70, CreateDefaultParent());

    private static Person CreateDefaultParent()
        => new("Vasili III of Russia", 28, 170, 60, null);
}

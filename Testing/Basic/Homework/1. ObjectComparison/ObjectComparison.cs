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

    [Test]
    [Description("Проверка текущего царя")]
    [Category("ToRefactor")]
    public void CheckCurrentTsar()
    {
        actualTsar.Name.Should().Be(expectedTsar.Name);
        actualTsar.Age.Should().Be(expectedTsar.Age);
        actualTsar.Height.Should().Be(expectedTsar.Height);
        actualTsar.Weight.Should().Be(expectedTsar.Weight);

        actualTsar.Parent.Name.Should().Be(expectedTsar.Parent.Name);
        actualTsar.Parent.Age.Should().Be(expectedTsar.Parent.Age);
        actualTsar.Parent.Height.Should().Be(expectedTsar.Parent.Height);
        actualTsar.Parent.Parent.Should().Be(expectedTsar.Parent.Parent);
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        // Какие недостатки у такого подхода? 
        // AreEqual не содержит в себе Assert, при падении теста мы не знаем, что произошло (неинформативный вывод)
        // При расширении класса Person, нам необходимо менять код уже написанного метода AreEqual
        // и компилятор нам не подскажет, что это нужно сделать
        // В тесте только вызывается метод AreEqual, приходится писать инфраструктурный код для его создания

        // Плюсы моего подхода:
        // При расширении класса Person, программа не скомпилируется, если не поменять код (предотвращает ошибки).
        // Код приходится менять только в генераторах объектов
        // При расширении класса Person, нам не нужно изменять тест (первый тест придется, компилятор не подскажет)
        // Логика сравнения перенесена в сам тест, не приходится писать лишний инфраструктурный код
        // P.S. создание объектов вынесено в Setup. Мы не сравниваем Id, создавать каждый раз новые данные для теста нет необходимости
        actualTsar.Should().BeEquivalentTo(expectedTsar, options =>
            options.Excluding(y => y.Id).Excluding(y => y.Parent.Id));
    }

    private static Person CreateDefaultTsar()
        => new("Ivan IV The Terrible", 54, 170, 70, CreateDefaultParent());

    private static Person CreateDefaultParent()
        => new("Vasili III of Russia", 28, 170, 60, null);
}

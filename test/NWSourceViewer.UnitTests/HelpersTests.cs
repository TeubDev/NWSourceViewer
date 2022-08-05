namespace NWSourceViewer.UnitTests;

public class HelpersTests
{
    [TestCase(-1, (uint)0, (uint)20, (uint)40, (uint)20, (uint)40, TestName = "40-level max in config. Standard base class.")]
    [TestCase(10, (uint)40, (uint)20, (uint)40, (uint)10, (uint)30, TestName = "40-level max in config. 10-level pre, 30-level total prestige class with more levels listed in 2da than allowed.")]
    [TestCase(10, (uint)5, (uint)20, (uint)40, (uint)5, (uint)5, TestName = "40-level max in config. 5-level class.")]
    [TestCase(-1, (uint)0, (uint)20, (uint)30, (uint)20, (uint)30, TestName = "30-level max in config. Standard base class.")]
    [TestCase(10, (uint)40, (uint)20, (uint)30, (uint)10, (uint)20, TestName = "30-level max in config. 10-level pre, 20-level total prestige class with more levels listed in 2da than allowed.")]
    [TestCase(10, (uint)5, (uint)20, (uint)30, (uint)5, (uint)5, TestName = "30-level max in config. 5-level class.")]
    [TestCase(-1, (uint)0, (uint)20, (uint)20, (uint)20, (uint)20, TestName = "20-level max in config. Standard base class.")]
    [TestCase(10, (uint)40, (uint)20, (uint)20, (uint)10, (uint)10, TestName = "20-level max in config. 10-level pre, 10-level total prestige class with more levels listed in 2da than allowed.")]
    [TestCase(10, (uint)5, (uint)20, (uint)20, (uint)5, (uint)5, TestName = "20-level max in config. 5-level class.")]
    public void GetMaxLevelsForClass(int maxPreEpicInClass, uint maxInClass, uint defaultMaxPreEpic, uint defaultMax, uint expectedPreEpic, uint expectedMax)
    {
        ClassModel classModel = new ClassModel
        {
            EpicLevel = maxPreEpicInClass,
            MaxLevel = maxInClass
        };

        var result = classModel.GetMaxLevels(defaultMaxPreEpic, defaultMax);

        Assert.Multiple(() =>
        {
            Assert.That(result.maxPreEpicLevel, Is.EqualTo(expectedPreEpic));
            Assert.That(result.maxLevel, Is.EqualTo(expectedMax));
        });
    }

    [Test]
    public void Split2daRow()
    {
        string valueToSplit = " stuff    \tmore \"inside quotes\"";
        var result = valueToSplit.Split2daRow();
        Assert.Multiple(() =>
        {
            Assert.That(result[0], Is.EqualTo("stuff"));
            Assert.That(result[1], Is.EqualTo("more"));
            Assert.That(result[2], Is.EqualTo("inside quotes"));
        });
    }
}

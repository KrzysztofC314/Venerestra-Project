using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DialogueParserTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void DialogueParserTestSimplePasses()
    {
        var parser = new DialogueParser(@".\Assets\Programmability\Tests\ParserInput.html");
        string expectedStartNodeTitle = "Possible phone call with Sam (2)";

        var dialogue = parser.Parse();

        Assert.AreEqual(expectedStartNodeTitle, dialogue.Title);
        Assert.AreEqual(2, dialogue.Responses.Length);
        foreach (var response in dialogue.Responses)
        {
            Assert.IsNotNull(response);
        }
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator DialogueParserTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}


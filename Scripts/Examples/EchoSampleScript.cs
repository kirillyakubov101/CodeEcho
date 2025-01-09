using UnityEngine;

/// <summary>
/// Please only use these types of params
/// INT | Float | String | None
/// </summary>

namespace CodeEcho.Samples
{
    public class EchoSampleScript : MonoBehaviour, ICodeEcho
    {
        [CodeEchoMark("SayHello")]
        public void SayHello()
        {
            Debug.Log("Hello, world!");
        }

        [CodeEchoMark("PrintNumber")]
        public void AddNumbers(int a)
        {
            Debug.Log($"Number: {a}");
        }

        [CodeEchoMark("Printfloat")]
        public void Addfloat(float a)
        {
            Debug.Log($"float: {a}");
        }

        [CodeEchoMark("PrintString")]
        public void AddString(string a)
        {
            Debug.Log($"string: {a}");
        }

        [CodeEchoMark]
        public void EmptyFunc()
        {
            Debug.Log("EmptyFunc");
        }

        [CodeEchoMark("ComplexFunc")]
        public void Complex_function_name_very_long()
        {
            Debug.Log("Do Complect Action");
        }
    }
}



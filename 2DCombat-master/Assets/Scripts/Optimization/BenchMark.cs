using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class BenchMark : MonoBehaviour
{
    [Range(0f, 10000000), SerializeField] float _iterations;

    private BenchMarkTest _benchMarkTest;

    private void Awake()
    {
        _benchMarkTest = GetComponent<BenchMarkTest>();
    }

    [ContextMenu("RunTest")]
    public void RunTest()
    {
        Stopwatch sw = Stopwatch.StartNew();
        sw.Start();

        for(int i=0; i< _iterations; i++)
        {
            _benchMarkTest.PerformBenchMarkTest();
        }
        sw.Stop();

        Debug.Log(sw.ElapsedMilliseconds + "ms");
    }
}

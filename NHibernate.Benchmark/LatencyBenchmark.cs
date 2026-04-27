using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Benchmark;

[SimpleJob(warmupCount: 3, iterationCount: 10, launchCount: 1)]
[MinColumn, MaxColumn]
public class LatencyBenchmark
{
    private Ping _ping;
    private byte[] _buffer;
    private PingOptions _options;

    private const string Host = "localhost";
    private const int Timeout = 1000;

    [GlobalSetup]
    public void Setup()
    {
        _ping = new Ping();
        _buffer = new byte[32];
        _options = new PingOptions(64, true);
    }

    [Benchmark]
    public long Latency()
    {
        var reply = _ping.Send(Host, Timeout, _buffer, _options);
        return reply.Status == IPStatus.Success ? reply.RoundtripTime : -1;
    }
}

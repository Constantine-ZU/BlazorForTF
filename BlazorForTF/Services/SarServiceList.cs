using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class SarServiceList
{
    public async Task<List<SarData>> RunSarAsync()
    {
        if (Environment.OSVersion.Platform == PlatformID.Unix)
        {
            return await RunSarOnUnixAsync();
        }
        else
        {
            return GenerateMockSarData();
        }
    }

    private async Task<List<SarData>> RunSarOnUnixAsync()
    {
        var sarOutput = new List<SarData>();
        var psi = new ProcessStartInfo
        {
            FileName = "/usr/bin/sar",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = new Process { StartInfo = psi })
        {
            process.Start();

            // Reading output
            while (!process.StandardOutput.EndOfStream)
            {
                string line = await process.StandardOutput.ReadLineAsync();
                var data = ParseSarOutput(line);
                if (data != null)
                {
                    sarOutput.Add(data);
                }
            }

            process.WaitForExit();
        }

        return sarOutput;
    }

    private List<SarData> GenerateMockSarData()
    {
        var sarData = new List<SarData>();
        var currentTime = DateTime.Now;
        Random random = new Random(DateTime.Now.Millisecond);
        for (int i = 0; i < 40; i++)
        {
            sarData.Add(new SarData
            {
                Time = currentTime.AddMinutes(-10 * i),//.ToString("HH:mm:ss"),
                CPU = "all",
                User = random.Next(0, 50) * random.NextDouble(),
                Nice = random.Next(5, 10) * random.NextDouble(),
                System = random.Next(10, 20) * random.NextDouble(),
                IOWait = random.Next(20, 30) * random.NextDouble(),
                Steal = random.Next(30, 40) * random.NextDouble(),
                Idle = random.Next(80, 90) * random.NextDouble(),  // Typically Idle will be high if other values are low
            }) ;
        }

        return sarData;
    }

    private SarData ParseSarOutput(string outputLine)
    {
        // Regex to extract the data from the sar output
        Regex regex = new Regex(@"(\d{2}:\d{2}:\d{2})\s+all\s+(\d+\.\d+)\s+(\d+\.\d+)\s+(\d+\.\d+)\s+(\d+\.\d+)\s+(\d+\.\d+)\s+(\d+\.\d+)");
        var match = regex.Match(outputLine);

        if (match.Success)
        {
            return new SarData
            {
                Time = DateTime.Parse(match.Groups[1].Value),
                CPU = "all",
                User = double.Parse(match.Groups[2].Value),
                Nice = double.Parse(match.Groups[3].Value),
                System = double.Parse(match.Groups[4].Value),
                IOWait = double.Parse(match.Groups[5].Value),
                Steal = double.Parse(match.Groups[6].Value),
                Idle = double.Parse(match.Groups[7].Value)
            };
        }

        return null;
    }
}



|             Method | Categories |       Mean |    Error |   StdDev | Ratio | RatioSD |    Gen0 |    Gen1 | Allocated | Alloc Ratio |
|------------------- |----------- |-----------:|---------:|---------:|------:|--------:|--------:|--------:|----------:|------------:|
|        Complex_Mvc |    Complex | 1,074.5 us | 20.83 us | 30.54 us |  1.00 |    0.00 | 46.8750 | 11.7188 | 308.92 KB |        1.00 |
|    Complex_Minimal |    Complex |   988.9 us | 12.96 us | 12.13 us |  0.91 |    0.03 | 46.8750 | 11.7188 | 294.17 KB |        0.95 |
|                    |            |            |          |          |       |         |         |         |           |             |
|            Get_Mvc |        Get |   262.0 us |  4.06 us |  3.80 us |  1.00 |    0.00 | 15.6250 |       - |  98.17 KB |        1.00 |
|        Get_Minimal |        Get |   229.2 us |  2.47 us |  2.19 us |  0.88 |    0.01 | 15.1367 |       - |  92.86 KB |        0.95 |
|                    |            |            |          |          |       |         |         |         |           |             |
|     Hypermedia_Mvc | Hypermedia |   362.0 us |  5.57 us |  5.21 us |  1.00 |    0.00 | 20.0195 |  2.9297 | 123.38 KB |        1.00 |
| Hypermedia_Minimal | Hypermedia |   320.1 us |  4.19 us |  3.92 us |  0.88 |    0.02 | 18.5547 |  2.9297 | 115.32 KB |        0.93 |


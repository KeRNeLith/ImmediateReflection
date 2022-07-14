using System;
using System.Runtime.CompilerServices;
using ImmediateReflection;

[assembly: CLSCompliant(true)]
[assembly: InternalsVisibleTo("ImmediateReflection.Benchmark" + PublicKey.Key)]
[assembly: InternalsVisibleTo("ImmediateReflection.Tests" + PublicKey.Key)]
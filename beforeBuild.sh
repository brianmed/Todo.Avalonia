#!/bin/bash

cat <<EOT > WhenBuilt.cs
using System;

namespace Todo
{
    public static class WhenBuilt
    {
        public static DateTime ItWas = DateTime.Parse("$(date '+%FT%T')");
    }
}
EOT

exit 0

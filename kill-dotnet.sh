#!/bin/bash
# Kill all dotnet processes for this project
pkill -f "dotnet.*OT.PresentationLayer" 2>/dev/null || true
echo "Dotnet processes killed (if any were running)"
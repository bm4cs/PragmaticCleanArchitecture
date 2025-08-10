#!/usr/bin/env bash

##########################################################################
# Cake build script bootstrap for Bash
##########################################################################

set -e

TARGET="Default"
CONFIGURATION="Release"

# Parse command line arguments
while [[ $# -gt 0 ]]; do
    case $1 in
        --target)
            TARGET="$2"
            shift 2
            ;;
        --configuration)
            CONFIGURATION="$2"
            shift 2
            ;;
        *)
            echo "Unknown argument: $1"
            exit 1
            ;;
    esac
done

echo "🚀 Preparing to run build script..."

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK is required to run this script. Please install .NET SDK from https://dotnet.microsoft.com/download"
    exit 1
fi

echo "📦 Restoring Cake tool..."
dotnet tool restore
if [ $? -ne 0 ]; then
    echo "❌ Failed to restore .NET tools"
    exit 1
fi

echo "🍰 Running Cake build script..."
dotnet cake build.cake --target="$TARGET" --configuration="$CONFIGURATION"
if [ $? -ne 0 ]; then
    echo "❌ Cake build failed"
    exit 1
fi

echo "✅ Build script completed successfully!"
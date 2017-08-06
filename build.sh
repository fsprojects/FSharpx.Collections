#!/bin/sh

mono .paket/paket.bootstrapper.exe
exit_code=$?
if [ $exit_code -ne 0 ]; then
	exit $exit_code
fi

mono .paket/paket.exe restore
exit_code=$?
if [ $exit_code -ne 0 ]; then
	exit $exit_code
fi

export VisualStudioVersion=14.0
mono packages/FAKE/tools/FAKE.exe $@ --fsiargs -d:MONO build.fsx 
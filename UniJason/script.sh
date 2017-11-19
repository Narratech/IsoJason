#!/bin/sh
if [ -f "$PWD/MASRunning.txt" ]
then
    echo "already running"
else
    echo NULL > MASRunning.txt
    java -jar "$PWD/Assets/UniJason/server.jar"
    echo "jar cerrado"
    rm MASRunning.txt
fi
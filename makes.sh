#!/bin/bash
cd ./CLIENT_1/
gmcs -t:library p2p_Client.cs  
gmcs -addmodule:p2p_Client.dll -pkg:glade-sharp-2.0 -resource:sharetubelogin.glade sharetube.cs

cd ../CLIENT_2/
gmcs -t:library p2p_Client.cs  
gmcs -addmodule:p2p_Client.dll -pkg:glade-sharp-2.0 -resource:sharetubelogin.glade sharetube.cs

cd ../CLIENT_3/
gmcs -t:library p2p_Client.cs  
gmcs -addmodule:p2p_Client.dll -pkg:glade-sharp-2.0 -resource:sharetubelogin.glade sharetube.cs

cd ../INDEX_SERVER/
gmcs indexServer.cs



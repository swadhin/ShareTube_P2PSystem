using System.Net;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.IO;
using System.Configuration;


namespace serverApplication
{
    public class indexServer
    {
		public class clientShareFileDB
		{
		public int clid;
		public String clfname;
		public String clfpath;
		public String clfsize;
		public String clip;
		
		}
		public clientShareFileDB[] csfdb=new clientShareFileDB[500];
		public String[,] usrpwd;
		public static int num=0;
		const int MAX_CLIENTS = 500;
		
		public AsyncCallback pfnWorkerCallBack ;
		private  Socket m_mainSocket;
		private  Socket [] m_workerSocket = new Socket[500];
		private int m_clientCount = 0;
		private int thID;
		public int crid;
		public int firsttime=1;
		public String filelistinfo=null;
		public String[,] userlist;
		public String cport;
	 	public indexServer()
                {
			m_clientCount=0;
			usrpwd = new String[50,2];
			userlist = new String[50,4];
                }

		
		public void database()
		{
			int i=0;
			int j=0;

			for(i=0;i<10;i++)
			{
				for(j=0;j<2;j++)
				{
				 usrpwd[i,j]="abcd"+i+j;
				}
			}
		}

		public int authenticate(String userpwd)
		{
			int i=0,j=0;
			for(i=0;i<userpwd.Length;i++)
			{
			     if(j<2)
				{
				if(userpwd[i]=='\n')
				j++;
				}
			      if(j==2)
				break;
			}
			int k=i;
			String nmpd=userpwd.Substring(0,k);
		
			cport=userpwd.Substring(k+1,userpwd.Length-k-3);
			Console.WriteLine("*****@#@ {0} &*& {1}",nmpd,cport);
			/*
			for(i=0;i<userpwd.Length;i++)
			Console.WriteLine("{0} #",userpwd[i]);
			*/
                        for(i=0;i<10;i++)
                        {
				 String here=this.usrpwd[i,0]+'\n'+this.usrpwd[i,1];
				 Console.WriteLine("??? {0} ",here);

                                 if(here==nmpd)
				 {
					return 1;
				 }
                        }
			return 0;

		}

                public void registerClient()
                {
		 try
                {
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                }

                public void indexingFiles()
                {
                }

                public void showActiveFileList()
                {
                }

                //public IPAddress[] sendLocation()
                //{
                //}

		public void populateList(int i,String userpwd)
		{
		Console.WriteLine("populate");

			
		  int first=userpwd.IndexOf('\n');
		   
		Console.WriteLine("{0} {1}",userpwd,first);
		   userlist[i,0]=userpwd.Substring(0,first);
		   userlist[i,1]=DateTime.Now.ToString("HH:mm:ss tt");
		   userlist[i,2]="Online";
		   userlist[i,3]=cport;
			
		Console.WriteLine("populate2");
		}
                public void removeFileList()
                {
                }
		
		public class SocketPacket
		{
			public System.Net.Sockets.Socket m_currentSocket;
			public byte[] dataBuffer = new byte[1024];
		}
	
		public void Service()
		{

		try
			{
				// Start listening...
				m_mainSocket.Listen(5);
				// Create the call back for any client connections...
				
				m_mainSocket.BeginAccept(new AsyncCallback (OnClientConnect), null);
				
				//UpdateControls(true);
				
			}
			catch(SocketException se)
			{
				Console.WriteLine(se.Message );
			}
	/*

            Console.WriteLine("\n >> Accept connection from client");

            while ((true))
            {
		registerClient();
            }
	*/
	
		}
		
	    	// This is the call back function, which will be invoked when a client is connected
		public void OnClientConnect(IAsyncResult asyn)
		{
			try
			{
				Console.WriteLine("onclient \n");
				// Here we complete/end the BeginAccept() asynchronous call
				// by calling EndAccept() - which returns the reference to
				// a new Socket object
				m_workerSocket[m_clientCount] = m_mainSocket.EndAccept (asyn);
				// Let the worker Socket do the further processing for the 
				// just connected client
				WaitForData(m_workerSocket[m_clientCount]);
				// Now increment the client count
				thID=m_clientCount;
				firsttime=1;
				++m_clientCount;
				// Display this client connection as a status message on the GUI	
				String str = String.Format("Client # {0} connected", m_clientCount);
				Console.WriteLine("{0}",str);
				//textBoxMsg.Text = str;
								
				// Since the main Socket is now free, it can go back and wait for
				// other clients who are attempting to connect
				//m_mainSocket.BeginAccept(new AsyncCallback ( OnClientConnect ),null);
			}
			catch(ObjectDisposedException)
			{
				//System.Diagnostics.Debugger.Log(0,"1","\n OnClientConnection: Socket has been closed\n");
			}
			catch(SocketException se)
			{
				 Console.WriteLine( se.Message );
			}
			
		}

		
		public void WaitForData(System.Net.Sockets.Socket soc)
		{
			try
			{
			Console.WriteLine("waitfor \n");
				if  ( pfnWorkerCallBack == null ){		
					// Specify the call back function which is to be 
					// invoked when there is any write activity by the 
					// connected client
					pfnWorkerCallBack = new AsyncCallback (OnDataReceived);
				}
				 

				SocketPacket theSocPkt = new SocketPacket ();
				theSocPkt.m_currentSocket 	= soc;
				//theSocPkt.m_currentSocket 	= s2;
				// Start receiving any data written by the connected client
				// asynchronously
				 //Console.WriteLine("before begin \n");
				soc .BeginReceive (theSocPkt.dataBuffer, 0, 
				                   theSocPkt.dataBuffer.Length,
				                   SocketFlags.None,
				                   pfnWorkerCallBack,
				                   theSocPkt);
				//Console.WriteLine("after begin \n");
			}
			catch(SocketException se)
			{
				 Console.WriteLine(se.Message );
			}

		}

	
		// This the call back function which will be invoked when the socket
		// detects any client writing of data on the stream

		public  void OnDataReceived(IAsyncResult asyn)
		{
			try
			{
			        Console.WriteLine("ondata \n");
				SocketPacket socketData = (SocketPacket)asyn.AsyncState ;

				int iRx  = 0 ;
				// Complete the BeginReceive() asynchronous call by EndReceive() method
				// which will return the number of characters written to the stream 
				// by the client
				iRx = socketData.m_currentSocket.EndReceive (asyn);
				char[] chars = new char[iRx +  1];
				System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
				int charLen = d.GetChars(socketData.dataBuffer, 
				                         0, iRx, chars, 0);
				System.String szData = new System.String(chars);
				
				if(firsttime ==1)
				{
				int k=this.authenticate(szData);
				crid=thID;	
				//richTextBoxReceivedMsg.AppendText(szData);
				Console.WriteLine("{0} {1} k={2}",szData,charLen,k);
	
				this.SendMsg(k.ToString()+'\n'+crid.ToString()+'\n');

				populateList(crid,szData);

				firsttime++;
				}
				// Continue the waiting for data on the Socket
				WaitForData( socketData.m_currentSocket);

				Console.WriteLine("{0} {1} ",szData,charLen);
				if(firsttime != 1)
				{
				int p,z;
				for(p=0 ; p < charLen ; p++)
				{
					if(szData[p] == '\n')
						break;
				}
				for(z=p+1 ; z < charLen ; z++)
				{
					
					if(szData[z] == '\n')
						break;
				}
				String sData=szData.Substring(0,p);
				int ID=Convert.ToInt32(szData.Substring(p+1,z-p-1));
				Console.WriteLine("{0} {1} {2} $$%%^^",sData,charLen,szData.Substring(p+1,z-p-1));
				
				if(sData == "logout")
				{
					Console.WriteLine("logout");
					closeClient(ID);
				}
				if(sData == "showlist")
                                {
					Console.WriteLine("showserver");
                                        SendList(ID);
                                }
				if(sData == "sharelist")
				{
					
					Console.WriteLine("populateshare");
                                        PopulateShareList(ID,szData,z);
				}
				if(sData == "searchlist")
                                {

                                        Console.WriteLine("sendshare");
                                        SendShareList(ID,szData.Substring(z+1,charLen-z-2));
                                }

				}
				// Continue the waiting for data on the Socket
				WaitForData( socketData.m_currentSocket);

			        Console.WriteLine("ondata1 \n");
			}
			catch (ObjectDisposedException )
			{
				//System.Diagnostics.Debugger.Log(0,"1","\nOnDataReceived: Socket has been closed\n");
			}
			catch(SocketException se)
			{
				 Console.WriteLine(se.Message );
			}
		}

		
		//For sendin administrator message from server to all clients
		public void SendMsg(String msg)
		{
			try
			{
				//Object objData = richTextBoxSendMsg.Text;
				byte[] byData = System.Text.Encoding.ASCII.GetBytes(msg);
				//for(int i = 0; i < m_clientCount; i++){
					if(m_workerSocket[crid] != null){
						if(m_workerSocket[crid].Connected){
							m_workerSocket[crid].Send (byData);
						}
					}
				//}
				
			}
			catch(SocketException se)
			{
				Console.WriteLine(se.Message );
			}
		}

		 public void SendList(int k)
                {
                        try
                        {
				Console.WriteLine("sendlist {0}",m_clientCount);
                                //Object objData = richTextBoxSendMsg.Text;
                                //byte[] byData = System.Text.Encoding.ASCII.GetBytes(msg);
				  
                                        if(m_workerSocket[k] != null){
                                                if(m_workerSocket[k].Connected){
							byte[] byData=null;
							String msglist=null;
                                	for(int i = 0; i < m_clientCount; i++){
						msglist += (userlist[i,0]+'\n'+userlist[i,1]+'\n'+userlist[i,2]+'\n');

						}
							Console.WriteLine("#######{0}",msglist);
							byData =System.Text.Encoding.UTF8.GetBytes(msglist);
                                                        m_workerSocket[k].Send (byData);

                                                }
                                        }
                                //}

                        }
                        catch(SocketException se)
                        {
                                Console.WriteLine(se.Message );
                        }
                }

		 public void PopulateShareList(int k,String smsg,int strt)
                {
                        try
                        {
				csfdb[num] = new clientShareFileDB();	
				 int p,z,y;
                                for(p=strt+1 ; p < smsg.Length ; p++)
                                {
                                        if(smsg[p] == '\n')
                                                break;
                                }
                                for(z=p+1 ; z <smsg.Length ; z++)
                                {

                                        if(smsg[z] == '\n')
                                                break;
                                }
				 for(y=z+1 ; y <smsg.Length ; y++)
                                {

                                        if(smsg[y] == '\n')
                                                break;
                                }
				Console.WriteLine("^^^^^^^^&&&&&&&&& {0} **************^^^^^^^^^^{1} {2} {3} {4} @@@",smsg,smsg.Substring(strt+1,p-strt-1),smsg.Substring(p+1,z-p-1),smsg.Substring(z+1,y-z-1),smsg.Substring(y+1,smsg.Length-y-1));
				
                                csfdb[num].clfname=String.Copy(smsg.Substring(strt+1,p-strt-1));
                                csfdb[num].clfpath=String.Copy(smsg.Substring(p+1,z-p-1));
                                csfdb[num].clfsize=String.Copy(smsg.Substring(z+1,y-z-1));
                                csfdb[num].clip=String.Copy(smsg.Substring(y+1,smsg.Length-y-4));
                                csfdb[num].clid=k;
				num++;
				
                                                        String msglist="GotIt"+'\n';
				 if(m_workerSocket[k] != null){
                                                if(m_workerSocket[k].Connected){
                                                        byte[] byData=null;
							byData =System.Text.Encoding.UTF8.GetBytes(msglist);
                                                        m_workerSocket[k].Send (byData);

                                                }
                                        }

			Console.WriteLine("{0} {1} ",k,msglist);
			}
                        catch(SocketException se)
                        {
                                Console.WriteLine(se.Message );
                        }
		}
		public void SendShareList(int k,String sfile)
		{
			try
				{
					int i,flag=0;
					crid=k;
					Console.WriteLine("OOOOOooTTTTTTT {0} UUUUUUUUUUUUUUUUUUkkkkkkkkkkkk",num);
					for(i=0;i<num;i++)
					{
					Console.WriteLine("%%%%%%%%%%%$$${0} {1} {2}##",csfdb[i].clfname,sfile,csfdb[i].clfname.CompareTo(sfile));
						if(csfdb[i].clfname.CompareTo(sfile)==0)
						{
						
				filelistinfo += csfdb[i].clfname+'\n'+csfdb[i].clfsize+'\n'+csfdb[i].clfpath+'\n'+userlist[csfdb[i].clid,0]+'\n'+csfdb[i].clip+'\n'+userlist[csfdb[i].clid,3]+'\n'+userlist[csfdb[i].clid,2]+'\n';
						Console.WriteLine("&&& {0} && ",filelistinfo);
						flag=1;
						}
						else
						{ 
						if(csfdb[i].clfname.Contains(sfile))
						{
				filelistinfo += csfdb[i].clfname+'\n'+csfdb[i].clfsize+'\n'+csfdb[i].clfpath+'\n'+userlist[csfdb[i].clid,0]+'\n'+csfdb[i].clip+'\n'+userlist[csfdb[i].clid,3]+'\n'+userlist[csfdb[i].clid,2]+'\n';

						flag=1;

						}
						}
					}
						if(i==num && flag==0)
						{
							SendMsg("No Result"+'\n'+"0"+'\n'+"No Result"+'\n'+"No Result"+'\n'+"No Result"+'\n'+"No Result"+'\n');
						}
						 if(i==num && flag==1)
                                                {
							Console.WriteLine("{0} ****",filelistinfo);
							SendMsg(filelistinfo);
							filelistinfo=null;
						}

						
					}
				
			catch(SocketException se)
                        {
                                Console.WriteLine(se.Message );
                        }

		}
		/*
		//For closing all connections
		void ButtonStopListenClick(object sender, System.EventArgs e)
		{
			CloseSockets();			
			//UpdateControls(false);
		}

		void ButtonCloseClick(object sender, System.EventArgs e)
	   	{
	   		CloseSockets();
	   		Close();
	   	}
		*/
		public void closeClient(int i)
		{
				if(m_workerSocket[i] != null){
					Console.WriteLine("aaaaaaaaaaaaaaaaaa");
					userlist[i,2]="Offline";
					m_workerSocket[i].Close();
					m_workerSocket[i] = null;
				}
		}

		//For closing all sockets (or a particular socket)
	   	public void CloseSockets()
	   	{	
	   		if(m_mainSocket != null){
	   			m_mainSocket.Close();
	   		}
			for(int i = 0; i < m_clientCount; i++){
				if(m_workerSocket[i] != null){
					m_workerSocket[i].Close();
					m_workerSocket[i] = null;
				}
			}	
	   	}
		
	   //----------------------------------------------------	
	   // This is a helper function used (for convenience) to 
	   // get the IP address of the local machine
   	   //----------------------------------------------------
   	 /*  String GetIP()
	   {	   
	   		String strHostName = Dns.GetHostName();
		
		   	// Find host by name
		   	IPHostEntry iphostentry = Dns.GetHostByName(strHostName);
		
		   	// Grab the first IP addresses
		   	String IPStr = "";
		   	foreach(IPAddress ipaddress in iphostentry.AddressList){
		        IPStr = ipaddress.ToString();
		   		return IPStr;
		   	}
		   	return IPStr;
	   }	*/	


		

        public static void Main(string[] args)
        {
		indexServer iSr = new indexServer();
		
		
		 Console.WriteLine("\n\n*************** ShareTube IndexServer Started*****************");
		
		 int port = System.Convert.ToInt32("9090");
		// Create the listening socket...
		iSr.m_mainSocket = new Socket(AddressFamily.InterNetwork, 
				                          SocketType.Stream, 
				                          ProtocolType.Tcp);
		IPEndPoint ipLocal = new IPEndPoint (IPAddress.Parse("127.0.0.1"), port);
		// Bind to local IP Address...
		iSr.m_mainSocket.Bind( ipLocal );
		iSr.database();

		while(true)
		{
		iSr.Service();
		}

        }


        }
    }


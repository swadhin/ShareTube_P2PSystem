using System.Net;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.IO;
using System.Web;
//using System.Web.Hosting;


namespace shareTubeClient
{
	public class socketClient
	{
		 public class sharefileInfo
        {
                public String fname;
                public String fpath;
                public String fsize;
                public String clip;
        }
		 public sharefileInfo[] sharetree= new sharefileInfo[50];
		
    		//private static ManualResetEvent sendDone = new ManualResetEvent(false);
		byte[] m_dataBuffer = new byte [500];
		IAsyncResult m_result;
		public AsyncCallback m_pfnCallBack ;
		public Socket m_clientSocket;
		public int authret=-1;
		public int starttime=1;
		public int show=0;
		public int share=0;
		public int sresult=0;
		public String loginlist=null;
		public String sharelist=null;
		public String sharesearchlist=null;
		public int ID;
		public static int no=0;
		//public static Thread t1,t2;
		public socketClient()
		{
		}
		
		[STAThread]
		public static void Main(string[] args)
		{
		}

		public void connectToServer(String srip,String srport,String clport,String user,String pwd)
		{
			try
			{
				//UpdateControls(false);
				// Create the socket instance
				m_clientSocket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
				
				// Cet the remote IP address
				IPAddress ip = IPAddress.Parse (srip);
				int iPortNo = System.Convert.ToInt16 (srport);
				// Create the end point 
				IPEndPoint ipEnd = new IPEndPoint (ip,iPortNo);
				// Connect to the remote host
				m_clientSocket.Connect ( ipEnd );
				if(m_clientSocket.Connected) {
					
					//sending username & password
					this.SendMessage(user+'\n');	
					this.SendMessage(pwd+'\n');		
					this.SendMessage(clport+'\n');		
					//this.SendMessage(user);
                                        //this.SendMessage(pwd);               
					//Wait for data asynchronously 
					WaitForData();
				        //authret=setauth();
				//	return authret;
					
				}
				else
					Console.WriteLine("Else");//return 0;
			}
			catch(SocketException se)
			{
				//string str;
				//str = "\nConnection failed, is the server running?\n" + se.Message;
				 Console.WriteLine(se.Message);
			//	return 0;
				//UpdateControls(false);
			}		

		}
		  public class SocketPacket
                {
                        public System.Net.Sockets.Socket thisSocket;
                        public byte[] dataBuffer = new byte[1000];
                }

		public void SendMessage(String str)
		{
			Console.WriteLine("sendMessage");
			try
			{
				byte[] byData = System.Text.Encoding.UTF8.GetBytes(str);
				if(m_clientSocket != null){
					m_clientSocket.Send (byData,0,byData.Length,SocketFlags.None);
			Console.WriteLine("sendMessage1");
					 //sendDone.WaitOne();

				}
				return;
			}
			catch(SocketException se)
			{
				 Console.WriteLine(se.Message );
			}	
		}

		public void WaitForData()
		{
			try
			{
				Console.WriteLine("Waitfordatsss");
				if  ( m_pfnCallBack == null ) 
				{
					m_pfnCallBack = new AsyncCallback (OnDataReceived);
				}
				SocketPacket theSocPkt = new SocketPacket ();
				theSocPkt.thisSocket = m_clientSocket;
				theSocPkt.dataBuffer=new byte [500];
				// Start listening to the data asynchronously
				Console.WriteLine("WaitfordatsssXXXXXXX");
				m_result = m_clientSocket.BeginReceive (theSocPkt.dataBuffer,
				                                        0, theSocPkt.dataBuffer.Length,
				                                        SocketFlags.None, 
				                                        m_pfnCallBack, 
				                                        theSocPkt);
			}
			catch(SocketException se)
			{
				Console.WriteLine("WaitfordatsssYYYYYYYYYYYy");
				 Console.WriteLine(se.Message );
			}

		}



		public  void OnDataReceived(IAsyncResult asyn)
		{
			try
			{
				Console.WriteLine("Ondatarec");
				SocketPacket theSockId = (SocketPacket)asyn.AsyncState ;
				int iRx  = theSockId.thisSocket.EndReceive (asyn);
				char[] chars = new char[iRx +  1];
				System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
				int charLen = d.GetChars(theSockId.dataBuffer, 0, iRx, chars, 0);
				System.String szData = new System.String(chars);
			
				//richTextRxMessage.Text = richTextRxMessage.Text + szData;
				Console.WriteLine("###{0} {1}##",szData,charLen);
				if(starttime==1)
				{
				int p;
				for(p=0 ; p < charLen ; p++)
                                {
                                        if(szData[p] == '\n')
                                                break;
                                }
                                String sData=szData.Substring(0,p);
                                ID=Convert.ToInt32(szData.Substring(p+1,charLen-p-2));
                                Console.WriteLine("{0} {1},ID={2} ***",sData,szData.Substring(p+1,charLen-p-2),ID);
				
				authret=Convert.ToInt32(sData);
				starttime++;
				}
				if(show==1)
				{
				show=0;
				loginlist=String.Copy(szData);
				show=2;
				Console.WriteLine("p2p # loginlist-> {0}",loginlist);
				//loginlist=null;
				}
				if(share == 1)
				{
				share=0;
				sharelist=String.Copy(szData);
				share=2;
				Console.WriteLine("p2p # sharelist-> {0}",sharelist);
				}
				 if(sresult == 1)
                                {
                                sresult=0;
                                sharesearchlist=String.Copy(szData);
                                sresult=2;
                                Console.WriteLine("p2p # sharesearchlist-> {0}",sharesearchlist);
                                }

				WaitForData();
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

		public void Disconnect(int I)
		{
			SendMessage("logout"+'\n'+I.ToString()+'\n');
			if ( m_clientSocket != null )
			{
				m_clientSocket.Close();
				m_clientSocket = null;
				//UpdateControls(false);
			}
		}

		public void Showlist(int I)
		{
		show=1;
		Console.WriteLine("{0}",I);
		SendMessage("showlist"+'\n'+I.ToString()+'\n');
		Console.WriteLine("show");
		//WaitForData();

		}
		public void Sharelist(int I,String mgs)
		{
		share=1;
		Console.WriteLine("{0}",I);
		SendMessage("sharelist"+'\n'+I.ToString()+'\n'+mgs+'\n');
		Console.WriteLine("share");
		}
			
		public void SearchList(int I,String mgs)
                {
                sresult=1;
                Console.WriteLine("{0} @@@",I);
                SendMessage("searchlist"+'\n'+I.ToString()+'\n'+mgs+'\n');
                Console.WriteLine("{0} ==88== search",mgs);
                }

		public class downloadAgent
		{
		String serip=null;
		String serport=null;
		String remotefilename=null;
		String localfilename=null;		

		 public downloadAgent(String sip,String spot,String rf,String lf)
		{
			   serip=String.Copy(sip);
			   serport=String.Copy(spot);
			   remotefilename=String.Copy(rf);
			   localfilename=String.Copy(lf);
			   //t1 = new Thread(new ThreadStart(this.DownloadToClient));
                	   //t1.Start() ;

		}
		 public void DownloadToClient()
                {
                        try
                        {
                                TcpClient tcpc = new TcpClient();
                                Byte[] read = new Byte[1024];
                                int port = Convert.ToInt32(serport);
                                //port = 8081;


                                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(serip), port);
                                tcpc.Connect(ep);



                                Stream s = tcpc.GetStream();
                                Byte[] b = Encoding.ASCII.GetBytes(remotefilename.ToCharArray());
                                s.Write( b, 0, b.Length );

                                int bytes;
                                FileStream fs = new FileStream(localfilename, FileMode.OpenOrCreate);
                                BinaryWriter w = new BinaryWriter(fs);


                                while( (bytes = s.Read(read, 0, read.Length)) != 0)
                                {
                                        w.Write(read, 0, bytes);
                                        read = new Byte[1024];
                                }
                                        tcpc.Close();
                                        w.Close();
                                        fs.Close();
                }

                catch(Exception ex)
                {
                        throw new Exception(ex.ToString());
                }
        }

	}
	  public class listenAgent
	  {
	  int mpot;
          public listenAgent(int pot)
	  {
		this.mpot=pot;
	  Console.WriteLine("***{0}",mpot);
		//t2 = new Thread(new ThreadStart(this.ListenForPeers));
	        //t2.Start() ;
		
	  }
	  public void ListenForPeers()
                {
                        try
                        {
				TcpListener tcpl=new TcpListener(this.mpot);
                                Encoding ASCII = Encoding.ASCII;

                                tcpl.Start();

                      //  while (true)
                        //{
				Console.WriteLine("jayantiiiiiiiiiiiiiiiiiiiiiii");
                                Socket s = tcpl.AcceptSocket();
                                NetworkStream DataStream = new NetworkStream(s);
                                String filename;
				Console.WriteLine("choooooooooooooooooooooooton");
                                Byte[] Buffer = new Byte[256];
                                DataStream.Read(Buffer, 0, 256);
                                filename = Encoding.ASCII.GetString(Buffer);
				Console.WriteLine("{0} ##",filename);
                                //StringBuilder sbFileName = new StringBuilder(filename);
                                //StringBuilder sbFileName2 = sbFileName.Replace("\"", "\\");
				StringBuilder sb = new StringBuilder(filename);

      				foreach (char invalidFileNameChar in Path.GetInvalidFileNameChars())
      				{
					Console.WriteLine("{0} @@@@@22##",invalidFileNameChar);

        				if (string.Empty != null)
          					sb.Replace(invalidFileNameChar.ToString(), string.Empty);
      				}
                                FileStream fs = new FileStream(sb.ToString(), FileMode.Open, FileAccess.Read);
                                BinaryReader reader = new BinaryReader(fs);
                                byte[] bytes = new byte[1024];

                                int read;

                                while((read = reader.Read(bytes, 0, bytes.Length)) != 0)
                                {
                                        DataStream.Write(bytes, 0, read);
                                }
				//tcpl.Close();
                                reader.Close();

                                DataStream.Flush();
                                DataStream.Close();
                        //}
                }

                catch(SocketException ex)
                {
                        Console.WriteLine(ex.ToString());
                }
        }
	}




	}
}


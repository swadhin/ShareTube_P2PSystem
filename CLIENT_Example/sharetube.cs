using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Gtk;
using Glade;
using shareTubeClient;
using System.Threading ;
using System.Web;
using System.Text;


public class GladeApp
{
        public static void Main (string[] args)
        {
                new GladeApp (args);
        }
 
        public GladeApp (string[] args) 
        {
                Application.Init();
 
                Glade.XML gxml = new Glade.XML (null, "sharetubelogin.glade", null, null);


                gxml.Autoconnect (this);

		this.treeattach();
                Application.Run();
        }

	string sfnm=null; 
        string sfsz=null;
        string sfpt=null;
        string sfus=null;
        string sfip=null; 
	string sfpot=null;
        string sfst=null;
	[Glade.Widget]
  	     Button client_searchbutton;

	[Glade.Widget]
  	     Button client_sharebutton;
       
	[Glade.Widget]
  	     Button client_searchfilebutton;

	[Glade.Widget]
  	     Button client_downloadfilebutton;
	[Glade.Widget]
  	     Button client_downloadbutton;

	[Glade.Widget] 
	Gtk.VBox client_vbox;

	[Glade.Widget]
        Gtk.HBox client_hbox1;
	
	[Glade.Widget]
        Gtk.HBox client_hbox2;
       
	[Glade.Widget]
        Gtk.Window login;

	[Glade.Widget]
        Gtk.Window client;

	[Glade.Widget]
        Gtk.FileChooserDialog cl_filechooserdialog;

	[Glade.Widget]
        Gtk.Dialog logout_dialogbox;

        [Glade.Widget]
        Gtk.Dialog ipokdialog;
	
	 [Glade.Widget]
        Gtk.Window confirmbox;

	[Glade.Widget]
        Gtk.Entry user;

	[Glade.Widget]
        Gtk.Entry password;
 
	[Glade.Widget]
        Gtk.Entry client_searchentry;

	 [Glade.Widget]
       Gtk.Entry serverip;

        [Glade.Widget]
       Gtk.Entry serverport;
       
	 [Glade.Widget]
       Gtk.Entry clientport;
	
	 [Glade.Widget]
	Gtk.Label label1;

	 [Glade.Widget]
        Gtk.Label iplabel;
	 [Glade.Widget]
	Gtk.TreeView tree;
	 [Glade.Widget]
	Gtk.Entry fileentry;

	public Thread t1,t2;
	Gtk.Label lb1=null,lb2=null;
	socketClient.listenAgent lA;
	public socketClient sc;
	//public String sharetree=null;

	public void treeattach()
	{
		// Create our TreeView
		if(lb1 != null && lb2 != null)
		{
			client_vbox.Remove(lb1);
                	client_vbox.Remove(lb2);
			lb1=null;
			lb2=null;
		}

		tree = new Gtk.TreeView ();
 
 
		// Create a column for the artist name
		Gtk.TreeViewColumn Column1 = new Gtk.TreeViewColumn ();
		Column1.Title = "Filename";
		
		// Create the text cell that will display the artist name
		Gtk.CellRendererText Column1NameCell = new Gtk.CellRendererText ();
		
		// Add the cell to the coln
		Column1.PackStart (Column1NameCell, true);
 

		// Create a column for the artist name
		Gtk.TreeViewColumn Column2 = new Gtk.TreeViewColumn ();
		Column2.Title = "  Filesize  ";
		
		// Create the text cell that will display the artist name
		Gtk.CellRendererText Column2NameCell = new Gtk.CellRendererText ();
		
		// Add the cell to the coln
		Column2.PackStart (Column2NameCell, true);
 
		// Create a column for the artist name
		Gtk.TreeViewColumn Column3 = new Gtk.TreeViewColumn ();
		Column3.Title = "  Filepath  ";
		
		// Create the text cell that will display the artist name
		Gtk.CellRendererText Column3NameCell = new Gtk.CellRendererText ();
		
		// Add the cell to the coln
		Column3.PackStart (Column3NameCell, true);
 
		// Create a column for the artist name
		Gtk.TreeViewColumn Column4 = new Gtk.TreeViewColumn ();
		Column4.Title = "    User    ";
		
		// Create the text cell that will display the artist name
		Gtk.CellRendererText Column4NameCell = new Gtk.CellRendererText ();
		
		// Add the cell to the coln
		Column4.PackStart (Column4NameCell, true);

		// Create a column for the artist name
                Gtk.TreeViewColumn Column5 = new Gtk.TreeViewColumn ();
                Column5.Title = "    IP    ";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column5NameCell = new Gtk.CellRendererText ();

                // Add the cell to the coln
                Column5.PackStart (Column5NameCell, true);

		// Create a column for the artist name
                Gtk.TreeViewColumn Column6 = new Gtk.TreeViewColumn ();
                Column6.Title = "    PORT    ";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column6NameCell = new Gtk.CellRendererText ();

                // Add the cell to the coln
                Column6.PackStart (Column6NameCell, true);

		
		// Create a column for the artist name
		
		// Create a column for the artist name
		Gtk.TreeViewColumn Column7 = new Gtk.TreeViewColumn ();
		Column7.Title = "  Status  ";
		
		// Create the text cell that will display the artist name
		Gtk.CellRendererText Column7NameCell = new Gtk.CellRendererText ();
		
		// Add the cell to the coln
		Column7.PackStart (Column7NameCell, true);

		// Add the columns to the TreeView
		tree.AppendColumn (Column1);
		tree.AppendColumn (Column2);
		tree.AppendColumn (Column3);
		tree.AppendColumn (Column4);
		tree.AppendColumn (Column5);
		tree.AppendColumn (Column6);
		tree.AppendColumn (Column7);
		// Tell the Cell Renderers which items in the model to display
		Column1.AddAttribute (Column1NameCell, "text", 0);
		Column2.AddAttribute (Column2NameCell, "text", 1);
		Column3.AddAttribute (Column3NameCell, "text", 2);
		Column4.AddAttribute (Column4NameCell, "text", 3);
		Column5.AddAttribute (Column5NameCell, "text", 4);
		Column6.AddAttribute (Column6NameCell, "text", 5);
		Column7.AddAttribute (Column7NameCell, "text", 6);
 
		// Create a model that will hold two strings - Artist Name and Song Title
		Gtk.ListStore fileListStore = new Gtk.ListStore (typeof (string), typeof (string),typeof (string), typeof (string),typeof (string),typeof(string),typeof(string));
 
		 Console.WriteLine("here1234567\n");
 

		 // Add some data to the store
	 fileListStore.AppendValues ("Sharetube", "Stube","ST","S","0.0.0.0","0000","SE");
	 //fileListStore.AppendValues ("Garbage", "Dog New Tricks1","asd","sadrf","tw4t");
	 //fileListStore.AppendValues ("Garbage", "Dog New Tricks2","asd","sadrf","tw4t");
	 //fileListStore.AppendValues ("Garbage", "Dog New Tricks3","asd","sadrf","tw4t");

		// Assign the model to the TreeView
		tree.Model = fileListStore;
		tree.SetSizeRequest(810,500);
		tree.Show();		
		// Add our tree to the window
		this.client_vbox.PackStart(tree);
		//client_vbox.Show();
		/*
		Gtk.HBox hbox= new HBox(false,0);
                Label label =new Label("                                                                                ");
		Button close= new Button(Stock.Close);
		label.Show();
		close.Show();
		hbox.PackStart(label,false,true,55);
		 hbox.PackStart(close,false,true,55);
		hbox.Show();
		 this.client_vbox.PackStart(hbox);
		client_vbox.Show();
		*/
	}

	public void treeattach2()
	{  
		if(lb1 != null && lb2 != null)
                {
                        client_vbox.Remove(lb1);
                        client_vbox.Remove(lb2);
			lb1=null;
			lb2=null;
                }


	       // Create our TreeView
                tree = new Gtk.TreeView ();


                // Create a column for the artist name
                Gtk.TreeViewColumn Column1 = new Gtk.TreeViewColumn ();
                Column1.Title = "Filename";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column1NameCell = new Gtk.CellRendererText ();

                // Add the cell to the coln
                Column1.PackStart (Column1NameCell, true);


                // Create a column for the artist name
                Gtk.TreeViewColumn Column2 = new Gtk.TreeViewColumn ();
                Column2.Title = "  Filesize  ";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column2NameCell = new Gtk.CellRendererText ();

                // Add the cell to the coln
                Column2.PackStart (Column2NameCell, true);

                // Create a column for the artist name
                Gtk.TreeViewColumn Column3 = new Gtk.TreeViewColumn ();
                Column3.Title = "  Filepath  ";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column3NameCell = new Gtk.CellRendererText ();

		// Add the cell to the coln
                Column3.PackStart (Column3NameCell, true);
		  // Add the columns to the TreeView
                tree.AppendColumn (Column1);
                tree.AppendColumn (Column2);
                tree.AppendColumn (Column3);
                // Tell the Cell Renderers which items in the model to display
                Column1.AddAttribute (Column1NameCell, "text", 0);
                Column2.AddAttribute (Column2NameCell, "text", 1);
                Column3.AddAttribute (Column3NameCell, "text", 2);

                // Create a model that will hold two strings - Artist Name and Song Title
                Gtk.ListStore fileListStore = new Gtk.ListStore (typeof (string), typeof (string),typeof (string));

                 Console.WriteLine("here1234567\n");


                 // Add some data to the store
	 fileListStore.AppendValues ("Sharetube", "Stube","ST","S","SE");
         /*
	 fileListStore.AppendValues ("Garbage", "Dog New Tricks","asd");
         fileListStore.AppendValues ("Garbage", "Dog New Tricks1","asd");
         fileListStore.AppendValues ("Garbage", "Dog New Tricks2","asd");
         fileListStore.AppendValues ("Garbage", "Dog New Tricks3","asd");
	*/
                // Assign the model to the TreeView
                tree.Model = fileListStore;

                tree.Show();
                // Add our tree to the window
                this.client_vbox.PackStart(tree);
                //client_vbox.Show();
                

	}

	
	 public void treeattach3()
        {
                
		if(lb1 != null && lb2 != null)
                {
                        client_vbox.Remove(lb1);
                        client_vbox.Remove(lb2);
			 lb1=null;
                        lb2=null;

                }


		// Create our TreeView
                tree = new Gtk.TreeView ();


                // Create a column for the artist name
                Gtk.TreeViewColumn Column1 = new Gtk.TreeViewColumn ();
                Column1.Title = "Filename";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column1NameCell = new Gtk.CellRendererText ();

                // Add the cell to the coln
                Column1.PackStart (Column1NameCell, true);


                // Create a column for the artist name
                Gtk.TreeViewColumn Column2 = new Gtk.TreeViewColumn ();
                Column2.Title = "  Filesize  ";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column2NameCell = new Gtk.CellRendererText ();

                // Add the cell to the coln
                Column2.PackStart (Column2NameCell, true);

                // Create a column for the artist name
                Gtk.TreeViewColumn Column3 = new Gtk.TreeViewColumn ();
                Column3.Title = "  Completed Size  ";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column3NameCell = new Gtk.CellRendererText ();

                // Add the cell to the coln
                Column3.PackStart (Column3NameCell, true);

                // Create a column for the artist name
                Gtk.TreeViewColumn Column4 = new Gtk.TreeViewColumn ();
                Column4.Title = "    Percent    ";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column4NameCell = new Gtk.CellRendererText ();
		  // Add the cell to the coln
                Column4.PackStart (Column4NameCell, true);

                // Create a column for the artist name
                Gtk.TreeViewColumn Column5 = new Gtk.TreeViewColumn ();
                Column5.Title = "  Client Uploading  ";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column5NameCell = new Gtk.CellRendererText ();


		 // Add the cell to the coln
                Column5.PackStart (Column5NameCell, true);

                // Add the columns to the TreeView
                tree.AppendColumn (Column1);
                tree.AppendColumn (Column2);
                tree.AppendColumn (Column3);
                tree.AppendColumn (Column4);
                tree.AppendColumn (Column5);
                // Tell the Cell Renderers which items in the model to display
                Column1.AddAttribute (Column1NameCell, "text", 0);
                Column2.AddAttribute (Column2NameCell, "text", 1);
                Column3.AddAttribute (Column3NameCell, "text", 2);
                Column4.AddAttribute (Column4NameCell, "text", 3);
                Column5.AddAttribute (Column5NameCell, "text", 4);

                // Create a model that will hold two strings - Artist Name and Song Title
                Gtk.ListStore fileListStore = new Gtk.ListStore (typeof (string), typeof (string),typeof (string), typeof (string),typeof (string));

                 Console.WriteLine("here1234567\n");


                 // Add some data to the store
	 fileListStore.AppendValues ("Sharetube", "Stube","ST","S","SE");
         /*fileListStore.AppendValues ("Garbage", "Dog New Tricks","asd","sadrf","tw4t");
         fileListStore.AppendValues ("Garbage", "Dog New Tricks1","asd","sadrf","tw4t");
         fileListStore.AppendValues ("Garbage", "Dog New Tricks2","asd","sadrf","tw4t");
         fileListStore.AppendValues ("Garbage", "Dog New Tricks3","asd","sadrf","tw4t");
	*/
                // Assign the model to the TreeView
                tree.Model = fileListStore;

                tree.Show();
                // Add our tree to the window
                this.client_vbox.PackStart(tree);

}
   public void treeattach4()
        {
		if(lb1 != null && lb2 != null)
                {
                        client_vbox.Remove(lb1);
                        client_vbox.Remove(lb2);
			 lb1=null;
                        lb2=null;

                }


                // Create our TreeView
                tree = new Gtk.TreeView ();


                // Create a column for the artist name
                Gtk.TreeViewColumn Column1 = new Gtk.TreeViewColumn ();
                Column1.Title = "Filename";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column1NameCell = new Gtk.CellRendererText ();

                // Add the cell to the coln
                Column1.PackStart (Column1NameCell, true);


                // Create a column for the artist name
                Gtk.TreeViewColumn Column2 = new Gtk.TreeViewColumn ();
                Column2.Title = "  Filesize  ";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column2NameCell = new Gtk.CellRendererText ();

                // Add the cell to the coln
                Column2.PackStart (Column2NameCell, true);

                // Create a column for the artist name
                Gtk.TreeViewColumn Column3 = new Gtk.TreeViewColumn ();
                Column3.Title = "  Bytes Copied  ";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column3NameCell = new Gtk.CellRendererText ();

                // Add the cell to the coln
                Column3.PackStart (Column3NameCell, true);
		 Gtk.TreeViewColumn Column4 = new Gtk.TreeViewColumn ();
                Column4.Title = "    Percent    ";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column4NameCell = new Gtk.CellRendererText ();
                  // Add the cell to the coln
                Column4.PackStart (Column4NameCell, true);

                // Create a column for the artist name
                Gtk.TreeViewColumn Column5 = new Gtk.TreeViewColumn ();
                Column5.Title = "  Client Name  ";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column5NameCell = new Gtk.CellRendererText ();


                 // Add the cell to the coln
                Column5.PackStart (Column5NameCell, true);

                // Add the columns to the TreeView
                tree.AppendColumn (Column1);
                tree.AppendColumn (Column2);
                tree.AppendColumn (Column3);
                tree.AppendColumn (Column4);
                tree.AppendColumn (Column5);
                // Tell the Cell Renderers which items in the model to display
                Column1.AddAttribute (Column1NameCell, "text", 0);
                Column2.AddAttribute (Column2NameCell, "text", 1);
                Column3.AddAttribute (Column3NameCell, "text", 2);
                Column4.AddAttribute (Column4NameCell, "text", 3);
                Column5.AddAttribute (Column5NameCell, "text", 4);

                // Create a model that will hold two strings - Artist Name and Song Title
                Gtk.ListStore fileListStore = new Gtk.ListStore (typeof (string), typeof (string),typeof (string), typeof (string),typeof (string));

                 Console.WriteLine("here1234567\n");
	                 // Add some data to the store
	 fileListStore.AppendValues ("Sharetube", "Stube","ST","S","SE");
         
	/*
	 fileListStore.AppendValues ("Garbage", "Dog New Tricks","asd","sadrf","tw4t");
         fileListStore.AppendValues ("Garbage", "Dog New Tricks1","asd","sadrf","tw4t");
         fileListStore.AppendValues ("Garbage", "Dog New Tricks2","asd","sadrf","tw4t");
         fileListStore.AppendValues ("Garbage", "Dog New Tricks3","asd","sadrf","tw4t");
	*/
                // Assign the model to the TreeView
                tree.Model = fileListStore;

                tree.Show();
                // Add our tree to the window
                this.client_vbox.PackStart(tree);

}


   public void treeattach5(String list)
        {
		if(lb1 != null && lb2 != null)
                {
                        client_vbox.Remove(lb1);
                        client_vbox.Remove(lb2);
			 lb1=null;
                        lb2=null;

                }


               // Create our TreeView
                tree = new Gtk.TreeView ();
		int j=0;

                // Create a column for the artist name
                Gtk.TreeViewColumn Column1 = new Gtk.TreeViewColumn ();
                Column1.Title = "Clientname";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column1NameCell = new Gtk.CellRendererText ();

                // Add the cell to the coln
                Column1.PackStart (Column1NameCell, true);


                // Create a column for the artist name
                Gtk.TreeViewColumn Column2 = new Gtk.TreeViewColumn ();
                Column2.Title = "  Login time  ";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column2NameCell = new Gtk.CellRendererText ();

                // Add the cell to the coln
                Column2.PackStart (Column2NameCell, true);

                // Create a column for the artist name
                Gtk.TreeViewColumn Column3 = new Gtk.TreeViewColumn ();
                Column3.Title = "  Status  ";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column3NameCell = new Gtk.CellRendererText ();

                // Add the cell to the coln
                Column3.PackStart (Column3NameCell, true);
                  // Add the columns to the TreeView
		tree.AppendColumn (Column1);
                tree.AppendColumn (Column2);
                tree.AppendColumn (Column3);
                // Tell the Cell Renderers which items in the model to display
                Column1.AddAttribute (Column1NameCell, "text", 0);
                Column2.AddAttribute (Column2NameCell, "text", 1);
                Column3.AddAttribute (Column3NameCell, "text", 2);

                // Create a model that will hold two strings - Artist Name and Song Title
                Gtk.ListStore fileListStore = new Gtk.ListStore (typeof (string), typeof (string),typeof (string));

                 Console.WriteLine("here1234567\n");
		int start=0,mov=0,k=0;
		String[] mmbrs=new String[30];

		Console.WriteLine("treeatt5 {0} *** {1} ",list,list.Length);
		while(true)
		{
		if(list[mov]=='\0')
			break;
		if(list[mov]=='\n')
		{
		Console.WriteLine("mov {0} {1}",start,mov);
			//if(start<mov)
			mmbrs[k]=String.Copy(list.Substring(start,mov-start));
			start=mov+1;
			k++;

		}
		mov++;
		}
         int l=0;
		Console.WriteLine("treeatt5 {0}",k);
                 // Add some data to the store
	 while(l<k)
	{
         fileListStore.AppendValues (mmbrs[l],mmbrs[l+1] ,mmbrs[l+2]);
	 l=l+3;
	}
                // Assign the model to the TreeView
                tree.Model = fileListStore;

                tree.Show();
                // Add our tree to the window
                this.client_vbox.PackStart(tree);
                //client_vbox.Show();


        }

 public void treeattach7(String list)
        {
		if(lb1 != null && lb2 != null)
                {
                        client_vbox.Remove(lb1);
                        client_vbox.Remove(lb2);
			 lb1=null;
                        lb2=null;

                }
		
		Console.WriteLine("TREEEATTTTTACHHHHH ->>>>> {0} ",list);


                client_vbox.Remove(tree);

                int j=0;
		tree = new Gtk.TreeView ();


                // Create a column for the artist name
                Gtk.TreeViewColumn Column1 = new Gtk.TreeViewColumn ();
                Column1.Title = "Filename";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column1NameCell = new Gtk.CellRendererText ();

                // Add the cell to the coln
                Column1.PackStart (Column1NameCell, true);


                // Create a column for the artist name
                Gtk.TreeViewColumn Column2 = new Gtk.TreeViewColumn ();
                Column2.Title = "  Filesize  ";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column2NameCell = new Gtk.CellRendererText ();

                // Add the cell to the coln
                Column2.PackStart (Column2NameCell, true);

                // Create a column for the artist name
                Gtk.TreeViewColumn Column3 = new Gtk.TreeViewColumn ();
                Column3.Title = "  Filepath  ";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column3NameCell = new Gtk.CellRendererText ();

                // Add the cell to the coln
                Column3.PackStart (Column3NameCell, true);

                // Create a column for the artist name
                Gtk.TreeViewColumn Column4 = new Gtk.TreeViewColumn ();
                Column4.Title = "    User    ";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column4NameCell = new Gtk.CellRendererText ();
			
		 // Add the cell to the coln
                Column4.PackStart (Column4NameCell, true);

		 // Create a column for the artist name
                Gtk.TreeViewColumn Column5 = new Gtk.TreeViewColumn ();
                Column5.Title = "    IP    ";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column5NameCell = new Gtk.CellRendererText ();

                 // Add the cell to the coln
                Column5.PackStart (Column5NameCell, true);
		 // Create a column for the artist name
                Gtk.TreeViewColumn Column6 = new Gtk.TreeViewColumn ();
                Column6.Title = "    PORT    ";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column6NameCell = new Gtk.CellRendererText ();

                // Add the cell to the coln
                Column6.PackStart (Column6NameCell, true);


                // Create a column for the artist name

                // Create a column for the artist name
                Gtk.TreeViewColumn Column7 = new Gtk.TreeViewColumn ();
                Column7.Title = "  Status  ";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column7NameCell = new Gtk.CellRendererText ();

                // Add the cell to the coln
                Column7.PackStart (Column7NameCell, true);


                // Add the columns to the TreeView
                tree.AppendColumn (Column1);
                tree.AppendColumn (Column2);
                tree.AppendColumn (Column3);
                tree.AppendColumn (Column4);
                tree.AppendColumn (Column5);
                tree.AppendColumn (Column6);
                tree.AppendColumn (Column7);

                // Tell the Cell Renderers which items in the model to display
                Column1.AddAttribute (Column1NameCell, "text", 0);
                Column2.AddAttribute (Column2NameCell, "text", 1);
                Column3.AddAttribute (Column3NameCell, "text", 2);
                Column4.AddAttribute (Column4NameCell, "text", 3);
                Column5.AddAttribute (Column5NameCell, "text", 4);
                Column6.AddAttribute (Column6NameCell, "text", 5);
                Column7.AddAttribute (Column7NameCell, "text", 6);

                // Create a model that will hold two strings - Artist Name and Song Title
                Gtk.ListStore fileListStore = new Gtk.ListStore (typeof (string), typeof (string),typeof (string), typeof (string),typeof (string),typeof (string),typeof(string));

	

                Console.WriteLine("here1234567\n");

                int start=0,mov=0,k=0;
                String[] mmbrs=new String[30];
		
		 int p=0,z=0,y=0,t=0,x=0,w=0,v=0,s=0;
	
		for(x=0;x<list.Length;x=v+1)
			{
				Console.WriteLine("x= {0}",x);
                                for(p=x ; p < list.Length ; p++)
                                {
                                        if(list[p] == '\n')
                                                break;
                                }
				if( p > list.Length)
					break;
				
                                for(z=p+1 ; z <list.Length ; z++)
                                {

                                        if(list[z] == '\n')
                                                break;
                                }
				if( z > list.Length)
					break;
                                 for(y=z+1 ; y <list.Length ; y++)
                                {

                                        if(list[y] == '\n')
                                                break;
                                }

				if( y > list.Length)
					break;

				     for(t=y+1 ; t <list.Length ; t++)
                                {

                                        if(list[t] == '\n')
                                                break;
                                }

				if( t > list.Length)
					break;

				  for(w=t+1 ; w <list.Length ; w++)
                                {

                                        if(list[w] == '\n')
                                                break;
                                }
				
				if( w > list.Length)
					break;
				for(s=w+1 ; s<list.Length ; s++)
				{
					if(list[s] == '\n')
                                                break;

				}
				 if( s > list.Length)
                                        break;

				   for(v=s+1 ; v <list.Length ; v++)
                                {

                                        if(list[v] == '\n')
                                                break;
                                }
				

                                if( v > list.Length)
                                        break;

				Console.WriteLine("{0} {1} {2} {3} {4} {5} {6} ",p,z,y,t,w,v,list.Length);
				if(x < list.Length)
				{
		Console.WriteLine("####!!! {0} {1} {2} {3} {4} ",list.Substring(x,p-x),list.Substring(p+1,z-p-1),list.Substring(z+1,y-z-1),list.Substring(y+1,t-y-1),list.Substring(t+1,w-t-1),list.Substring(w+1,v-w-1));
        	fileListStore.AppendValues (list.Substring(x,p-x),list.Substring(p+1,z-p-1),list.Substring(z+1,y-z-1),list.Substring(y+1,t-y-1),list.Substring(t+1,w-t-1),list.Substring(w+1,s-w-1),list.Substring(s+1,v-s-1));
				}
			}
		  Console.WriteLine("{0} {1} {2} {3} {4} {5} {6} ",p,z,y,t,w,v,list.Length);

                 // Add some data to the store
                // Assign the model to the TreeView
                tree.Model = fileListStore;
		tree.SetSizeRequest(810,500);
		
		tree.Selection.Changed += new EventHandler(OnSearchSelectionChanged);
                tree.Show();
                // Add our tree to the window
                this.client_vbox.PackStart(tree);

	}

	public void OnSearchSelectionChanged (object o, EventArgs args)
        {
                TreeIter iter;
                TreeModel model;
 
                if (((TreeSelection)o).GetSelected (out model, out iter))
                {
                         sfnm = (string) model.GetValue (iter, 0);
                         sfsz = (string) model.GetValue (iter, 1);
                         sfpt = (string) model.GetValue (iter, 2);
                         sfus = (string) model.GetValue (iter, 3);
                         sfip = (string) model.GetValue (iter, 4);
                         sfpot = (string) model.GetValue (iter, 5);
                         sfst = (string) model.GetValue (iter, 6);
                        Console.WriteLine ("{0} {1} {2} {3} {4} {5} was selected", sfnm,sfsz,sfpt,sfus,sfip,sfpot,sfst);
                }
	}

	 public void on_login_submit_clicked( object o, EventArgs e)
        {
           Console.WriteLine("Button1 press");
	   //label1.Text=serverport.Text;
           confirmbox.ShowAll();
	}

	 public void on_confirmbox_ok_clicked( object o, EventArgs e)
        {
		int l=0;
		int auth=0;

           Console.WriteLine("Button2 press");
	   //Console.WriteLine("{0} {1}",user.Text,password.Text);
	  	// See if we have text on the IP and Port text fields
			if(serverip.Text == "" || serverport.Text == ""||clientport.Text == ""){
				confirmbox.Hide();
				ipokdialog.Show();
				Console.WriteLine("here");
				//return;
			}
			else
			{	
			
			socketClient scl = new socketClient();
			sc=scl;
			int myport=Convert.ToInt32(clientport.Text);
			sc.connectToServer(serverip.Text,serverport.Text,clientport.Text,user.Text,password.Text) ;
			Console.WriteLine("{0}",scl.authret);
			//sc.nListenForPeers();
			 lA = new socketClient.listenAgent(myport);
			 t1 = new Thread(new ThreadStart(lA.ListenForPeers));
			 t1.Start();
			while(true)
			{
			if(sc.authret==0)
			{
				iplabel.Text="Connection failed, is the server running?\n Or you are not registered \n Or your server ip/port incorrect \n";
				ipokdialog.ShowAll();	
				break;		
			}
			else if(sc.authret ==1)
			{
					
			Console.WriteLine("Connected to Server\n");
           		login.Hide();
           		confirmbox.Hide();
	   		client.ShowAll();
			break;
			}
			}	
			}

//	   PeertoPeer.Transfer tcl= new PeertoPeer.Transfer(8888);
//	   tcl.loginToIndexServer(user.Text,password.Text);
        }

	 public void treeattach6()
        {
		if(lb1 != null && lb2 != null)
                {
                        client_vbox.Remove(lb1);
                        client_vbox.Remove(lb2);
			 lb1=null;
                        lb2=null;

                }


               // Create our TreeView
                tree = new Gtk.TreeView ();


                // Create a column for the artist name
                Gtk.TreeViewColumn Column1 = new Gtk.TreeViewColumn ();
                Column1.Title = "Filename";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column1NameCell = new Gtk.CellRendererText ();

                // Add the cell to the coln
                Column1.PackStart (Column1NameCell, true);


                // Create a column for the artist name
                Gtk.TreeViewColumn Column2 = new Gtk.TreeViewColumn ();
                Column2.Title = "  Filesize  ";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column2NameCell = new Gtk.CellRendererText ();

                // Add the cell to the coln
                Column2.PackStart (Column2NameCell, true);

                // Create a column for the artist name
                Gtk.TreeViewColumn Column3 = new Gtk.TreeViewColumn ();
                Column3.Title = "  Filepath  ";

                // Create the text cell that will display the artist name
                Gtk.CellRendererText Column3NameCell = new Gtk.CellRendererText ();

                // Add the cell to the coln
                Column3.PackStart (Column3NameCell, true);
                  // Add the columns to the TreeView
                tree.AppendColumn (Column1);
                tree.AppendColumn (Column2);
                tree.AppendColumn (Column3);
                // Tell the Cell Renderers which items in the model to display
                Column1.AddAttribute (Column1NameCell, "text", 0);
                Column2.AddAttribute (Column2NameCell, "text", 1);
                Column3.AddAttribute (Column3NameCell, "text", 2);
                // Create a model that will hold two strings - Artist Name and Song Title
                Gtk.ListStore fileListStore = new Gtk.ListStore (typeof (string), typeof (string),typeof (string));

                Console.WriteLine("here1234567\n");


                int l=0;
                 // Add some data to the store
		while(l<socketClient.no)
        	{	
         		fileListStore.AppendValues (sc.sharetree[l].fname,sc.sharetree[l].fsize,sc.sharetree[l].fpath);
         		l=l+1;
        	}
		
                // Assign the model to the TreeView
                tree.Model = fileListStore;

                tree.Show();
                // Add our tree to the window
                this.client_vbox.PackStart(tree);
                //client_vbox.Show();


        }

	  public void on_confirmbox_cancel_clicked( object o, EventArgs e)
        {
           Console.WriteLine("Button3 press");
           confirmbox.Hide();
	   ipokdialog.Hide();
        }
	
	public void on_login_destroy( object o, EventArgs e)
        {
           Console.WriteLine("gone win1");
           Application.Quit();
        }
	
	public void on_confirmbox_destroy( object o, EventArgs e)
        {
           Console.WriteLine("gone win2");
           confirmbox.Hide();
        }
	
 public void on_client_searchbutton_clicked( object o, EventArgs e)
        {
		                if(lb1 != null && lb2 != null)
                {
                       client_vbox.Remove(lb1);
                        client_vbox.Remove(lb2);
			lb1=null;
			lb2=null;
                }


		if(client_hbox2 !=null)
			client_hbox2.Hide();
		if( tree != null)
			tree.Hide();
		
		Gtk.HBox hbox= new HBox(false,5);
		
		Entry enry = new Entry();
                Button close= new Button("  Search  ");
		close.SetSizeRequest(120,21);
		client_searchentry=enry;
		//Button close=client_searchfilebutton;
		close.Clicked += new EventHandler (on_client_searchfilebutton_clicked);
                Button apply= new Button("  Download  ");
		apply.SetSizeRequest(130,21);
		//Button apply=client_searchfilebutton;
		apply.Clicked += new EventHandler (on_client_downloadfilebutton_clicked);
		hbox.SetSizeRequest(810,21);
		close.BorderWidth=0;
                close.Show();
		apply.Show();
		enry.Show();
                hbox.PackStart(enry,true,true,10);
                hbox.PackStart(close,false,false,10);
                hbox.PackStart(apply,false,false,10);
		client_hbox2=hbox;
                client_hbox2.Show();
		client_vbox.Homogeneous=false;
                //client_vbox.Spacing=100;
		this.client_vbox.PackStart(hbox);
		this.treeattach();
	}

public void on_client_sharedbutton_clicked( object o, EventArgs e)
        {
		                if(lb1 != null && lb2 != null)
                {
                        client_vbox.Remove(lb1);
                        client_vbox.Remove(lb2);
                	 lb1=null;
                        lb2=null;

		}


 		if(client_hbox2 !=null)
                      client_hbox2.Hide();
                if( tree != null)
                        tree.Hide();
                this.treeattach6();
	
		
        }
public void on_client_uploadbutton_clicked( object o, EventArgs e)
        {
		if(lb1 != null && lb2 != null)
                {
                        client_vbox.Remove(lb1);
                        client_vbox.Remove(lb2);
			 lb1=null;
                        lb2=null;

                }


 		if(client_hbox2 !=null)
                      client_hbox2.Hide();
                if( tree != null)
                        tree.Hide();
                this.treeattach3();

	
        }
public void on_client_downloadbutton_clicked( object o, EventArgs e)
        {
		  if(lb1 != null && lb2 != null)
                {
                        client_vbox.Remove(lb1);
                        client_vbox.Remove(lb2);
			 lb1=null;
                        lb2=null;

                }

	 if(client_hbox2 !=null)
                      client_hbox2.Hide();
                if( tree != null)
                        tree.Hide();
                this.treeattach4();

        }
public void on_client_sharebutton_clicked( object o, EventArgs e)
        {
		  if(lb1 != null && lb2 != null)
                {
                        client_vbox.Remove(lb1);
                        client_vbox.Remove(lb2);
			 lb1=null;
                        lb2=null;

                }

		client_hbox2.Hide();
		tree.Hide();
		
		Gtk.HBox hbox= new HBox(false,3);
                Label label =new Label(" File Name (To be shared)  : ");
		Entry enry = new Entry();
                Button close= new Button("  Browse  ");
		 close.SetSizeRequest(105,21);
                Button apply= new Button(Stock.Apply);
		 apply.SetSizeRequest(105,21);

                 close.Clicked += new EventHandler (on_client_browsefilebutton_clicked);
		 apply.Clicked += new EventHandler (on_client_applyfilebutton_clicked);

		fileentry=enry;

		label.Show();
                close.Show();
		apply.Show();
		fileentry.Show();
                hbox.PackStart(label,false,false,5);
                hbox.PackStart(enry,true,true,5);
                hbox.PackStart(close,false,false,5);
                hbox.PackStart(apply,false,false,5);

		 hbox.SetSizeRequest(810,21);
		 client_hbox2.SetSizeRequest(810,21);
		
		 lb1 = new Label("                                                   ");
		 lb2 = new Label("                                                   ");
		 
		client_hbox2=hbox;

                 lb1.SetSizeRequest(750,160);
                 lb2.SetSizeRequest(750,350);
		
		lb1.Show();
		lb2.Show();
                client_hbox2.Show();
		this.client_vbox.PackStart(lb1);
		this.client_vbox.PackStart(hbox);
		this.client_vbox.PackStart(lb2);


        }
public void on_client_loginlistbutton_clicked( object o, EventArgs e)
        {
	
	  if(lb1 != null && lb2 != null)
                {
                        client_vbox.Remove(lb1);
                        client_vbox.Remove(lb2);
			 lb1=null;
                        lb2=null;

                }

	 sc.Showlist(sc.ID);
	while(true)
	{
	 //sc.WaitForData();
	if(sc.show == 2)
		break;
	//Console.WriteLine("here");
	}
	
	 if(client_hbox2 !=null)
                      client_hbox2.Hide();
                if( tree != null)
                        tree.Hide();
		if(sc.loginlist!=null)

                this.treeattach5(sc.loginlist);
		sc.loginlist=null;

        }

public void on_client_searchfilebutton_clicked( object o, EventArgs e)
        {
		Console.WriteLine("@@@@@@ {0} @@@@@@@",client_searchentry.Text);
		sc.SearchList(sc.ID,client_searchentry.Text);
		while(true)
		{
			if(sc.sresult==2)
			break;
		}
		treeattach7(sc.sharesearchlist);
		Console.WriteLine("searchFile");
        }

public void on_client_downloadfilebutton_clicked( object o, EventArgs e)
        {
		  if(lb1 != null && lb2 != null)
                {
                        client_vbox.Remove(lb1);
                        client_vbox.Remove(lb2);
			 lb1=null;
                        lb2=null;
  
              }

		  socketClient.downloadAgent d=new socketClient.downloadAgent(sfip,sfpot,sfnm,sfnm+'d');
		  t2 = new Thread(new ThreadStart(d.DownloadToClient));
                  t2.Start();

		//sc.DownloadToClient(sfip,sfpt,sfnm+'d');

		Console.WriteLine("downloadFile");
        }

public void on_client_browsefilebutton_clicked(object o, EventArgs e)
	{
	Console.WriteLine("browseFile");
        cl_filechooserdialog.ShowAll();
	}

public void on_client_applyfilebutton_clicked(object o, EventArgs e)
        {
	String s=null;
	FileInfo f=null;
	int l=-1;
	if(fileentry.Text != null)
	{
	 s = String.Copy(fileentry.Text);
	 f = new FileInfo(s);
	 l=s.LastIndexOf('/');
	}
	 //String strHostName = new String ("");
	 String strHostName = Dns.GetHostName ();
	 Console.WriteLine ("Local Machine's Host Name: " +  strHostName);
	 IPHostEntry ipEntry = Dns.GetHostByName (strHostName);
	 IPAddress [] addr = ipEntry.AddressList;
	
	  for (int i = 0; i < addr.Length; i++)
          {
              Console.WriteLine ("IP Address {0}: {1} ", i, addr[i].ToString ());
          }

	 //String ip=sc.GetIP();
	 String name=fileentry.Text.Substring(l+1,fileentry.Text.Length-l-1);
	 String sndList=name+'\n'+fileentry.Text+'\n'+f.Length.ToString()+'\n'+addr[0].ToString ()+'\n';
	 //sharetree += sndList;
	sc.sharetree[socketClient.no]=new socketClient.sharefileInfo(); 	
	sc.sharetree[socketClient.no].fname=String.Copy(name);
	sc.sharetree[socketClient.no].fpath=String.Copy(fileentry.Text);
	sc.sharetree[socketClient.no].fsize=String.Copy(f.Length.ToString());
	sc.sharetree[socketClient.no].clip=String.Copy(addr[0].ToString ());
	socketClient.no++;
	
	//String sndList=String.Copy(fileentry.Text);
	 sc.Sharelist(sc.ID,sndList);
	// Console.WriteLine("ip - >{0}",sc.GetIP());
	
	 while(true)
	{
		if(sc.share==2)
			break;
	}
	
	 Console.WriteLine("applyFile");
        }

public void on_client_logoutbutton_clicked(object o, EventArgs e)
        {
         Console.WriteLine("lologoutbutton");
	logout_dialogbox.ShowAll();
        }

public void on_logout_ok_clicked(object o, EventArgs e)
        {
         Console.WriteLine("logoutok");

	 if (t1.IsAlive == true)
        {
            t1.Abort();
        }

         Console.WriteLine("{0}ccccccccc",sc.ID);
	 sc.Disconnect(sc.ID);
	
	 Application.Quit();
         Console.WriteLine("logoutokfinish");
        }
public void on_logout_cancel_clicked(object o, EventArgs e)
        {
         Console.WriteLine("logcancel");
	logout_dialogbox.Hide();
        }
public void on_button1_clicked(object o, EventArgs e)
        {
         Console.WriteLine("filechs1");
	cl_filechooserdialog.Hide();
        }

public void on_button2_clicked(object o, EventArgs e)
        {
         Console.WriteLine("filechs2");
	fileentry.Text=String.Copy(cl_filechooserdialog.Filename);
	cl_filechooserdialog.Hide();
        }

public void on_ipokbutton_clicked(object o, EventArgs e)
        {
         Console.WriteLine("ipok");
	 ipokdialog.Hide();
	}
public void on_sr_searchbutton_clicked( object o, EventArgs e)
        {
        }
public void on_sr_sharebutton_clicked( object o, EventArgs e)
        {
        }
public void on_sr_adminbutton_clicked( object o, EventArgs e)
        {
        }

public void on_sr_searchfilebutton_clicked( object o, EventArgs e)
        {
        }
}

using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Services;
using System.Windows.Forms;
using System.Drawing;
    
[Serializable]
public class BuddyListForm : Form
{
        
    private ListView BuddyListView;
    private ImageList Images;
    private StatusBar Status;
    private System.Timers.Timer Timer;
    
    private ArtooService Service;
    private int Token = -1;
    private String[] Buddies;
    private ArrayList ChatBoxes;
    public String Me;
    
    private class ChatBox
    {
        public String Buddy;
        public ChatForm Window;
    }
        
    public BuddyListForm(string server)
    {
        InitializeComponent();

        if(server == null || server.Length == 0)
        {
            server = "localhost";
        }
        string url = String.Format("tcp://{0}:8892/ArtooService", server);

        ChannelServices.RegisterChannel(new TcpChannel());
        WellKnownClientTypeEntry remotetype = new WellKnownClientTypeEntry(typeof(ArtooService),"tcp://localhost:8892/ArtooService");
        RemotingConfiguration.RegisterWellKnownClientType(remotetype);
        Service = new ArtooService();
        ChatBoxes = new ArrayList();
    }
        
    protected override void Dispose( bool disposing )
    {
        base.Dispose( disposing );
        
        if(disposing && Token != -1)
        {
            Service.Logout(Me, Token);
            Token = -1;
        }
    }
        
    public void InitializeComponent()
    {
        this.BuddyListView = new ListView();
        this.Status = new StatusBar();
        this.Timer = new System.Timers.Timer();
        this.SuspendLayout();
        
        this.BuddyListView.Dock = DockStyle.Fill;
        this.BuddyListView.View = View.List;
        this.BuddyListView.ItemActivate += new EventHandler(BuddyListView_ItemActivate);
        
        this.Images = new ImageList();
        this.Images.Images.AddStrip(Bitmap.FromFile("icons.bmp"));
        this.Images.TransparentColor = Color.FromArgb(255, 0, 255);
        
        this.BuddyListView.LargeImageList = this.Images;
        this.BuddyListView.SmallImageList = this.Images;
        
        this.Status.Text = "Not logged in.";
        
        this.Timer.Enabled = true;
		this.Timer.Interval = 1000;
		this.Timer.SynchronizingObject = this;
		this.Timer.Elapsed += new System.Timers.ElapsedEventHandler(this.Timer_Elapsed);
        
        this.Text = "Artoo Instant Messenger";
        this.Size = new Size(200, 460);
        this.MinimumSize = new Size(180, 180);
        this.Font = new Font("Tahoma", 8);
            
        this.Controls.AddRange(new System.Windows.Forms.Control[]
		{
		    this.BuddyListView,
		    this.Status
		});
		
		MainMenu mainMenu = new MainMenu();
		mainMenu.MenuItems.AddRange(new MenuItem[]
		{
		    new MenuItem("&File"),
		    new MenuItem("&Buddies"),
		    new MenuItem("&Status")
		});
		mainMenu.MenuItems[0].MenuItems.AddRange(new MenuItem[]
		{
		    new MenuItem("&Connect...", new EventHandler(Connect_Click)),
		    new MenuItem("&Disconnect", new EventHandler(Disconnect_Click)),
		    new MenuItem("-"),
		    new MenuItem("&Exit", new EventHandler(Exit_Click))
		});
		mainMenu.MenuItems[1].MenuItems.AddRange(new MenuItem[]
		{
		    new MenuItem("&Add...", new EventHandler(Add_Click))
		});
		mainMenu.MenuItems[2].MenuItems.AddRange(new MenuItem[]
		{
		    new MenuItem("&Online", new EventHandler(Online_Click)),
		    new MenuItem("O&ffline", new EventHandler(Offline_Click)),
		    new MenuItem("&Busy", new EventHandler(Busy_Click))
		});
		this.Menu = mainMenu;
            
        this.ResumeLayout(false);
    }
        
    [STAThread]
    static void Main(string[] args)
    {
        string server = null;
        if(args.Length == 1)
        {
            server = args[0];
        }

        Application.Run(new BuddyListForm(server));
    }
    
    private void Exit_Click(object sender, System.EventArgs e)
    {
        Application.Exit();
    }
    
    private void Add_Click(object sender, System.EventArgs e)
    {
        if(Token == -1) return;
        
        AddBuddyForm buddyForm = new AddBuddyForm();
        if(buddyForm.ShowDialog(this) == DialogResult.OK)
        {
            String buddy = buddyForm.AddBuddyTextBox.Text;
            for(int i = 0; i < Buddies.Length; i++)
            {
                if(String.Compare(Buddies[i], buddy) == 0)
                    return;
            }
            Service.AddBuddy(Me, Token, buddy);
        }
        
    }
        
    private void Connect_Click(object sender, System.EventArgs e)
    {
        LoginForm loginForm = new LoginForm();
        if(loginForm.ShowDialog(this) == DialogResult.OK)
        {
            Status.Text = "Logging in...";
            Token = Service.Login(Me = loginForm.LoginTextBox.Text, loginForm.PasswordTextBox.Text);
            if(Token != -1)
            {
                Status.Text = "Logged in.";
                UpdateBuddies();
            }
            else Status.Text = "Not logged in.";
            
        }
    }
    
    private void Disconnect_Click(object sender, System.EventArgs e)
    {
        if(Token != -1)
            Service.Logout(Me, Token);
        
        Token = -1;
    }
    
    public void SendMessage(String buddy, String message)
    {
        if(Token == -1) return;
        
        Service.SendMessage(Me, Token, buddy, message);
    }
    
    private void BuddyListView_ItemActivate(object sender, System.EventArgs e)
    {
        if(Token == -1) return;
        
        ChatBox c;
        String buddy = BuddyListView.SelectedItems[0].Text;
        IEnumerator boxes = ChatBoxes.GetEnumerator();
        while(boxes.MoveNext())
        {
            c = (ChatBox) boxes.Current;
            if(String.Compare(c.Buddy, buddy) == 0)
                return;
        }
        c = new ChatBox();
        c.Buddy = buddy;
        c.Window = new ChatForm(buddy, this);
        c.Window.Show();
        ChatBoxes.Add(c);
    }
    
    private void Online_Click(object sender, System.EventArgs e)
    {
        if(Token == -1) return;
        
        ArtooService.Status s;
        s.Code = ArtooService.StatusType.Online;
        s.Description = ArtooService.StatusType.Online.ToString();
        Service.SetStatus(Me, Token, s);
    }
    
    private void Offline_Click(object sender, System.EventArgs e)
    {
        if(Token == -1) return;
        
        ArtooService.Status s;
        s.Code = ArtooService.StatusType.Offline;
        s.Description = ArtooService.StatusType.Offline.ToString();
        Service.SetStatus(Me, Token, s);
    }
    
    private void Busy_Click(object sender, System.EventArgs e)
    {
        if(Token == -1) return;
        
        ArtooService.Status s;
        s.Code = ArtooService.StatusType.Busy;
        s.Description = ArtooService.StatusType.Busy.ToString();
        Service.SetStatus(Me, Token, s);
    }
    
    private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        if(Token == -1)
        {
            this.BuddyListView.Items.Clear();
            this.Status.Text = "Not logged in.";
            return;
        }
        
        UpdateBuddies();
        Status.Text = Service.GetStatus(Me, Token, Me).Description;
        String message = Service.GetMessage(Me, Token);
        if(message.Length > 0)
        {
            String[] parts = message.Split(':');
            String buddy = parts[0];
            ChatBox c;
            IEnumerator boxes = ChatBoxes.GetEnumerator();
            while(boxes.MoveNext())
            {
                c = (ChatBox) boxes.Current;
                if(String.Compare(c.Buddy, buddy) == 0)
                {
                    c.Window.AddMessage(message);
                    return;
                }
            }
            c = new ChatBox();
            c.Buddy = buddy;
            c.Window = new ChatForm(buddy, this);
            c.Window.Show();
            ChatBoxes.Add(c);
            c.Window.AddMessage(message);
        }
    }
    
    private void UpdateBuddies()
    {
        Buddies = Service.GetBuddies(Me, Token);
        ArtooService.Status s;
        
        this.BuddyListView.Items.Clear();
        for(int i = 0; i < Buddies.Length; i++)
        {
            s = Service.GetStatus(Me, Token, Buddies[i]);
            this.BuddyListView.Items.Add(Buddies[i], (Int32)s.Code);
        }
    }
        
        
}
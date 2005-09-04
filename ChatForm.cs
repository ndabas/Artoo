using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Services;
using System.Windows.Forms;
using System.Drawing;
    
[Serializable]
public class ChatForm : Form
{
        
    private TextBox MessageBox;
    private TextBox WriteBox;
    private Button SendButton;
    
    public String Buddy;
    private BuddyListForm BuddyForm;
    
    public ChatForm(String buddy, BuddyListForm parent)
    {
        Buddy = buddy;
        BuddyForm = parent;
        InitializeComponent();
        
    }
        
    protected override void Dispose( bool disposing )
    {
        base.Dispose( disposing );
    }
        
    public void InitializeComponent()
    {
        this.MessageBox = new TextBox();
        this.WriteBox = new TextBox();
        this.SendButton = new Button();
        this.SuspendLayout();
        
        this.MessageBox.Location = new Point(12, 12);
        this.MessageBox.Multiline = true;
        this.MessageBox.Size = new Size(370, 280);
        
        this.WriteBox.Location = new Point(12, 304);
        this.WriteBox.Multiline = true;
        this.WriteBox.Size = new Size(300, 60);
        
        this.SendButton.Text = "Send";
        this.SendButton.Location = new Point(324, 304);
        this.SendButton.Size = new Size(60, 60);
        this.SendButton.Click += new EventHandler(SendButton_Click);
        
        this.Text = Buddy;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.AcceptButton = SendButton;
        this.Size = new Size(406, 412);
        this.MinimumSize = new Size(406, 412);
        this.Font = new Font("Tahoma", 8);
            
        this.Controls.AddRange(new System.Windows.Forms.Control[]
		{
		    this.MessageBox,
		    this.WriteBox,
		    this.SendButton
		});
		    
        this.ResumeLayout(false);
    }
    
    public void AddMessage(String message)
    {
        MessageBox.Text += "\r\n" + message;
    }
    
    private void SendButton_Click(object sender, EventArgs e)
    {
        if(WriteBox.Text.Length == 0) return;
        BuddyForm.SendMessage(Buddy, WriteBox.Text);
        AddMessage(BuddyForm.Me + ": " + WriteBox.Text);
        WriteBox.Text = "";
    }
        
        
}
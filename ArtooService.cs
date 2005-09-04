using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Services;

public class ArtooService : MarshalByRefObject
{
    private ArtooBase ArtooDataSet;
    private int NextToken;
    private ArrayList MessageList;
    
    [Serializable]
    public struct Status
    {
        public String Description;
        public StatusType Code;
    }
    
    public enum StatusType
    {
        Online,
        Offline,
        Busy,
        Unknown = 5
    }
    
    private class Message
    {
        public String To;
        public String MessageText;
    }
    
    public ArtooService()
    {
        ArtooDataSet = new ArtooBase();
        ArtooDataSet.ReadXml("ArtooBase.xml");
        NextToken = 42;
        MessageList = new ArrayList();
    }
    
    public int Login(String username, String password)
    {
        ArtooBase.UserRow[] rows = (ArtooBase.UserRow[])ArtooDataSet.User.Select("id = \'" + username + "\'");
        if(rows.Length > 0 && String.Compare(rows[0].password, password) == 0)
        {
                ArtooBase.StatusRow[] sr = rows[0].GetStatusRows();
                if(sr.Length > 0)
                {
                    sr[0].id = Convert.ToInt32(StatusType.Online);
                    sr[0].StatusText = StatusType.Online.ToString();
                }
                return rows[0].token = NextToken++;
        }
        return -1;
    }
    
    public void Logout(String username, int token)
    {
        ArtooBase.UserRow[] rows = (ArtooBase.UserRow[])ArtooDataSet.User.Select("id = \'" + username + "\'");
        if(rows.Length > 0 && rows[0].token == token)
        {
                ArtooBase.StatusRow[] sr = rows[0].GetStatusRows();
                if(sr.Length > 0)
                {
                    sr[0].id = Convert.ToInt32(StatusType.Offline);
                    sr[0].StatusText = StatusType.Offline.ToString();
                }
                rows[0].token = -1;
        }
    }
    
    public void SetStatus(String username, int token, Status status)
    {
        ArtooBase.UserRow[] rows = (ArtooBase.UserRow[])ArtooDataSet.User.Select("id = \'" + username + "\'");
        if(rows.Length > 0 && rows[0].token == token)
        {
                ArtooBase.StatusRow[] sr = rows[0].GetStatusRows();
                if(sr.Length > 0)
                {
                    sr[0].id = (Int32)status.Code;
                    sr[0].StatusText = status.Description;
                }
        }
    }
    
    public void AddBuddy(String me, int token, String buddy)
    {
        ArtooBase.UserRow[] rows = (ArtooBase.UserRow[])ArtooDataSet.User.Select("id = \'" + me + "\'");
        if(rows.Length == 1 && rows[0].token == token)
        {
            ArtooDataSet.Buddy.AddBuddyRow(buddy, rows[0]);
        }
    }
    
    public void SendMessage(String me, int token, String buddy, String  message)
    {
        ArtooBase.UserRow[] rows = (ArtooBase.UserRow[])ArtooDataSet.User.Select("id = \'" + me + "\'");
        if(rows.Length == 1 && rows[0].token == token)
        {
            Message m = new Message();
            m.To = buddy;
            m.MessageText = me + ": " + message;
            MessageList.Add(m);
        }
    }
    
    public String GetMessage(String me, int token)
    {
        ArtooBase.UserRow[] rows = (ArtooBase.UserRow[])ArtooDataSet.User.Select("id = \'" + me + "\'");
        if(rows.Length == 1 && rows[0].token == token)
        {
            Message m;
            IEnumerator messages = MessageList.GetEnumerator();
            while(messages.MoveNext())
            {
                m = (Message) messages.Current;
                if(String.Compare(m.To, me) == 0)
                {
                    MessageList.Remove(m);
                    return m.MessageText;
                }
            }
        }
        return String.Empty;
    }
    
    public Status GetStatus(String me, int token, String buddy)
    {
        ArtooBase.UserRow[] rows = (ArtooBase.UserRow[])ArtooDataSet.User.Select("id = \'" + me + "\'");
        Status s;
        s.Code = StatusType.Unknown;
        s.Description = String.Empty;
        if(rows.Length == 1 && rows[0].token == token)
        {
            ArtooBase.UserRow[] br = (ArtooBase.UserRow[])ArtooDataSet.User.Select("id = \'" + buddy + "\'");
            if(br.Length > 0)
            {
                ArtooBase.StatusRow[] sr = br[0].GetStatusRows();
                if(sr.Length > 0)
                {
                    s = new Status();
                    s.Code = (StatusType)sr[0].id;
                    s.Description = sr[0].StatusText;
                    return s;
                }
            }
        }
        return s;
    }
    
    public String[] GetBuddies(String me, int token)
    {
        ArtooBase.UserRow[] rows = (ArtooBase.UserRow[])ArtooDataSet.User.Select("id = \'" + me + "\'");
        if(rows.Length == 1 && rows[0].token == token)
        {
            ArtooBase.BuddyRow[] br = rows[0].GetBuddyRows();
            String[] ids = new String[br.Length];
            for(int i = 0; i < br.Length; i++)
            {
                ids[i] = br[i].id;
            }
            return ids;
        }
        return null;
    }
    
    public void Shutdown()
    {
        ArtooBase.UserDataTable users = ArtooDataSet.User;
        for(int i = 0; i < users.Count; i++)
        {
            users[i].token = -1;
            ArtooBase.StatusRow[] sr = users[i].GetStatusRows();
            if(sr.Length > 0)
            {
                sr[0].id = (Int32)StatusType.Offline;
                sr[0].StatusText = StatusType.Offline.ToString();
            }
        }
        ArtooDataSet.WriteXml("ArtooBase.xml");
    }
}
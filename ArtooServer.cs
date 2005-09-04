using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Services;

public class ArtooServer
{
    public static void Main(String[] args)
    {
        TcpChannel channel = new TcpChannel(8892);
        ChannelServices.RegisterChannel(channel);
        
        ArtooService service = new ArtooService();
        ObjRef obj = RemotingServices.Marshal(service, "ArtooService");
        
        Console.WriteLine("Artoo IM Server started. Press ENTER to stop.");
        Console.ReadLine();
        
        service.Shutdown();
        
        RemotingServices.Unmarshal(obj);
        RemotingServices.Disconnect(service);
    }
}  
all : ArtooServer.exe UsersAdmin.exe Artoo.exe

run : ArtooServer.exe UsersAdmin.exe Artoo.exe
  start ArtooServer.exe
  start Artoo.exe

UsersAdmin.exe : UsersAdminForm.cs ArtooBase.cs
  csc /target:winexe /debug+ /warn:4 /out:UsersAdmin.exe UsersAdminForm.cs ArtooBase.cs

ArtooServer.exe : ArtooServer.cs ArtooService.dll
  csc /target:exe /debug+ /warn:4 /r:ArtooService.dll /out:ArtooServer.exe ArtooServer.cs

ArtooService.dll : ArtooService.cs ArtooBase.cs
  csc /target:library /debug+ /warn:4 /out:ArtooService.dll ArtooService.cs ArtooBase.cs
  
Artoo.exe : BuddyListForm.cs LoginForm.cs AddBuddyForm.cs ChatForm.cs ArtooService.dll
  csc /target:winexe /debug+ /warn:4 /r:ArtooService.dll /out:Artoo.exe BuddyListForm.cs LoginForm.cs ChatForm.cs AddBuddyForm.cs
  
clean:
  del /s *.exe *.pdb *.dll *.resources

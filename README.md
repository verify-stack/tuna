Tuna - basic 2013L ASP.NET CORE revival
==============================
> [!CAUTION]
> Legally, I am not responsible for any losses (financial, mental, etc). This is just a project to **recreate**, not profit.

> [!NOTE]
> **NONE**, I repeat. **NONE** of the code has been stolen from leaked sources, instead used from public sources (e.g Wayback Machine). In no way, I profit from this project.
> Also, some bit's of the site may be inaccurate! Be warned...

Tuna is a basic 2013L revival of the ROBLOX site at the time using Wayback Machine snapshots and archived pages.

## requirements
### required
* Visual Studio 2022 (you can use older versions i guess)
* .NET 8 (via the Visual Studio Installer)
* ASP.NET (via the Visual Studio Installer)
* basic understanding of C# and ASP.NET Core MVC
* PGSQL Database (not yet)
### optional
* any client before 2014L patched
* a domain/host for it

## how 2 run
This little tutorial will show you how to host the site in a development environment.
### compile Roblox.KeyHelper (optional)
Roblox.KeyHelper is the given RSA generator for clients. Compile the project and you're ready to go!
### setup config
* URL
  * Go to Roblox.Configuration, then open Settings.cs.
  * Find BaseURL and replace the current URL ("http://%s.roblox.com") to your domain (e.g "http://%s.replacedomain.replacetld").
* Basic Client Info
  * Go to Roblox.Configuration, then open Settings.cs.
  * Disable Clients.
    * Set IsClientsSetup to false.
    * IsClientsSetup : Are clients deployed and ready to use?
  * Enable Clients.
    * Make sure IsClientsSetup is set to true.
    * UseNewRBXSig : Should we use the new --rbxsig%sig% format or the old %sig% format?
    * PrimaryKey : Where the PrivateKey is located.
    * KeysFolder : Where the Keys folder is located.
    * LuaFolder : Where the Lua (scripts for the client and studio to run) folder is located.
* Private Keys (optional)
  * Run your RSA generator to generate keys the length of 1024.
    * If you're using Roblox.KeyHelper, run this command: "Roblox.KeyHelper -g -l 1024".
    * If you're using Toolbox.KeyGenerator, run Toolbox.KeyGenerator.
    * If you're using RBXSigTools, run KeyGenerator.exe.
    * If you're using OpenSSL, run "openssl genrsa -out PrivateKey.pem 1024" and "openssl rsa -in PrivateKey.pem -pubout -out PublicKeyBlob.txt".
  * Put the key pairs generated into the Keys folder.
  * Make sure your client is patched to the generated PublicKeyBlob.txt.
### change corescripts (optional)
CoreScripts are defined in the asset folder with a .core. You can delete all of them and create new using your corescripts.
### handle assets
The way assets are handled in Tuna is via replacing the given input with the new output.
* _burl : The base URL, this is not defined as BaseURL. It's just the www subdomain.
* _aurl : The API URL, the api subdomain.

ex:
print("_burl is the best website!") becomes print("http://www.roblox.com is the best website!")

### hosting the site
You can build the project and run dotnet cli, or use Visual Studio (recommended) to run it. It doesn't matter.

## checklist
- [ ] Roblox.Website
	- [x] /Landing (not much info)
	- [ ] /Game (basic functions but nothing else)
	- [x] /Asset (Most likely done, just connect to DB)
	- [ ] Database
- [x] Roblox.Configuration
- [x] Roblox.KeyHelper
	- [x] Generate Keys
	- [x] Sign scripts
	- [ ] Bugs
		- [ ] Requirment to include an argument for non-argument options (e.g -g)
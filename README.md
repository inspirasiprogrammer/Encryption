Rijndael managed encryption
=================

This is an example project to show you how you can use .NET's Rijndael managed ecryption from a script based language that can work with COM objects

So how do I do it?
====================

- Get this project from Github and change the InputKey constant to another GUID !!!
  (internal const string Inputkey = "560A18CD-6346-4CF0-A2E8-671F9B6B9EA9";)
- Build the project and get the Encryption.dll file
- Register the Encryption.dll file with the program REGASM.EXE (you can find this program in the .NET installation folder)

<b>Regasm.exe /codebase Encryption.dll</b>

You now can use the DLL like any other COM based file.

VBScript example
====================
dim encryption
dim text
set encryption = createobject("Encryption.Rijndael")

text = encryption.Encrypt("My secret text", "MySecretSaltKey")
msgbox text

msgbox encryption.Decrypt(text, "MySecretSaltKey")

License
=======

Copyright 2013-2015 Kees van Spelde.
Licensed under the The Code Project Open License (CPOL) 1.02; you may not use this software except in compliance with the License. You may obtain a copy of the License at:
http://www.codeproject.com/info/cpol10.aspx
Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.

Core Team
=========
    Sicos1977 (Kees van Spelde)

Support
=======
If you like my work then please consider a donation as a thank you.

<a href="https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=NS92EXB2RDPYA" target="_blank"><img src="https://www.paypalobjects.com/en_US/i/btn/btn_donate_LG.gif" /></a>

# Third-Party Notices — USB

This tree is Dave Robinson's working copy of two VB.NET USB experiments. Third-party material remains under its original terms (or none, where none were supplied).

## Mecanique EasyHID / mcHID.dll (no license supplied)

`USBProject` is a VB6-to-VB.NET upgrade of Mecanique EasyHID sample code. `AssemblyCompany` is Mecanique. The HID host talks to a device through native `mcHID.dll` via P/Invoke in `mcHIDInterface.vb`. The native DLL is **not** in this tree.

Upgrade provenance (`USBProject.log`, UTF-16) records the original VB6 project:

- `C:\Program Files (x86)\Mecanique\EasyHID\USBProject\VisualBASIC\USBProject.vbp`
- Output: `C:\Program Files (x86)\Mecanique\EasyHID\USBProject\VisualBASIC\Project1.NET`

No license file for Mecanique EasyHID or `mcHID.dll` was present in the Historical Dev folder. Do not treat that HID interface or the upgraded USBProject sample as VaderConsulting MIT-licensed original work.

Dave's separate `USB` WinForms project (WMI PnP list / shell hooks, assembly copyright 2007) is his own code and is covered by the MIT license in `LICENSE`.

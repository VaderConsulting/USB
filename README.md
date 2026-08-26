# USB

Two VB.NET USB experiments. USBProject is a VB6-upgraded HID host (Mecanique mcHID.dll) for a demo board (VID 0x1234) with start/stop/delay/temperature/LED/echo commands. USB is a separate VS 2005 WinForms that lists PnP devices on plug/unplug via WMI and shell hooks.

**Source last updated:** 2009-01-07 · **Language:** VB.NET · **Target:** .NET 2.0 (USB) / 3.5 (USBProject) · **Output:** two WinForms exes

## Solution structure

| Project | Language | Type | Purpose |
|---------|----------|------|---------|
| `USBProject` | VB.NET | WinForms exe (`AssemblyName` Project1) | VB6-to-VB.NET upgraded HID host via Mecanique `mcHID.dll` (P/Invoke in `mcHIDInterface.vb`) for a demo board VendorID 4660 (`0x1234`) / ProductID 1. Commands: Start, Stop, Set Delay, Output temperature, Output to LEDs, Echo. Native `mcHID.dll` is **not** in the tree. |
| `USB` | VB.NET | WinForms exe | Nested `USB\USB.vbproj`. Lists PnP devices (`Win32_PnPEntity`) on plug/unplug via WMI `Win32_DeviceChangeEvent` and `RegisterShellHookWindow`. No `TargetFrameworkVersion` (VS 2005 = .NET 2.0). |

Keep `USBProject.log` (UTF-16 UpgradeLog) and `_UpgradeReport_Files` as VB6-to-VB.NET upgrade provenance.

## How to open

Open `USBProject.sln` in Visual Studio 2008 (solution format 10.00, ToolsVersion 3.5). Open `USB.sln` in Visual Studio 2005 (solution format 9.00).

## Attribution and provenance

Working copy from Dave Robinson's OneDrive Historical Dev folder `USB`. USBProject `AssemblyCompany` Mecanique; HID interface is `mcHID.dll` (P/Invoke in `mcHIDInterface.vb`). Upgraded from VB6 at `C:\Program Files (x86)\Mecanique\EasyHID\USBProject\VisualBASIC\USBProject.vbp` (see `USBProject.log`). USB assembly copyright 2007.

## License

MIT © 2026 VaderConsulting for Dave Robinson's code. See `LICENSE`. Mecanique EasyHID / `mcHID.dll` has no license in this tree; see `THIRD_PARTY_NOTICES.md`.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MetroManager {
	enum ActivateOptions {
		None = 0x00000000,  // No flags set
		DesignMode = 0x00000001,  // The application is being activated for design mode, and thus will not be able to
		NoErrorUI = 0x00000002,  // Do not show an error dialog if the app fails to activate.                                
		NoSplashScreen = 0x00000004,  // Do not show the splash screen when activating the app.
	}

	[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("2e941141-7f97-4756-ba1d-9decde894a3d")]
	interface IApplicationActivationManager {
		int ActivateApplication(string appUserModelId, string arguments, ActivateOptions options, out uint processId);
		int ActivateForFile(string appUserModelId, IntPtr pShelItemArray, string verb, out uint processId);
		int ActivateForProtocol(string appUserModelId, IntPtr pShelItemArray, string verb, out uint processId);
	}
}

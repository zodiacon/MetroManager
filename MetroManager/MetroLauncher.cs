using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MetroManager {
	[ComImport, Guid("45BA127D-10A8-46EA-8AB7-56EA9078943C")]
	class ApplicationActivationManager { }

	static class MetroLauncher {
		public static uint LaunchApp(string appUserModelId, string arguments = null) {
			Debug.Assert(!string.IsNullOrWhiteSpace(appUserModelId));
			Debug.Assert(Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Minor >= 2);

			var activation = (IApplicationActivationManager)new ApplicationActivationManager();
			uint pid;
			int hr = activation.ActivateApplication(appUserModelId, arguments ?? string.Empty, ActivateOptions.NoErrorUI, out pid);
			if(hr < 0)
				Marshal.ThrowExceptionForHR(hr);
			return pid;
		}
	}
}

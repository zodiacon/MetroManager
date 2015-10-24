using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MetroManager {
	[ComImport, Guid("45BA127D-10A8-46EA-8AB7-56EA9078943C")]
	class ApplicationActivationManager { }

	static class MetroLauncher {
		public static uint LaunchApp(string packageFullName, string arguments = null) {
			IntPtr pir = IntPtr.Zero;
			try {
				int error = OpenPackageInfoByFullName(packageFullName, 0, out pir);
				Debug.Assert(error == 0);
				if(error != 0)
					throw new Win32Exception(error);

				int length = 0, count;
				GetPackageApplicationIds(pir, ref length, null, out count);

				var buffer = new byte[length];
				error = GetPackageApplicationIds(pir, ref length, buffer, out count);
				Debug.Assert(error == 0);
				if(error != 0)
					throw new Win32Exception(error);

				var appUserModelId = Encoding.Unicode.GetString(buffer, IntPtr.Size * count, length - IntPtr.Size * count);

				var activation = (IApplicationActivationManager)new ApplicationActivationManager();
				uint pid;
				int hr = activation.ActivateApplication(appUserModelId, arguments ?? string.Empty, ActivateOptions.NoErrorUI, out pid);
				if(hr < 0)
					Marshal.ThrowExceptionForHR(hr);
				return pid;
			}
			finally {
				if(pir != IntPtr.Zero)
					ClosePackageInfo(pir);
			}

		}

		[DllImport("kernel32")]
		static extern int OpenPackageInfoByFullName([MarshalAs(UnmanagedType.LPWStr)] string fullName, uint reserved, out IntPtr packageInfo);

		[DllImport("kernel32")]
		static extern int GetPackageApplicationIds(IntPtr pir, ref int bufferLength, byte[] buffer, out int count);

		[DllImport("kernel32")]
		static extern int ClosePackageInfo(IntPtr pir);
	}

}

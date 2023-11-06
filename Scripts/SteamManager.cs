using Godot;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CoreLauncher.Scripts;
using Godot.Collections;

namespace CoreLauncher.Scripts;

public enum CredentialsState {
	LoggedIn,
	NeedsSteamGuardCode,
	Invalid
}

public static class SteamManager {
	public static string SteamPath = "";

	private static Process _loginProcess = null;
	private static StreamWriter _loginProcessWriter = null;
	private static StreamReader _loginProcessReader = null;
	private static double _loginProgress = 0.0;
	private static string _loginState = "";
	
	public static void RunGame() {
		OS.Execute($"{FileManager.GetPath(PathType.Project)}/Commands/RunGame.bat", new [] {GetPath()}, new Godot.Collections.Array());
	}

	public static void Login(string username, string password) {
		string command = $"/c \"{FileManager.GetPath(PathType.Project)}Commands/SteamCMD/Login.bat {GetPath().Replace(" ", "__")}\" {username} {password}";
		ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", /*$"/c \"{FileManager.GetPath(PathType.Project)}Commands/SteamCMDTest.bat\""*/command);
		GD.Print(command);
		processInfo.CreateNoWindow = false;
		processInfo.UseShellExecute = false;
		processInfo.RedirectStandardInput = true;
		processInfo.RedirectStandardOutput = true;

		_loginProcess = Process.Start(processInfo);

		if (_loginProcess != null) {
			_loginProcessWriter = _loginProcess.StandardInput;
			_loginProcessReader = _loginProcess.StandardOutput;
		}
		else {
			GD.PrintErr("Error starting login process (null).");
		}
	}
	
	public static Task<CredentialsState> ValidateCredentials(string username, string password) {
		Login(username, password);
		GD.Print("Debug");

		return Task.Run(() => {
			while (true) {
				string line = _loginProcessReader.ReadLine();

				if (string.IsNullOrEmpty(line)) {
					continue;
				}
				
				GD.Print(line);
			
				string pattern = "password: FAILED";
				Match match = Regex.Match(line, pattern);
				if (match.Success) {
					_loginProcess.Close();
					return CredentialsState.Invalid;
				}
			
				pattern = "This computer has not been authenticated for your account using Steam Guard.";
				match = Regex.Match(line, pattern);
				if (match.Success) {
					_loginProcess.Close();
					return CredentialsState.NeedsSteamGuardCode;
				}
			
				pattern = "Waiting for user info...OK";
				match = Regex.Match(line, pattern);
				if (match.Success) {
					_loginProcess.Close();
					return CredentialsState.LoggedIn;
				}
			}
		});
	}

	public static void EnterSteamGuardCode(string steamGuardCode) {
		if (_loginProcess == null || _loginProcessWriter == null || _loginProcessReader == null) {
			GD.PrintErr("Can't enter steam guard code when no login process is running.");
			return;
		}
		
		_loginProcessWriter.WriteLine(steamGuardCode + "\n\r");
		
		_loginProcess.WaitForExit();
		int exitCode = _loginProcess.ExitCode;
		
		GD.Print($"Login process finished with an exit code of {exitCode}.");
		
		_loginProcess.Close();

		_loginProcess = null;
		_loginProcessWriter = null;
		_loginProcessReader = null;
	}

	public static double GetLoginProgress(out string loginState) {
		if (_loginProcess == null || _loginProcessWriter == null || _loginProcessReader == null) {
			GD.PrintErr("Can't get login progress when no login process is running.");
			
			loginState = "Error, no login process running.";
			return _loginProgress;
		}
		
		_loginState = "Loading SteamCMD...";
		
		while (!_loginProcessReader.EndOfStream) {
			string line = _loginProcessReader.ReadLine();

			if (line == null) {
				break;
			}
			
			string pattern = @"Checking for available update\.\.\.";
			Match match = Regex.Match(line, pattern);
			if (match.Success) {
				_loginState = "Checking for SteamCMD updates...";
				_loginProgress = 0.1;
				continue;
			}
			
			pattern = @"\[ (16)%\] Downloading update";
			match = Regex.Match(line, pattern);
			if (match.Success) {
				_loginState = "Installing SteamCMD update...";

				string installPercentText = match.Captures.FirstOrDefault()?.Value;

				if (int.TryParse(installPercentText, out int installPercent)) {
					_loginProgress = 0.1 + (installPercent / 100.0) * 0.6;
				}
				continue;
			}
			
			pattern = @"Extracting package\.\.\.";
			match = Regex.Match(line, pattern);
			if (match.Success) {
				_loginState = "Extracting SteamCMD update...";
				_loginProgress = 0.8;
				continue;
			}
			
			pattern = @"Installing update\.\.\.";
			match = Regex.Match(line, pattern);
			if (match.Success) {
				_loginState = "Installing SteamCMD update...";
				_loginProgress = 0.9;
				continue;
			}
			
			pattern = @"Logging in user";
			match = Regex.Match(line, pattern);
			if (match.Success) {
				_loginState = "Logging in...";
				_loginProgress = 1.0;
				
				loginState = _loginState;
				return _loginProgress;
			}
		}

		loginState = _loginState;
		return _loginProgress;
	}

	public static string GetPath() {
		return SteamPath != "" ? SteamPath : FileManager.GetPath(PathType.Steam);
	}

	public static void SetPath(string path) {
		SteamPath = path;
	}

	public static bool ValidatePath(string path) {
		return FileManager.PathContains(path, "steam.exe");
	}
}

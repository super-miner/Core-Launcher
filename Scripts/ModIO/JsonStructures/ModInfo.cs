using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CoreLauncher.Scripts.Systems.Fetch;
using Godot;

namespace CoreLauncher.Scripts.ModIO.JsonStructures; 

public class ModInfo {
	private static readonly Regex ExtraDataRegex = new Regex(@"CL_Data\((.+)\)");
	
	[JsonInclude] [JsonPropertyName("name")] public string Name;
	[JsonInclude] [JsonPropertyName("id")] public int Id;
	[JsonInclude] [JsonPropertyName("submitted_by")] public AuthorInfo Author;
	[JsonInclude] [JsonPropertyName("summary")] public string Summary;
	[JsonInclude] [JsonPropertyName("description")] public string Description;
	[JsonInclude] [JsonPropertyName("modfile")] public ModFileInfo ModFile;
	[JsonInclude] [JsonPropertyName("logo")] public LogoInfo Logo;
	[JsonInclude] [JsonPropertyName("dependencies")] public bool HasDependencies;
	[JsonInclude] [JsonPropertyName("tags")] public List<TagInfo> Tags = null;
	
	private DependencyListInfo _dependenciesList;
	private ExtraData _extraData;

	public async Task Init() {
		GD.Print($"Mod: [{Name}] by [{Author.Username}] has: [{Tags.Any(tag => tag.Name == "Script (Elevated Access)")}]");
		await Logo.Init();
	}
	
	public ExtraData GetExtraData() {
		if (_extraData == null) {
			Match match = ExtraDataRegex.Match(Description);

			if (!match.Success) {
				return null;
			}
			
			_extraData = ExtraData.FromString(match.Groups[1].ToString());
		}
		
		return _extraData;
	}

	public bool IsElevatedAccess() {
		return Tags.Any(tag => tag.Name == "Script (Elevated Access)");
	}

	public async Task<List<int>> GetDependencies() {
		if (!HasDependencies) {
			return null;
		}
		
		if (_dependenciesList != null) {
			return _dependenciesList.GetDependencies();
		}
		
		ExtraData extraData = GetExtraData();
		
		if (extraData?.Dependencies != null) {
			return extraData.Dependencies;
		}
				
		string dependencyUrl = ModManager.GetUrl(UrlType.DependenciesList, this);
		string jsonString = await FetchManager.FetchString(dependencyUrl);
		
		_dependenciesList = JsonSerializer.Deserialize<DependencyListInfo>(jsonString);
		return _dependenciesList.GetDependencies();
	}

	public string GetDonationLink() {
		ExtraData extraData = GetExtraData();
		
		if (extraData != null && !string.IsNullOrEmpty(extraData.DonationLink)) {
			return extraData.DonationLink;
		}

		return null;
	}
	
	public bool GetServerSide() {
		ExtraData extraData = GetExtraData();
		
		if (extraData != null) {
			return extraData.ServerSide;
		}

		return true;
	}

	public bool GetClientSide() {
		ExtraData extraData = GetExtraData();
		
		if (extraData != null) {
			return extraData.ClientSide;
		}

		return true;
	}
	
	public bool GetLibrary() {
		if (GetTags().Contains("Library")) {
			return true;
		}
		
		ExtraData extraData = GetExtraData();
		
		if (extraData != null) {
			return extraData.Library;
		}

		return false;
	}
	
	public List<string> GetTags() {
		List<string> output = new List<string>();

		foreach (TagInfo tag in Tags) {
			output.Add(tag.Name);
		}

		return output;
	}
	
	public override string ToString() {
		return ModManager.GetModLocalDirectoryName(Id, Name, ModFile.Version);
	}
}
